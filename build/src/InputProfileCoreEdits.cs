using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace DuckGame.IncreasedPlayerLimit
{
    public class InputProfileCoreEdits
    {
        // God himself wept at this code
        // There must be a better way to override a getter because this is shocking
        
        public static void defaultProfiles()
        {
            Type inputtype = typeof(InputProfileCore);
            PropertyInfo _devicesProperty = inputtype.GetProperty("defaultProfiles", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo methodToReplace = _devicesProperty.GetGetMethod(true);
            MethodInfo methodToInject = typeof(InputProfileCoreEdits).GetMethod("defaultProfilesReplace", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            RuntimeHelpers.PrepareMethod(methodToReplace.MethodHandle);
            RuntimeHelpers.PrepareMethod(methodToInject.MethodHandle);

            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    int* inj = (int*)methodToInject.MethodHandle.Value.ToPointer() + 2;
                    int* tar = (int*)methodToReplace.MethodHandle.Value.ToPointer() + 2;
#if DEBUG
                    Console.WriteLine("\nVersion x84 Debug\n");

                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;

                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    //                    Console.WriteLine("\nVersion x84 Release\n");
                    *tar = *inj;
#endif
                }
                else
                {

                    long* inj = (long*)methodToInject.MethodHandle.Value.ToPointer() + 1;
                    long* tar = (long*)methodToReplace.MethodHandle.Value.ToPointer() + 1;
#if DEBUG
                    Console.WriteLine("\nVersion x64 Debug\n");
                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;


                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    //                    Console.WriteLine("\nVersion x64 Release\n");
                    *tar = *inj;
#endif
                }
            }


        }

        public List<InputProfile> defaultProfilesReplace()
        {
            return new List<InputProfile>() { InputProfile.core.Get("MPPlayer1"), InputProfile.core.Get("MPPlayer2"), InputProfile.core.Get("MPPlayer3"), InputProfile.core.Get("MPPlayer4"), InputProfile.core.Get("MPPlayer5"), InputProfile.core.Get("MPPlayer6"), InputProfile.core.Get("MPPlayer7"), InputProfile.core.Get("MPPlayer8") };
        }

    }
}