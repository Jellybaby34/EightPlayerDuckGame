using System;
using System.Reflection;
using System.Collections.Generic;

namespace DuckGame.IncreasedPlayerLimit
{
    public class PersonaEdits
    {
        public static void _personas()
        {
            List<DuckPersona> objTemp = new List<DuckPersona>()
            {   new DuckPersona(new Vec3(255f, 255f, 255f)),
                new DuckPersona(new Vec3(125f, 125f, 125f)),
                new DuckPersona(new Vec3(247f, 224f, 90f)),
                new DuckPersona(new Vec3(205f, 107f, 29f)),
                new DuckPersona(new Vec3(200f, 100f, 29f)),
                new DuckPersona(new Vec3(5f, 207f, 150f)),
                new DuckPersona(new Vec3(0f, 0f, 200f)),
                new DuckPersona(new Vec3(255f, 0f, 255f))
            };

            Type typea = typeof(Persona);
            FieldInfo info2 = typea.GetField("_personas", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            info2.SetValue(null, objTemp);
        }
    }
}