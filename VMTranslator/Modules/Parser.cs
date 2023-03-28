using VMTranslator.Interfaces;

namespace VMTranslator.Modules
{
    public class Parser : IParser
    {
        private string[] arithmeticCommands =
        {
            "add", "sub", "neg", "eq", "gt", "lt", "and", "or", "not"
        };
        private StreamReader sr;
        public Parser(StreamReader sr)
        {
            this.sr = sr;
        }

        public string CleanCurrentLine(string input)
        {
            if (input.Contains("//"))
                input = input.Remove(input.IndexOf("//"));
            input = input.ToLower();
            return input.Trim();
        }

        public string CommandType(string command)
        {
            string[] commandLine = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (commandLine.Length == 1)
            {
                if (commandLine[0] == "return")
                    return $"C_RETURN";
                else
                    return "C_ARITHMETIC";
            }
            else
                return $"C_{commandLine[0].ToUpper()}";
        }

        public string Arg1(string command) => command.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];

        public int Arg2(string command) => Convert.ToInt32(command.Substring(command.LastIndexOf(" ") + 1));
    }
}
