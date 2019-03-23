using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DuckGame.EightPlayerDuckGame
{
    public class ProfileBox2Edits
    {
        [HarmonyPatch(typeof(ProfileBox2), "Spawn")]
        static class ProfileBox2_Spawn_Prefix
        {
            static FieldInfo controllerIndexField = AccessTools.Field(typeof(ProfileBox2), "_controllerIndex");
            static FieldInfo teamSelectionField = AccessTools.Field(typeof(ProfileBox2), "_teamSelection");
            static FieldInfo gunSpawnPointField = AccessTools.Field(typeof(ProfileBox2), "_gunSpawnPoint");
            static FieldInfo windowField = AccessTools.Field(typeof(ProfileBox2), "_window");

            private static bool Prefix(ProfileBox2 __instance)
            {
                int indexvalue = (int)controllerIndexField.GetValue(__instance);

                if (__instance.duck != null)
                {
                    teamSelectionField.SetValue(__instance, indexvalue);
                    Teams.all[indexvalue].Join(__instance.profile, true);
                    __instance.ReturnControl();
                    return false;
                }

                Vec2 gunSpawn = (Vec2)gunSpawnPointField.GetValue(__instance);
                __instance.gun = new VirtualShotgun(gunSpawn.x, gunSpawn.y);
                __instance.gun.roomIndex = (byte)indexvalue;
                Level.Add(__instance.gun);

                if (indexvalue == 2 || indexvalue  == 4 || indexvalue == 7)
                {
                    __instance.duck = new Duck(__instance.x + 46f, __instance.y + 30f, __instance.profile);
                    Window window = new Window(__instance.x + 2f, __instance.y + 34f);
                    window.noframe = true;
                    Level.Add(window);
                    windowField.SetValue(__instance, window);
                }
                // NEED WORKAROUND FOR TWO WINDOWS
                else if (indexvalue == 1 || indexvalue == 6)
                {
                    __instance.duck = new Duck(__instance.x + 46f, __instance.y + 30f, __instance.profile);
                    Window window = new Window(__instance.x + 2f, __instance.y + 34f);
                    window.noframe = true;
                    Level.Add(window);
                    windowField.SetValue(__instance, window);
                    window = new Window(__instance.x + 78f, __instance.y + 34f);
                    window.noframe = true;
                    Level.Add(window);
                    windowField.SetValue(__instance, window);
                }
                else
                {
                    __instance.duck = new Duck(__instance.x + 46f, __instance.y + 30f, __instance.profile);
                    Window window = new Window(__instance.x + 78f, __instance.y + 34f);
                    window.noframe = true;
                    Level.Add(window);
                    windowField.SetValue(__instance, window);
                }

                Level.Add(__instance.duck);
                if (__instance.duck != null && __instance.duck.HasEquipment(typeof(TeamHat)))
                {
                    __instance._hatSelector.hat = (__instance.duck.GetEquipment(typeof(TeamHat)) as TeamHat);
                }

                return false;
            }
        }

        // Literally editing the method byte by byte. Kill me
        [HarmonyPatch]
        static class ProfileBox2_Constructor_Patch
        {
            static MethodBase TargetMethod()
            {
                return AccessTools.Constructor(typeof(ProfileBox2), new Type[] { typeof(float), typeof(float), typeof(InputProfile), typeof(Profile), typeof(TeamSelect2), typeof(int) });
            }

            [HarmonyPriority(Harmony.Priority.First)]
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
            {


                List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

                // ldstr "selectDoorLeftPC"
                codes[96].operand = (string)Mod.GetPath<EightPlayerDuckGame>("selectDoorLeft");
                // ldstr "selectDoorRight"
                codes[107].operand = (string)Mod.GetPath<EightPlayerDuckGame>("selectDoorRight");
                // ldstr "selectDoorLeftBlank"
                codes[118].operand = (string)Mod.GetPath<EightPlayerDuckGame>("selectDoorLeftBlank");
                // ldstr "selectDoorRightBlank"
                codes[129].operand = (string)Mod.GetPath<EightPlayerDuckGame>("selectDoorRightBlank");
                // ldstr "doorSpinner"
                codes[140].operand = (string)Mod.GetPath<EightPlayerDuckGame>("doorSpinner");
                // ldc.i4.s 25
                codes[141].operand = 17;
                // ldc.i4.s 25
                codes[142].operand = 16;
                // ldstr "doorSpinner"
                codes[162].operand = (string)Mod.GetPath<EightPlayerDuckGame>("doorSpinner");
                // ldc.i4.s 25
                codes[163].operand = 17;
                // ldc.i4.s 25
                codes[164].operand = 16;

                // Generate all the labels we'll use               
                Label isLeftSideRoom = il.DefineLabel();
                Label isNotLeft = il.DefineLabel();
                Label isMiddleRoom = il.DefineLabel();
                Label isNotMiddle = il.DefineLabel();
                Label exitRoomInit = il.DefineLabel();
                
                // Build our new chunk of code to insert into the method
                List<CodeInstruction> newRoomInit = new List<CodeInstruction>{
                    new CodeInstruction(OpCodes.Ldarg_S, 6), // Load the "index" argument onto the stack
                    new CodeInstruction(OpCodes.Brfalse_S, isLeftSideRoom), // If index == 0, jump to isLeftSideRoom
                    new CodeInstruction(OpCodes.Ldarg_S, 6), // Load the "index" argument onto the stack
                    new CodeInstruction(OpCodes.Ldc_I4_3, null), // Load 3 onto the stack
                    new CodeInstruction(OpCodes.Beq_S, isLeftSideRoom), // If index == 3, jump to isLeftSideRoom
                    new CodeInstruction(OpCodes.Ldarg_S, 6), // Load the "index" argument onto the stack
                    new CodeInstruction(OpCodes.Ldc_I4_5, null), // Load 5 onto the stack
                    new CodeInstruction(OpCodes.Bne_Un, isNotLeft), // If index != 5, jump to isNotLeft

                    // Set 1
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { isLeftSideRoom } }, // load 'this' onto the stack, Also start of isLeftSideRoom
                    new CodeInstruction(OpCodes.Ldstr, Mod.GetPath<EightPlayerDuckGame>("leftRoomBackground")), // Set leftRoomBackground
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Sprite), new Type[] { typeof(string), typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_roomLeftBackground")),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldstr, Mod.GetPath<EightPlayerDuckGame>("leftRoomForeground")), // Set leftRoomForeground
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Sprite), new Type[] { typeof(string), typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_roomLeftForeground")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the first invisible block
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)2),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)47),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)64),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)10),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(InvisibleBlock), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(PhysicsMaterial) })),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the second invisible block
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)3),
                    new CodeInstruction(OpCodes.Sub, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)80),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)4),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(InvisibleBlock), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(PhysicsMaterial) })),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the third invisible block
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)66),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)44),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)14),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)15),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(InvisibleBlock), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(PhysicsMaterial) })),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the fourth invisible block
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)3),
                    new CodeInstruction(OpCodes.Sub, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)4),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)60),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(InvisibleBlock), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(PhysicsMaterial) })),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the platform for the hat selector
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)11),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)45),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(ScaffoldingTileset), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stloc_0, null),
                    new CodeInstruction(OpCodes.Ldloc_0, null),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ScaffoldingTileset), "neverCheap")),
                    new CodeInstruction(OpCodes.Ldloc_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldloc_0, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)-0.5),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Depth), "op_Implicit")),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Property(typeof(Transform), "depth").GetSetMethod()),
                    new CodeInstruction(OpCodes.Ldloc_0, null),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(AutoPlatform), "PlaceBlock")),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldstr, Mod.GetPath<EightPlayerDuckGame>("readyRight")),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Sprite), new Type[] { typeof(string), typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_readySign")),
                    // Set _gunSpawnPoint
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)42),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)46),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Vec2), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_gunSpawnPoint")),
                    new CodeInstruction(OpCodes.Br, exitRoomInit),

                    new CodeInstruction(OpCodes.Ldarg_S, 6) {labels = new List<Label>() { isNotLeft } },
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Beq_S, isMiddleRoom),
                    new CodeInstruction(OpCodes.Ldarg_S, 6),
                    new CodeInstruction(OpCodes.Ldc_I4_6, null),
                    new CodeInstruction(OpCodes.Bne_Un, isNotMiddle),
                    // Set 2
                    new CodeInstruction(OpCodes.Ldarg_0, null) {labels = new List<Label>() { isMiddleRoom } },
                    new CodeInstruction(OpCodes.Ldstr, Mod.GetPath<EightPlayerDuckGame>("middleRoomBackground")),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Sprite), new Type[] { typeof(string), typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_roomLeftBackground")),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldstr, Mod.GetPath<EightPlayerDuckGame>("middleRoomForeground")),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Sprite), new Type[] { typeof(string), typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_roomLeftForeground")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the first invisible block
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)2),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)47),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)64),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)10),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(InvisibleBlock), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(PhysicsMaterial) })),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the second invisible block
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)3),
                    new CodeInstruction(OpCodes.Sub, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)80),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)4),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(InvisibleBlock), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(PhysicsMaterial) })),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the third invisible block
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)66),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)42),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)15),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)15),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(InvisibleBlock), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(PhysicsMaterial) })),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the fourth invisible block
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)1),
                    new CodeInstruction(OpCodes.Sub, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)42),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)15),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)15),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(InvisibleBlock), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(PhysicsMaterial) })),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the platform for the hat selector
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)56),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)45),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(ScaffoldingTileset), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stloc_0, null),
                    new CodeInstruction(OpCodes.Ldloc_0, null),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ScaffoldingTileset), "neverCheap")),
                    new CodeInstruction(OpCodes.Ldloc_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldloc_0, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)(float)-0.5),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Depth), "op_Implicit")),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Property(typeof(Transform), "depth").GetSetMethod()),
                    new CodeInstruction(OpCodes.Ldloc_0, null),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(AutoPlatform), "PlaceBlock")),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldstr, Mod.GetPath<EightPlayerDuckGame>("readyLeft")),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Sprite), new Type[] { typeof(string), typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_readySign")),
                    // Set _gunSpawnPoint
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)40),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)46),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Vec2), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_gunSpawnPoint")),
                    new CodeInstruction(OpCodes.Br, exitRoomInit),

                    // Set 3
                    new CodeInstruction(OpCodes.Ldarg_0, null) {labels = new List<Label>() { isNotMiddle } },
                    new CodeInstruction(OpCodes.Ldstr, Mod.GetPath<EightPlayerDuckGame>("rightRoomBackground")),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Sprite), new Type[] { typeof(string), typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_roomLeftBackground")),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldstr, Mod.GetPath<EightPlayerDuckGame>("rightRoomForeground")),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Sprite), new Type[] { typeof(string), typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_roomLeftForeground")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the first invisible block
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, 2f),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)47),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)77),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)8),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(InvisibleBlock), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(PhysicsMaterial) })),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the second invisible block
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)3),
                    new CodeInstruction(OpCodes.Sub, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)77),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)4),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(InvisibleBlock), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(PhysicsMaterial) })),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the third invisible block
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)42),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)14),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)15),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(InvisibleBlock), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(PhysicsMaterial) })),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the fourth invisible block
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)77),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)3),
                    new CodeInstruction(OpCodes.Sub, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)4),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)60),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(InvisibleBlock), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(PhysicsMaterial) })),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldarg_0, null), // Create the platform for the hat selector
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)68),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)45),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(ScaffoldingTileset), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stloc_0, null),
                    new CodeInstruction(OpCodes.Ldloc_0, null),
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ScaffoldingTileset), "neverCheap")),
                    new CodeInstruction(OpCodes.Ldloc_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Level), "Add")),
                    new CodeInstruction(OpCodes.Ldloc_0, null),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)-0.5),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Depth), "op_Implicit")),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Property(typeof(Transform), "depth").GetSetMethod()),
                    new CodeInstruction(OpCodes.Ldloc_0, null),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(AutoPlatform), "PlaceBlock")),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldstr, Mod.GetPath<EightPlayerDuckGame>("readyLeft")),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)0),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Sprite), new Type[] { typeof(string), typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_readySign")),
                    // Set _gunSpawnPoint
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)38),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)46),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Vec2), new Type[] { typeof(float), typeof(float) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_gunSpawnPoint")),
                    new CodeInstruction(OpCodes.Nop, null) { labels = new List<Label>() { exitRoomInit } }
                };

                // This is to fix a jump that gets broken by our injected code.
                Label fixJump = il.DefineLabel();
                codes[537] = new CodeInstruction(OpCodes.Brfalse_S, fixJump);

                Label isLeftProj = il.DefineLabel();
                Label isNotLeftProj = il.DefineLabel();
                Label isMiddleProj = il.DefineLabel();
                Label isNotMiddleProj = il.DefineLabel();
                Label exitRoomProj = il.DefineLabel();

                List<CodeInstruction> newRoomProj = new List<CodeInstruction>
                {
                    new CodeInstruction(OpCodes.Ldarg_S, 6) { labels = new List<Label>() { fixJump } }, // Load the "index" argument onto the stack
                    new CodeInstruction(OpCodes.Brfalse_S, isLeftProj), // If index == 0, jump to isLeftSideRoom
                    new CodeInstruction(OpCodes.Ldarg_S, 6), // Load the "index" argument onto the stack
                    new CodeInstruction(OpCodes.Ldc_I4_3, null), // Load 3 onto the stack
                    new CodeInstruction(OpCodes.Beq_S, isLeftProj), // If index == 3, jump to isLeftSideRoom
                    new CodeInstruction(OpCodes.Ldarg_S, 6), // Load the "index" argument onto the stack
                    new CodeInstruction(OpCodes.Ldc_I4_5, null), // Load 5 onto the stack
                    new CodeInstruction(OpCodes.Bne_Un, isNotLeftProj), // If index != 5, jump to isNotLeft

                    // Set 1, IsLeftProj
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { isLeftProj } },
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)42),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)46),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ProfileBox2), "_playerProfile")),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(TeamProjector), new Type[] { typeof(float), typeof(float), typeof(Profile) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_projector")),
                    new CodeInstruction(OpCodes.Br, exitRoomProj),

                    new CodeInstruction(OpCodes.Ldarg_S, 6) { labels = new List<Label>() { isNotLeftProj } },
                    new CodeInstruction(OpCodes.Ldc_I4_1, null),
                    new CodeInstruction(OpCodes.Beq_S, isMiddleProj),
                    new CodeInstruction(OpCodes.Ldarg_S, 6),
                    new CodeInstruction(OpCodes.Ldc_I4_4, null),
                    new CodeInstruction(OpCodes.Beq_S, isMiddleProj),
                    new CodeInstruction(OpCodes.Ldarg_S, 6),
                    new CodeInstruction(OpCodes.Ldc_I4_6, null),
                    new CodeInstruction(OpCodes.Bne_Un, isNotMiddleProj),

                    // Set 2, IsMiddleProj
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { isMiddleProj } },
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)40),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)46),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ProfileBox2), "_playerProfile")),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(TeamProjector), new Type[] { typeof(float), typeof(float), typeof(Profile) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_projector")),
                    new CodeInstruction(OpCodes.Br, exitRoomProj),
                    
                    // Set 3
                    new CodeInstruction(OpCodes.Ldarg_0, null) { labels = new List<Label>() { isNotMiddleProj } },
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "x").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)38),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Transform), "y").GetGetMethod()),
                    new CodeInstruction(OpCodes.Ldc_R4, (float)46),
                    new CodeInstruction(OpCodes.Add, null),
                    new CodeInstruction(OpCodes.Ldarg_0, null),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ProfileBox2), "_playerProfile")),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(TeamProjector), new Type[] { typeof(float), typeof(float), typeof(Profile) })),
                    new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ProfileBox2), "_projector")),
                    new CodeInstruction(OpCodes.Nop, null) { labels = new List<Label>() { exitRoomProj } }

                };

                codes.RemoveRange(556, 56);
                codes.InsertRange(556, newRoomProj);
                codes.RemoveRange(174, 291);
                codes.InsertRange(174, newRoomInit);

                return codes.AsEnumerable();
            }
        }

        // Custom draw method to align our new sprites we use
        [HarmonyPatch(typeof(ProfileBox2), "Draw")]
        [HarmonyPatch(new Type[] { })]
        public static class ProfileBox2_Draw_Patch
        {
            static FieldInfo projectorField = AccessTools.Field(typeof(ProfileBox2), "_projector");
            static FieldInfo doorLeftField = AccessTools.Field(typeof(ProfileBox2), "_doorLeft");
            static FieldInfo doorRightField = AccessTools.Field(typeof(ProfileBox2), "_doorRight");
            static FieldInfo doorLeftBlankField = AccessTools.Field(typeof(ProfileBox2), "_doorLeftBlank");
            static FieldInfo doorRightBlankField = AccessTools.Field(typeof(ProfileBox2), "_doorRightBlank");
            static FieldInfo fontSmallField = AccessTools.Field(typeof(ProfileBox2), "_fontSmall");
            static FieldInfo doorIconField = AccessTools.Field(typeof(ProfileBox2), "_doorIcon");
            static FieldInfo doorSpinnerField = AccessTools.Field(typeof(ProfileBox2), "_doorSpinner");
            static FieldInfo roomLeftBackgroundField = AccessTools.Field(typeof(ProfileBox2), "_roomLeftBackground");
            static FieldInfo roomLeftForegroundField = AccessTools.Field(typeof(ProfileBox2), "_roomLeftForeground");
            static FieldInfo hostCrownField = AccessTools.Field(typeof(ProfileBox2), "_hostCrown");
            static FieldInfo tutorialTVField = AccessTools.Field(typeof(ProfileBox2), "_tutorialTV");
            static FieldInfo tutorialMessagesField = AccessTools.Field(typeof(ProfileBox2), "_tutorialMessages");
            static FieldInfo fontField = AccessTools.Field(typeof(ProfileBox2), "_font");
            static FieldInfo screenFadeField = AccessTools.Field(typeof(ProfileBox2), "_screenFade");
            static FieldInfo selectConsoleField = AccessTools.Field(typeof(ProfileBox2), "_selectConsole");
            static FieldInfo consoleHighlightField = AccessTools.Field(typeof(ProfileBox2), "_consoleHighlight");
            static FieldInfo consoleFlashField = AccessTools.Field(typeof(ProfileBox2), "_consoleFlash");
            static FieldInfo consoleFadeField = AccessTools.Field(typeof(ProfileBox2), "_consoleFade");
            static FieldInfo readySignField = AccessTools.Field(typeof(ProfileBox2), "_readySign");

            private static bool Prefix(ProfileBox2 __instance)
            {
                TeamProjector projector = projectorField.GetValue(__instance) as TeamProjector;

                if (__instance._hatSelector != null && __instance._hatSelector.fadeVal > 0.9f && __instance._hatSelector._roomEditor._mode != REMode.Place)
                {
                    projector.visible = false;
                    projectorField.SetValue(__instance, projector);
                    if (__instance.duck != null)
                    {
                        __instance.duck.mindControl = new InputProfile("");
                    }
                    return false;
                }
                if (__instance.duck != null)
                {
                    __instance.duck.mindControl = null;
                }
                projector.visible = true;
                projectorField.SetValue(__instance, projector);

                if (__instance._tooManyPulse > 0.01f)
                {
                    Graphics.DrawStringOutline("ROOM FULL", __instance.position + new Vec2(0f, 36f), Color.Red * __instance._tooManyPulse, Color.Black * __instance._tooManyPulse, 0.95f, null, 2f);
                }
                if (__instance._noMorePulse > 0.01f)
                {
                    Graphics.DrawStringOutline(" NO MORE ", __instance.position + new Vec2(0f, 36f), Color.Red * __instance._noMorePulse, Color.Black * __instance._noMorePulse, 0.95f, null, 2f);
                }
                __instance._tooManyPulse = Lerp.Float(__instance._tooManyPulse, 0f, 0.05f);
                __instance._noMorePulse = Lerp.Float(__instance._noMorePulse, 0f, 0.05f);

                int index = __instance.controllerIndex;

                if (__instance._doorX < 82f)
                {
                    Sprite doorLeftBlank = doorLeftField.GetValue(__instance) as Sprite;
                    Sprite doorRightBlank = doorRightField.GetValue(__instance) as Sprite;
                    bool flag = __instance.profile.slotType == SlotType.Closed;
                    bool flag2 = __instance.profile.slotType == SlotType.Friend;
                    bool flag3 = __instance.profile.slotType == SlotType.Invite;
                    bool flag4 = __instance.profile.slotType == SlotType.Reserved;
                    bool flag5 = __instance.profile.slotType == SlotType.Local;
                    bool flag6 = __instance.profile.networkStatus != DuckNetStatus.Disconnected;
                    if (Network.isActive)
                    {
                        doorLeftBlank = doorLeftBlankField.GetValue(__instance) as Sprite;
                        doorRightBlank = doorRightBlankField.GetValue(__instance) as Sprite;
                    }
                    else
                    {
                        flag = false;
                        flag2 = false;
                        flag3 = false;
                        flag4 = false;
                        flag6 = false;
                    }
                    doorLeftBlank = doorLeftBlankField.GetValue(__instance) as Sprite;
                    doorRightBlank = doorRightBlankField.GetValue(__instance) as Sprite;

                    BitmapFont fontSmall = fontSmallField.GetValue(__instance) as BitmapFont;
                    SpriteMap doorIcon = doorIconField.GetValue(__instance) as SpriteMap;
                    SpriteMap doorSpinner = doorSpinnerField.GetValue(__instance) as SpriteMap;

                    if (index == 2 || index == 4 || index == 7)
                    {
                        Rectangle sourceRectangle = new Rectangle(__instance._doorX, 0f, (doorLeftField.GetValue(__instance) as Sprite).width, (doorLeftField.GetValue(__instance) as Sprite).height);
                        Graphics.Draw(doorLeftBlank, __instance.x - 1f, __instance.y, sourceRectangle);
                        Rectangle sourceRectangle2 = new Rectangle(-__instance._doorX, 0f, (doorRightField.GetValue(__instance) as Sprite).width, (doorRightField.GetValue(__instance) as Sprite).height);
                        Graphics.Draw(doorRightBlank, __instance.x + 38f, __instance.y, sourceRectangle2);

                        if (__instance._doorX == 0f)
                        {
                            fontSmall.depth = doorLeftBlank.depth + 10;
                            fontSmall.scale = new Vec2(0.75f, 0.75f);
                            if (!Network.isActive || (flag5 && Network.isServer))
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 10;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("PRESS", new Vec2(__instance.x + 4f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("START", new Vec2(__instance.x + 50f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 8;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                            }
                            else if (flag6)
                            {
                                doorSpinner.depth = doorLeftBlank.depth + 10;
                                Graphics.Draw(doorSpinner, __instance.x + 31f, __instance.y + 20f);
                            }
                            else if (flag2)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 11;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("PALS", new Vec2(__instance.x + 4f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("ONLY", new Vec2(__instance.x + 50f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag3)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 12;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("VIPS", new Vec2(__instance.x + 4f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("ONLY", new Vec2(__instance.x + 50f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag4 && __instance.profile.reservedUser != null)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 12;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                float num = 120f;
                                float x = __instance.x + 10f;
                                Graphics.DrawRect(new Vec2(x, __instance.y + 35f), new Vec2(x + num, __instance.y + 52f), Color.Black, doorLeftBlank.depth + 20, true, 1f);
                                string text2 = "WAITING FOR";
                                fontSmall.Draw(text2, new Vec2(x + num / 2f - fontSmall.GetWidth(text2, false, null) / 2f, __instance.y + 36f), Color.White, doorLeftBlank.depth + 30, null, false);
                                text2 = __instance.profile.name;
                                if (text2.Length > 16)
                                {
                                    text2 = text2.Substring(0, 16);
                                }
                                fontSmall.Draw(text2, new Vec2(x + num / 2f - fontSmall.GetWidth(text2, false, null) / 2f, __instance.y + 44f), Color.White, doorLeftBlank.depth + 30, null, false);
                            }
                            else if (flag5)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 13;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("HOST", new Vec2(__instance.x + 4f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("SLOT", new Vec2(__instance.x + 50f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                            else
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 9;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("OPEN", new Vec2(__instance.x + 4f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("SLOT", new Vec2(__instance.x + 50f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                        }
                    }
                    else if (index == 1 || index == 6)
                    {
                        Rectangle sourceRectangle5 = new Rectangle(__instance._doorX, 0.0f, (doorLeftField.GetValue(__instance) as Sprite).width, (doorLeftField.GetValue(__instance) as Sprite).height);
                        Graphics.Draw(doorLeftBlank, __instance.x - 1f, __instance.y, sourceRectangle5);
                        Rectangle sourceRectangle6 = new Rectangle(-__instance._doorX, 0.0f, (doorRightField.GetValue(__instance) as Sprite).width, (doorRightField.GetValue(__instance) as Sprite).height);
                        Graphics.Draw(doorRightBlank, __instance.x + 38f, __instance.y, sourceRectangle6);
                        if (__instance._doorX == 0.0)
                        {
                            fontSmall.depth = doorLeftBlank.depth + 10;
                            fontSmall.scale = new Vec2(0.75f, 0.75f);
                            if (!Network.isActive || flag5 && Network.isServer)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 10;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("PRESS", new Vec2(__instance.x + 4f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("START", new Vec2(__instance.x + 50f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 8;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                            }
                            else if (flag6)
                            {
                                doorSpinner.depth = doorLeftBlank.depth + 10;
                                Graphics.Draw(doorSpinner, __instance.x + 31f, __instance.y + 20f);
                            }
                            else if (flag2)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 11;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("PALS", new Vec2(__instance.x + 6f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("ONLY", new Vec2(__instance.x + 52f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag3)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 12;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("VIPS", new Vec2(__instance.x + 6f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("ONLY", new Vec2(__instance.x + 52f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag4 && __instance.profile.reservedUser != null)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 12;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                float num = 120f;
                                float x = __instance.x + 10f;
                                Graphics.DrawRect(new Vec2(x, __instance.y + 35f), new Vec2(x + num, __instance.y + 52f), Color.Black, doorLeftBlank.depth + 20, true, 1f);
                                string text1 = "WAITING FOR";
                                fontSmall.Draw(text1, new Vec2((float)(x + num / 2.0 - fontSmall.GetWidth(text1, false, (InputProfile)null) / 2.0), __instance.y + 36f), Color.White, doorLeftBlank.depth + 30, null, false);
                                string text2 = __instance.profile.name;
                                if (text2.Length > 16)
                                    text2 = text2.Substring(0, 16);
                                fontSmall.Draw(text2, new Vec2((float)(x + num / 2.0 - fontSmall.GetWidth(text2, false, (InputProfile)null) / 2.0), __instance.y + 44f), Color.White, doorLeftBlank.depth + 30, null, false);
                            }
                            else if (flag5)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 13;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("HOST", new Vec2(__instance.x + 6f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("SLOT", new Vec2(__instance.x + 52f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                            else
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 9;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("OPEN", new Vec2(__instance.x + 6f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("SLOT", new Vec2(__instance.x + 52f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                        }
                    }
                    else
                    {
                        Rectangle sourceRectangle3 = new Rectangle(__instance._doorX, 0f, (doorLeftField.GetValue(__instance) as Sprite).width, (doorLeftField.GetValue(__instance) as Sprite).height);
                        Graphics.Draw(doorLeftBlank, __instance.x - 1f, __instance.y, sourceRectangle3);
                        Rectangle sourceRectangle4 = new Rectangle(-__instance._doorX, 0f, (doorRightField.GetValue(__instance) as Sprite).width, (doorRightField.GetValue(__instance) as Sprite).height);
                        Graphics.Draw(doorRightBlank, __instance.x + 38f, __instance.y, sourceRectangle4);
                        if (__instance._doorX == 0f)
                        {
                            fontSmall.depth = doorLeftBlank.depth + 10;
                            fontSmall.scale = new Vec2(0.75f, 0.75f);
                            if (!Network.isActive || (flag5 && Network.isServer))
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 10;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("PRESS", new Vec2(__instance.x + 4f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("START", new Vec2(__instance.x + 50f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 8;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                            }
                            else if (flag6)
                            {
                                doorSpinner.depth = doorLeftBlank.depth + 10;
                                Graphics.Draw(doorSpinner, __instance.x + 31f, __instance.y + 20f);
                            }
                            else if (flag2)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 11;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("PALS", new Vec2(__instance.x + 4f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("ONLY", new Vec2(__instance.x + 50f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag3)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 12;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("VIPS", new Vec2(__instance.x + 4f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("ONLY", new Vec2(__instance.x + 50f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                            else if (flag4 && __instance.profile.reservedUser != null)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 12;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                float num2 = 120f;
                                float x2 = __instance.x + 10f;
                                Graphics.DrawRect(new Vec2(x2, __instance.y + 35f), new Vec2(x2 + num2, __instance.y + 52f), Color.Black, doorLeftBlank.depth + 20, true, 1f);
                                string text3 = "WAITING FOR";
                                fontSmall.Draw(text3, new Vec2(x2 + num2 / 2f - fontSmall.GetWidth(text3, false, null) / 2f, __instance.y + 36f), Color.White, doorLeftBlank.depth + 30, null, false);
                                text3 = __instance.profile.name;
                                if (text3.Length > 16)
                                {
                                    text3 = text3.Substring(0, 16);
                                }
                                fontSmall.Draw(text3, new Vec2(x2 + num2 / 2f - fontSmall.GetWidth(text3, false, null) / 2f, __instance.y + 44f), Color.White, doorLeftBlank.depth + 30, null, false);
                            }
                            else if (flag5)
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 13;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("HOST", new Vec2(__instance.x + 4f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("SLOT", new Vec2(__instance.x + 50f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                            else
                            {
                                doorIcon.depth = doorLeftBlank.depth + 10;
                                doorIcon.frame = 9;
                                Graphics.Draw(doorIcon, __instance.x + 31f, __instance.y + 20f);
                                fontSmall.DrawOutline("OPEN", new Vec2(__instance.x + 4f, __instance.y + 26f), Color.White, Colors.BlueGray, doorLeftBlank.depth + 10);
                                fontSmall.DrawOutline("SLOT", new Vec2(__instance.x + 50f, __instance.y + 26f), Color.White, Colors.BlueGray, doorRightBlank.depth + 10);
                            }
                        }
                    }
                }
                if (__instance.profile.team == null)
                {
                    return false;
                }
                if (__instance._doorX > 0f)
                {
                    Sprite roomLeftForeground = roomLeftForegroundField.GetValue(__instance) as Sprite;
                    Sprite roomLeftBackground = roomLeftBackgroundField.GetValue(__instance) as Sprite;
                    Sprite hostCrown = hostCrownField.GetValue(__instance) as Sprite;

                    if (index == 2 || index == 4 || index == 7)
                    {
                        Graphics.Draw(roomLeftBackground, __instance.x - 1f, __instance.y + 1f);
                        Graphics.Draw(roomLeftForeground, __instance.x - 1f, __instance.y + 1f, new Rectangle(0.0f, 0.0f, 82f, 56f));

                        if (Network.isActive && ((Network.isServer && __instance.profile.connection == DuckNetwork.localConnection) || __instance.profile.connection == Network.host))
                        {
                            hostCrown.depth = -0.5f;
                            Graphics.Draw(hostCrown, __instance.x + 68f, __instance.y + 13f);
                        }
                    }
                    else if (index == 1 || index == 6)
                    {

                        Graphics.Draw(roomLeftBackground, __instance.x - 1f, __instance.y + 1f);
                        Graphics.Draw(roomLeftForeground, __instance.x - 1f, __instance.y + 1f, new Rectangle(0.0f, 0.0f, 82f, 56f));

                        if (Network.isActive && ((Network.isServer && __instance.profile.connection == DuckNetwork.localConnection) || __instance.profile.connection == Network.host))
                        {
                            hostCrown.depth = -0.5f;
                            Graphics.Draw(hostCrown, __instance.x + 56f, __instance.y + 13f);
                        }
                    }
                    else
                    {
                        Graphics.Draw(roomLeftBackground, __instance.x - 1f, __instance.y + 1f);
                        Graphics.Draw(roomLeftForeground, __instance.x - 1f, __instance.y + 1f, new Rectangle(0.0f, 0.0f, 82f, 56f));

                        if (Network.isActive && ((Network.isServer && __instance.profile.connection == DuckNetwork.localConnection) || __instance.profile.connection == Network.host))
                        {
                            hostCrown.depth = -0.5f;
                            Graphics.Draw(hostCrown, __instance.x + 11f, __instance.y + 13f);
                        }
                    }

                    BitmapFont font = fontField.GetValue(__instance) as BitmapFont;

                    font.alpha = 1f;
                    font.depth = 0.6f;

                    string currentDisplayName = __instance.profile.team.currentDisplayName;
                    SpriteMap selectConsole = selectConsoleField.GetValue(__instance) as SpriteMap;
                    Sprite consoleHighlight = consoleHighlightField.GetValue(__instance) as Sprite;
                    selectConsole.depth = -0.5f;
                    consoleHighlight.depth = -0.49f;

                    Vec2 consolePos;
                    Sprite consoleFlash = consoleFlashField.GetValue(__instance) as Sprite;
                    float consoleFade = (float)consoleFadeField.GetValue(__instance);
                    Sprite readySign = readySignField.GetValue(__instance) as Sprite;

                    if (index == 2 || index == 4 || index == 7)
                    {
                        consolePos = new Vec2(__instance.x + 58f, __instance.y + 20f);
                        consoleFlash.scale = new Vec2(0.75f, 0.75f);
                        if (selectConsole.imageIndex == 0)
                            consoleFlash.alpha = 0.3f;
                        else if (selectConsole.imageIndex == 1)
                            consoleFlash.alpha = 0.1f;
                        else if (selectConsole.imageIndex == 2)
                            consoleFlash.alpha = 0.0f;
                        Graphics.Draw(consoleFlash, consolePos.x + 9f, consolePos.y + 7f);
                        Graphics.Draw(selectConsole, consolePos.x, consolePos.y);
                        if (consoleFade > 0.01)
                        {
                            consoleHighlight.alpha = consoleFade;
                            Graphics.Draw(consoleHighlight, consolePos.x, consolePos.y);
                        }
                        Graphics.Draw(readySign, __instance.x + 1f, __instance.y + 2f);

                        float num2 = 0.0f;
                        float num3 = 0.0f;
                        font.scale = new Vec2(0.45f, 0.45f);
                        if (currentDisplayName.Length > 9)
                        {
                            font.scale = new Vec2(0.4f, 0.4f);
                            num2 = 0f;
                            num3 = 1f;
                        }
                        if (currentDisplayName.Length > 12)
                        {
                            font.scale = new Vec2(0.35f, 0.35f);
                            num2 = 0.5f;
                            num3 = 1f;
                        }
                        font.Draw(currentDisplayName, (__instance.x + 50.0f - font.GetWidth(currentDisplayName, false, null) / 2.0f) - num3, __instance.y + 52f + num2, Color.White, 0.7f, null, false);
                    }
                    else if (index == 1 || index == 6)
                    {
                        consolePos = new Vec2(__instance.x + 46f, __instance.y + 20f);
                        consoleFlash.scale = new Vec2(0.75f, 0.75f);
                        if (selectConsole.imageIndex == 0)
                            consoleFlash.alpha = 0.3f;
                        else if (selectConsole.imageIndex == 1)
                            consoleFlash.alpha = 0.1f;
                        else if (selectConsole.imageIndex == 2)
                            consoleFlash.alpha = 0.0f;
                        Graphics.Draw(consoleFlash, consolePos.x + 9f, consolePos.y + 7f);
                        Graphics.Draw(selectConsole, consolePos.x, consolePos.y);
                        if (consoleFade > 0.01)
                        {
                            consoleHighlight.alpha = consoleFade;
                            Graphics.Draw(consoleHighlight, consolePos.x, consolePos.y);
                        }
                        Graphics.Draw(readySign, __instance.x + 1f, __instance.y + 2f);

                        float num2 = 0.0f;
                        float num3 = 0.0f;
                        font.scale = new Vec2(0.45f, 0.45f);
                        if (currentDisplayName.Length > 9)
                        {
                            font.scale = new Vec2(0.4f, 0.4f);
                            num2 = 0f;
                            num3 = 1f;
                        }
                        if (currentDisplayName.Length > 12)
                        {
                            font.scale = new Vec2(0.35f, 0.35f);
                            num2 = 0.5f;
                            num3 = 1f;
                        }
                        font.Draw(currentDisplayName, (__instance.x + 40.0f - font.GetWidth(currentDisplayName, false, null) / 2.0f) - num3, __instance.y + 52f + num2, Color.White, 0.7f, null, false);
                    }
                    else
                    {
                        consolePos = new Vec2(__instance.x + 1f, __instance.y + 20f);
                        consoleFlash.scale = new Vec2(0.75f, 0.75f);
                        if (selectConsole.imageIndex == 0)
                            consoleFlash.alpha = 0.3f;
                        else if (selectConsole.imageIndex == 1)
                            consoleFlash.alpha = 0.1f;
                        else if (selectConsole.imageIndex == 2)
                            consoleFlash.alpha = 0.0f;
                        Graphics.Draw(consoleFlash, consolePos.x + 9f, consolePos.y + 7f);
                        Graphics.Draw(selectConsole, consolePos.x, consolePos.y);
                        if (consoleFade > 0.01)
                        {
                            consoleHighlight.alpha = consoleFade;
                            Graphics.Draw(consoleHighlight, consolePos.x, consolePos.y);
                        }
                        Graphics.Draw(readySign, __instance.x + 53f, __instance.y + 2f);

                        float num2 = 0.0f;
                        float num3 = 0.0f;
                        font.scale = new Vec2(0.45f, 0.45f);
                        if (currentDisplayName.Length > 9)
                        {
                            font.scale = new Vec2(0.4f, 0.4f);
                            num2 = 0f;
                            num3 = 1f;
                        }
                        if (currentDisplayName.Length > 12)
                        {
                            font.scale = new Vec2(0.35f, 0.35f);
                            num2 = 0.5f;
                            num3 = 1f;
                        }
                        font.Draw(currentDisplayName, (__instance.x + 37.0f - font.GetWidth(currentDisplayName, false, null) / 2.0f) - num3, __instance.y + 52f + num2, Color.White, 0.7f, null, false);
                    }
                    AccessTools.Field(typeof(ProfileBox2), "_consolePos").SetValue(__instance, consolePos);
                }

                return false;
            }
        }
    }
}
