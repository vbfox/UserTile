namespace BlackFox.UserTile.Registry
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
                if (key == null)
                {
                    throw new ArgumentException(string.Format("The SubKey named {0} can't be opened", name), "names");
                }
            }

            return key;
        }
    }
}