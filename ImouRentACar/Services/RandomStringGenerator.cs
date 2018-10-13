using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Services
{
    public class RandomStringGenerator
    {
        private readonly Random random = new Random();

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
