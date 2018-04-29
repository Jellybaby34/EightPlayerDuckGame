using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DuckGame.EightPlayerDuckGame
{
    class UISlotEditorEdits
    {
        [HarmonyPatch(typeof(UISlotEditor), "Update")]
        public static class UISlotEditor_Update_Transpiler
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

                Label leftPressed = il.DefineLabel();
                Label notLeftPressed = il.DefineLabel();
                Label rightPressed = il.DefineLabel();
                Label notRightPressed = il.DefineLabel();
                Label upPressedA = il.DefineLabel();
                Label upPressedB = il.DefineLabel();
                Label upPressedC = il.DefineLabel();
                Label upPressedD = il.DefineLabel();
                Label notUpPressed = il.DefineLabel();
                Label downPressedA = il.DefineLabel();
                Label downPressedB = il.DefineLabel();
                Label downPressedC = il.DefineLabel();
                Label downPressedD = il.DefineLabel();
                Label notDownPressed = il.DefineLabel();

                List<CodeInstruction> newslotselectioncode = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldstr, "LEFT"),
                    new CodeInstruction(OpCodes.Ldstr, "Any"),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Input), "Pressed")),
                    new CodeInstruction(OpCodes.Brfalse, notLeftPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Beq_S, leftPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_2, null),
                    new CodeInstruction(OpCodes.Beq_S, leftPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_4, null),
                    new CodeInstruction(OpCodes.Beq_S, leftPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_6, null),
                    new CodeInstruction(OpCodes.Beq_S, leftPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_7, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, notLeftPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { leftPressed } },
                    new CodeInstruction(OpCodes.Dup, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Sub, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),

                    new CodeInstruction(OpCodes.Ldstr, "RIGHT") { labels = new List<Label>() { notLeftPressed } },
                    new CodeInstruction(OpCodes.Ldstr, "Any"),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Input), "Pressed")),
                    new CodeInstruction(OpCodes.Brfalse, notRightPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_0, null),
                    new CodeInstruction(OpCodes.Beq_S, rightPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Beq_S, rightPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_3, null),
                    new CodeInstruction(OpCodes.Beq_S, rightPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_5, null),
                    new CodeInstruction(OpCodes.Beq_S, rightPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_6, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, notRightPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { rightPressed } },
                    new CodeInstruction(OpCodes.Dup, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),

                    new CodeInstruction(OpCodes.Ldstr, "UP") { labels = new List<Label>() { notRightPressed } },
                    new CodeInstruction(OpCodes.Ldstr, "Any"),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Input), "Pressed")),
                    new CodeInstruction(OpCodes.Brfalse, notUpPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_3, null),
                    new CodeInstruction(OpCodes.Blt_S, notUpPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_3, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, upPressedA),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Dup, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_3, null),
                    new CodeInstruction(OpCodes.Sub, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Br, notUpPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>(){ upPressedA } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_4, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, upPressedB),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Dup, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_2, null),
                    new CodeInstruction(OpCodes.Sub, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Br, notUpPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>(){ upPressedB } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_5, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, upPressedC),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Dup, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_2, null),
                    new CodeInstruction(OpCodes.Sub, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Br, notUpPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>(){ upPressedC } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_6, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, upPressedD),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Dup, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_5, null),
                    new CodeInstruction(OpCodes.Sub, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Br, notUpPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>(){ upPressedD } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_7, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, notUpPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Dup, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_3, null),
                    new CodeInstruction(OpCodes.Sub, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),

                    new CodeInstruction(OpCodes.Ldstr, "DOWN") { labels = new List<Label>() { notUpPressed } },
                    new CodeInstruction(OpCodes.Ldstr, "Any"),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Input), "Pressed")),
                    new CodeInstruction(OpCodes.Brfalse, notDownPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_5, null),
                    new CodeInstruction(OpCodes.Bge_S, notDownPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_0, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, downPressedA),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Dup, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_3, null),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Br, notDownPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>(){ downPressedA } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, downPressedB),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Dup, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_5, null),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Br, notDownPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>(){ downPressedB } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_2, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, downPressedC),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Dup, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_2, null),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Br, notDownPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>(){ downPressedC } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_3, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, downPressedD),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Dup, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_2, null),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Br, notDownPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>(){ downPressedD } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_4, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, notDownPressed),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Dup, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_3, null),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Nop, null) {labels = new List<Label>() { notDownPressed } }
                };

                Label fixJump = il.DefineLabel();
                codes[131].operand = fixJump;

                Label isSlot2 = il.DefineLabel();
                Label isSlot3 = il.DefineLabel();
                Label isSlot4 = il.DefineLabel();
                Label isSlot5 = il.DefineLabel();
                Label isSlot6 = il.DefineLabel();
                Label isSlot7 = il.DefineLabel();
                Label isSlot8 = il.DefineLabel();

                Label exitSlotHighlight = il.DefineLabel();

                List<CodeInstruction> newslothighlightcode = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { fixJump } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Brtrue_S, isSlot2),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldc_R4, 0f),
                    new CodeInstruction(OpCodes.Ldc_R4, 0f),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Vec2), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_rectPosition")),
                    new CodeInstruction(OpCodes.Br, exitSlotHighlight),

                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { isSlot2 } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, isSlot3),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldc_R4, 119f),
                    new CodeInstruction(OpCodes.Ldc_R4, 0f),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Vec2), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_rectPosition")),
                    new CodeInstruction(OpCodes.Br, exitSlotHighlight),

                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { isSlot3 } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_2, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, isSlot4),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldc_R4, 238f),
                    new CodeInstruction(OpCodes.Ldc_R4, 0f),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Vec2), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_rectPosition")),
                    new CodeInstruction(OpCodes.Br, exitSlotHighlight),

                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { isSlot4 } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_3, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, isSlot5),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldc_R4, 0f),
                    new CodeInstruction(OpCodes.Ldc_R4, 61f),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Vec2), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_rectPosition")),
                    new CodeInstruction(OpCodes.Br, exitSlotHighlight),

                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { isSlot5 } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_4, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, isSlot6),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldc_R4, 238f),
                    new CodeInstruction(OpCodes.Ldc_R4, 61f),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Vec2), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_rectPosition")),
                    new CodeInstruction(OpCodes.Br, exitSlotHighlight),

                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { isSlot6 } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_5, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, isSlot7),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldc_R4, 0f),
                    new CodeInstruction(OpCodes.Ldc_R4, 120f),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Vec2), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_rectPosition")),
                    new CodeInstruction(OpCodes.Br, exitSlotHighlight),

                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { isSlot7 } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_6, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, isSlot8),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldc_R4, 119f),
                    new CodeInstruction(OpCodes.Ldc_R4, 120f),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Vec2), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_rectPosition")),
                    new CodeInstruction(OpCodes.Br, exitSlotHighlight),

                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { isSlot8 } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(UISlotEditor), "_slot")),
                    new CodeInstruction(OpCodes.Ldc_I4_7, null),
                    new CodeInstruction(OpCodes.Bne_Un_S, exitSlotHighlight),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldc_R4, 238f),
                    new CodeInstruction(OpCodes.Ldc_R4, 120f),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Vec2), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(UISlotEditor), "_rectPosition")),
                    new CodeInstruction(OpCodes.Nop, null) {labels = new List<Label>() {  exitSlotHighlight } },
                };

                codes.RemoveRange(135, 38);
                codes.InsertRange(135, newslothighlightcode);
                codes.RemoveRange(55, 70);
                codes.InsertRange(55, newslotselectioncode);

                return codes.AsEnumerable();
            }
        }

        [HarmonyPatch(typeof(UISlotEditor), "Draw")]
        [HarmonyPatch(new Type[] { })]
        public static class UISlotEditor_Draw_Transpiler
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);

                codes[19].operand = 82f;
                codes[20].operand = 59f;
                codes[32].operand = 300f;
                codes[41].operand = 300f;
                codes[56].operand = 82f;
                codes[65].operand = 300f;
                codes[82].operand = 59f;
                codes[87].operand = 82f;
                codes[109].operand = 82f;

                return codes.AsEnumerable();
            }
        }
    }
}
