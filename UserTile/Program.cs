namespace BlackFox.UserTile
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using Microsoft.Win32;
    using Microsoft.Win32.SafeHandles;

    static class Ex
    {
        public static RegistryKey OpenSubKeys(this RegistryKey key, params string[] names)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (names == null) throw new ArgumentNullException("names");

            foreach (var name in names)
            {
                key = key.OpenSubKey(name);
                if (key == null)
                {
                    throw new ArgumentException(string.Format("The SubKey named {0} can't be opened", name), "names");
                }
            }

            return key;
        }
    }

    static class NativeMethods
    {
        [DllImport("advapi32.dll")]
        public static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName,
            int[] lpReserved, ref int lpType, [Out] byte[] lpData, 
            ref int lpcbData);
    }

    class Program
    {
        static RegistryKey UsersKey
        {
            get { return Registry.LocalMachine.OpenSubKeys("SAM", "SAM", "Domains", "Account", "Users"); }
        }

        static RegistryKey UsersNameKey
        {
            get { return UsersKey.OpenSubKeys("Names"); }
        }

        static int GetValueKind(RegistryKey key, string name)
        {
            if (key == null) throw new ArgumentNullException("key");

            var lpType = 0;
            var lpcbData = 0;
            var result = NativeMethods.RegQueryValueEx(key.Handle, name, null, ref lpType, null, ref lpcbData); 
            if (result != 0)
            {
                throw new Win32Exception(result);
            }

            return lpType;
        }

        static RegistryKey UsernameToUserKey(string userName)
        {
            var namedKey = UsersNameKey.OpenSubKey(userName);

            if (namedKey == null)
            {
                throw new ArgumentException(
                    "There is no information stored in the registry for the local user " + userName, "userName");
            }

            var kind = GetValueKind(namedKey, null);
            var kindString = kind.ToString("X8");
            
            var hexKey = UsersKey.OpenSubKey(kindString);

            if (hexKey == null)
            {
                var message = string.Format("The local user {0} was found but not it's corresponding Hex key \"{1}\".",
                    userName, namedKey.GetValueKind(null));
                throw new ArgumentException(
                    message,
                    "userName");
            }

            return hexKey;
        }

        byte[] GetUserTileData(string userName)
        {
            var key = UsernameToUserKey(userName);

            return (byte[])key.GetValue("UserTile");
        }

        static IEnumerable<string> GetAvailableUsers()
        {
            return UsersNameKey.GetSubKeyNames();
        }

        static void Main(string[] args)
        {
            Main(Arguments.Parse(args));
        }

        static void Main(Arguments args)
        {
            var key = UsernameToUserKey("vbfox");
            Console.WriteLine(key.Name);
            Console.ReadLine();
        }
    }
}
