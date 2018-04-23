// Welcome to attempt three of eight player duck game. This time we're using the patch library "Harmony".
// https://github.com/pardeike/Harmony
// Use it wisely.

using System.Reflection;
using Harmony;

// The title of your mod, as displayed in menus
[assembly: AssemblyTitle("Eight Player Duck Game")]

// The author of the mod
[assembly: AssemblyCompany("TheSpicyChef")]

// The description of the mod
[assembly: AssemblyDescription("Allows up to eight players instead of the default four.")]

// The mod's version
[assembly: AssemblyVersion("1.0.0.1")]

namespace DuckGame.EightPlayerDuckGame
{
    public class EightPlayerDuckGame : Mod
    {

        // The mod's priority; this property controls the load order of the mod.
        public override Priority priority
		{
			get { return Priority.Highest; }
		}

		// This function is run before all mods are finished loading.
		protected override void OnPreInitialize()
		{
            // All the methods have comments above them in their respective classes. I've tried to be as helpful as possible.

            // I'm using an unofficial merge of the current harmony branch and a branch that handles try/catch/finally statements.
            // I'll drop that when the official implementation of try/catch/finally statements are added.
            var harmony = HarmonyInstance.Create("duckgame.EightPlayerDuckGame");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            // Methods where we don't need to inject anything and we just edit fields
            PersonaEdits.AddNewPersonas();
            InputEdits.AddExtraDevices();
            TeamSelect2Edits.UpdateOnlineSettings();
            TeamsCoreEdits.AddExtraTeams();
            Input.InitDefaultProfiles();
            DuckNetworkEdits.AddNewNetmessageTypes(); // Method is actually part of "Initialize" in the Network class of DuckGame.
            base.OnPreInitialize();
		}

        // This function is run after all mods are loaded.
        protected override void OnPostInitialize()
		{
            base.OnPostInitialize();
        }

    }

}
