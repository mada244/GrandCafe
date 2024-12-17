using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Utils
{
    public class Utils
    {
        public static string GenerateHexId(int numBytes)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[numBytes];
                rng.GetBytes(data);
                return BitConverter.ToString(data).Replace("-", "").ToLower();
            }
        }
    }
}
