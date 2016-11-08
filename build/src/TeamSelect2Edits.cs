using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace DuckGame.IncreasedPlayerLimit
{
    public class TeamSelect2Edits
    {
        public void OnNetworkConnecting(Profile p)
        {
                    /*            Type typea = typeof(ProfileBox2);
            FieldInfo info2 = typea.GetField("_profiles", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            List<ProfileBox2> _profiles = info2.GetValue(null) as List<ProfileBox2>;
            ProfileBox2 profileBox2_1 = new ProfileBox2(1f, 1f, InputProfile.Get("MPPlayer1"), defaultProfile1, this, 0);
            _profiles.Add(profileBox2_1);
            Level.Add((Thing)profileBox2_1);
*/            //            List<ProfileBox2> _profiles = new List<ProfileBox2>();

            TeamSelect2 ayy = (TeamSelect2) Level.current;
            if (p.networkIndex > 4)
            {
                // 0 is player 1's box so we should put player 5 in there as well
                ayy._profiles[p.networkIndex - 5].PrepareDoor();
            }
            ayy._profiles[p.networkIndex].PrepareDoor();
        }

        public static void OnlineSettings()
        {
            List<MatchSetting> onlineSettings = new List<MatchSetting>()
            { new MatchSetting() { id = "maxplayers", name = "MAX PLAYERS", value = (object)8, min = 2, max = 8, step = 1 },
              new MatchSetting() { id = "teams", name = "TEAMS", value = (object)false },
              new MatchSetting() { id = "modifiers", name = "MODIFIERS", value = (object)false, filtered = true, filterOnly = true },
              new MatchSetting() { id = "type", name = "TYPE", value = (object)2, min = 0, max = 2, createOnly = true, valueStrings = new List<string>() { "PRIVATE", "FRIENDS", "PUBLIC" } }
            };

            Type typea = typeof(TeamSelect2);
            FieldInfo info2 = typea.GetField("onlineSettings", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            info2.SetValue(null, onlineSettings);
        }
    }
}