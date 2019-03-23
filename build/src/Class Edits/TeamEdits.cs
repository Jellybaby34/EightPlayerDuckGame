using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace DuckGame.EightPlayerDuckGame
{
    public class TeamEdits
    {      
        [HarmonyPatch(typeof(Team))]
        [HarmonyPatch("hat", MethodType.Getter)]
        public static class Team_Hat_Transpiler
        {
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
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
        
        [HarmonyPatch(typeof(Team))]
        [HarmonyPatch("capeTexture", MethodType.Getter)]
        public static class Team_CapeTexture_Transpiler
        {
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
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
