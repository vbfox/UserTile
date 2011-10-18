/*
 * Copyright (c) 2011, Roncaglia Julien <julien@roncaglia.fr>
 * 
 * This program is open source; you can redistribute it and/or modify it under the terms of the BSD 2-Clause license 
 * as specified in COPYING.txt
 */

namespace BlackFox.UserTile
{
    using System.Collections.Generic;
    using NDesk.Options;

    class Arguments
    {
        readonly OptionSet optionSet;
        List<string> additionalArgs;

        Operation operation = Operation.ShowHelp;

        public Arguments()
        {
            optionSet = new OptionSet()
                .Add("l|list", "List all local users", v => operation = Operation.ListUsers)
                .Add("u|user=", "Specify the user to query/modify", v => UserName = v)
                .Add("x|export-data=", "Export the tile binary data to the specified file", v =>
                {
                    operation = Operation.ExportTileData;
                    Path = v;
                })
                .Add("e|export|export-tile=", "Export the user picture to the specified file", v =>
                {
                    operation = Operation.ExportTile;
                    Path = v;
                })
                .Add("set-data=", "Set the user tile binary data to the specified file content", v =>
                {
                    operation = Operation.SetUserTileData;
                    Path = v;
                })
                .Add("s|set|set-tile=", "Set the user tile to the specified picture", v =>
                {
                    operation = Operation.SetUserTile;
                    Path = v;
                })
                .Add("h|?|help", "Show this help", v => operation = Operation.ShowHelp);
        }

        public Operation Operation
        {
            get { return operation; }
        }

        public IEnumerable<string> AdditionalArgs
        {
            get { return additionalArgs; }
        }

        public OptionSet OptionSet
        {
            get { return optionSet; }
        }

        public string Path { get; private set; }

        public string UserName { get; private set; }

        public static Arguments Parse(string[] args)
        {
            var result = new Arguments();
            result.ParseCore(args);
            return result;
        }

        void ParseCore(IEnumerable<string> args)
        {
            additionalArgs = OptionSet.Parse(args);
        }
    }
}
