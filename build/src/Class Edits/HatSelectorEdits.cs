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


/*
 * 		private int TeamIndexAdd(int index, int plus, bool alwaysThree = true)
		{
			if (alwaysThree && index < 4 && index >= 0)
			{
				index = 3;
			}
			int num = index + plus;
			if (num >= __instance.AllTeams().Count)
			{
				return num - __instance.AllTeams().Count + 3;
			}
			if (num < 3)
			{
				return __instance.AllTeams().Count + (num - 3);
			}
			return num;
		}

    // DuckGame.HatSelector
// Token: 0x06001D46 RID: 7494 RVA: 0x0011C5B4 File Offset: 0x0011A7B4
public override void Update()
{
	bool flag = true;
	if (__instance._profileBoxNumber < 0 || __instance.inputProfile == null)
	{
		return;
	}
	if (__instance.connection == DuckNetwork.localConnection && __instance.inputProfile.Pressed("ANY", false))
	{
		__instance.authority = ++__instance.authority;
	}
	if (Network.isActive && __instance.connection == DuckNetwork.localConnection && Profiles.experienceProfile != null && __instance.profile.linkedProfile == Profiles.experienceProfile)
	{
		if (MonoMain.pauseMenu != null)
		{
			__instance.authority = ++__instance.authority;
			if (MonoMain.pauseMenu is UILevelBox)
			{
				__instance._gettingXP = true;
				UILevelBox pauseMenu = MonoMain.pauseMenu as UILevelBox;
				__instance._gettingXPCompletion = (pauseMenu._dayProgress + pauseMenu._xpProgress) / 2f * 0.7f;
			}
			else
			{
				__instance._gettingXPCompletion = 0.7f;
				if (MonoMain.pauseMenu is UIFuneral)
				{
					__instance._gettingXPCompletion = 0.8f;
				}
				else if (MonoMain.pauseMenu is UIGachaBox)
				{
					__instance._gettingXPCompletion = 0.9f;
				}
			}
		}
		else
		{
			__instance._gettingXP = false;
			__instance._gettingXPCompletion = 0f;
		}
	}
	if (Network.isActive && __instance.connection != null && __instance.connection.status == ConnectionStatus.Disconnected)
	{
		__instance._gettingXP = false;
	}
	__instance._fade = Lerp.Float(__instance._fade, (__instance._open && !__instance._profileSelector.open && !__instance._roomEditor.open) ? 1f : 0f, 0.1f);
	__instance._blackFade = Lerp.Float(__instance._blackFade, __instance._open ? 1f : 0f, 0.1f);
	__instance._screen.Update();
	if (__instance._screen.transitioning)
	{
		return;
	}
	if (__instance._profileSelector.open || __instance._roomEditor.open)
	{
		return;
	}
	if (Profiles.IsDefault(__instance._profile))
	{
		flag = false;
	}
	if (Profiles.experienceProfile == null || Profiles.experienceProfile.GetTotalFurnitures() <= 0)
	{
		flag = false;
	}
	__instance._editRoomDisabled = false;
	if (NetworkDebugger.enabled)
	{
		flag = true;
		__instance._editRoomDisabled = false;
	}
	else if (!flag && Network.isActive)
	{
		flag = true;
		__instance._editRoomDisabled = true;
	}
	if (__instance.isArcadeHatSelector)
	{
		flag = false;
	}
	if (!__instance._open)
	{
		if (__instance._fade < 0.01f && __instance._closing)
		{
			__instance._closing = false;
			__instance._box.ReturnControl();
		}
		return;
	}
	if (__instance._profile.team == null)
	{
		return;
	}
	__instance._lcdFlashInc += Rando.Float(0.3f, 0.6f);
	__instance._lcdFlash = 0.9f + ((float)Math.Sin((double)__instance._lcdFlashInc) + 1f) / 2f * 0.1f;
	if (__instance._slideTo != 0f && __instance._slide != __instance._slideTo)
	{
		__instance._slide = Lerp.Float(__instance._slide, __instance._slideTo, 0.1f);
	}
	else if (__instance._slideTo != 0f && __instance._slide == __instance._slideTo)
	{
		__instance._slide = 0f;
		__instance._slideTo = 0f;
		__instance._teamSelection = __instance._desiredTeamSelection;
		Team allTeam = __instance.AllTeams()[(int)__instance._desiredTeamSelection];
		if (!Main.isDemo || allTeam.inDemo)
		{
			__instance.SelectTeam();
		}
	}
	if (__instance._upSlideTo != 0f && __instance._upSlide != __instance._upSlideTo)
	{
		__instance._upSlide = Lerp.Float(__instance._upSlide, __instance._upSlideTo, 0.1f);
	}
	else if (__instance._upSlideTo != 0f && __instance._upSlide == __instance._upSlideTo)
	{
		__instance._upSlide = 0f;
		__instance._upSlideTo = 0f;
		__instance._teamSelection = __instance._desiredTeamSelection;
		Team allTeam2 = __instance.AllTeams()[(int)__instance._desiredTeamSelection];
		if (!Main.isDemo || allTeam2.inDemo)
		{
			__instance.SelectTeam();
		}
	}
	if (__instance._selection == HSSelection.ChooseTeam)
	{
		if (__instance._desiredTeamSelection == __instance._teamSelection)
		{
			bool flag2 = false;
			if (__instance.inputProfile.Down("LEFT"))
			{
				if (__instance._desiredTeamSelection < 4)
				{
					__instance._desiredTeamSelection = (short)(__instance.AllTeams().Count - 1);
				}
				else if (__instance._desiredTeamSelection == 4)
				{
					__instance._desiredTeamSelection = (short)__instance.ControllerNumber();
				}
				else
				{
					__instance._desiredTeamSelection -= 1;
				}
				__instance._slideTo = -1f;
				flag2 = true;
				SFX.Play("consoleTick", 1f, 0f, 0f, false);
			}
			if (__instance.inputProfile.Down("RIGHT"))
			{
				if ((int)__instance._desiredTeamSelection >= __instance.AllTeams().Count - 1)
				{
					__instance._desiredTeamSelection = (short)__instance.ControllerNumber();
				}
				else if (__instance._desiredTeamSelection < 4)
				{
					__instance._desiredTeamSelection = 4;
				}
				else
				{
					__instance._desiredTeamSelection += 1;
				}
				__instance._slideTo = 1f;
				flag2 = true;
				SFX.Play("consoleTick", 1f, 0f, 0f, false);
			}
			if (__instance.inputProfile.Down("UP"))
			{
				if (__instance._desiredTeamSelection < 4)
				{
					__instance._desiredTeamSelection = 0;
				}
				else
				{
					__instance._desiredTeamSelection -= 3;
				}
				__instance._desiredTeamSelection -= 5;
				if (__instance._desiredTeamSelection < 0)
				{
					__instance._desiredTeamSelection += (short)(__instance.AllTeams().Count - 3);
				}
				if (__instance._desiredTeamSelection == 0)
				{
					__instance._desiredTeamSelection = (short)__instance.ControllerNumber();
				}
				else
				{
					__instance._desiredTeamSelection += 3;
				}
				__instance._upSlideTo = -1f;
				flag2 = true;
				SFX.Play("consoleTick", 1f, 0f, 0f, false);
			}
			if (__instance.inputProfile.Down("DOWN"))
			{
				if (__instance._desiredTeamSelection < 4)
				{
					__instance._desiredTeamSelection = 0;
				}
				else
				{
					__instance._desiredTeamSelection -= 3;
				}
				__instance._desiredTeamSelection += 5;
				if ((int)__instance._desiredTeamSelection >= __instance.AllTeams().Count - 3)
				{
					__instance._desiredTeamSelection -= (short)(__instance.AllTeams().Count - 3);
				}
				if (__instance._desiredTeamSelection == 0)
				{
					__instance._desiredTeamSelection = (short)__instance.ControllerNumber();
				}
				else
				{
					__instance._desiredTeamSelection += 3;
				}
				__instance._upSlideTo = 1f;
				flag2 = true;
				SFX.Play("consoleTick", 1f, 0f, 0f, false);
			}
			if (__instance.inputProfile.Pressed("SELECT", false) && !flag2)
			{
				if (__instance._profile.team.locked)
				{
					SFX.Play("consoleError", 1f, 0f, 0f, false);
				}
				else
				{
					SFX.Play("consoleSelect", 0.4f, 0f, 0f, false);
					__instance._selection = HSSelection.Main;
					__instance._screen.DoFlashTransition();
					__instance.ConfirmTeamSelection();
				}
			}
			if (__instance.inputProfile.Pressed("QUACK", false))
			{
				__instance._desiredTeamSelection = (short)__instance.GetTeamIndex(__instance._startingTeam);
				__instance._teamSelection = __instance._desiredTeamSelection;
				__instance.SelectTeam();
				__instance.ConfirmTeamSelection();
				SFX.Play("consoleCancel", 0.4f, 0f, 0f, false);
				__instance._selection = HSSelection.Main;
				__instance._screen.DoFlashTransition();
			}
		}
		Vec2 position = __instance.position;
		__instance.position = Vec2.Zero;
		__instance._screen.BeginDraw();
		float num = -18f;
		__instance._profile.persona.sprite.alpha = __instance._fade;
		__instance._profile.persona.sprite.color = Color.White;
		__instance._profile.persona.sprite.color = new Color(__instance._profile.persona.sprite.color.r, __instance._profile.persona.sprite.color.g, __instance._profile.persona.sprite.color.b);
		__instance._profile.persona.sprite.depth = 0.9f;
		__instance._profile.persona.sprite.scale = new Vec2(1f, 1f);
		Graphics.Draw(__instance._profile.persona.sprite, base.x + 70f, base.y + 60f + num, 0.9f);
		short num2 = 0;
		bool flag3 = false;
		if ((int)__instance._teamSelection >= __instance.AllTeams().Count)
		{
			num2 = __instance._teamSelection;
			__instance._teamSelection = (short)(__instance.AllTeams().Count - 1);
			flag3 = true;
		}
		int count = __instance.AllTeams().Count;
		for (int index = 0; index < 5; index++)
		{
			for (int index2 = 0; index2 < 7; index2++)
			{
				int plus = -3 + index2 + (index - 2) * 5;
				float x = base.x + 2f + (float)(index2 * 22) + -__instance._slide * 20f;
				float num3 = base.y + 37f + -__instance._upSlide * 20f;
				int index3 = __instance.TeamIndexAdd((int)__instance._teamSelection, plus, true);
				if (index3 == 3)
				{
					index3 = __instance.ControllerNumber();
				}
				Team allTeam3 = __instance.AllTeams()[index3];
				float num8 = base.x + 2f;
				float num9 = base.x + 2f + 154f;
				float num4 = base.x + (num9 - num8) / 2f - 9f;
				float num5 = Maths.Clamp((50f - Math.Abs(x - num4)) / 50f, 0f, 1f);
				float num6 = Maths.NormalizeSection(num5, 0.9f, 1f) * 0.8f + 0.2f;
				if (num5 < 0.5f)
				{
					num6 = Maths.NormalizeSection(num5, 0.1f, 0.2f) * 0.3f;
				}
				num6 = Maths.NormalizeSection(num5, 0f, 0.1f) * 0.3f;
				if (index == 0)
				{
					num3 -= num5 * 3f;
					if (__instance._upSlide < 0f)
					{
						num6 = Math.Abs(__instance._upSlide) * num6;
					}
					else
					{
						num6 = 0f;
					}
				}
				else if (index == 1)
				{
					num3 -= num5 * 3f;
					if (__instance._upSlide > 0f)
					{
						num6 = (1f - Math.Abs(__instance._upSlide)) * num6;
					}
				}
				else if (index == 2)
				{
					num3 -= num5 * 4f * (1f - Math.Abs(__instance._upSlide));
					if (__instance._upSlide > 0f)
					{
						num3 -= num5 * 3f * Math.Abs(__instance._upSlide);
					}
					else
					{
						num3 += num5 * 4f * Math.Abs(__instance._upSlide);
					}
					num6 = Maths.NormalizeSection(num5, 0.9f, 1f) * 0.7f + num6;
				}
				else if (index == 3)
				{
					float num7 = Math.Max(0f, __instance._upSlide);
					num3 += num5 * 4f * (1f - num7) + -num5 * 4f * num7;
					if (__instance._upSlide < 0f)
					{
						num6 = (1f - Math.Abs(__instance._upSlide)) * num6;
					}
				}
				else if (index == 4)
				{
					num3 += num5 * 4f;
					if (__instance._upSlide > 0f)
					{
						num6 = Math.Abs(__instance._upSlide) * num6;
					}
					else
					{
						num6 = 0f;
					}
				}
				if (num6 >= 0.01f)
				{
					Main.SpecialCode = "DRAW DUCK SET PROPS";
					__instance._profile.persona.sprite.alpha = __instance._fade;
					__instance._profile.persona.sprite.color = Color.White;
					__instance._profile.persona.sprite.color = new Color(__instance._profile.persona.sprite.color.r, __instance._profile.persona.sprite.color.g, __instance._profile.persona.sprite.color.b);
					__instance._profile.persona.sprite.depth = 0.9f;
					__instance._profile.persona.sprite.scale = new Vec2(1f, 1f);
					DuckRig.GetHatPoint(__instance._profile.persona.sprite.imageIndex);
					SpriteMap spriteMap = allTeam3.hat;
					Vec2 vec2 = allTeam3.hatOffset;
					if (allTeam3.locked)
					{
						spriteMap = __instance._lock;
						if (allTeam3.name == "Chancy")
						{
							spriteMap = __instance._goldLock;
						}
						vec2 = new Vec2(-10f, -10f);
					}
					bool flag4 = Main.isDemo && !allTeam3.inDemo;
					if (flag4)
					{
						spriteMap = __instance._demoBox;
					}
					spriteMap.depth = 0.95f;
					spriteMap.alpha = __instance._profile.persona.sprite.alpha;
					spriteMap.color = Color.White * num6;
					spriteMap.scale = new Vec2(1f, 1f);
					if (!flag4)
					{
						spriteMap.center = new Vec2(16f, 16f) + vec2;
					}
					if (index3 > 3 && __instance._fade > 0.01f)
					{
						Vec2 pixel = Vec2.Zero;
						if (flag4)
						{
							pixel = new Vec2(x + 2f, num3 + num + (float)(index * 20) - 20f + 1f);
						}
						else
						{
							pixel = new Vec2(x, num3 + num + (float)(index * 20) - 20f);
						}
						pixel = Maths.RoundToPixel(pixel);
						Graphics.Draw(spriteMap, pixel.x, pixel.y);
					}
					__instance._profile.persona.sprite.color = Color.White;
					spriteMap.color = Color.White;
					__instance._profile.persona.sprite.scale = new Vec2(1f, 1f);
					spriteMap.scale = new Vec2(1f, 1f);
				}
			}
		}
		__instance._font.alpha = __instance._fade;
		__instance._font.depth = 0.96f;
		Main.SpecialCode = "AFTER DRAW HAT";
		string str2 = "NO PROFILE";
		if (!Profiles.IsDefault(__instance._profile))
		{
			str2 = __instance._profile.name;
		}
		if (__instance._selection == HSSelection.ChooseProfile)
		{
			str2 = "> " + str2 + " <";
		}
		if (__instance._selection == HSSelection.ChooseTeam)
		{
			string text = "<              >";
			Vec2 pixel2 = new Vec2(base.x + __instance.width / 2f - __instance._font.GetWidth(text, false, null) / 2f, base.y + 60f + num);
			pixel2 = Maths.RoundToPixel(pixel2);
			__instance._font.Draw(text, pixel2.x, pixel2.y, Color.White, 0.95f, null, false);
		}
		Main.SpecialCode = "EXTRA AFTER DRAW HAT";
		string text2 = __instance._profile.team.name;
		if (text2 == Teams.Player1.name || text2 == Teams.Player2.name || text2 == Teams.Player3.name || text2 == Teams.Player4.name)
		{
			text2 = "No Team";
		}
		if (__instance._profile.team.locked)
		{
			text2 = "LOCKED";
		}
		__instance._font.scale = new Vec2(1f, 1f);
		float width = __instance._font.GetWidth(text2, false, null);
		Vec2 pos = new Vec2(base.x + __instance.width / 2f - width / 2f, base.y + 25f + num);
		pos = Maths.RoundToPixel(pos);
		__instance._font.Draw(text2, pos.x, pos.y, Color.LimeGreen * ((__instance._selection == HSSelection.ChooseTeam) ? 1f : 0.6f), 0.95f, null, false);
		Graphics.DrawLine(pos + new Vec2(-10f, 4f), pos + new Vec2(width + 10f, 4f), Color.White * 0.1f, 2f, 0.93f);
		Main.SpecialCode = "WAY AFTER DRAW HAT";
		string text4 = "@SELECT@";
		__instance._font.Draw(text4, base.x + 4f, base.y + 79f, new Color(180, 180, 180), 0.95f, __instance.profileInput, false);
		text4 = "@QUACK@";
		__instance._font.Draw(text4, base.x + 122f, base.y + 79f, new Color(180, 180, 180), 0.95f, __instance.profileInput, false);
		__instance._screen.EndDraw();
		__instance.position = position;
		if (flag3)
		{
			__instance._teamSelection = num2;
			return;
		}
	}
	else if (__instance._selection == HSSelection.Main)
	{
		if (__instance.inputProfile.Pressed("UP", false))
		{
			if (__instance._mainSelection > 0)
			{
				__instance._mainSelection -= 1;
				SFX.Play("consoleTick", 1f, 0f, 0f, false);
				if (__instance._editRoomDisabled && __instance._mainSelection == 2)
				{
					__instance._mainSelection = 1;
				}
			}
		}
		else if (__instance.inputProfile.Pressed("DOWN", false))
		{
			if (__instance._mainSelection < (flag ? 3 : 2))
			{
				__instance._mainSelection += 1;
				SFX.Play("consoleTick", 1f, 0f, 0f, false);
			}
			if (__instance._editRoomDisabled && __instance._mainSelection == 2)
			{
				__instance._mainSelection = 3;
			}
		}
		else if (__instance.inputProfile.Pressed("SELECT", false))
		{
			if (__instance._mainSelection == 1 && !Network.isActive)
			{
				__instance._profileSelector.Open(__instance._profile);
				SFX.Play("consoleSelect", 0.4f, 0f, 0f, false);
				__instance._fade = 0f;
				__instance._screen.DoFlashTransition();
			}
			else if (__instance._mainSelection == 0)
			{
				__instance._selection = HSSelection.ChooseTeam;
				SFX.Play("consoleSelect", 0.4f, 0f, 0f, false);
				__instance._screen.DoFlashTransition();
			}
			else if (__instance._mainSelection == (flag ? 3 : 2))
			{
				__instance._open = false;
				__instance._closing = true;
				SFX.Play("consoleCancel", 0.4f, 0f, 0f, false);
				__instance._selection = HSSelection.Main;
			}
			else if (flag && __instance._mainSelection == 2)
			{
				__instance._editingRoom = true;
				__instance._roomEditor.Open(__instance._profile);
				SFX.Play("consoleSelect", 0.4f, 0f, 0f, false);
				__instance._fade = 0f;
				__instance._screen.DoFlashTransition();
			}
		}
		else if (__instance.inputProfile.Pressed("QUACK", false))
		{
			__instance._open = false;
			__instance._closing = true;
			SFX.Play("consoleCancel", 0.4f, 0f, 0f, false);
			__instance._selection = HSSelection.Main;
		}
		else if (__instance._mainSelection == 1 && __instance.inputProfile.Pressed("SHOOT", false) && !Profiles.IsDefault(__instance._profile))
		{
			__instance._profileSelector.EditProfile(__instance._profile);
			SFX.Play("consoleSelect", 0.4f, 0f, 0f, false);
			__instance._fade = 0f;
			__instance._screen.DoFlashTransition();
		}
		__instance._screen.BeginDraw();
		__instance._font.scale = new Vec2(1f, 1f);
		string text3 = "@LWING@CUSTOM DUCK@RWING@";
		__instance._font.Draw(text3, Maths.RoundToPixel(new Vec2(__instance.width / 2f - __instance._font.GetWidth(text3, false, null) / 2f, 10f)), Color.White, 0.95f, null, false);
		if (Profiles.IsDefault(__instance._profile))
		{
			text3 = "Pick Profile";
		}
		else
		{
			text3 = __instance._profile.name;
		}
		float width2 = __instance._font.GetWidth(text3, false, null);
		Vec2 pixel3 = Maths.RoundToPixel(new Vec2(__instance.width / 2f - width2 / 2f, 39f));
		__instance._font.Draw(text3, pixel3, Colors.MenuOption * ((__instance._mainSelection == 1) ? 1f : 0.6f), 0.95f, null, false);
		if (__instance._mainSelection == 1)
		{
			Graphics.Draw(__instance._contextArrow, pixel3.x - 8f, pixel3.y);
		}
		if (flag)
		{
			text3 = "@RAINBOWICON@Edit Room";
			width2 = __instance._font.GetWidth(text3, false, null);
			pixel3 = Maths.RoundToPixel(new Vec2(__instance.width / 2f - width2 / 2f, 48f));
			__instance._font.Draw(text3, pixel3, __instance._editRoomDisabled ? Colors.SuperDarkBlueGray : (Colors.MenuOption * ((__instance._mainSelection == 2) ? 1f : 0.6f)), 0.95f, null, true);
			if (__instance._mainSelection == 2)
			{
				Graphics.Draw(__instance._contextArrow, pixel3.x - 8f, pixel3.y);
			}
		}
		if (!__instance._profile.team.hasHat)
		{
			text3 = "|MENUORANGE|Choose Hat";
		}
		else
		{
			text3 = "|LIME|" + __instance._profile.team.name + "|MENUORANGE| HAT";
		}
		width2 = __instance._font.GetWidth(text3, false, null);
		pixel3 = Maths.RoundToPixel(new Vec2(__instance.width / 2f - width2 / 2f, 30f));
		__instance._font.Draw(text3, pixel3, Color.White * ((__instance._mainSelection == 0) ? 1f : 0.6f), 0.95f, null, false);
		if (__instance._mainSelection == 0)
		{
			Graphics.Draw(__instance._contextArrow, pixel3.x - 8f, pixel3.y);
		}
		text3 = "EXIT";
		width2 = __instance._font.GetWidth(text3, false, null);
		pixel3 = Maths.RoundToPixel(new Vec2(__instance.width / 2f - width2 / 2f, (float)(50 + (flag ? 12 : 9))));
		__instance._font.Draw(text3, pixel3, Colors.MenuOption * ((__instance._mainSelection == (flag ? 3 : 2)) ? 1f : 0.6f), 0.95f, null, false);
		if (__instance._mainSelection == (flag ? 3 : 2))
		{
			Graphics.Draw(__instance._contextArrow, pixel3.x - 8f, pixel3.y);
		}
		string text5 = "@SELECT@";
		__instance._font.Draw(text5, 4f, 79f, new Color(180, 180, 180), 0.95f, __instance.profileInput, false);
		if (__instance._mainSelection == 1 && !Profiles.IsDefault(__instance._profile))
		{
			text5 = "@SHOOT@";
		}
		else
		{
			text5 = "@QUACK@";
		}
		__instance._font.Draw(text5, 122f, 79f, new Color(180, 180, 180), 0.95f, __instance.profileInput, false);
		__instance._consoleText.color = new Color(140, 140, 140);
		Graphics.Draw(__instance._consoleText, 30f, 18f);
		__instance._screen.EndDraw();
	}
}

*/
