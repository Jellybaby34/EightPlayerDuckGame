using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DuckGame.IncreasedPlayerLimit
{
    public class ProfileBox2Edits
    {
        // This is a terrible practice as any update may change the assembly code and everything will burn.
        public static void rightRoomCorrection()
        {
            RuntimeHelpers.PrepareMethod(typeof(ProfileBox2).GetProperty("rightRoom", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetGetMethod(true).MethodHandle);

            IntPtr ptr = typeof(ProfileBox2).GetProperty("rightRoom", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetGetMethod(true).MethodHandle.GetFunctionPointer();// Where we want to change

            long srcAddress = (long)ptr; //memory location of caller + size of call sequence (5 bytes, 1 for the opcode, 4 for the offset)

            /* Assembly OpCodes for the bytes being injected. We replace the code with a modulo 2 and return true if 1 remains.
               Since doing actual modulus is some what complicated and I only want to do it with 2, we use AND 01 which does the
               same thing as value % 2.

                DuckGame.ProfileBox2::get_rightRoom - mov eax,[ecx+000001A0]
                DuckGame.ProfileBox2::get_rightRoom+6- and eax,01 { 1 }
                DuckGame.ProfileBox2::get_rightRoom+9- cmp eax,01 { 1 }
                DuckGame.ProfileBox2::get_rightRoom+C- je DuckGame.ProfileBox2::get_rightRoom+15
                DuckGame.ProfileBox2::get_rightRoom+E- mov eax,00000000 { 0 }
                DuckGame.ProfileBox2::get_rightRoom+13- ret 
                DuckGame.ProfileBox2::get_rightRoom+14- nop
                DuckGame.ProfileBox2::get_rightRoom+15- mov eax,00000001 { 1 }
                DuckGame.ProfileBox2::get_rightRoom+1A- ret 
            */

            byte[] redirector = new byte[] { 0x8B, 0x81, 0xA0, 0x01, 0x00, 0x00, 0x83, 0xE0, 0x01, 0x83, 0xF8, 0x01, 0x74, 0x07, 0xB8, 0x00, 0x00, 0x00, 0x00, 0xC3, 0x90, 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC3 };

            Marshal.Copy(redirector, 0, (IntPtr)srcAddress, redirector.Length);


        }
    }
}