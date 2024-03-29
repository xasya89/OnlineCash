﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCash
{
    public static class StringHelper
    {
        public static string CreateMD5(this string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }

        public static string ToStringWithDotted(this decimal value) => value.ToString().Replace(",", ".");
        public static string ToStringWithDotted(this decimal? value) => value.ToString()?.Replace(",", ".") ?? "";
    }
}
