/*
 * Copyright (c) 2011, Roncaglia Julien <julien@roncaglia.fr>
 * 
 * This program is open source; you can redistribute it and/or modify it under the terms of the BSD 2-Clause license 
 * as specified in COPYING.txt
 */
namespace BlackFox.UserTile
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using BlackFox.UserTile.RegistryUtils;

    class Program
    {
        static void Main(string[] args)
        {
            Main(Arguments.Parse(args));
        }

        static void Main(Arguments args)
        {
            if (args == null) throw new ArgumentNullException("args");

            switch (args.Operation)
            {
                case Operation.ListUsers:
                    ListUsers();
                    break;

                case Operation.ExportTileData:
                    ExportTileData(args.UserName, args.Path);
                    break;

                case Operation.ExportTile:
                    ExportTile(args.UserName, args.Path);
                    break;

                case Operation.SetUserTileData:
                    SetUserTileData(args.UserName, args.Path);
                    break;

                case Operation.SetUserTile:
                    SetUserTile(args.UserName, args.Path);
                    break;

                default:
                    ShowHelp(args);
                    break;
            }
        }

        static void SetUserTile(string userName, string path)
        {
            var userTile = new UserTileBinary { Format = "bmp", SourcePath = path };

            userTile.SetImageData(Image.FromFile(path));

            var userTileData = new MemoryStream();
            userTile.SaveTo(userTileData);
            SetUserTileData(userName, userTileData.ToArray());
        }

        static void SetUserTileData(string userName, string path)
        {
            var data = File.ReadAllBytes(path);

            SetUserTileData(userName, data);
        }

        static void SetUserTileData(string userName, byte[] data)
        {
            var userKeyName = LocalAccounts.GetUserKeyName(userName);

            // We can't really use the .Net API there as it expect to be run a normal user and seem to consider that
            // the registry is read-only while running as the Local System user.
            NativeMethods.SetBinaryValue(LocalAccounts.UsersKey, userKeyName, "UserTile", data);
        }

        static void ExportTile(string userName, string path)
        {
            var data = GetUserTileData(userName);

            var stream = new MemoryStream(data);
            var userTile = UserTileBinary.LoadFrom(stream);

            Console.WriteLine("UserTile is in {0} format", userTile.Format);
            Console.WriteLine("  Originaly in {0}", userTile.SourcePath);

            File.WriteAllBytes(path, userTile.ImageData);
        }

        static byte[] GetUserTileData(string userName)
        {
            var data = LocalAccounts.GetUserTileData(userName);
            if (data == null)
            {
                Console.WriteLine("No user tile stored");
                Environment.Exit(1);
            }
            return data;
        }

        static void ExportTileData(string userName, string path)
        {
            var data = LocalAccounts.GetUserTileData(userName);
            if (data == null)
            {
                Console.WriteLine("No user tile stored");
                Environment.Exit(1);
            }

            using (var stream = File.OpenWrite(path))
            {
                stream.Write(data, 0, data.Length);
            }
        }

        static void ListUsers()
        {
            Console.WriteLine("Users : ");
            foreach (var userName in LocalAccounts.GetUserNames())
            {
                Console.WriteLine("\t- {0}", userName);
            }
        }

        static void ShowHelp(Arguments args)
        {
            var asm = Assembly.GetExecutingAssembly().GetName();
            Console.WriteLine("{0} v{1}.{2}", asm.Name, asm.Version.Major, asm.Version.Minor);
            Console.WriteLine();
            args.OptionSet.WriteOptionDescriptions(Console.Out);
        }
    }
}
