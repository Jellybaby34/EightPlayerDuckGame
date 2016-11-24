using System;
using System.Collections.Generic;
using System.Reflection;

namespace DuckGame.IncreasedPlayerLimit
{
    public static class OptionsEdits
    {

        public static void Initialize()
        {
            Type type = typeof(Duck);
            var assembly = Assembly.GetAssembly(type);
            var OptionsType = assembly.GetType("DuckGame.Options");
            dynamic optionsMenu = OptionsType.GetField("_optionsMenu", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic optionsData = OptionsType.GetField("_data", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            Injection.install(8, "UpdateLevelChange", "UpdateLevelChangeReplace");

            optionsMenu = new UIMenu("@WRENCH@OPTIONS@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f, "@DPAD@ADJUST @QUACK@BACK", (InputProfile)null, false);
            optionsMenu.Add((UIComponent)new UIMenuItemSlider("SFX VOLUME", (UIMenuAction)null, new FieldBinding((object)optionsData, "sfxVolume", 0.0f, 1f, 0.1f), 0.125f, new Color()), true);
            optionsMenu.Add((UIComponent)new UIMenuItemSlider("MUSIC VOLUME", (UIMenuAction)null, new FieldBinding((object)optionsData, "musicVolume", 0.0f, 1f, 0.1f), 0.125f, new Color()), true);
            optionsMenu.Add((UIComponent)new UIMenuItemToggle("SHENANIGANS", (UIMenuAction)null, new FieldBinding((object)optionsData, "shennanigans", 0.0f, 1f, 0.1f), new Color(), (FieldBinding)null, (List<string>)null, false, false), true);
            optionsMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
            optionsMenu.Add((UIComponent)new UIMenuItemToggle("FULLSCREEN", (UIMenuAction)null, new FieldBinding((object)optionsData, "fullscreen", 0.0f, 1f, 0.1f), new Color(), (FieldBinding)null, (List<string>)null, false, false), true);
            optionsMenu.Add((UIComponent)new UIText(" ", Color.White, UIAlign.Center, 0.0f, (InputProfile)null), true);
            optionsMenu.Add((UIComponent)new UIMenuItem("BACK", (UIMenuAction)new UIMenuActionCloseMenuCallFunction((UIComponent)optionsMenu, new UIMenuActionCloseMenuCallFunction.Function(OptionsEdits.close)), UIAlign.Center, new Color(), true), true);

            FieldInfo test = OptionsType.GetField("_optionsMenu", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            test.SetValue(null, optionsMenu);

            optionsMenu.Close();
        }

        public static void close()
        {
            Type OptionsType = Assembly.GetAssembly(typeof(Duck)).GetType("DuckGame.Options");
            MethodInfo optionsClose = OptionsType.GetMethod("OptionsMenuClosed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            optionsClose.Invoke(null, null);
        }
    }
}