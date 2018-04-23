using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DuckGame.EightPlayerDuckGame
{
    class ConsoleScreenEdits
    {
        [HarmonyPatch]
        static class ConsoleScreen_Constructor_Transpiler
        {
            static MethodBase TargetMethod()
            {
                return AccessTools.Constructor(typeof(ConsoleScreen), new Type[] { typeof(float), typeof(float), typeof(HatSelector) });
            }

            [HarmonyPriority(Harmony.Priority.First)]
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

                codes[29].operand = 330;
                codes[30].operand = 240;

                return codes.AsEnumerable();
            }
        }
    }
}
