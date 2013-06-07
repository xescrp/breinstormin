// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
using System;
using System.Runtime.InteropServices;

namespace breinstormin.profiler
{
  
    public class RegisterDLL
    {
        public RegisterDLL()
        {
 
        }

        private static bool initialized = false;

        internal static bool Register()
        {
            if (!initialized)
            {
                if (DllRegisterServer() == 0)
                    initialized = true;
                else
                    throw new Exception("Couldn't register profilerOBJ.dll");
            }
            return initialized;
        }

        [DllImport("profilerOBJ.dll")]
        private static extern int DllRegisterServer();
    }
}
