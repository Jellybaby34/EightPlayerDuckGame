using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace DuckGame.IncreasedPlayerLimit
{
    public class TeamSelect2Edits
    {
        public static void OnNetworkConnecting(Profile p)
        {
            List<ProfileBox2> _profiles = typeof(TeamSelect2).GetField("_profiles", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).GetValue(Level.current as TeamSelect2) as List<ProfileBox2>;

            /*
            if (p.networkIndex > 4)
            {
                // 0 is player 1's box so we should put player 5 in there as well
                _profiles[p.networkIndex - 5].PrepareDoor();
                //_profiles[0].PrepareDoor();
            }
            */
            //_profiles[p.networkIndex].PrepareDoor();
            _profiles[p.networkIndex].PrepareDoor();
        }

        public static void OnlineSettings()
        {
//            List<ProfileBox2> _profiles = typeof(TeamSelect2).GetField("_profiles", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).GetValue(Level.current as TeamSelect2) as List<ProfileBox2>;

            List<MatchSetting> onlineSettings = new List<MatchSetting>()
            { new MatchSetting() { id = "maxplayers", name = "MAX PLAYERS", value = 8, min = 2, max = 8, step = 1 },
              new MatchSetting() { id = "teams", name = "TEAMS", value = false },
              new MatchSetting() { id = "modifiers", name = "MODIFIERS", value = false, filtered = true, filterOnly = true },
              new MatchSetting() { id = "type", name = "TYPE", value = 2, min = 0, max = 2, createOnly = true, valueStrings = new List<string>() { "PRIVATE", "FRIENDS", "PUBLIC" } }
            };

            Type typea = typeof(TeamSelect2);
            FieldInfo info2 = typea.GetField("onlineSettings", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            info2.SetValue(Level.current as TeamSelect2, onlineSettings);

        }

        // We are hooking this because loads of stuff in Initialize is either private or internal so I'm adding stuff to the calls it does
        public static void UpdateModifierStatus()
        {
            // Extra stuff here - Very dangerous + slow. Need better method

            StackFrame frame = new StackFrame(1);
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            var name = method.Name;

            if (name == "Initialize" && type == typeof(TeamSelect2)) 
            {
                List<DuckPersona> _personas = typeof(Persona).GetField("_personas", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).GetValue(null) as List<DuckPersona>;
                FieldInfo _profilesField = typeof(TeamSelect2).GetField("_profiles", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                dynamic _profiles = _profilesField.GetValue(Level.current as TeamSelect2);

                Profile defaultProfile5 = Profiles.all.FirstOrDefault(x =>
                {
                    if (x.team != null)
                        return x.persona == _personas[4];
                    return false;
                }) ?? Profiles.core.all.ElementAt(4);

                Profile defaultProfile6 = Profiles.all.FirstOrDefault(x =>
                {
                    if (x.team != null)
                        return x.persona == _personas[5];
                    return false;
                }) ?? Profiles.core.all.ElementAt(5);

                Profile defaultProfile7 = Profiles.all.FirstOrDefault(x =>
                {
                    if (x.team != null)
                        return x.persona == _personas[6];
                    return false;
                }) ?? Profiles.core.all.ElementAt(6);

                Profile defaultProfile8 = Profiles.all.FirstOrDefault(x =>
                {
                    if (x.team != null)
                        return x.persona == _personas[7];
                    return false;
                }) ?? Profiles.core.all.ElementAt(7);
                float xpos = 1f;
                ProfileBox2 profileBox2_5 = new ProfileBox2(xpos, 180f, InputProfile.Get("MPPlayer5"), defaultProfile5, (TeamSelect2)Level.current, 4);
                _profiles.Add(profileBox2_5);
                Level.Add(profileBox2_5);
                ProfileBox2 profileBox2_6 = new ProfileBox2(xpos + 178f, 180f, InputProfile.Get("MPPlayer6"), defaultProfile6, (TeamSelect2)Level.current, 5);
                _profiles.Add(profileBox2_6);
                Level.Add(profileBox2_6);
                ProfileBox2 profileBox2_7 = new ProfileBox2(xpos, 270f, InputProfile.Get("MPPlayer7"), defaultProfile7, (TeamSelect2)Level.current, 6);
                _profiles.Add(profileBox2_7);
                Level.Add(profileBox2_7);
                ProfileBox2 profileBox2_8 = new ProfileBox2(xpos + 178f, 270f, InputProfile.Get("MPPlayer8"), defaultProfile8, (TeamSelect2)Level.current, 7);
                _profiles.Add(profileBox2_8);
                Level.Add(profileBox2_8);

                // Expand the view and tweak boxes
                Level.current.camera = new Camera(0f, 0f, -1f, (Graphics.height / 2f));
                Layer.HUD.camera = new Camera(0f, 0f, -1f, (Graphics.height / 2f));

            }

            // Normal stuff here

            Type typea = typeof(TeamSelect2);
            FieldInfo info2 = typea.GetField("_modifierStatus", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            Dictionary<string,bool> _modifierStatus = info2.GetValue(Level.current) as Dictionary<string,bool>;

            bool flag = false;
            foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
            {
                _modifierStatus[unlock.id] = false;
                if (unlock.enabled)
                {
                    flag = true;
                    _modifierStatus[unlock.id] = true;
                }
            }
            if (!Network.isActive || !Network.isServer || Steam.lobby == null)
                return;
            Steam.lobby.SetLobbyData("modifiers", flag ? "true" : "false");
        }

        public static void DoInvite()
        {
            bool _attemptingToInvite = (bool) typeof(TeamSelect2).GetField("_attemptingToInvite", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).GetValue(Level.current as TeamSelect2);

            if (!Network.isActive)
            {
                bool _didHost = (bool)typeof(TeamSelect2).GetField("_didHost", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).GetValue(Level.current as TeamSelect2);

                TeamSelect2.FillMatchmakingProfiles();
                DuckNetwork.Host(8, NetworkLobbyType.FriendsOnly);
                (Level.current as TeamSelect2).PrepareForOnline();
                _didHost = true;

            }
            _attemptingToInvite = true;
        }

        public static void FillMatchmakingProfiles()
        {
            for (int index = 0; index < 8; ++index)
            {
                if (Level.current is TeamSelect2)
                    (Level.current as TeamSelect2).ClearTeam(index);
            }
            UIMatchmakingBox.matchmakingProfiles.Clear();
            foreach (Profile profile in Profiles.active)
            {
                profile.team = Teams.all[Persona.Number(profile.persona)];
                MatchmakingPlayer matchmakingPlayer = new MatchmakingPlayer() { duckIndex = (byte)Persona.Number(profile.persona), inputProfile = profile.inputProfile, team = profile.team };
                matchmakingPlayer.customData = (byte[])null;
                UIMatchmakingBox.matchmakingProfiles.Add(matchmakingPlayer);
            }
        }
    }
}