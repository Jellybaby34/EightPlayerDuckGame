using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace DuckGame.EightPlayerDuckGame
{
    public class DuckEdits
    {

        // The _profileIndexBinding has 2 bits by default which can only store 0-3. We need at least 3 to store 0-7 for the extra players
        // We loop through the IL code until we find the one we want, change it then break out and return it for harmony to compile.
        [HarmonyPatch]
        static class Duck_Constructor_Patch
        {
            static MethodBase TargetMethod()
            {
                return AccessTools.Constructor( typeof(Duck) , new Type[] { typeof(float), typeof(float), typeof(Profile) } );
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Ldc_I4_2)
                    {
                        codes[i].opcode = OpCodes.Ldc_I4_3;
                        break;
                    }
                }
                return codes.AsEnumerable();
            }
        }

    }
}
