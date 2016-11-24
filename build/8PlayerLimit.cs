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

// The mod's version. I don't follow versioning at all.
[assembly: AssemblyVersion("0.1.0.3")]

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
            else if (typereplace == 6)
            {
                typereplace1 = typeof(HatSelector);
                typeinject1 = typeof(HatSelectorEdits);
            }
            else if (typereplace == 7)
            {
            // Empty because I was going to inject into BufferedGhostState but didn't actually need to.
            }
            else if (typereplace == 8)
            {
                typereplace1 = typeof(Level);
                typeinject1 = typeof(LevelEdits);
            }

            MethodInfo methodToReplace = typereplace1.GetMethod(methodtoreplace, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            MethodInfo methodToInject = typeinject1.GetMethod(methodtoinject, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public );
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
            // The order of these is important for some things, others not. I'll document what each one does eventually.

            // Load the embedded DLL which has unsafe code. Allows duck game to compile this source and still use unsafe code.
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            // Get the profile core. Its not supposed to be accessed outside the main thread but I won't tell if you won't.
            ProfilesCore profilecore = typeof(Profiles).GetField("_core", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).GetValue(null) as ProfilesCore;

            // Normally generates 4 MPPlayer profiles, now does 8
            Injection.install(2, "InitDefaultProfiles", "InitDefaultProfiles");

            // Add our extra personas to the table
            PersonaEdits._personas();

            // Replace the get_defaultProfiles() with one that handles the extra 4 player teams
            InputProfileCoreEdits.defaultProfiles();

            // Duck Game would just check which profiles were past the first 4 and decide they were custom, changed to skip the first 8
            ProfilesCoreEdits.allCustomProfiles();

            // Generates the default Player1-Player8 Profiles now
            Injection.install(5, "Initialize", "Initialize");

            // Returns the first 8 profiles as defaults
            Injection.install(5, "IsDefault", "IsDefault");

            // Generates the extra 4 GenericControllers for the added players
            InputEdits.Initialize();

            // Invokes the new InitDefaultProfiles(). This may not be needed as it may get injected before it gets run the first time. I'll check
            InputEdits.InitDefaultProfiles();

            // Generates 8 Netduck profiles instead of the default 4
            Injection.install(1, "RecreateProfiles", "RecreateProfiles");

            // Allows the host to select between 2 and 8 players maximum for their lobby
            TeamSelect2Edits.OnlineSettings();

            // Add the extra 4 default teams that will be used for the extra players
            TeamsCoreEdits.Initialize();

            // Generate new profile boxes for the lobby level. Also tweaks the camera for it. Will make it not stretched at some point
            Injection.install(4, "UpdateModifierStatus", "UpdateModifierStatus");

            // DoInvite would call Host(4, LobbyType.FriendsOnly) so we change it to 8 max players
            Injection.install(4, "DoInvite", "DoInvite");

            // Would only purge the first 4 boxes by default. Now purges all 8
            Injection.install(4, "FillMatchmakingProfiles", "FillMatchmakingProfiles");

            // ClearTeam() would return if the index was >=4. We need it to be >=8 for the extra teams
            Injection.install(4, "ClearTeam", "ClearTeam");

            // Return the extra inputprofiles for the extra teams
            Injection.install(6, "ControllerNumber", "ControllerNumber");

            // Trying to detour ConfirmTeamSelection was proving a nightmare so I did this garbage solution.
            // A check at the bottom of ConfirmTeamSelection was returning incorrectly due to the extra teams and crashing
            // When I tried injecting my new method, it was never called for some reason. Not sure why, probably due to instancing
            // To deal with this I edited FilterTeam which gets called at the start of ConfirmTeamSelection and did a check where 
            // if the _desiredteamselection was <= 7 then set it to 0. This meant the check that was crashing returned correctly
            // and as far as I can tell there aren't any noticeable side effects. Will find better method eventually.
            Injection.install(6, "FilterTeam", "FilterTeam");

            // Because inline removes TeamSelect2.OnNetworkConnecting, I have to inject the methods that called it to change them
            Injection.install(3, "JoinLocalDuck", "JoinLocalDuck");
            Injection.install(3, "OnMessageFromNewClient", "OnMessageFromNewClient");
            Injection.install(3, "OnMessage", "OnMessage");

            // New NMChangeSlot net message as the old one only handled 4 parameters and we need 8.
            Injection.install(3, "ChangeSlotSettings", "ChangeSlotSettings");

            // DuckNetwork.Update calls Host( 4, LobbyType.FriendsOnly ) on inviting someone. Since performance would be garbage reflecting all
            // the private variables, we detour host instead.
            Injection.install(3, "Host", "Host");

            // Only would clear custom data for the first 4 profiles so now it clears all 8
            Injection.install(3, "Join", "Join");

            // Adds our new netmessage to the list of netmessage types so that the game doesn't crash when it encounters it
            DuckNetworkEdits.UpdateNetmessageTypes();

            // netProfileIndex determines how many bits are used to store it. Since we need to store up to the value of seven, we need 3 bits
            // rather than the default 2. Best way I thought would be change it when the ducks get added but idk.
            Injection.install(8, "Add", "AddReplace");

            // Method to inject a call into LevelUpdateChange so that it calls our varient which then checks for rock scoreboard
            // then calls our patched Initialize.
            LevelEdits.UpdateLevelChange();

            // NMVoteToSkip checks whether the player index is greater than 3 and discards it if it is. We want it to be discarded when greater
            // than 7.
            NMVoteToSkipEdits.Activate();

            // Change the get method for determining if a room if on the right or left to Value % 2. Will return 1 if on the right.
            // We inject pure assembly so THIS WILL BREAK EVENTUALLY.
            ProfileBox2Edits.rightRoomCorrection();

            // Injection.install(0, "UpdateQuack", "injectionMethod1"); // Disables quack to check everything loaded right

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
