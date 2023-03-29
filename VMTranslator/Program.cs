using VMTranslator.Modules;

public class Program
{
    private static void Main(string[] args)
    {
        string vmFileName = Path.GetFileNameWithoutExtension(args[0]);
        string outputFilePath = $@"{Path.GetDirectoryName(args[0])}\{vmFileName}.asm";

        StreamReader sr = new(Path.GetFullPath(args[0]));
        Parser parser = new(sr);

        StreamWriter sw = new(outputFilePath);
        CodeWriter codeWriter = new(sw, vmFileName);

        while (!sr.EndOfStream)
        {
            string currentLine = sr.ReadLine();

            currentLine = parser.CleanCurrentLine(currentLine);
            if (currentLine == "")
                continue;
            string commandType = parser.CommandType(currentLine);
            switch (commandType)
            {
                case "C_ARITHMETIC": codeWriter.WriteArithmetic(currentLine); break;
                case "C_PUSH": case "C_POP": codeWriter.WritePushPop(commandType, parser.Arg1(currentLine), parser.Arg2(currentLine)); break;
                case "C_LABEL": case "C_GOTO": case "C_IF-GOTO": codeWriter.WriteBranching(commandType, parser.Arg1(currentLine)); break;
                case "C_FUNCTION": codeWriter.WriteFunction(parser.Arg1(currentLine), parser.Arg2(currentLine)); break;
                case "C_RETURN": codeWriter.WriteReturn(); break;
                case "C_CALL": codeWriter.WriteCall(parser.Arg1(currentLine), parser.Arg2(currentLine)); break;
                default: throw new ArgumentException("Invalid command.");
            }

        }
        sw.Close();
        Console.WriteLine($"{vmFileName} converted to assembly.");
    }
}