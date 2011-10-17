namespace BlackFox.UserTile
{
    using System;
    using System.IO;
    using System.Reflection;

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

                default:
                    ShowHelp(args);
                    break;
            }
        }

        static void ExportTile(string userName, string path)
        {
            var data = LocalAccounts.GetUserTileData(userName);
            var stream = new MemoryStream(data);
            var userTile = UserTileBinary.LoadFrom(stream);
        }

        static void ExportTileData(string userName, string path)
        {
            var data = LocalAccounts.GetUserTileData(userName);
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
