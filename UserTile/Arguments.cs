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

        public static Arguments Parse(string[] args)
        {
            var result = new Arguments();
            result.ParseCore(args);
            return result;
        }

        void ParseCore(IEnumerable<string> args)
        {
            additionalArgs = optionSet.Parse(args);
        }
    }
}