using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace DuckGame.IncreasedPlayerLimit
{
    public class TeamSelect2Edits
    {
        public static void OnNetworkConnecting(Profile p)
        {
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