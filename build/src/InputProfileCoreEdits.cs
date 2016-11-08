using System;
using System.Reflection;
using System.Collections.Generic;

namespace DuckGame.IncreasedPlayerLimit
{
    public class InputProfileCoreEdits
    {
        public static void defaultProfiles()
        {
            Type inputtype = typeof(InputProfileCore);
            PropertyInfo _devicesProperty = inputtype.GetProperty("defaultProfiles", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo methodToReplace = _devicesProperty.GetGetMethod(true);
            MethodInfo methodToInject = typeof(InputProfileCoreEdits).GetMethod("defaultProfilesReplace", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            UnsafeCode.CodeInjection(methodToReplace,  methodToInject);
        }

        public List<InputProfile> defaultProfilesReplace()
        {
            return new List<InputProfile>() { InputProfile.core.Get("MPPlayer1"), InputProfile.core.Get("MPPlayer2"), InputProfile.core.Get("MPPlayer3"), InputProfile.core.Get("MPPlayer4"), InputProfile.core.Get("MPPlayer5"), InputProfile.core.Get("MPPlayer6"), InputProfile.core.Get("MPPlayer7"), InputProfile.core.Get("MPPlayer8") };
        }

    }
}