﻿namespace BlackFox.UserTile
{
    using System;
    using System.Collections.Generic;
    using BlackFox.UserTile.Registry;
    using Microsoft.Win32;

    class Program
    {
        static RegistryKey UsersKey
        {
            get { return RegistryExtensions.OpenSubKeys(Registry.LocalMachine, "SAM", "SAM", "Domains", "Account", "Users"); }
        }

        static RegistryKey UsersNameKey
        {
            get { return UsersKey.OpenSubKeys("Names"); }
        }



        static RegistryKey UsernameToUserKey(string userName)
        {
            var namedKey = UsersNameKey.OpenSubKey(userName);

            if (namedKey == null)
            {
                throw new ArgumentException(
                    "There is no information stored in the registry for the local user " + userName, "userName");
            }

            var kind = NativeMethods.GetValueKind(namedKey, null);
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
