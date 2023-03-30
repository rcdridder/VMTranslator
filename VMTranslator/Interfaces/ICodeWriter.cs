namespace VMTranslator.Interfaces
{
    public interface ICodeWriter
    {
        /// <summary>
        /// Writes arithmetic command in HACK-assembly.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        void WriteArithmetic(string command);
        /// <summary>
        /// Writes push/pop command in HACK-assembly.
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="segement"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        void WritePushPop(string commandType, string segement, int index);
        /// <summary>
        /// Writes branching command in HACK-assembly.
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="segment"></param>
        /// <param name="index"></param>
        void WriteBranching(string commandType, string dest);
        /// <summary>
        /// Writes function declaration in HACK-assembly.
        /// </summary>
        /// <param name="functioName"></param>
        /// <param name="nVars"></param>
        void WriteFunction(string functioName, int nVars);
        /// <summary>
        /// Writes call command in HACK-assembly. 
        /// </summary>
        /// <param name="functioName"></param>
        /// <param name="nArgs"></param>
        void WriteCall(string functioName, int nArgs);
        /// <summary>
        /// Writes return command in HACK-assembly.
        /// </summary>
        void WriteReturn();
    }
}
