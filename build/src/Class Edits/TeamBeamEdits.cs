using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DuckGame.EightPlayerDuckGame
{
    public class TeamBeamEdits
    {
        [HarmonyPatch(typeof(TeamBeam), "TakeDuck")]
        public static class TeamBeam_TakeDuck_Prefix
        {
            static FieldInfo ducksField = AccessTools.Field(typeof(TeamBeam), "_ducks");
            static FieldInfo gunsField = AccessTools.Field(typeof(TeamBeam), "_guns");

            public static bool Prefix(TeamBeam __instance, Duck d)
            {
                if (!(ducksField.GetValue(__instance) as List<BeamDuck>).Any((BeamDuck t) => t.duck == d))
                {
                    float num;
                    if (d.y <= 59f)
                    {
                        num = 25f;
                    }
                    else if (60f <= d.y && d.y <= 119f)
                    {
                        num = 85f;
                    }
                    else
                    {
                        num = 145f;
                    }

                    SFX.Play("stepInBeam", 1f, 0f, 0f, false);
                    d.beammode = true;
                    d.immobilized = true;
                    d.crouch = false;
                    d.sliding = false;
                    if (d.holdObject != null)
                    {
                        (gunsField.GetValue(__instance) as List<Thing>).Add(d.holdObject);
                    }
                    d.ThrowItem(true);
                    d.solid = false;
                    d.grounded = false;
                    d.offDir = 1;
                    (ducksField.GetValue(__instance) as List<BeamDuck>).Add(new BeamDuck
                    {
                        duck = d,
                        entryHeight = num,
                        leaving = false,
                        entryDir = ((d.x < __instance.x) ? -1 : 1),
                        sin = new SinWave(0.1f, 0f),
                        sin2 = new SinWave(0.05f, 0f)
                    });
                }

                return false;
            }
        }
    }
}
