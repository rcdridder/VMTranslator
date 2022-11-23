using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMTranslator.Interfaces;

namespace VMTranslator.Modules
{
    public class Parser: IParser
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
            string commandType;
            if (arithmeticCommands.Contains(command))
                return "C_ARITHMETIC";
            else
                commandType = command.Substring(0, command.Length - command.Substring(command.IndexOf(" ")).Length);
            switch(commandType)
            {
                case "push": return "C_PUSH";
                case "pop": return "C_POP";
                default: throw new ArgumentException("Invalid command type.");
            }
        }

        public string Arg1(string command)
        {
            if(arithmeticCommands.Contains(command))
                return command.Substring(command.IndexOf(" ") + 1);
            else
                return command.Substring(command.IndexOf(" ") + 1, command.LastIndexOf(" ") - command.IndexOf(" ") - 1);
        }

        public int Arg2(string command) => Convert.ToInt32(command.Substring(command.LastIndexOf(" ") + 1));
    }
}
