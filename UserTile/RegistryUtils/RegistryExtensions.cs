/*
 * Copyright (c) 2011, Roncaglia Julien <julien@roncaglia.fr>
 * 
 * This program is open source; you can redistribute it and/or modify it under the terms of the BSD 2-Clause license 
 * as specified in COPYING.txt
 */

namespace BlackFox.UserTile.RegistryUtils
{
    using System;
    using Microsoft.Win32;

    static class RegistryExtensions
    {
        public static RegistryKey OpenSubKeys(this RegistryKey key, params string[] names)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (names == null) throw new ArgumentNullException("names");

            foreach (var name in names)
            {
                key = key.OpenSubKey(name);
                if (key == null) throw new ArgumentException(string.Format("The SubKey named {0} can't be opened", name), "names");
            }

            return key;
        }
    }
}
