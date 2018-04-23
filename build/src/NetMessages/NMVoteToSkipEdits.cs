using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace DuckGame.EightPlayerDuckGame
{
    class NMVoteToSkipEdits
    {
        // When a vote to skip packet is received, it checks whether the player slot is between 0 and 4.
        // Our higher slots get their packets dropped so lets make it check slots up to 8.
        [HarmonyPatch(typeof(NMVoteToSkip), "Activate")]
        public static class NMVoteToSkip_Activate_Transpiler
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Ldc_I4_4)
                    {
                        codes[i].opcode = OpCodes.Ldc_I4_8;
                        break;
                    }
                }
                return codes.AsEnumerable();
            }
        }

    }
}
