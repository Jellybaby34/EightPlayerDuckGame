using System;
using System.Reflection;
using System.Collections.Generic;

namespace DuckGame.EightPlayerDuckGame
{
    public class PersonaEdits
    {
        // The persona list typically has just the 4 default personas. We want 8 dammit!
        // We generate our own persona list and set the "_personas" field to our new list.
        public static void AddNewPersonas()
        {
            // Backup of the array incase I change colour values and forget them
            /*
                new DuckPersona(new Vec3(255f, 255f, 255f)), // White
                new DuckPersona(new Vec3(125f, 125f, 125f)), // Grey
                new DuckPersona(new Vec3(247f, 224f, 90f)),  // Yellow
                new DuckPersona(new Vec3(205f, 107f, 29f)),  // Brown
                new DuckPersona(new Vec3(171f, 86f, 199f)),  // Purple
                new DuckPersona(new Vec3(163f, 206f, 39f)),  // Green
                new DuckPersona(new Vec3(49f, 162f, 242f)),  // Blue
                new DuckPersona(new Vec3(192f, 31f, 46f))      // Red
            */

            List<DuckPersona> personaList = new List<DuckPersona>()
            {   new DuckPersona(new Vec3(255f, 255f, 255f)), // White
                new DuckPersona(new Vec3(125f, 125f, 125f)), // Grey
                new DuckPersona(new Vec3(247f, 224f, 90f)),  // Yellow
                new DuckPersona(new Vec3(205f, 107f, 29f)),  // Brown
                new DuckPersona(new Vec3(171f, 86f, 199f)),  // Purple
                new DuckPersona(new Vec3(163f, 206f, 39f)),  // Green
                new DuckPersona(new Vec3(49f, 162f, 242f)),  // Blue
                new DuckPersona(new Vec3(192f, 31f, 46f))    // Red
            };

            Type personaType = typeof(Persona);
            FieldInfo personaField = personaType.GetField("_personas", BindingFlags.Static | BindingFlags.NonPublic );
            personaField.SetValue(null, personaList);
        }
    }
}