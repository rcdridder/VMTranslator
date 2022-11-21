using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMTranslator.Interfaces
{
    public interface ICodeWriter
    {
        /// <summary>
        /// Returns arithmetic command in HACK-assembly.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        string WriteArithmetic(string command);
        /// <summary>
        /// Returns push/pop command in HACK-assembly.
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="segement"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        string WritePushPop(string commandType, string segement, int index);
    }
}
