using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Harmony;
using System.Reflection.Emit;

namespace DuckGame.EightPlayerDuckGame
{

    public class ProfilesCoreEdits
    {
        // This property starts with index = 4, and adds any profiles above that as custom profiles
        // Our extra 4 would be considered "custom" but we don't want that so we change the start to 8.
        [HarmonyPatch(typeof(ProfilesCore))]
        [HarmonyPatch("allCustomProfiles", MethodType.Getter)]
        public static class ProfilesCore_allCustomProfiles_Transpiler
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Ldc_I4_4)
                    {
                        codes[i].opcode = OpCodes.Ldc_I4_8;
                        break;
                    }
                }
                return codes.AsEnumerable();
            }
        }

        // This method checks if our profile is any of the default 4 and if not, return false.
        // Our extra 4 shouldn't be considered "custom" so we change the loop to check the first 8.
        [HarmonyPatch(typeof(ProfilesCore), "IsDefault")]
        public static class ProfilesCore_IsDefault_Prefix
        {
            [HarmonyPrefix]
            public static bool Prefix(ProfilesCore __instance, ref bool __result, ref Profile p)
            {
                for (int index = 0; index < 8; index++)
                {
                    if (__instance._profiles[index] == p)
                        __result = true;
                    return false;
                }
                __result = false;

                return false;
            }
        }


        [HarmonyPatch(typeof(ProfilesCore), "Initialize")]
        public static class ProfilesCore_Initialize_Prefix
        {

            [HarmonyPrefix]
            public static bool Prefix(ProfilesCore __instance)
            {
                if (__instance == null)
                    throw new Exception("Instance was null, PANIC");

                List<DuckPersona> personas = Persona.all as List<DuckPersona>;

                if (personas == null)
                    throw new Exception("personas is null");

                __instance._profiles = new List<Profile>()
            {
                new Profile("Player1", InputProfile.Get("MPPlayer1"), Teams.Player1, Persona.Duck1, false, "PLAYER1"),
                new Profile("Player2", InputProfile.Get("MPPlayer2"), Teams.Player2, Persona.Duck2, false, "PLAYER2"),
                new Profile("Player3", InputProfile.Get("MPPlayer3"), Teams.Player3, Persona.Duck3, false, "PLAYER3"),
                new Profile("Player4", InputProfile.Get("MPPlayer4"), Teams.Player4, Persona.Duck4, false, "PLAYER4"),
                new Profile("Player5", InputProfile.Get("MPPlayer5"), Teams.core.teams[4], personas[4], false, "PLAYER5"),
                new Profile("Player6", InputProfile.Get("MPPlayer6"), Teams.core.teams[5], personas[5], false, "PLAYER6"),
                new Profile("Player7", InputProfile.Get("MPPlayer7"), Teams.core.teams[6], personas[6], false, "PLAYER7"),
                new Profile("Player8", InputProfile.Get("MPPlayer8"), Teams.core.teams[7], personas[7], false, "PLAYER8")
            };

                Profile.loading = true;
                string[] files = DuckFile.GetFiles(DuckFile.profileDirectory, "*.*");
                List<Profile> profileList1 = new List<Profile>();
                foreach (string path in files)
                {
                    XDocument xdocument = DuckFile.LoadXDocument(path);
                    if (xdocument != null)
                    {
                        string name = xdocument.Element((XName)"Profile").Element((XName)"Name").Value;
                        bool flag = false;
                        Profile p = __instance._profiles.FirstOrDefault<Profile>((Func<Profile, bool>)(pro => pro.name == name));
                        if (p == null || !Profiles.IsDefault(p))
                        {
                            p = new Profile("", (InputProfile)null, (Team)null, (DuckPersona)null, false, (string)null);
                            p.fileName = path;
                            flag = true;
                        }
                        IEnumerable<XElement> source = xdocument.Elements((XName)"Profile");
                        if (source != null)
                        {
                            foreach (XElement element1 in source.Elements<XElement>())
                            {
                                if (element1.Name.LocalName == "ID" && !Profiles.IsDefault(p))
                                    p.SetID(element1.Value);
                                else if (element1.Name.LocalName == "Name")
                                    p.name = element1.Value;
                                else if (element1.Name.LocalName == "Mood")
                                    p.funslider = Change.ToSingle((object)element1.Value);
                                else if (element1.Name.LocalName == "NS")
                                    p.numSandwiches = Change.ToInt32((object)element1.Value);
                                else if (element1.Name.LocalName == "MF")
                                    p.milkFill = Change.ToInt32((object)element1.Value);
                                else if (element1.Name.LocalName == "LML")
                                    p.littleManLevel = Change.ToInt32((object)element1.Value);
                                else if (element1.Name.LocalName == "NLM")
                                    p.numLittleMen = Change.ToInt32((object)element1.Value);
                                else if (element1.Name.LocalName == "LMB")
                                    p.littleManBucks = Change.ToInt32((object)element1.Value);
                                else if (element1.Name.LocalName == "RSXP")
                                    p.roundsSinceXP = Change.ToInt32((object)element1.Value);
                                else if (element1.Name.LocalName == "TimesMet")
                                    p.timesMetVincent = Change.ToInt32((object)element1.Value);
                                else if (element1.Name.LocalName == "TimesMet2")
                                    p.timesMetVincentSale = Change.ToInt32((object)element1.Value);
                                else if (element1.Name.LocalName == "TimesMet3")
                                    p.timesMetVincentSell = Change.ToInt32((object)element1.Value);
                                else if (element1.Name.LocalName == "TimesMet4")
                                    p.timesMetVincentImport = Change.ToInt32((object)element1.Value);
                                else if (element1.Name.LocalName == "TimeOfDay")
                                    p.timeOfDay = Change.ToSingle((object)element1.Value);
                                else if (element1.Name.LocalName == "CD")
                                    p.currentDay = Change.ToInt32((object)element1.Value);
                                else if (element1.Name.LocalName == "XtraPoints")
                                    p.xp = Change.ToInt32((object)element1.Value);
                                else if (element1.Name.LocalName == "FurniPositions")
                                    p.furniturePositionData = BitBuffer.FromString(element1.Value);
                                else if (element1.Name.LocalName == "Fowner")
                                    p.furnitureOwnershipData = BitBuffer.FromString(element1.Value);
                                else if (element1.Name.LocalName == "SteamID")
                                {
                                    p.steamID = Change.ToUInt64((object)element1.Value);
                                    if ((long)p.steamID != 0L)
                                        profileList1.Add(p);
                                }
                                else if (element1.Name.LocalName == "LastKnownName")
                                    p.lastKnownName = element1.Value;
                                else if (element1.Name.LocalName == "Stats")
                                    p.stats.Deserialize(element1);
                                else if (element1.Name.LocalName == "Unlocks")
                                {
                                    string[] strArray = element1.Value.Split('|');
                                    p.unlocks = new List<string>((IEnumerable<string>)strArray);
                                }
                                else if (element1.Name.LocalName == "Tickets")
                                    p.ticketCount = Convert.ToInt32(element1.Value);
                                else if (element1.Name.LocalName == "Mappings" && !MonoMain.defaultControls)
                                {
                                    p.inputMappingOverrides.Clear();
                                    foreach (XElement element2 in element1.Elements())
                                    {
                                        if (element2.Name.LocalName == "InputMapping")
                                        {
                                            DeviceInputMapping deviceInputMapping = new DeviceInputMapping();
                                            deviceInputMapping.Deserialize(element2);
                                            p.inputMappingOverrides.Add(deviceInputMapping);
                                        }
                                    }
                                }
                            }
                        }
                        if (flag)
                            __instance._profiles.Add(p);
                    }
                }
                byte localFlippers = Profile.CalculateLocalFlippers();
                Profile p1 = (Profile)null;

                if (Profiles.allCustomProfiles == null)
                    throw new Exception("Custom Profiles null.");

                if (Steam.user != null && (long)Steam.user.id != 0L)
                {
                    string str = Steam.user.id.ToString();
                    foreach (Profile allCustomProfile in Profiles.allCustomProfiles)
                    {
                        if ((long)allCustomProfile.steamID == (long)Steam.user.id && allCustomProfile.id == str && allCustomProfile.rawName == str)
                        {
                            p1 = allCustomProfile;
                            break;
                        }
                    }
                    if (p1 == null)
                    {
                        p1 = new Profile(Steam.user.id.ToString(), (InputProfile)null, (Team)null, (DuckPersona)null, false, Steam.user.id.ToString())
                        {
                            steamID = Steam.user.id
                        };
                        Profiles.Add(p1);
                        __instance.Save(p1);
                    }
                }
                if (p1 != null)
                {
                    __instance._profiles.Remove(p1);
                    __instance._profiles.Insert(8, p1);
                    List<Profile> source = new List<Profile>();
                    List<Profile> profileList2 = new List<Profile>();
                    foreach (Profile allCustomProfile in Profiles.allCustomProfiles)
                    {
                        string str = allCustomProfile.steamID.ToString();
                        if ((long)allCustomProfile.steamID != 0L)
                        {
                            if (allCustomProfile.id == str && allCustomProfile.rawName == str)
                                source.Add(allCustomProfile);
                            else
                                profileList2.Add(allCustomProfile);
                        }
                    }
                    using (List<Profile>.Enumerator enumerator = profileList2.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Profile pro = enumerator.Current;
                            Profile p2 = source.FirstOrDefault<Profile>((Func<Profile, bool>)(x => (long)x.steamID == (long)pro.steamID));
                            if (p2 == null)
                            {
                                p2 = new Profile(pro.steamID.ToString(), (InputProfile)null, (Team)null, (DuckPersona)null, false, pro.steamID.ToString());
                                p2.steamID = pro.steamID;
                                Profiles.Add(p2);
                            }
                            p2.stats = (ProfileStats)((DataClass)p2.stats + (DataClass)pro.stats);
                            foreach (KeyValuePair<string, List<ChallengeSaveData>> keyValuePair in (MultiMap<string, ChallengeSaveData, List<ChallengeSaveData>>)Challenges.saveData)
                            {
                                ChallengeSaveData challengeSaveData1 = (ChallengeSaveData)null;
                                List<ChallengeSaveData> challengeSaveDataList = new List<ChallengeSaveData>();
                                foreach (ChallengeSaveData challengeSaveData2 in keyValuePair.Value)
                                {
                                    if (challengeSaveData2.profileID == pro.id || challengeSaveData2.profileID == p2.id)
                                    {
                                        challengeSaveData2.profileID = p2.id;
                                        if (challengeSaveData1 == null)
                                            challengeSaveData1 = challengeSaveData2;
                                        else if (challengeSaveData2.trophy > challengeSaveData1.trophy)
                                        {
                                            challengeSaveDataList.Add(challengeSaveData1);
                                            challengeSaveData1 = challengeSaveData2;
                                        }
                                        else
                                            challengeSaveDataList.Add(challengeSaveData2);
                                    }
                                }
                                foreach (ChallengeSaveData challengeSaveData2 in challengeSaveDataList)
                                {
                                    string str = challengeSaveData2.profileID + "OBSOLETE";
                                    challengeSaveData2.profileID = str;
                                }
                                if (challengeSaveData1 != null)
                                    Challenges.Save(keyValuePair.Key);
                            }
                            __instance._profiles.Remove(pro);
                            DuckFile.Delete(pro.fileName);
                            __instance.Save(p2);
                        }
                    }
                }
                foreach (Profile profile in __instance._profiles)
                {
                    profile.flippers = localFlippers;
                    profile.ticketCount = Challenges.GetTicketCount(profile);
                    if (profile.ticketCount < 0)
                        profile.ticketCount = 0;
                }
                Profile.loading = false;

                return false;
            }
        }

    }
}