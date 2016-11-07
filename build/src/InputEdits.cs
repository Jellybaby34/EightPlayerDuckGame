using System;
using System.Reflection;

namespace DuckGame.IncreasedPlayerLimit
{
    public class InputEdits
    {
        // InitDefaultProfiles
        public static void InitDefaultProfiles()
        {

            for (int index = 0; index < 8; ++index)
            {
                InputProfile inputProfile = InputProfile.Add("MPPlayer" + (index + 1).ToString());
                inputProfile.Map(Input.GetDevice<GenericController>(index), "LEFT", 4, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "RIGHT", 8, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "UP", 1, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "DOWN", 2, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "JUMP", 4096, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "SHOOT", 16384, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "GRAB", 32768, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "QUACK", 8192, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "START", 16, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "STRAFE", 256, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "RAGDOLL", 512, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "LTRIGGER", 8388608, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "RTRIGGER", 4194304, false);
                inputProfile.Map(Input.GetDevice<GenericController>(index), "SELECT", 4096, false);
                if (index == 0)
                    InputProfile.active = inputProfile;
            }
            Input.ApplyDefaultMappings();
            InputProfile.Add("Blank");
        }

        // Initialize
        public static void Initialize()
        {
            Type inputtype = typeof(Input);
            FieldInfo _devicesField = inputtype.GetField("_devices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _devicesVal = _devicesField.GetValue(null);

            FieldInfo _gamePadsField = inputtype.GetField("_gamePads", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            dynamic _gamePadsVal = _devicesField.GetValue(null);


            // Player 5
            GenericController genericController5 = new GenericController(4);
            _devicesVal.Add((InputDevice)genericController5);
            _gamePadsVal.Add(genericController5);

            // Player 6
            GenericController genericController6 = new GenericController(5);
            _devicesVal.Add((InputDevice)genericController6);
            _gamePadsVal.Add(genericController6);

            // Player 7
            GenericController genericController7 = new GenericController(6);
            _devicesVal.Add((InputDevice)genericController7);
            _gamePadsVal.Add(genericController7);

            // Player 8
            GenericController genericController8 = new GenericController(7);
            _devicesVal.Add((InputDevice)genericController8);
            _gamePadsVal.Add(genericController8);
        }
    }
}
