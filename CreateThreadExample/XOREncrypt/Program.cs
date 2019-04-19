using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XOREncrypt
{
    class Program
    {
        // Simple XOR routine
        static byte[] XorByteArray(byte[] origBytes, char[] cryptor)
        {
            byte[] result = new byte[origBytes.Length];
            int j = 0;
            for (int i = 0; i < origBytes.Length; i++)
            {
                // If we're at the end of the encryption key, move
                // pointer back to beginning.
                if (j == cryptor.Length - 1)
                {
                    j = 0;
                }
                // Perform the XOR operation
                byte res = (byte)(origBytes[i] ^ Convert.ToByte(cryptor[j]));
                // Store the result
                result[i] = res;
                // Increment the pointer of the XOR key
                j += 1;
            }
            // Return results
            return result;
        }
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("ERROR: Need to pass only the path to the shell code file to encrypt.");
                Environment.Exit(1);
            }
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Could not find path to shellcode bin file: {0}", args[0]);
                Environment.Exit(1);
            }
            byte[] shellcodeBytes = File.ReadAllBytes(args[0]);
            // This is the encryption key. If changed, must also be changed in the
            // project that runs the shellcode.
            char[] cryptor = new char[] { 'E', 'x', 'a', 'm', 'p', 'l', 'e', 'K', 'e', 'y', '\0' };
            byte[] encShellcodeBytes = XorByteArray(shellcodeBytes, cryptor);
            File.WriteAllBytes("encrypted.bin", encShellcodeBytes);
            Console.WriteLine("Wrote encoded binary to encrypted.bin.");
        }
    }
}
