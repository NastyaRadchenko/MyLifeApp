using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeClient
{
    public class ByteConvert
    {
        private static byte[] ConvertToBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.OpenReadStream().CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static byte[] GetBytesFromFile(IFormFile file)
        {
            byte[] bytes = new byte[file.Length];
            var resultInBytes = ConvertToBytes(file);
            Array.Copy(resultInBytes, bytes, resultInBytes.Length);
            return bytes;
        }

        public static string GetStringFromBytes(byte[] bytes)
        {
            var base64 = Convert.ToBase64String(bytes);
            var fileString = String.Format("data:image/png;base64,{0}", base64);
            return fileString;
        }
    }
}
