using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DuckGame.EightPlayerDuckGame
{
    class ConnectionStatusUIEdits
    {
        [HarmonyPatch(typeof(ConnectionStatusUI), "Draw")]
        [HarmonyPatch(new Type[] { })]
        static class ConnectionStatusUI_Draw_Transpiler
        {
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
            {
                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

                Label yellowDuck = il.DefineLabel();
                Label brownDuck = il.DefineLabel();
                Label purpleDuck = il.DefineLabel();
                Label greenDuck = il.DefineLabel();
                Label blueDuck = il.DefineLabel();
                Label redDuck = il.DefineLabel();
                Label exitLabel = il.DefineLabel();

                List<CodeInstruction> newCodes = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldloc_S, 4),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ConnectionStatusBar), "profile")),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Property(typeof(Profile), "networkIndex").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Bne_Un, yellowDuck),
                    new CodeInstruction(OpCodes.Ldstr, "|LIGHTGRAY|"),
                    new CodeInstruction(OpCodes.Stloc_S, 15),
                    new CodeInstruction(OpCodes.Br, exitLabel),

                    new CodeInstruction(OpCodes.Ldloc_S, 4) { labels = new List<Label>() { yellowDuck } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ConnectionStatusBar), "profile")),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Property(typeof(Profile), "networkIndex").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_I4_2, null),
                    new CodeInstruction(OpCodes.Bne_Un, brownDuck),
                    new CodeInstruction(OpCodes.Ldstr, "|DGYELLOW|"),
                    new CodeInstruction(OpCodes.Stloc_S, 15),
                    new CodeInstruction(OpCodes.Br, exitLabel),

                    new CodeInstruction(OpCodes.Ldloc_S, 4) { labels = new List<Label>() { brownDuck } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ConnectionStatusBar), "profile")),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Property(typeof(Profile), "networkIndex").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_I4_3, null),
                    new CodeInstruction(OpCodes.Bne_Un, purpleDuck),
                    new CodeInstruction(OpCodes.Ldstr, "|MENUORANGE|"),
                    new CodeInstruction(OpCodes.Stloc_S, 15),
                    new CodeInstruction(OpCodes.Br, exitLabel),

                    new CodeInstruction(OpCodes.Ldloc_S, 4) { labels = new List<Label>() { purpleDuck } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ConnectionStatusBar), "profile")),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Property(typeof(Profile), "networkIndex").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_I4_4, null),
                    new CodeInstruction(OpCodes.Bne_Un, greenDuck),
                    new CodeInstruction(OpCodes.Ldstr, "|DGPURPLE|"),
                    new CodeInstruction(OpCodes.Stloc_S, 15),
                    new CodeInstruction(OpCodes.Br, exitLabel),

                    new CodeInstruction(OpCodes.Ldloc_S, 4) { labels = new List<Label>() { greenDuck } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ConnectionStatusBar), "profile")),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Property(typeof(Profile), "networkIndex").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_I4_5, null),
                    new CodeInstruction(OpCodes.Bne_Un, blueDuck),
                    new CodeInstruction(OpCodes.Ldstr, "|DGGREEN|"),
                    new CodeInstruction(OpCodes.Stloc_S, 15),
                    new CodeInstruction(OpCodes.Br, exitLabel),

                    new CodeInstruction(OpCodes.Ldloc_S, 4) { labels = new List<Label>() { blueDuck } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ConnectionStatusBar), "profile")),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Property(typeof(Profile), "networkIndex").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_I4_6, null),
                    new CodeInstruction(OpCodes.Bne_Un, redDuck),
                    new CodeInstruction(OpCodes.Ldstr, "|DGBLUE|"),
                    new CodeInstruction(OpCodes.Stloc_S, 15),
                    new CodeInstruction(OpCodes.Br, exitLabel),

                    new CodeInstruction(OpCodes.Ldloc_S, 4) { labels = new List<Label>() { redDuck } },
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ConnectionStatusBar), "profile")),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Property(typeof(Profile), "networkIndex").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_I4_7, null),
                    new CodeInstruction(OpCodes.Bne_Un, exitLabel),
                    new CodeInstruction(OpCodes.Ldstr, "|DGRED|"),
                    new CodeInstruction(OpCodes.Stloc_S, 15),
                    new CodeInstruction(OpCodes.Nop, null) { labels = new List<Label>() { exitLabel } },
                };

                codes.RemoveRange(360, 23);
                codes.InsertRange(360, newCodes);

                return codes.AsEnumerable();
            }
        }
    }
}
