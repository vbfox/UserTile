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
                case Operation.ShowHelp:
                    ShowHelp(args);
                    break;

                case Operation.ListUsers:
                    ListUsers();
                    break;

                case Operation.ExtractUserTileData:
                    ExportUserTileData(args.UserName, args.Path);
                    break;
            }
        }

        static void ExportUserTileData(string userName, string path)
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
