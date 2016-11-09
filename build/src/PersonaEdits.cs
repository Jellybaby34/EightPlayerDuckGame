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
            {   new DuckPersona(new Vec3(255f, 255f, 255f)), // White
                new DuckPersona(new Vec3(125f, 125f, 125f)), // Grey
                new DuckPersona(new Vec3(247f, 224f, 90f)),  // Yellow
                new DuckPersona(new Vec3(205f, 107f, 29f)),  // Brown
                new DuckPersona(new Vec3(171f, 86f, 199f)),  // Purple
                new DuckPersona(new Vec3(114f, 199f, 86f)),  // Green
                new DuckPersona(new Vec3(86f, 171f, 200f)),  // Blue
                new DuckPersona(new Vec3(235f, 0f, 0f))      // Red
            };

            Type typea = typeof(Persona);
            FieldInfo info2 = typea.GetField("_personas", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            info2.SetValue(null, objTemp);
        }
    }
}