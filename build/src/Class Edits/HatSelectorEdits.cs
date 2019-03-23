using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DuckGame.EightPlayerDuckGame
{
    class HatSelectorEdits
    {
        [HarmonyPatch(typeof(HatSelector), "ControllerNumber")]
        public static class HatSelector_ControllerNumber_Prefix
        {
            [HarmonyPrefix]
            public static bool Prefix(HatSelector __instance, ref int __result)
            {
                if (Network.isActive)
                {
                    __result = Maths.Clamp(__instance.profileBoxNumber, 0, 7);
                    return false;
                }
                if (__instance.inputProfile.name == "MPPlayer1")
                {
                    __result = 0;
                    return false;
                }
                if (__instance.inputProfile.name == "MPPlayer2")
                {
                    __result = 1;
                    return false;
                }
                if (__instance.inputProfile.name == "MPPlayer3")
                {
                    __result = 2;
                    return false;
                }
                if (__instance.inputProfile.name == "MPPlayer4")
                {
                    __result = 3;
                    return false;
                }
                if (__instance.inputProfile.name == "MPPlayer5")
                {
                    __result = 4;
                    return false;
                }
                if (__instance.inputProfile.name == "MPPlayer6")
                {
                    __result = 5;
                    return false;
                }
                if (__instance.inputProfile.name == "MPPlayer7")
                {
                    __result = 6;
                    return false;
                }
                if (__instance.inputProfile.name == "MPPlayer8")
                {
                    __result = 7;
                    return false;
                }
                __result = 0;
                return false;
            }
        }

        [HarmonyPatch(typeof(HatSelector), "ConfirmTeamSelection")]
        public static class HatSelector_ConfirmTeamSelection_Transpiler
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count; i++)
                {
                    if (codes[i].opcode == OpCodes.Ldc_I4_3)
                    {
                        codes[i].opcode = OpCodes.Ldc_I4_7;
                        break;
                    }
                }
                return codes.AsEnumerable();
            }
        }

        [HarmonyPatch(typeof(HatSelector), "TeamIndexAdd")]
        public static class HatSelector_TeamIndexAdd_Transpiler
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
                    }

                    if (codes[i].opcode == OpCodes.Ldc_I4_3)
                    {
                        codes[i].opcode = OpCodes.Ldc_I4_7;
                    }
                }
                return codes.AsEnumerable();
            }
        }

        [HarmonyPatch(typeof(HatSelector), "Update")]
        public static class HatSelector_Update_Transpiler
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);

                codes[0] = new CodeInstruction(OpCodes.Ldc_I4_0, null);

                codes[352] = new CodeInstruction(OpCodes.Ldc_I4_8, null);
                codes[365] = new CodeInstruction(OpCodes.Ldc_I4_8, null);
                codes[413] = new CodeInstruction(OpCodes.Ldc_I4_8, null);
                codes[416] = new CodeInstruction(OpCodes.Ldc_I4_8, null);
                codes[445] = new CodeInstruction(OpCodes.Ldc_I4_8, null);
                codes[454] = new CodeInstruction(OpCodes.Ldc_I4_7, null);
                codes[475] = new CodeInstruction(OpCodes.Ldc_I4_7, null);
                codes[493] = new CodeInstruction(OpCodes.Ldc_I4_7, null);
                codes[516] = new CodeInstruction(OpCodes.Ldc_I4_8, null);
                codes[525] = new CodeInstruction(OpCodes.Ldc_I4_7, null);
                codes[541] = new CodeInstruction(OpCodes.Ldc_I4_7, null);
                codes[550] = new CodeInstruction(OpCodes.Ldc_I4_7, null);
                codes[568] = new CodeInstruction(OpCodes.Ldc_I4_7, null);
                codes[812] = new CodeInstruction(OpCodes.Ldc_I4_7, null);

                return codes.AsEnumerable();
            }
        }

        [HarmonyPatch(typeof(HatSelector), "Draw")]
        [HarmonyPatch(new Type[] { })]
        public static class HatSelector_Draw_Transpiler
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);

                codes[246].operand = -1f;
                codes[247].operand = 0f;

                codes[390].operand = 0.65f;
                codes[391].operand = 0.65f;
                codes[399].operand = 10f;
                codes[403].operand = 52f;

                codes[425].operand = 0.65f;
                codes[426].operand = 0.65f;
                codes[434].operand = 71f;
                codes[445].operand = 52f;

                codes.RemoveRange(285, 100);

                return codes.AsEnumerable();
            }
        }
    }
}
