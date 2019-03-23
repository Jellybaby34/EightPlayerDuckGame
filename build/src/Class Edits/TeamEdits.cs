using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace DuckGame.EightPlayerDuckGame
{
    class TeamEdits
    {
        [HarmonyPatch(typeof(Team))]
        [HarmonyPatch("hat", MethodType.Getter)]
        static class Team_Hat_Transpiler
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

                codes[0x18].opcode = OpCodes.Ldc_I4_8;

                return codes.AsEnumerable();
            }
        }

        [HarmonyPatch(typeof(Team))]
        [HarmonyPatch("capeTexture", MethodType.Getter)]
        static class Team_CapeTexture_Transpiler
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

                codes[0x18].opcode = OpCodes.Ldc_I4_8;

                return codes.AsEnumerable();
            }
        }
    }
}
