using Harmony;
using System.Collections.Generic;

namespace DuckGame.EightPlayerDuckGame
{
    public class InputProfileCoreEdits
    {
        // This property normally returns the first 4 profiles.
        // We extend it to return the 8 profiles we now have.
        [HarmonyPatch(typeof(InputProfileCore))]
        [HarmonyPatch("defaultProfiles", PropertyMethod.Getter)]
        public static class InputProfileCore_defaultProfiles_Prefix
        {
            [HarmonyPrefix]
            public static bool Prefix( InputProfileCore __instance, ref List<InputProfile> __result )
            {
                __result = new List<InputProfile>() {

                    __instance.Get("MPPlayer1"),
                    __instance.Get("MPPlayer2"),
                    __instance.Get("MPPlayer3"),
                    __instance.Get("MPPlayer4"),
                    __instance.Get("MPPlayer5"),
                    __instance.Get("MPPlayer6"),
                    __instance.Get("MPPlayer7"),
                    __instance.Get("MPPlayer8")

                };

                return false;
            }

        }
    }
}