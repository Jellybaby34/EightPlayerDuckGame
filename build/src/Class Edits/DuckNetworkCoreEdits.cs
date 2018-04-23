using System;
using System.Reflection;
using Harmony;
using System.Linq;

namespace DuckGame.EightPlayerDuckGame
{
    public class DuckNetworkCoreEdits
    {
        // This method normally ends the loop when index < 4, we want it to be index < 8 so the extra profiles we need are created
        [HarmonyPatch(typeof(DuckNetworkCore), "RecreateProfiles")]
        public static class DuckNetworkCore_RecreateProfiles_Prefix
        {
            public static bool Prefix(DuckNetworkCore __instance)
            {
                __instance.profiles.Clear();
                for (int index = 0; index < 8; ++index)
                    __instance.profiles.Add(new Profile("Netduck" + (index + 1).ToString(), InputProfile.GetVirtualInput(index), null, Persona.all.ElementAt(index), true, null)
                    {
                        networkIndex = (byte)index
                    });

                return false;
            }
        }
    }
}