using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMTranslator.Modules
{
    public class Parser
    {
        private StreamReader sr;
        public Parser(StreamReader sr)
        {
            this.sr = sr;
        }

        public string CommandType(string command)
        {
            throw new NotImplementedException();
        }

        public string Arg1(string command)
        {
            throw new NotImplementedException();
        }

        public int Arg2(string command)
        {
            throw new NotImplementedException();
        }
    }
}
