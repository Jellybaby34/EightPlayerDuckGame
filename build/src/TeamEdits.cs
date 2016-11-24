using System;
using System.Collections.Generic;
using System.Reflection;
/*
namespace DuckGame.IncreasedPlayerLimit
{
    public class TeamEdits
    {
        public static SpriteMap hat()
        {
            Type inputtype = typeof(InputProfileCore);
            PropertyInfo _devicesProperty = inputtype.GetProperty("defaultProfiles", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo methodToReplace = _devicesProperty.GetGetMethod(true);
            MethodInfo methodToInject = typeof(InputProfileCoreEdits).GetMethod("defaultProfilesReplace", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            UnsafeCode.CodeInjection(methodToReplace, methodToInject);
        }

        public SpriteMap hatReplace()
        {
            if (Network.isActive)
            {
                int index = Teams.all.IndexOf(profile.team);
                if (index != -1 && index < 8)
                {
                    Profile profile = DuckNetwork.profiles[index];
                    Team team = (Team)null;
                    if (Teams.core._facadeMap.TryGetValue(profile.steamID, out team))
                        return team.hat;
                }
            }
            return this._hat;
        }
    }
}*/