using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace DuckGame.IncreasedPlayerLimit
{
    public class ProfilesCoreEdits
    {
        public static ProfilesCore profilecore = typeof(Profiles).GetField("_core", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).GetValue(null) as ProfilesCore;

        public static void defaultProfiles()
        {
            PropertyInfo _profilesProperty = typeof(ProfilesCore).GetProperty("defaultProfiles", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            MethodInfo methodToReplace = _profilesProperty.GetGetMethod(true);
            MethodInfo methodToInject = typeof(ProfilesCoreEdits).GetMethod("defaultProfilesReplace", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            UnsafeCode.CodeInjection(methodToReplace, methodToInject);
        }

        public List<Profile> defaultProfilesReplace()
        {
            List<Profile> _profilesValue = typeof(ProfilesCore).GetField("_profiles", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).GetValue(profilecore) as List<Profile>;
            return new List<Profile>(_profilesValue.GetRange(0, 8));
        }

        public static void allCustomProfiles()
        {
            PropertyInfo _profilesProperty = typeof(ProfilesCore).GetProperty("allCustomProfiles", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            MethodInfo methodToReplace = _profilesProperty.GetGetMethod();
            MethodInfo methodToInject = typeof(ProfilesCoreEdits).GetMethod("allCustomProfilesReplace", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            UnsafeCode.CodeInjection(methodToReplace, methodToInject);
        }

        public List<Profile> allCustomProfilesReplace()
        {
            List<Profile> _profilesValue = typeof(ProfilesCore).GetField("_profiles", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).GetValue(profilecore) as List<Profile>;

            List<Profile> profileList = new List<Profile>();
            for (int index = 8; index < _profilesValue.Count; ++index)
                profileList.Add(_profilesValue[index]);
            return profileList;
        }

        public static void Initialize()
        {
            Type typea = typeof(Persona);
            FieldInfo info2 = typea.GetField("_personas", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            List<DuckPersona> _personas = info2.GetValue(null) as List<DuckPersona>;

            List<Profile> _profilesList = new List<Profile>()
        {
            new Profile("Player1", InputProfile.Get("MPPlayer1"), Teams.Player1, Persona.Duck1, false, "PLAYER1"),
            new Profile("Player2", InputProfile.Get("MPPlayer2"), Teams.Player2, Persona.Duck2, false, "PLAYER2"),
            new Profile("Player3", InputProfile.Get("MPPlayer3"), Teams.Player3, Persona.Duck3, false, "PLAYER3"),
            new Profile("Player4", InputProfile.Get("MPPlayer4"), Teams.Player4, Persona.Duck4, false, "PLAYER4"),
            new Profile("Player5", InputProfile.Get("MPPlayer5"), Teams.core.teams[4], _personas[4], false, "PLAYER5"),
            new Profile("Player6", InputProfile.Get("MPPlayer6"), Teams.core.teams[5], _personas[5], false, "PLAYER6"),
            new Profile("Player7", InputProfile.Get("MPPlayer7"), Teams.core.teams[6], _personas[6], false, "PLAYER7"),
            new Profile("Player8", InputProfile.Get("MPPlayer8"), Teams.core.teams[7], _personas[7], false, "PLAYER8")
        };

            FieldInfo _profilesField = typeof(ProfilesCore).GetField("_profiles", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            _profilesField.SetValue(profilecore, _profilesList);

            Profile.loading = true;
            string[] files = DuckFile.GetFiles(DuckFile.profileDirectory, "*.*");
            List<Profile> profileList1 = new List<Profile>();
            foreach (string path in files)
            {
                XDocument xdocument = DuckFile.LoadXDocument(path);
                if (xdocument != null)
                {
                    string name = xdocument.Element("Profile").Element("Name").Value;
                    bool flag = false;
                    Profile p = _profilesList.FirstOrDefault<Profile>(pro => pro.name == name);
                    if (p == null || !Profiles.IsDefault(p))
                    {
                        p = new Profile("", null, null, null, false, null);
                        p.fileName = path;
                        flag = true;
                    }
                    IEnumerable<XElement> source = xdocument.Elements("Profile");
                    if (source != null)
                    {
                        foreach (XElement element1 in source.Elements<XElement>())
                        {
                            if (element1.Name.LocalName == "ID" && !Profiles.IsDefault(p))
                                p.SetID(element1.Value);
                            else if (element1.Name.LocalName == "Name")
                                p.name = element1.Value;
                            else if (element1.Name.LocalName == "Mood")
                                p.funslider = Convert.ToSingle(element1.Value, CultureInfo.InvariantCulture);
                            else if (element1.Name.LocalName == "SteamID")
                            {
                                p.funslider = Convert.ToUInt64(element1.Value, CultureInfo.InvariantCulture);
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
                                p.unlocks = new List<string>(strArray);
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
                        _profilesList.Add(p);
                }
            }
            byte localFlippers = Profile.CalculateLocalFlippers();
            Profile p1 = null;
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
                    p1 = new Profile(Steam.user.id.ToString(), null, null, null, false, Steam.user.id.ToString());
                    p1.steamID = Steam.user.id;
                    Profiles.Add(p1);
                    profilecore.Save(p1);
                }
            }
            if (p1 != null)
            {
                _profilesList.Remove(p1);
                _profilesList.Insert(8, p1);
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
                        Profile p2 = source.FirstOrDefault<Profile>(x => (long)x.steamID == (long)pro.steamID);
                        if (p2 == null)
                        {
                            p2 = new Profile(pro.steamID.ToString(), null, null, null, false, pro.steamID.ToString());
                            p2.steamID = pro.steamID;
                            Profiles.Add(p2);
                        }
                        p2.stats = (ProfileStats)(p2.stats + pro.stats);
                        foreach (KeyValuePair<string, List<ChallengeSaveData>> keyValuePair in Challenges.saveData)
                        {
                            ChallengeSaveData challengeSaveData1 = null;
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
                        _profilesList.Remove(pro);
                        DuckFile.Delete(pro.fileName);
                        profilecore.Save(p2);
                    }
                }
            }
            foreach (Profile profile in _profilesList)
            {
                profile.flippers = localFlippers;
                profile.ticketCount = Challenges.GetTicketCount(profile);
                if (profile.ticketCount < 0)
                    profile.ticketCount = 0;
            }
            Profile.loading = false;
        }

    }
}