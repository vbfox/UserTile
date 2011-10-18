namespace BlackFox.UserTile.RegistryUtils
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;

    static class NativeMethods
    {
        const int KEY_SET_VALUE = 0x0002;

        [DllImport("advapi32.dll")]
        static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName,
            int[] lpReserved, ref int lpType, [Out] byte[] lpData, 
            ref int lpcbData);

        [DllImport("advapi32.dll")]
        static extern int RegSetValueEx(SafeRegistryHandle hKey, String lpValueName, int reserved,
            RegistryValueKind dwType, byte[] lpData, int cbData);

        [DllImport("advapi32.dll")]
        static extern int RegOpenKeyEx(SafeRegistryHandle hKey, String lpSubKey, int ulOptions, int samDesired,
            out SafeRegistryHandle hkResult);

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

        public static void SetBinaryValue(RegistryKey parent, string keyName, string valueName, byte[] data)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (keyName == null) throw new ArgumentNullException("keyName");
            if (data == null) throw new ArgumentNullException("data");

            SafeRegistryHandle result;

            var ret = RegOpenKeyEx(parent.Handle, keyName, 0, KEY_SET_VALUE, out result);
            if (ret != 0)
            {
                throw new Win32Exception(ret);
            }

            ret = RegSetValueEx(result, valueName, 0, RegistryValueKind.Binary, data, data.Length);
            if (ret != 0)
            {
                throw new Win32Exception(ret);
            }
        }
    }
}