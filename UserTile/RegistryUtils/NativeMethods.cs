namespace BlackFox.UserTile.Registry
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;

    static class NativeMethods
    {
        [DllImport("advapi32.dll")]
        static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName,
            int[] lpReserved, ref int lpType, [Out] byte[] lpData, 
            ref int lpcbData);

        public static int GetValueKind(RegistryKey key, string name)
        {
            if (key == null) throw new ArgumentNullException("key");

            var lpType = 0;
            var lpcbData = 0;
            var result = RegQueryValueEx(key.Handle, name, null, ref lpType, null, ref lpcbData);
            if (result != 0)
            {
                throw new Win32Exception(result);
            }

            return lpType;
        }
    }
}