using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMTranslator.Interfaces
{
    public interface IParser
    {

        /// <summary>
        /// Removes comments and whitespace from current line.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string CleanCurrentLine(string input);
        /// <summary>
        /// Returns current command type.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        string CommandType(string command);
        /// <summary>
        /// Parses first argument of current command (if command = C_Arithmetic returns command itself).
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        string Arg1(string command);
        /// <summary>
        /// Parses second argument of current command (if command = C_Push/Pop/Function/Call).
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        int Arg2(string command);
    }
}
