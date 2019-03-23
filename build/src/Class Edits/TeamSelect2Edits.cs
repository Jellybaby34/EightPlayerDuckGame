using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace DuckGame.EightPlayerDuckGame
{
    public class TeamSelect2Edits
    {
        public static void UpdateOnlineSettings()
        {
            List<MatchSetting> onlineSettings = new List<MatchSetting>() { new MatchSetting() { id = "maxplayers", name = "MAX PLAYERS", value = 8, min = 2, max = 8, step = 1 }, new MatchSetting() { id = "teams", name = "TEAMS", value = (object)false }, new MatchSetting() { id = "modifiers", name = "MODIFIERS", value = (object)false, filtered = true, filterOnly = true }, new MatchSetting() { id = "type", name = "TYPE", value = (object)2, min = 0, max = 2, createOnly = true, valueStrings = new List<string>() { "PRIVATE", "FRIENDS", "PUBLIC" } } };
            TeamSelect2.onlineSettings = onlineSettings;
        }

        [HarmonyPatch(typeof(TeamSelect2), "DoInvite")]
        public static class TeamSelect2_DoInvite_Transpiler
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
                }
                return codes.AsEnumerable();
            }
        }

        [HarmonyPatch(typeof(TeamSelect2), "ClearTeam")]
        public static class TeamSelect2_ClearTeam_Transpiler
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

        [HarmonyPatch(typeof(TeamSelect2), "FillMatchmakingProfiles")]
        public static class TeamSelect2_FillMatchmakingProfiles_Transpiler
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
                }
                return codes.AsEnumerable();
            }
        }

        [HarmonyPatch(typeof(TeamSelect2), "Update")]
        public static class TeamSelect2_Update_Transpiler
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

        [HarmonyPatch(typeof(TeamSelect2), "Initialize")]
        public static class TeamSelect2_Initialize_Prefix
        {
            [HarmonyPrefix]
            static bool Prefix(TeamSelect2 __instance)
            {

                // TO IMPLEMENT
                //++Global.data.bootedSinceUpdate;
                //Global.Save();
                // TO IMPLEMENT

                List<DuckPersona> personas = Persona.all as List<DuckPersona>;
                // Get the private fields by reflection
                Type teamselect2type = typeof(TeamSelect2);

                dynamic littleFont = teamselect2type.GetField("_littleFont", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic countdownScreen = teamselect2type.GetField("_countdownScreen", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic countdown = teamselect2type.GetField("_countdown", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic profiles = teamselect2type.GetField("_profiles", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic font = teamselect2type.GetField("_font", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic buttons = teamselect2type.GetField("_buttons", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic beam = teamselect2type.GetField("_beam", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic configGroup = teamselect2type.GetField("_configGroup", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic multiplayerMenu = teamselect2type.GetField("_multiplayerMenu", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic modifierMenu = teamselect2type.GetField("_modifierMenu", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic levelSelectMenu = teamselect2type.GetField("_levelSelectMenu", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic playOnlineGroup = teamselect2type.GetField("_playOnlineGroup", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic playOnlineMenu = teamselect2type.GetField("_playOnlineMenu", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic hostGameMenu = teamselect2type.GetField("_hostGameMenu", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic browseGamesMenu = teamselect2type.GetField("_browseGamesMenu", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic miniHostGameMenu = teamselect2type.GetField("_miniHostGameMenu", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic joinGameMenu = teamselect2type.GetField("_joinGameMenu", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic filtersMenu = teamselect2type.GetField("_filtersMenu", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic filterModifierMenu = teamselect2type.GetField("_filterModifierMenu", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic matchmaker = teamselect2type.GetField("_matchmaker", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic hostGameSettingsMenu = teamselect2type.GetField("_hostGameSettingsMenu", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic hostGameModifierMenu = teamselect2type.GetField("_hostGameModifierMenu", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic createGame = teamselect2type.GetField("_createGame", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                dynamic hostGame = teamselect2type.GetField("_hostGame", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
                // End of getting fields

                TeamSelect2.customLevels = TeamSelect2.prevCustomLevels = 0;
                if (!Network.isActive)
                    Level.core.gameInProgress = false;
                if (!Level.core.gameInProgress)
                {
                    Main.ResetMatchStuff();
                    Main.ResetGameStuff();
                    DuckNetwork.ClosePauseMenu();
                }
                else
                {
                    ConnectionStatusUI.Hide();
                    if (Network.isServer)
                    {
                        if (Steam.lobby != null)
                        {
                            Steam.lobby.SetLobbyData("started", "false");
                            Steam.lobby.joinable = true;
                        }
                        DuckNetwork.inGame = false;
                        foreach (Profile profile in DuckNetwork.profiles)
                        {
                            if (profile.connection == null && profile.slotType != SlotType.Reserved)
                                profile.slotType = SlotType.Closed;
                        }
                    }
                }
                if (Network.isActive && Network.isServer)
                    DuckNetwork.ChangeSlotSettings();
                littleFont = new BitmapFont("smallBiosFontUI", 7, 5);
                countdownScreen = new Sprite("title/wideScreen", 0.0f, 0.0f);
                __instance.backgroundColor = Color.Black;
                DuckNetwork.levelIndex = (byte)0;
                if (Network.isActive && Network.isServer)
                    GhostManager.context.SetGhostIndex((NetIndex16)32);
                countdown = new SpriteMap("countdown", 32, 32, false);
                countdown.center = new Vec2(16f, 16f);
                Profile defaultProfile1 = Profiles.all.FirstOrDefault<Profile>((Func<Profile, bool>)(x =>
                {
                    if (x.team != null)
                        return x.persona == Persona.Duck1;
                    return false;
                })) ?? Profiles.DefaultPlayer1;
                Profile defaultProfile2 = Profiles.all.FirstOrDefault<Profile>((Func<Profile, bool>)(x =>
                {
                    if (x.team != null)
                        return x.persona == Persona.Duck2;
                    return false;
                })) ?? Profiles.DefaultPlayer2;
                Profile defaultProfile3 = Profiles.all.FirstOrDefault<Profile>((Func<Profile, bool>)(x =>
                {
                    if (x.team != null)
                        return x.persona == Persona.Duck3;
                    return false;
                })) ?? Profiles.DefaultPlayer3;
                Profile defaultProfile4 = Profiles.all.FirstOrDefault<Profile>((Func<Profile, bool>)(x =>
                {
                    if (x.team != null)
                        return x.persona == Persona.Duck4;
                    return false;
                })) ?? Profiles.DefaultPlayer4;
                Profile defaultProfile5 = Profiles.all.FirstOrDefault<Profile>((Func<Profile, bool>)(x =>
                {
                    if (x.team != null)
                        return x.persona == personas[4];
                    return false;
                })) ?? Profiles.core.all.ElementAt(4);
                Profile defaultProfile6 = Profiles.all.FirstOrDefault<Profile>((Func<Profile, bool>)(x =>
                {
                    if (x.team != null)
                        return x.persona == personas[5];
                    return false;
                })) ?? Profiles.core.all.ElementAt(5);
                Profile defaultProfile7 = Profiles.all.FirstOrDefault<Profile>((Func<Profile, bool>)(x =>
                {
                    if (x.team != null)
                        return x.persona == personas[6];
                    return false;
                })) ?? Profiles.core.all.ElementAt(6);
                Profile defaultProfile8 = Profiles.all.FirstOrDefault<Profile>((Func<Profile, bool>)(x =>
                {
                    if (x.team != null)
                        return x.persona == personas[7];
                    return false;
                })) ?? Profiles.core.all.ElementAt(7);

                float xpos = 1f;
                ProfileBox2 profileBox2_1 = new ProfileBox2(xpos, 1f, InputProfile.Get("MPPlayer1"), defaultProfile1, __instance, 0);
                profiles.Add(profileBox2_1);
                Level.Add((Thing)profileBox2_1);
                ProfileBox2 profileBox2_2 = new ProfileBox2(xpos + 119f, 1f, InputProfile.Get("MPPlayer2"), defaultProfile2, __instance, 1);
                profiles.Add(profileBox2_2);
                Level.Add((Thing)profileBox2_2);
                ProfileBox2 profileBox2_3 = new ProfileBox2(xpos + 238f, 1f, InputProfile.Get("MPPlayer3"), defaultProfile3, __instance, 2);
                profiles.Add(profileBox2_3);
                Level.Add((Thing)profileBox2_3);
                ProfileBox2 profileBox2_4 = new ProfileBox2(xpos, 62f, InputProfile.Get("MPPlayer4"), defaultProfile4, __instance, 3);
                profiles.Add(profileBox2_4);
                Level.Add((Thing)profileBox2_4);
                ProfileBox2 profileBox2_5 = new ProfileBox2(xpos + 238f, 62f, InputProfile.Get("MPPlayer5"), defaultProfile5, __instance, 4);
                profiles.Add(profileBox2_5);
                Level.Add((Thing)profileBox2_5);
                ProfileBox2 profileBox2_6 = new ProfileBox2(xpos, 121f, InputProfile.Get("MPPlayer6"), defaultProfile6, __instance, 5);
                profiles.Add(profileBox2_6);
                Level.Add((Thing)profileBox2_6);
                ProfileBox2 profileBox2_7 = new ProfileBox2(xpos + 119f, 121f, InputProfile.Get("MPPlayer7"), defaultProfile7, __instance, 6);
                profiles.Add(profileBox2_7);
                Level.Add((Thing)profileBox2_7);
                ProfileBox2 profileBox2_8 = new ProfileBox2(xpos + 238f, 121f, InputProfile.Get("MPPlayer8"), defaultProfile8, __instance, 7);
                profiles.Add(profileBox2_8);
                Level.Add((Thing)profileBox2_8);

                Saxaphone spicySax = new Saxaphone(160f, 100f);
                spicySax.infinite = true;
                Level.Add(spicySax);

                if (Network.isActive)
                    __instance.PrepareForOnline();
                else
                    __instance.BuildPauseMenu(false);
                font = new BitmapFont("biosFont", 8, -1);
                font.scale = new Vec2(1f, 1f);
                buttons = new SpriteMap("buttons", 14, 14, false);
                buttons.CenterOrigin();
                buttons.depth = (Depth)0.9f;
                Music.Play("CharacterSelect", true, 0.0f);
                beam = new TeamBeam(101f, 0.0f);
                Level.Add((Thing) beam);

                beam = new TeamBeam(219f, 0.0f);
                Level.Add((Thing) beam);
                TeamSelect2.UpdateModifierStatus();
                configGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
                multiplayerMenu = new UIMenu("@LWING@MATCH SETTINGS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f, "@DPAD@ADJUST  @SELECT@SELECT", (InputProfile)null, false);
                modifierMenu = new UIMenu("MODIFIERS", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, -1f, "@DPAD@ADJUST  @QUACK@BACK", (InputProfile)null, false);
                levelSelectMenu = (UIMenu)new LevelSelectCompanionMenu(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, multiplayerMenu);
                foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
                {
                    if (unlock.unlocked)
                        modifierMenu.Add((UIComponent)new UIMenuItemToggle(unlock.shortName, (UIMenuAction)null, new FieldBinding((object)unlock, "enabled", 0.0f, 1f, 0.1f), new Color(), (FieldBinding)null, (List<string>)null, false, false), true);
                    else
                        modifierMenu.Add((UIComponent)new UIMenuItem("@TINYLOCK@LOCKED", (UIMenuAction)null, UIAlign.Center, Color.Red, false), true);
                }
                modifierMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
                modifierMenu.Add((UIComponent)new UIMenuItem("OK", (UIMenuAction)new UIMenuActionOpenMenu((UIComponent)modifierMenu, (UIComponent)multiplayerMenu), UIAlign.Center, new Color(), true), true);
                modifierMenu.Close();
                foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
                {
                    if (!(matchSetting.id == "workshopmaps") || Network.available)
                        multiplayerMenu.AddMatchSetting(matchSetting, false, true);
                }
                multiplayerMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
                multiplayerMenu.Add((UIComponent)new UIModifierMenuItem((UIMenuAction)new UIMenuActionOpenMenu((UIComponent)multiplayerMenu, (UIComponent)modifierMenu), UIAlign.Center, new Color(), false), true);
                multiplayerMenu.Add((UIComponent)new UICustomLevelMenu((UIMenuAction)new UIMenuActionOpenMenu((UIComponent)multiplayerMenu, (UIComponent)levelSelectMenu), UIAlign.Center, new Color(), false), true);
                multiplayerMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
                multiplayerMenu.Add((UIComponent)new UIMenuItem("OK", (UIMenuAction)new UIMenuActionCloseMenu(configGroup), UIAlign.Center, new Color(), true), true);
                multiplayerMenu.Close();
                configGroup.Add((UIComponent)multiplayerMenu, false);
                configGroup.Add((UIComponent)modifierMenu, false);
                configGroup.Add((UIComponent)levelSelectMenu, false);
                configGroup.Close();
                Level.Add((Thing)configGroup);
                playOnlineGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
                playOnlineMenu = new UIMenu("@PLANET@PLAY ONLINE@PLANET@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f, "@DPAD@ADJUST  @SELECT@SELECT", (InputProfile)null, false);
                hostGameMenu = new UIMenu("@LWING@CREATE GAME@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f, "@DPAD@ADJUST  @SELECT@SELECT", (InputProfile)null, false);
                browseGamesMenu = (UIMenu)new UIServerBrowser(playOnlineMenu, "SERVER BROWSER", Layer.HUD.camera.width, Layer.HUD.camera.height, 550f, -1f, "@DPAD@@SELECT@JOIN @SHOOT@REFRESH @QUACK@BACK", (InputProfile)null);
                miniHostGameMenu = new UIMenu("@LWING@HOST GAME@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f, "@DPAD@ADJUST  @SELECT@SELECT", (InputProfile)null, false);
                joinGameMenu = new UIMenu("@LWING@FIND GAME@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f, "@DPAD@ADJUST  @SELECT@SELECT", (InputProfile)null, false);
                filtersMenu = new UIMenu("@LWING@FILTERS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f, "@SELECT@SELECT  @GRAB@TYPE", (InputProfile)null, false);
                filterModifierMenu = new UIMenu("@LWING@FILTER MODIFIERS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, -1f, "@DPAD@ADJUST  @SELECT@SELECT", (InputProfile)null, false);
                matchmaker = new UIMatchmakingBox(joinGameMenu, Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f);
                hostGameSettingsMenu = new UIMenu("@LWING@SETTINGS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f, "@DPAD@ADJUST  @SELECT@SELECT", (InputProfile)null, false);
                hostGameModifierMenu = new UIMenu("@LWING@MODIFIERS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, -1f, "@DPAD@ADJUST  @SELECT@SELECT", (InputProfile)null, false);
                if ((string)typeof(ModLoader).GetProperty("modHash", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null,null) != "nomods")
                {
                    playOnlineMenu.Add((UIComponent)new UIMenuItem("FIND GAME", (UIMenuAction)new UIMenuActionCloseMenuCallFunction((UIComponent)playOnlineMenu, new UIMenuActionCloseMenuCallFunction.Function(__instance.OpenNoModsFindGame)), UIAlign.Center, new Color(), false), true);
                    playOnlineMenu.Add((UIComponent)new UIMenuItem("CREATE GAME", (UIMenuAction)new UIMenuActionCloseMenuCallFunction((UIComponent)playOnlineMenu, new UIMenuActionCloseMenuCallFunction.Function(__instance.OpenNoModsCreateGame)), UIAlign.Center, new Color(), false), true);
                }
                else
                {
                    playOnlineMenu.Add((UIComponent)new UIMenuItem("FIND GAME", (UIMenuAction)new UIMenuActionOpenMenu((UIComponent)playOnlineMenu, (UIComponent)joinGameMenu), UIAlign.Center, new Color(), false), true);
                    playOnlineMenu.Add((UIComponent)new UIMenuItem("CREATE GAME", (UIMenuAction)new UIMenuActionOpenMenu((UIComponent)playOnlineMenu, (UIComponent)hostGameMenu), UIAlign.Center, new Color(), false), true);
                }
                playOnlineMenu.Add((UIComponent)new UIMenuItem("BROWSE GAMES", (UIMenuAction)new UIMenuActionOpenMenu((UIComponent)playOnlineMenu, (UIComponent)browseGamesMenu), UIAlign.Center, new Color(), false), true);
                playOnlineMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
                playOnlineMenu.Add((UIComponent)new UIMenuItem("CANCEL", (UIMenuAction)new UIMenuActionCloseMenuCallFunction(playOnlineGroup, new UIMenuActionCloseMenuCallFunction.Function(__instance.ClosedOnline)), UIAlign.Center, new Color(), true), true);
                playOnlineMenu.Close();
                playOnlineGroup.Add((UIComponent)playOnlineMenu, false);
                foreach (MatchSetting onlineSetting in TeamSelect2.onlineSettings)
                {
                    if (!onlineSetting.filterOnly)
                        hostGameMenu.AddMatchSetting(onlineSetting, false, true);
                }
                hostGameMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
                hostGameMenu.Add((UIComponent)new UIMenuItem("CREATE GAME", (UIMenuAction)new UIMenuActionCloseMenuSetBoolean(playOnlineGroup, createGame), UIAlign.Center, new Color(), false), true);
                hostGameMenu.Add((UIComponent)new UIMenuItem("BACK", (UIMenuAction)new UIMenuActionOpenMenu((UIComponent)hostGameMenu, (UIComponent)playOnlineMenu), UIAlign.Center, new Color(), true), true);
                hostGameMenu.Close();
                browseGamesMenu.Close();
                playOnlineGroup.Add((UIComponent)browseGamesMenu, false);
                playOnlineGroup.Add((UIComponent)hostGameMenu, false);
                miniHostGameMenu.AddMatchSetting(TeamSelect2.GetOnlineSetting("type"), false, true);
                miniHostGameMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
                miniHostGameMenu.Add((UIComponent)new UIMenuItem("HOST GAME", (UIMenuAction)new UIMenuActionCloseMenuSetBoolean((UIComponent)miniHostGameMenu, hostGame), UIAlign.Center, new Color(), false), true);
                miniHostGameMenu.Add((UIComponent)new UIMenuItem("CANCEL", (UIMenuAction)new UIMenuActionCloseMenu((UIComponent)miniHostGameMenu), UIAlign.Center, new Color(), true), true);
                miniHostGameMenu.Close();
                Level.Add((Thing)miniHostGameMenu);
                foreach (MatchSetting onlineSetting in TeamSelect2.onlineSettings)
                {
                    if (!onlineSetting.createOnly)
                        joinGameMenu.AddMatchSetting(onlineSetting, true, true);
                }
                joinGameMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
                joinGameMenu.Add((UIComponent)new UIMenuItem("FIND GAME", (UIMenuAction)new UIMenuActionOpenMenu((UIComponent)joinGameMenu, (UIComponent)matchmaker), UIAlign.Center, new Color(), false), true);
                joinGameMenu.Add((UIComponent)new UIMenuItem("BACK", (UIMenuAction)new UIMenuActionOpenMenu((UIComponent)joinGameMenu, (UIComponent)playOnlineMenu), UIAlign.Center, new Color(), true), true);
                joinGameMenu.Close();
                playOnlineGroup.Add((UIComponent)joinGameMenu, false);
                foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
                {
                    if (!(matchSetting.id == "workshopmaps") || Network.available)
                        filtersMenu.AddMatchSetting(matchSetting, true, true);
                }
                filtersMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
                filtersMenu.Add((UIComponent)new UIModifierMenuItem((UIMenuAction)new UIMenuActionOpenMenu((UIComponent)filtersMenu, (UIComponent)filterModifierMenu), UIAlign.Center, new Color(), false), true);
                filtersMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
                filtersMenu.Add((UIComponent)new UIMenuItem("|DGBLUE|CLEAR FILTERS", (UIMenuAction)new UIMenuActionCallFunction(new UIMenuActionCallFunction.Function(__instance.ClearFilters)), UIAlign.Center, new Color(), false), true);
                filtersMenu.Add((UIComponent)new UIMenuItem("BACK", (UIMenuAction)new UIMenuActionOpenMenu((UIComponent)filtersMenu, (UIComponent)joinGameMenu), UIAlign.Center, new Color(), true), true);
                filtersMenu.Close();
                playOnlineGroup.Add((UIComponent)filtersMenu, false);
                foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
                    filterModifierMenu.Add((UIComponent)new UIMenuItemToggle(unlock.shortName, (UIMenuAction)null, new FieldBinding((object)unlock, "enabled", 0.0f, 1f, 0.1f), new Color(), new FieldBinding((object)unlock, "filtered", 0.0f, 1f, 0.1f), (List<string>)null, false, false), true);
                filterModifierMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
                filterModifierMenu.Add((UIComponent)new UIMenuItem("BACK", (UIMenuAction)new UIMenuActionOpenMenu((UIComponent)filterModifierMenu, (UIComponent)filtersMenu), UIAlign.Center, new Color(), true), true);
                filterModifierMenu.Close();
                playOnlineGroup.Add((UIComponent)filterModifierMenu, false);
                foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
                {
                    if (!(matchSetting.id == "workshopmaps") || Network.available)
                        hostGameSettingsMenu.AddMatchSetting(matchSetting, false, true);
                }
                hostGameSettingsMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
                hostGameSettingsMenu.Add((UIComponent)new UIModifierMenuItem((UIMenuAction)new UIMenuActionOpenMenu((UIComponent)hostGameSettingsMenu, (UIComponent)hostGameModifierMenu), UIAlign.Center, new Color(), false), true);
                hostGameSettingsMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
                hostGameSettingsMenu.Add((UIComponent)new UIMenuItem("BACK", (UIMenuAction)new UIMenuActionOpenMenu((UIComponent)hostGameSettingsMenu, (UIComponent)hostGameMenu), UIAlign.Center, new Color(), true), true);
                hostGameSettingsMenu.Close();
                playOnlineGroup.Add((UIComponent)hostGameSettingsMenu, false);
                foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
                    hostGameModifierMenu.Add((UIComponent)new UIMenuItemToggle(unlock.shortName, (UIMenuAction)null, new FieldBinding((object)unlock, "enabled", 0.0f, 1f, 0.1f), new Color(), (FieldBinding)null, (List<string>)null, false, false), true);
                hostGameModifierMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
                hostGameModifierMenu.Add((UIComponent)new UIMenuItem("BACK", (UIMenuAction)new UIMenuActionOpenMenu((UIComponent)hostGameModifierMenu, (UIComponent)hostGameSettingsMenu), UIAlign.Center, new Color(), true), true);
                hostGameModifierMenu.Close();
                playOnlineGroup.Add((UIComponent)hostGameModifierMenu, false);
                matchmaker.Close();
                playOnlineGroup.Add((UIComponent)matchmaker, false);
                playOnlineGroup.Close();
                Level.Add((Thing)playOnlineGroup);
                Graphics.fade = 0.0f;
                Layer l = new Layer("HUD2", -85, new Camera(), false, new Vec2());
                l.camera.width /= 2f;
                l.camera.height /= 2f;
                Layer.Add(l);
                Layer hud = Layer.HUD;
                Layer.HUD = l;
                Editor.gamepadMode = true;
                Layer.HUD = hud;
                
                // Start of setting private fields
                teamselect2type.GetField("_littleFont", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, littleFont);
                teamselect2type.GetField("_countdownScreen", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, countdownScreen);
                teamselect2type.GetField("_countdown", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, countdown);
                teamselect2type.GetField("_profiles", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, profiles);
                teamselect2type.GetField("_font", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, font);
                teamselect2type.GetField("_buttons", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, buttons);
                teamselect2type.GetField("_beam", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, beam);
                teamselect2type.GetField("_configGroup", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, configGroup);
                teamselect2type.GetField("_multiplayerMenu", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, multiplayerMenu);
                teamselect2type.GetField("_modifierMenu", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, modifierMenu);
                teamselect2type.GetField("_levelSelectMenu", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, levelSelectMenu);
                teamselect2type.GetField("_playOnlineGroup", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, playOnlineGroup);
                teamselect2type.GetField("_playOnlineMenu", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, playOnlineMenu);
                teamselect2type.GetField("_hostGameMenu", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, hostGameMenu);
                teamselect2type.GetField("_browseGamesMenu", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, browseGamesMenu);
                teamselect2type.GetField("_miniHostGameMenu", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, miniHostGameMenu);
                teamselect2type.GetField("_joinGameMenu", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, joinGameMenu);
                teamselect2type.GetField("_filtersMenu", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, filtersMenu);
                teamselect2type.GetField("_filterModifierMenu", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, filterModifierMenu);
                teamselect2type.GetField("_matchmaker", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, matchmaker);
                teamselect2type.GetField("_hostGameSettingsMenu", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, hostGameSettingsMenu);
                teamselect2type.GetField("_hostGameModifierMenu", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, hostGameModifierMenu);
                teamselect2type.GetField("_createGame", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, createGame);
                teamselect2type.GetField("_hostGame", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, hostGame);
                // End of setting private fields
                
                if (DuckNetwork.ShowUserXPGain() || !Unlockables.HasPendingUnlocks())
                    return false;
                MonoMain.pauseMenu = (UIComponent)new UIUnlockBox(Unlockables.GetPendingUnlocks().ToList<Unlockable>(), Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f);                

                return false;

            }
        }
    }
}