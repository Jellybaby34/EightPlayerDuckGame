using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DuckGame.IncreasedPlayerLimit
{
    public class NMVoteToSkipEdits
    {

        public static void Activate()
        {
            RuntimeHelpers.PrepareMethod(typeof(NMVoteToSkip).GetMethod("Activate").MethodHandle);

            IntPtr ptr = typeof(NMVoteToSkip).GetMethod("Activate").MethodHandle.GetFunctionPointer();// Where we want to change

            long srcAddress = (long)ptr + 18; //memory location of caller + offset of the index < 4 instruction. We change the value from 4 to 8

            byte[] redirector = new byte[] {
                        0x08   // The hex code for the value 8
                    };

            Marshal.Copy(redirector, 0, (IntPtr)srcAddress, redirector.Length);

        }
    }
}