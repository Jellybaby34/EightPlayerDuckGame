using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DuckGame.EightPlayerDuckGame
{
    class RockScoreboardEdits
    {
        [HarmonyPatch(typeof(RockScoreboard), "Initialize")]
        static class RockScoreboard_Initialize_Transpiler
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

                codes[471].opcode = OpCodes.Ldc_I4_8;

                codes[489].operand = 13f;
                codes.RemoveRange(491, 17);

                return codes.AsEnumerable();
            }
        }
    }
}
