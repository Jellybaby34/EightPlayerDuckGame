/* If you're reading this then you are looking at my disgusting source that allows duck game to host 8 players
   rather than the basic 4. 
*/

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;

// The title of your mod, as displayed in menus
[assembly: AssemblyTitle("8 Player Duck Game Alpha")]

// The author of the mod
[assembly: AssemblyCompany("Ainsley Harriott (Jellybaby44)")]

// The description of the mod
[assembly: AssemblyDescription("Edits various bits of duck game to allow up to 8 players to join")]

// The mod's version
[assembly: AssemblyVersion("0.0.0.1")]

namespace DuckGame.IncreasedPlayerLimit
{

    public class Injection
    {

        // DuckNetworkCore.cs
        private void duckNetworkCoreInjection()
        {

            DuckNetworkCore DuckNetworkCore = DuckNetwork.core;

            DuckNetworkCore.profiles.Clear();
            for (int index = 0; index < 8; ++index)
                DuckNetworkCore.profiles.Add(new Profile("Netduck" + (index + 1).ToString(), InputProfile.GetVirtualInput(index), null, Persona.all.ElementAt(index), true, null)
                {
                    networkIndex = (byte)index
                });

        }
        // End of DuckNetworkCore.cs

        // Input.cs
        // InitDefaultProfiles
        public static void inputInitProfilesInjection()
        {

            for (int index = 0; index < 8; ++index)
            {
                InputProfile inputProfile = InputProfile.Add("MPPlayer" + (index + 1).ToString());
                inputProfile.Map(Input.GetDevice<GenericController>(index), "LEFT", 4, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "RIGHT", 8, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "UP", 1, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "DOWN", 2, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "JUMP", 4096, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "SHOOT", 16384, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "GRAB", 32768, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "QUACK", 8192, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "START", 16, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "STRAFE", 256, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "RAGDOLL", 512, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "LTRIGGER", 8388608, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "RTRIGGER", 4194304, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "SELECT", 4096, false);
                if (index == 0)
                    InputProfile.active = inputProfile;
            }
            Input.ApplyDefaultMappings();
            InputProfile.Add("Blank");
        }
        // End of InitDefaultProfiles

        // Initialize
        public static void inputInitializeCall()
        {
            Type inputtype = typeof(Input);
            FieldInfo _devicesField = inputtype.GetField("_devices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _devicesVal = _devicesField.GetValue(null);

            FieldInfo _gamePadsField = inputtype.GetField("_gamePads", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _gamePadsVal = _devicesField.GetValue(null);


            // Player 5
            GenericController genericController5 = new GenericController(4);
            _devicesVal.Add((InputDevice)genericController5);
            _gamePadsVal.Add(genericController5);

            // Player 6
            GenericController genericController6 = new GenericController(5);
            _devicesVal.Add((InputDevice)genericController6);
            _gamePadsVal.Add(genericController6);

            // Player 7
            GenericController genericController7 = new GenericController(6);
            _devicesVal.Add((InputDevice)genericController7);
            _gamePadsVal.Add(genericController7);

            // Player 8
            GenericController genericController8 = new GenericController(7);
            _devicesVal.Add((InputDevice)genericController8);
            _gamePadsVal.Add(genericController8);
        }
        //End of Initialize
        //End of Input.cs

        //InputProfileCore.cs

        // God himself wept at this code
        public static void inputProfileCoreCall()
        {
            List<InputProfile> mpplayerlist = new List<InputProfile>() { InputProfile.core.Get("MPPlayer1"), InputProfile.core.Get("MPPlayer2"), InputProfile.core.Get("MPPlayer3"), InputProfile.core.Get("MPPlayer4"), InputProfile.core.Get("MPPlayer5"), InputProfile.core.Get("MPPlayer6"), InputProfile.core.Get("MPPlayer7"), InputProfile.core.Get("MPPlayer8") };
            Type inputtype = typeof(InputProfileCore);
            PropertyInfo _devicesProperty = inputtype.GetProperty("defaultProfiles", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            object instance = Activator.CreateInstance(_devicesProperty.PropertyType);
//            _devicesProperty.SetValue(instance, mpplayerlist, null);

            MethodInfo methodToReplace = _devicesProperty.GetGetMethod(true);
            MethodInfo methodToInject = typeof(Injection).GetMethod("injectionmethod2", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            RuntimeHelpers.PrepareMethod(methodToReplace.MethodHandle);
            RuntimeHelpers.PrepareMethod(methodToInject.MethodHandle);

            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    int* inj = (int*)methodToInject.MethodHandle.Value.ToPointer() + 2;
                    int* tar = (int*)methodToReplace.MethodHandle.Value.ToPointer() + 2;
#if DEBUG
                    Console.WriteLine("\nVersion x84 Debug\n");

                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;

                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    //                    Console.WriteLine("\nVersion x84 Release\n");
                    *tar = *inj;
#endif
                }
                else
                {

                    long* inj = (long*)methodToInject.MethodHandle.Value.ToPointer() + 1;
                    long* tar = (long*)methodToReplace.MethodHandle.Value.ToPointer() + 1;
#if DEBUG
                    Console.WriteLine("\nVersion x64 Debug\n");
                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;


                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    //                    Console.WriteLine("\nVersion x64 Release\n");
                    *tar = *inj;
#endif
                }
            }


        }
        //End of InputProfileCore.cs

        // Persona.cs
        public static void personaCall()
        {
            List<DuckPersona> objTemp = new List<DuckPersona>() { new DuckPersona(new Vec3(byte.MaxValue, byte.MaxValue, byte.MaxValue)), new DuckPersona(new Vec3(125f, 125f, 125f)), new DuckPersona(new Vec3(247f, 224f, 90f)), new DuckPersona(new Vec3(205f, 107f, 29f)), new DuckPersona(new Vec3(200f, 100f, 29f)), new DuckPersona(new Vec3(5f, 207f, 150f)), new DuckPersona(new Vec3(0f, 0f, 200f)), new DuckPersona(new Vec3(255f, 0f, 255f)) };
            Type typea = typeof(Persona);
            FieldInfo info2 = typea.GetField("_personas", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            info2.SetValue(null, objTemp);
        }
        // End of Persona.cs    

        private void injectionMethod1()
        {
            return;
        }

        public List<InputProfile> injectionmethod2()
        {
           InputProfile DefaultPlayer5 = InputProfile.core.Get("MPPlayer5");
/*            InputProfile DefaultProfile6 = InputProfile.core.Get("MPPlayer6");
            InputProfile DefaultProfile7 = InputProfile.core.Get("MPPlayer7");
            InputProfile DefaultProfile8 = InputProfile.core.Get("MPPlayer8");
*/            return new List<InputProfile>() { InputProfile.core.Get("MPPlayer1"), InputProfile.core.Get("MPPlayer2"), InputProfile.core.Get("MPPlayer3"), InputProfile.core.Get("MPPlayer4"), DefaultPlayer5, InputProfile.core.Get("MPPlayer6"), InputProfile.core.Get("MPPlayer7"), InputProfile.core.Get("MPPlayer8") };
        }

        public static void install(int type, string methodtoreplace, string methodtoinject)
        {
            Type type2 = null;
            if (type == 0)
                type2 = typeof(Duck);
            else if (type == 1)
                type2 = typeof(DuckNetworkCore);
            else if (type == 2)
                type2 = typeof(Input);

            MethodInfo methodToReplace = type2.GetMethod(methodtoreplace, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            MethodInfo methodToInject = typeof(Injection).GetMethod(methodtoinject, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            RuntimeHelpers.PrepareMethod(methodToReplace.MethodHandle);
            RuntimeHelpers.PrepareMethod(methodToInject.MethodHandle);

            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    int* inj = (int*)methodToInject.MethodHandle.Value.ToPointer() + 2;
                    int* tar = (int*)methodToReplace.MethodHandle.Value.ToPointer() + 2;
#if DEBUG
                    Console.WriteLine("\nVersion x84 Debug\n");

                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;

                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
//                    Console.WriteLine("\nVersion x84 Release\n");
                    *tar = *inj;
#endif
                }
                else
                {

                    long* inj = (long*)methodToInject.MethodHandle.Value.ToPointer() + 1;
                    long* tar = (long*)methodToReplace.MethodHandle.Value.ToPointer() + 1;
#if DEBUG
                    Console.WriteLine("\nVersion x64 Debug\n");
                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;


                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
//                    Console.WriteLine("\nVersion x64 Release\n");
                    *tar = *inj;
#endif
                }
            }
        }

    }

    public class IncreasedPlayerLimit : Mod
    {
		// The mod's priority; this property controls the load order of the mod.
		public override Priority priority
		{
			get { return Priority.Highest; }
		}

		// This function is run before all mods are finished loading.
		protected override void OnPreInitialize()
		{
            // This is broken down between calls where I replace lists and value that won't be called except once
            // and methods which will be ran multiple times so I inject my own code using reflection
            // This is a terrible practice but I want this mod to be downloadable from the workshop
            // and to not need to redistribute a modded executable.
/*            List<InputDevice> objTemp = new List<InputDevice>();
            Type typea = typeof(Input);
            FieldInfo info2 = typea.GetField("_devices", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            info2.SetValue(null, objTemp);
*/
            Injection.install(2, "InitDefaultProfiles", "inputInitProfilesInjection");
            Injection.inputInitializeCall();
            Injection.inputInitProfilesInjection();
            // Calls
//            Input.Initialize();
//            Injection.inputInitializeCall();
            Injection.inputProfileCoreCall();
            Injection.personaCall();


            // Injections
            // Because I can't fathom out how to pass a type as a variable I just pass an int and have that
            // be used to look up the type I want. Probably not the best idea but meh.

            Injection.install(1, "RecreateProfiles", "duckNetworkCoreInjection");

            Injection.install(0, "UpdateQuack", "injectionMethod1"); // Disables quack to check everything loaded right

            // Base
            base.OnPreInitialize();
		}

		// This function is run after all mods are loaded.
		protected override void OnPostInitialize()
		{
			base.OnPostInitialize();
		}
	}
}
