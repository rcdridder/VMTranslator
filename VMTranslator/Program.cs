using VMTranslator.Modules;

public class Program
{
    private static void Main(string[] args)
    {
        if (Path.GetExtension(args[0]) == ".vm")
        {
            string vmFileName = Path.GetFileNameWithoutExtension(args[0]);
            string outputFileName = $@"{Path.GetDirectoryName(args[0])}\{vmFileName}.asm";
            using CodeWriter codeWriter = new(outputFileName);
            codeWriter.vmFileName = vmFileName;
            TranslateFile(args[0], codeWriter);
        }
        else if (Directory.Exists(args[0]))
        {
            string outputFileName = $@"{args[0]}\{Path.GetFileName(args[0])}.asm";
            string[] vmFilesInDirectory = Directory.GetFiles($@"{args[0]}", "*.vm");
            using CodeWriter codeWriter = new(outputFileName);
            codeWriter.Initialization();
            foreach (string file in vmFilesInDirectory)
            {
                codeWriter.vmFileName = Path.GetFileNameWithoutExtension(file);
                TranslateFile(file, codeWriter);
            }
        }
        else
            throw new ArgumentException("Invalid file or directory.");
    }

    private static void TranslateFile(string filePath, CodeWriter codeWriter)
    {
        StreamReader sr = new(filePath);
        Parser parser = new(sr);

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
    }
}