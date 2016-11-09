/* If you're reading this then you are looking at my disgusting source that allows duck game to host 8 players
   rather than the basic 4. 

   There are various things that need to be done in order to allow duck game to host more than 4 players without crashing outright.
*/

using System;
using System.IO;
using System.Reflection;

// The title of your mod, as displayed in menus
[assembly: AssemblyTitle("8 Player Duck Game Alpha")]

// The author of the mod
[assembly: AssemblyCompany("Ainsley Harriott (Jellybaby44)")]

// The description of the mod
[assembly: AssemblyDescription("Edits various bits of duck game to allow up to 8 players to join")]

// The mod's version
[assembly: AssemblyVersion("0.0.0.3")]

namespace DuckGame.IncreasedPlayerLimit
{

    public class Injection
    {

        private void injectionMethod1()
        {
            return;
        }

        public static void install(int typereplace, string methodtoreplace, string methodtoinject)
        {
            Type typereplace1 = null;
            Type typeinject1 = null;
            if (typereplace == 0)
            {
                typereplace1 = typeof(Duck);
                typeinject1 = typeof(Injection);
                
            }
            else if (typereplace == 1)
            {
                typereplace1 = typeof(DuckNetworkCore);
                typeinject1 = typeof(DuckNetworkCoreEdits);
            }
            else if (typereplace == 2)
            {
                typereplace1 = typeof(Input);
                typeinject1 = typeof(InputEdits);
            }
            else if (typereplace == 3)
            {
                typereplace1 = typeof(DuckNetwork);
                typeinject1 = typeof(DuckNetworkEdits);
            }
            else if (typereplace == 4)
            {
                typereplace1 = typeof(TeamSelect2);
                typeinject1 = typeof(TeamSelect2Edits);
            }
            else if (typereplace == 5)
            {
                typereplace1 = typeof(ProfilesCore);
                typeinject1 = typeof(ProfilesCoreEdits);
            }
            MethodInfo methodToReplace = typereplace1.GetMethod(methodtoreplace, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            MethodInfo methodToInject = typeinject1.GetMethod(methodtoinject, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            UnsafeCode.CodeInjection(methodToReplace, methodToInject);
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
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            ProfilesCore profilecore = typeof(Profiles).GetField("_core", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).GetValue(null) as ProfilesCore;

            Injection.install(2, "InitDefaultProfiles", "InitDefaultProfiles");

            PersonaEdits._personas();


            InputProfileCoreEdits.defaultProfiles();
            ProfilesCoreEdits.allCustomProfiles();
            Injection.install(5, "Initialize", "Initialize");

            profilecore.Initialize();

            InputEdits.Initialize();
            InputEdits.InitDefaultProfiles();


            Injection.install(1, "RecreateProfiles", "RecreateProfiles");

            TeamSelect2Edits.OnlineSettings();

            // Generate new profile boxes
            Injection.install(4, "UpdateModifierStatus", "UpdateModifierStatus");


            // Because inline removed TeamSelect2.OnNetworkConnecting, I have to inject the methods that called it to change them
            Injection.install(3, "JoinLocalDuck", "JoinLocalDuck");
            Injection.install(3, "OnMessageFromNewClient", "OnMessageFromNewClient");
            Injection.install(3, "OnMessage", "OnMessage");


//            Injection.install(0, "UpdateQuack", "injectionMethod1"); // Disables quack to check everything loaded right
            // Won't be needed in full release

            // Base
            base.OnPreInitialize();
		}

		// This function is run after all mods are loaded.
		protected override void OnPostInitialize()
		{
			base.OnPostInitialize();
		}


        // Used to load the embedded DLL rather than have the user put it in the Duck Game folder
        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Load();
        }

        public static Assembly Load()
        {
            byte[] ba = null;
            //string resource = "DuckGame.IncreasedPlayerLimit.CodeInjectionDLL.dll";
            string resource = "IncreasedPlayerLimit.CodeInjectionDLL.dll";
            Assembly curAsm = Assembly.GetExecutingAssembly();
            using (Stream stm = curAsm.GetManifestResourceStream(resource))
            {
                ba = new byte[(int)stm.Length];
                stm.Read(ba, 0, (int)stm.Length);

                return Assembly.Load(ba);
            }
        }

    }
}
