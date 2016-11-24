using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;

namespace DuckGame.IncreasedPlayerLimit
{
    public class DuckNetworkEdits
    {
        // Because TeamSelect2.OnNetworkConnecting gets inlined, we have to change all these functions. Thanks Mr. Compiler
        // All logging is commented out because DevConsole is an internal class. Will find a way around at some point

        // Get the private method CreateProfile
        public static MethodInfo createProfile = typeof(DuckNetwork).GetMethod("CreateProfile", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        // Get the private field _core
        public static DuckNetworkCore _core = (DuckNetworkCore) typeof(DuckNetwork).GetField("_core", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
        // Get the private method OpenTeamSwitchDialogue
        public static MethodInfo openTeamSwitchDialogue = typeof(DuckNetwork).GetMethod("OpenTeamSwitchDialogue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        public static void UpdateNetmessageTypes()
        {
            Type typea = typeof(Network);
            FieldInfo info2 = typea.GetField("_typeToMessageID", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            dynamic _typeToMessageID = info2.GetValue(Level.current) as Map<ushort, System.Type>;
            
            IEnumerable<System.Type> subclasses = Editor.GetSubclasses(typeof(NetMessage));
            _typeToMessageID.Clear();
            ushort key = 1;
            foreach (System.Type type in subclasses)
            {
                if (type.GetCustomAttributes(typeof(FixedNetworkID), false).Length != 0)
                {
                    FixedNetworkID customAttribute = (FixedNetworkID)type.GetCustomAttributes(typeof(FixedNetworkID), false)[0];
                    if (customAttribute != null)
                        _typeToMessageID.Add(type, customAttribute.FixedID);
                }
            }
            foreach (System.Type type in subclasses)
            {
                if (!_typeToMessageID.ContainsValue(type))
                {
                    while (_typeToMessageID.ContainsKey(key))
                        ++key;
                    _typeToMessageID.Add(type, key);
                    ++key;
                }
            }
        }

        public static void Host(int maxPlayers, NetworkLobbyType lobbyType)
        {
            if (_core.status != DuckNetStatus.Disconnected)
                return;
            //            DevConsole.Log(DCSection.DuckNet, "|LIME|Hosting new server. ", -1);
            
            // Rubbish fix for Update calling host with 4 max players. Need better way
            if (maxPlayers == 4 && lobbyType == NetworkLobbyType.FriendsOnly)
            {
                maxPlayers = 8;
            }

            DuckNetwork.Reset();
            foreach (Profile universalProfile in Profiles.universalProfileList)
                universalProfile.team = (Team)null;
            _core.error = (DuckNetErrorInfo)null;
            TeamSelect2.DefaultSettings();
            Network.HostServer(lobbyType, maxPlayers, "duckGameServer", 1337);
            DuckNetwork.localConnection.AttemptConnection();
            foreach (Profile profile in DuckNetwork.profiles)
            {
                profile.slotType = lobbyType != NetworkLobbyType.Private ? (lobbyType != NetworkLobbyType.FriendsOnly ? SlotType.Open : SlotType.Friend) : SlotType.Invite;
                if ((int)profile.networkIndex >= maxPlayers)
                    profile.slotType = SlotType.Closed;
            }
            int num = 1;
            _core.localDuckIndex = -1;
            foreach (MatchmakingPlayer matchmakingProfile in UIMatchmakingBox.matchmakingProfiles)
            {
                string name = Network.activeNetwork.core.GetLocalName();
                if (num > 1)
                    name = name + "(" + num.ToString() + ")";
                if (_core.localDuckIndex == -1)
                {
                    _core.localDuckIndex = (int)matchmakingProfile.duckIndex;
                    _core.hostDuckIndex = (int)matchmakingProfile.duckIndex;
                }
                Profile profile = (Profile) createProfile.Invoke(null, new object[] { _core.localConnection, name, (int)matchmakingProfile.duckIndex, matchmakingProfile.inputProfile, false, false, false } );
                if (num > 1)
                    profile.slotType = SlotType.Local;
                profile.networkStatus = DuckNetStatus.Connected;
                if (matchmakingProfile.team != null)
                {
                    if (matchmakingProfile.team.customData != null)
                    {
                        profile.team = Teams.all[(int)matchmakingProfile.duckIndex];
                        Team.MapFacade(profile.steamID, matchmakingProfile.team);
                    }
                    else
                        profile.team = matchmakingProfile.team;
                }
                ++num;
            }
            _core.localConnection.isHost = true;
            _core.status = DuckNetStatus.Connecting;
        }

        public static void Join(string id, string ip = "localhost")
        {
            if (DuckNetwork.core.status != DuckNetStatus.Disconnected)
                return;
            DuckNetwork.Reset();
            foreach (Profile universalProfile in Profiles.universalProfileList)
                universalProfile.team = null;
            for (int index = 0; index < 8; ++index)
                Teams.all[index].customData = null;
            foreach (Profile profile in DuckNetwork.profiles)
                profile.slotType = SlotType.Open;
            DuckNetwork.core.error = null;
            DuckNetwork.core.localDuckIndex = -1;
            TeamSelect2.DefaultSettings();
            Network.JoinServer(id, 1337, ip);
            DuckNetwork.localConnection.AttemptConnection();
            DuckNetwork.core.attemptTimeout = 15f;
            DuckNetwork.core.status = DuckNetStatus.EstablishingCommunication;
        }

        public static void ChangeSlotSettings()
        {
            bool flag1 = true;
            bool flag2 = true;
            int num = 0;
            foreach (Profile profile in DuckNetwork.profiles)
            {
                if (profile.connection != DuckNetwork.localConnection)
                {
                    if (profile.slotType != SlotType.Friend)
                        flag1 = false;
                    if (profile.slotType != SlotType.Invite)
                        flag2 = false;
                    if (profile.slotType != SlotType.Closed)
                        ++num;
                }
                else
                    ++num;
            }
            if (!Network.isServer)
                return;
            if (Steam.lobby != null)
            {
                Steam.lobby.type = !flag1 ? (!flag2 ? SteamLobbyType.Public : SteamLobbyType.Private) : SteamLobbyType.FriendsOnly;
                Steam.lobby.maxMembers = 32;
            }
            Send.Message(new NMChangeSlotsEdits((byte)DuckNetwork.profiles[0].slotType, (byte)DuckNetwork.profiles[1].slotType, (byte)DuckNetwork.profiles[2].slotType, (byte)DuckNetwork.profiles[3].slotType, (byte)DuckNetwork.profiles[4].slotType, (byte)DuckNetwork.profiles[5].slotType, (byte)DuckNetwork.profiles[6].slotType, (byte)DuckNetwork.profiles[7].slotType));
        }

        public static Profile JoinLocalDuck(InputProfile input)
        {

            int num = 1;
            foreach (Profile profile in DuckNetwork.profiles)
            {
                if (profile.connection == DuckNetwork.localConnection)
                    ++num;
            }
            string name = Network.activeNetwork.core.GetLocalName();
            if (num > 1)
                name = name + "(" + num.ToString() + ")";

            //Invoke the private method. Hope its not too slow
            Profile profile1 = (Profile) createProfile.Invoke(null, new object[] { DuckNetwork.localConnection, name, -1, input, false, false, true });

            if (profile1 == null)
                return (Profile)null;
            profile1.networkStatus = !Network.isClient ? DuckNetStatus.Connected : DuckNetStatus.Connecting;

            // Our new OnNetworkConnecting which should bloody work
            TeamSelect2Edits.OnNetworkConnecting(profile1);

            DuckNetwork.SendNewProfile(profile1, DuckNetwork.localConnection, false);
            return profile1;
        }

        public static NetMessage OnMessageFromNewClient(NetMessage m)
        {
            if (Network.isServer)
            {
                if (m is NMRequestJoin)
                {
                    if (DuckNetwork.inGame)
                        return (NetMessage)new NMGameInProgress();
                    NMRequestJoin nmRequestJoin = m as NMRequestJoin;
//                    DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Join attempt from " + nmRequestJoin.name, -1);
                    NMVersionMismatch.Type code = DuckNetwork.CheckVersion(nmRequestJoin.id);
                    if (code != NMVersionMismatch.Type.Match)
                    {
//                        DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|" + nmRequestJoin.name + " had a version mismatch.", -1);
//                        return (NetMessage)new NMVersionMismatch(code, DG.version);
                          return (NetMessage)new NMVersionMismatch(code, Assembly.GetEntryAssembly().GetName().Version.ToString());
                    }
                    Type methodtype = typeof(DuckNetwork);
                    MethodInfo createProfile = methodtype.GetMethod("CreateProfile", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

//                    Profile profile = DuckNetwork.CreateProfile(m.connection, nmRequestJoin.name, -1, (InputProfile)null, nmRequestJoin.hasCustomHats, nmRequestJoin.wasInvited, false);
                    Profile profile = (Profile)createProfile.Invoke(null, new object[] { m.connection, nmRequestJoin.name, -1, (InputProfile)null, nmRequestJoin.hasCustomHats, nmRequestJoin.wasInvited, false });

                    if (profile == null)
                    {
//                        DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|" + nmRequestJoin.name + " could not join, server is full.", -1);
                        return (NetMessage)new NMServerFull();
                    }
                    profile.flippers = nmRequestJoin.flippers;
                    profile.networkStatus = DuckNetStatus.WaitingForLoadingToBeFinished;
                    _core.status = DuckNetStatus.Connected;

                    TeamSelect2Edits.OnNetworkConnecting(profile);

                    DuckNetwork.SendNewProfile(profile, m.connection, false);

                    // More slots so we need bigger method to handle them
                    Send.Message(new NMChangeSlotsEdits((byte)DuckNetwork.profiles[0].slotType, (byte)DuckNetwork.profiles[1].slotType, (byte)DuckNetwork.profiles[2].slotType, (byte)DuckNetwork.profiles[3].slotType, (byte)DuckNetwork.profiles[4].slotType, (byte)DuckNetwork.profiles[5].slotType, (byte)DuckNetwork.profiles[6].slotType, (byte)DuckNetwork.profiles[7].slotType), m.connection);
                    TeamSelect2.SendMatchSettings(m.connection, true);
                    return (NetMessage)null;
                }
                if (m is NMMessageIgnored)
                    return (NetMessage)null;
            }
            else
            {
                if (m is NMRequestJoin)
                {
//                    DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Another computer has requested a matchmaking connection.", -1);
                    return (NetMessage)new NMGameInProgress();
                }
                if (m is NMMessageIgnored)
                    return (NetMessage)null;
            }
            return (NetMessage)new NMMessageIgnored();
        }

        public static void OnMessage(NetMessage m)
        {
            if (m is NMJoinDuckNetwork)
//                DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Join message", -1);
            if (DuckNetwork.status == DuckNetStatus.Disconnected)
                return;
            if (m == null)
                Main.codeNumber = 13371;
            UIMatchmakingBox.pulseNetwork = true;
            if (DuckNetwork.GetProfiles(m.connection).Count == 0 && m.connection != Network.host)
            {
                Main.codeNumber = 13372;
                NetMessage msg = DuckNetwork.OnMessageFromNewClient(m);
                if (msg == null)
                    return;
                Send.Message(msg, m.connection);
            }
            else
            {
                if (DuckNetwork.HandleCoreConnectionMessages(m) || DuckNetwork.status == DuckNetStatus.Disconnecting)
                    return;
                Main.codeNumber = 13373;
                foreach (Profile profile in DuckNetwork.GetProfiles(m.connection))
                {
                    if (profile.networkStatus == DuckNetStatus.Disconnecting || profile.networkStatus == DuckNetStatus.Disconnected || profile.networkStatus == DuckNetStatus.Failure)
                        return;
                }
                Main.codeNumber = m.typeIndex;
                if (Network.isServer)
                {
                    if (m is NMLateJoinDuckNetwork)
                    {
                        if (!(Level.current is TeamSelect2))
                        {
                            Send.Message(new NMGameInProgress(), NetMessagePriority.ReliableOrdered, m.connection);
                        }
                        else
                        {
                            NMLateJoinDuckNetwork lateJoinDuckNetwork = m as NMLateJoinDuckNetwork;
//                            DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Late join attempt from " + lateJoinDuckNetwork.name, -1);
//                            Profile profile = DuckNetwork.CreateProfile(m.connection, lateJoinDuckNetwork.name, (int)lateJoinDuckNetwork.duckIndex, (InputProfile)null, false, false, false);
                            Profile profile = (Profile)createProfile.Invoke(null, new object[] { m.connection, lateJoinDuckNetwork.name, (int)lateJoinDuckNetwork.duckIndex, null, false, false, false });
                            if (profile != null)
                            {
                                profile.networkStatus = DuckNetStatus.Connected;
                                TeamSelect2Edits.OnNetworkConnecting(profile);
                                DuckNetwork.SendNewProfile(profile, m.connection, true);
                            }
                            else
                                Send.Message(new NMServerFull(), NetMessagePriority.ReliableOrdered, m.connection);
                        }
                    }
                    else if (m is NMJoinedDuckNetwork)
                    {
                        foreach (Profile profile in DuckNetwork.GetProfiles(m.connection))
//                            DevConsole.Log(DCSection.DuckNet, "|LIME|" + profile.name + " Has joined the DuckNet", -1);
                        Send.Message(new NMSwitchLevel("@TEAMSELECT", DuckNetwork.levelIndex, (ushort)0, false, 0U), m.connection);
                    }
                    else if (m is NMClientLoadedLevel)
                    {
                        if ((m as NMClientLoadedLevel).levelIndex == DuckNetwork.levelIndex)
                            m.connection.wantsGhostData = (m as NMClientLoadedLevel).levelIndex;
                        else
                            ;// DevConsole.Log(DCSection.DuckNet, "|DGRED|" + m.connection.identifier + " LOADED WRONG LEVEL! (" + DuckNetwork.levelIndex + " VS " + (m as NMClientLoadedLevel).levelIndex + ")", -1);
                    }
                    else if (m is NMSetTeam)
                    {
                        NMSetTeam nmSetTeam = m as NMSetTeam;
                        if (nmSetTeam.duck < 0 || nmSetTeam.duck >= 8)
                            return;
                        Profile profile = DuckNetwork.profiles[nmSetTeam.duck];
                        if (profile.connection == null || profile.team == null)
                            return;
                        profile.team = Teams.all[nmSetTeam.team];
                        if (!DuckNetwork.OnTeamSwitch(profile))
                            return;
                        Send.MessageToAllBut(new NMSetTeam(nmSetTeam.duck, nmSetTeam.team), NetMessagePriority.ReliableOrdered, m.connection);
                    }
                    else
                    {
                        if (!(m is NMSpecialHat))
                            return;
                        NMSpecialHat nmSpecialHat = m as NMSpecialHat;
                        Team t = Team.Deserialize(nmSpecialHat.GetData());
                        foreach (Profile profile in DuckNetwork.profiles)
                        {
                            if ((long)profile.steamID == (long)nmSpecialHat.link)
                            {
                                if (t != null)
                                    Team.MapFacade(profile.steamID, t);
                                else
                                    Team.ClearFacade(profile.steamID);
                                Send.MessageToAllBut(new NMSpecialHat(t, profile.steamID), NetMessagePriority.ReliableOrdered, m.connection);
                            }
                        }
                    }
                }
                else if (m is NMSpecialHat)
                {
                    NMSpecialHat nmSpecialHat = m as NMSpecialHat;
                    Team t = Team.Deserialize(nmSpecialHat.GetData());
                    foreach (Profile profile in DuckNetwork.profiles)
                    {
                        if ((long)profile.steamID == (long)nmSpecialHat.link)
                        {
                            if (t != null)
                                Team.MapFacade(profile.steamID, t);
                            else
                                Team.ClearFacade(profile.steamID);
                        }
                    }
                }
                else if (m is NMJoinDuckNetwork)
                {
                    NMRemoteJoinDuckNetwork remoteJoinDuckNetwork = m as NMRemoteJoinDuckNetwork;
                    if (remoteJoinDuckNetwork == null)
                    {
//                        DevConsole.Log(DCSection.DuckNet, "|LIME|Connection with host was established!", -1);
                        NMJoinDuckNetwork nmJoinDuckNetwork = m as NMJoinDuckNetwork;
                        _core.status = DuckNetStatus.Connected;
                        if (DuckNetwork.profiles[nmJoinDuckNetwork.duckIndex].connection == DuckNetwork.localConnection)
                        {
                            DuckNetwork.profiles[nmJoinDuckNetwork.duckIndex].networkStatus = DuckNetStatus.Connected;
                        }
                        else
                        {
//                            Profile profile = DuckNetwork.CreateProfile(DuckNetwork.localConnection, Network.activeNetwork.core.GetLocalName(), (int)nmJoinDuckNetwork.duckIndex, UIMatchmakingBox.matchmakingProfiles.Count > 0 ? UIMatchmakingBox.matchmakingProfiles[0].inputProfile : InputProfile.DefaultPlayer1, Teams.core.extraTeams.Count > 0, false, false);
                            Profile profile = (Profile)createProfile.Invoke(null, new object[] { DuckNetwork.localConnection, Network.activeNetwork.core.GetLocalName(), (int)nmJoinDuckNetwork.duckIndex, UIMatchmakingBox.matchmakingProfiles.Count > 0 ? UIMatchmakingBox.matchmakingProfiles[0].inputProfile : InputProfile.DefaultPlayer1, Teams.core.extraTeams.Count > 0, false, false });

                            _core.localDuckIndex = nmJoinDuckNetwork.duckIndex;
                            profile.flippers = Profile.CalculateLocalFlippers();
                            profile.networkStatus = DuckNetStatus.WaitingForLoadingToBeFinished;
                        }
                    }
                    else
                    {
                        NetworkConnection networkConnection = remoteJoinDuckNetwork.connection;
                        Main.codeNumber = 133701;
                        if (remoteJoinDuckNetwork.identifier == "SERVER")
                        {
                            Main.codeNumber = 133702;
//                            Profile profile1 = DuckNetwork.CreateProfile(networkConnection, remoteJoinDuckNetwork.name, (int)remoteJoinDuckNetwork.duckIndex, (InputProfile)null, remoteJoinDuckNetwork.hasCustomHats, false, false);
                            Profile profile1 = (Profile)createProfile.Invoke(null, new object[] { networkConnection, remoteJoinDuckNetwork.name, (int)remoteJoinDuckNetwork.duckIndex, null, remoteJoinDuckNetwork.hasCustomHats, false, false });
                            profile1.flippers = remoteJoinDuckNetwork.flippers;
                            profile1.team = Teams.all[remoteJoinDuckNetwork.team];
                            if (_core.hostDuckIndex == -1)
                                _core.hostDuckIndex = remoteJoinDuckNetwork.duckIndex;
                            Main.codeNumber = 133703;
                            bool flag = false;
                            foreach (Profile profile2 in DuckNetwork.GetProfiles(networkConnection))
                            {
                                if (profile2 != profile1)
                                {
                                    profile1.networkStatus = profile2.networkStatus;
                                    flag = true;
                                    break;
                                }
                            }
                            Main.codeNumber = 133704;
                            if (flag)
                                return;
                            profile1.networkStatus = DuckNetStatus.WaitingForLoadingToBeFinished;
                        }
                        else
                        {
                            Main.codeNumber = 133705;
                            bool flag = false;
                            DuckNetStatus duckNetStatus = DuckNetStatus.NeedsNotificationWhenReadyForData;
                            foreach (Profile profile in DuckNetwork.GetProfiles(networkConnection))
                            {
                                if (profile.connection.identifier == remoteJoinDuckNetwork.identifier)
                                {
                                    networkConnection = profile.connection;
                                    flag = true;
                                    duckNetStatus = profile.networkStatus;
                                    break;
                                }
                            }
                            Main.codeNumber = 133706;
                            if (!flag)
                            {
                                networkConnection = Network.activeNetwork.core.AttemptConnection(remoteJoinDuckNetwork.identifier);
                                if (networkConnection == null)
                                {
                                    DuckNetwork.RaiseError(new DuckNetErrorInfo()
                                    {
                                        error = DuckNetError.InvalidConnectionInformation,
                                        message = "Invalid connection information (" + remoteJoinDuckNetwork.identifier + ")"
                                    });
                                    return;
                                }
                            }
                            Main.codeNumber = 133707;
//                            Profile profile1 = DuckNetwork.CreateProfile(networkConnection, remoteJoinDuckNetwork.name, remoteJoinDuckNetwork.duckIndex, (InputProfile)null, remoteJoinDuckNetwork.hasCustomHats, false, false);
                            Profile profile1 = (Profile)createProfile.Invoke(null, new object[] { networkConnection, remoteJoinDuckNetwork.name, remoteJoinDuckNetwork.duckIndex, (InputProfile)null, remoteJoinDuckNetwork.hasCustomHats, false, false });
                            profile1.team = Teams.all[remoteJoinDuckNetwork.team];
                            profile1.networkStatus = duckNetStatus;
                            profile1.flippers = remoteJoinDuckNetwork.flippers;
                        }
                    }
                }
                else if (m is NMEndOfDuckNetworkData)
                {
                    Send.Message(new NMJoinedDuckNetwork(), m.connection);
                    foreach (Profile profile in DuckNetwork.profiles)
                    {
                        if (profile.connection == DuckNetwork.localConnection)
                            Send.Message(new NMProfileInfo(profile.networkIndex, profile.stats.unloyalFans, profile.stats.loyalFans));
                    }
                }
                else if (m is NMEndOfGhostData)
                {
                    if ((m as NMEndOfGhostData).levelIndex == DuckNetwork.levelIndex)
                    {
                        //                        DevConsole.Log(DCSection.DuckNet, "|DGGREEN|Received Host Level Information (" + (m as NMEndOfGhostData).levelIndex + ").", -1);
                        Level.current.TransferComplete(m.connection);
                        DuckNetwork.SendToEveryone(new NMLevelDataReady(DuckNetwork.levelIndex));
                        foreach (Profile profile in DuckNetwork.GetProfiles(DuckNetwork.localConnection))
                            profile.connection.loadingStatus = (m as NMEndOfGhostData).levelIndex;
                    }
                    else
                        ;// DevConsole.Log(DCSection.DuckNet, "|DGRED|Recieved data for wrong level.", -1);
                }
                else if (m is NMSetTeam)
                {
                    NMSetTeam nmSetTeam = m as NMSetTeam;
                    if (nmSetTeam.duck < 0 || nmSetTeam.duck >= 8)
                        return;
                    Profile profile = DuckNetwork.profiles[nmSetTeam.duck];
                    if (profile.connection == null || profile.team == null)
                        return;
                    profile.team = Teams.all[nmSetTeam.team];
                }
                else
                {
                    if (!(m is NMTeamSetDenied))
                        return;
                    NMTeamSetDenied nmTeamSetDenied = m as NMTeamSetDenied;
                    if (nmTeamSetDenied.duck < 0 || nmTeamSetDenied.duck >= 8)
                        return;
                    Profile profile = DuckNetwork.profiles[nmTeamSetDenied.duck];
                    if (profile.connection != DuckNetwork.localConnection || profile.team == null || Teams.all.IndexOf(profile.team) != nmTeamSetDenied.team)
                        return;
                    openTeamSwitchDialogue.Invoke(null, new[] { profile });
                }
            }
        }

    }
}