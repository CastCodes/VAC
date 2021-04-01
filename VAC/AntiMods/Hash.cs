using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace VAC.AntiMods
{
    public static class HashAlgorithmExtensions
    {
        public static string CreateMd5ForFolder(string path)
        {
            List<string> list =
                ((IEnumerable<string>) Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
                .OrderBy<string, string>((Func<string, string>) (p => p)).ToList<string>();
            MD5 md5 = MD5.Create();
            for (int index = 0; index < list.Count; ++index)
            {
                byte[] numArray = File.ReadAllBytes(list[index]);
                if (index == list.Count - 1)
                    md5.TransformFinalBlock(numArray, 0, numArray.Length);
                else
                    md5.TransformBlock(numArray, 0, numArray.Length, numArray, 0);
            }

            return BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
        }
    }
}