/*
 * Copyright (c) 2011, Roncaglia Julien <julien@roncaglia.fr>
 * 
 * This program is open source; you can redistribute it and/or modify it under the terms of the BSD 2-Clause license 
 * as specified in COPYING.txt
 */

namespace BlackFox.UserTile
{
    using System;
    using System.Collections.Generic;
    using BlackFox.UserTile.RegistryUtils;
    using Microsoft.Win32;

    static class LocalAccounts
    {
        public static RegistryKey UsersKey
        {
            get { return Registry.LocalMachine.OpenSubKeys("SAM", "SAM", "Domains", "Account", "Users"); }
        }

        static RegistryKey UsersNameKey
        {
            get { return UsersKey.OpenSubKeys("Names"); }
        }

        public static string GetUserKeyName(string userName)
        {
            var namedKey = UsersNameKey.OpenSubKey(userName, RegistryKeyPermissionCheck.ReadWriteSubTree);

            if (namedKey == null)
            {
                var message = string.Format("There is no information stored in the registry for the local user {0}",
                    userName);
                throw new ArgumentException(message, "userName");
            }

            var kind = NativeMethods.GetValueKind(namedKey, null);
            return kind.ToString("X8");
        }

        public static RegistryKey GetUserKey(string userName, bool writable)
        {
            var kindString = GetUserKeyName(userName);

            var hexKey = UsersKey.OpenSubKey(kindString);

            if (hexKey == null)
            {
                var message = string.Format("The local user {0} was found but not it's corresponding Hex key \"{1}\".",
                    userName, kindString);
                throw new ArgumentException(message, "userName");
            }

            return hexKey;
        }

        public static byte[] GetUserTileData(string userName)
        {
            var key = GetUserKey(userName, false);

            return (byte[])key.GetValue("UserTile");
        }

        public static IEnumerable<string> GetUserNames()
        {
            return UsersNameKey.GetSubKeyNames();
        }
    }
}
