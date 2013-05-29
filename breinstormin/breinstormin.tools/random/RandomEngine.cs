using System;
using System.Collections.Generic;
using System.Text;


namespace breinstormin.tools.random
{
    public class RandomEngine
    {

        public static int GetRandomNumber(int min, int max) 
        {
            System.Security.Cryptography.RNGCryptoServiceProvider rng = 
                new System.Security.Cryptography.RNGCryptoServiceProvider(); 
            byte[] buffer = new byte[4]; 
            rng.GetBytes(buffer); 
            int result = BitConverter.ToInt32(buffer, 0); 
            return new System.Random(result).Next(min, max);
        }
    }
}
