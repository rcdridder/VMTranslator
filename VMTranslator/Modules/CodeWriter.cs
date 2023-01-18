using VMTranslator.Interfaces;

namespace VMTranslator.Modules
{
    public class CodeWriter : ICodeWriter
    {
        private Dictionary<string, string> vmSegments = new Dictionary<string, string>()
        {
            { "argument", "ARG" },
            { "local", "LCL" },
            { "this", "THIS" },
            { "that", "THAT" }
        };
        private StreamWriter sw;
        private string vmFileName;
        private int labelCounter = 0;
        public CodeWriter(StreamWriter sw, string vmFileName)
        {
            this.sw = sw;
            this.vmFileName = vmFileName;
        }

        public void WriteArithmetic(string command)
        {
            switch (command)
            {
                case "add":
                    sw.WriteLine("//add");
                    AsmPop();
                    AsmGet();
                    sw.WriteLine("D=D+M");
                    AsmPush(); break;
                case "sub":
                    sw.WriteLine("//sub");
                    AsmPop();
                    AsmGet();
                    sw.WriteLine("D=M-D");
                    AsmPush(); break;
                case "neg":
                    sw.WriteLine("//neg");
                    sw.Write(
                        "@SP\n" +
                        "A=M-1\n" +
                        "D=M\n" +
                        "M=-D\n"); break;
                case "eq":
                    sw.WriteLine("//eq");
                    AsmCompare("D;JEQ"); break;
                case "gt":
                    sw.WriteLine("//gt");
                    AsmCompare("D;JGT"); break;
                case "lt":
                    sw.WriteLine("//lt");
                    AsmCompare("D;JLT"); break;
                case "and":
                    sw.WriteLine("//and");
                    AsmPop();
                    AsmGet();
                    sw.WriteLine("D=D&M");
                    AsmPush(); break;
                case "or":
                    sw.WriteLine("//or");
                    AsmPop();
                    AsmGet();
                    sw.WriteLine("D=D|M");
                    AsmPush(); break;
                case "not":
                    sw.WriteLine("//not");
                    sw.Write(
                        "@SP\n" +
                        "M=M-1\n" +
                        "@SP\n" +
                        "A=M\n" +
                        "D=!M\n" +
                        "M=D\n" +
                        "@SP\n" +
                        "M=M+1\n"); break;
                default:
                    throw new ArgumentException("Invalid arithmetic operation.");
            }
        }

        public void WriteBranching(string command, string dest)
        {
            if (command == "C_LABEL")
                sw.WriteLine($"({dest})");
            else if (command == "C_GOTO")
            {
                sw.Write(
                    "@SP\n" +
                    "M=M-1\n" +
                    "@SP\n" +
                    "A=M\n" +
                    "D=M\n" +
                    $"@{dest}\n" +
                    "0;JMP\n");

            }
            else if (command == "C_IF-GOTO")
            {
                sw.Write(
                    "@SP\n" +
                    "M=M-1\n" +
                    "@SP\n" +
                    "A=M\n" +
                    "D=M\n" +
                    $"@{dest}\n" +
                    "D;JNE\n");
            }
            else
                throw new ArgumentException("Invalid command.");
        }

        public void WritePushPop(string commandType, string segment, int index)
        {
            if (commandType == "C_PUSH")
            {
                switch (segment)
                {
                    case "constant":
                        sw.WriteLine($"//push constant {index}");
                        sw.Write(
                            $"@{index}\n" +
                            "D=A\n");
                        AsmPush(); break;
                    case "pointer":
                        sw.WriteLine($"//push pointer {index}");
                        if (index == 0)
                            sw.WriteLine("@THIS");
                        else if (index == 1)
                            sw.WriteLine("@THAT");
                        else
                            throw new ArgumentException("Invalid pointer value.");
                        sw.WriteLine("D=M");
                        AsmPush(); break;
                    case "static":
                        sw.WriteLine($"//push static {index}");
                        sw.Write(
                            $"@{vmFileName}.{index}\n" +
                            "D=M\n");
                        AsmPush(); break;
                    case "temp":
                        sw.WriteLine($"//push temp {index}");
                        int location = 5 + index;
                        sw.Write(
                            $"@{location}\n" +
                            "D=M\n");
                        AsmPush(); break;
                    default:
                        sw.WriteLine($"//push arg,lcl,this,that {index}");
                        sw.Write(
                        $"@{vmSegments[segment]}\n" +
                        "D=M\n" +
                        $"@{index}\n" +
                        "A=D+A\n" +
                        "D=M\n");
                        AsmPush(); break;
                }
            }
            else if (commandType == "C_POP")
            {
                switch (segment)
                {
                    case "pointer":
                        sw.WriteLine($"//pop pointer {index}");
                        AsmPop();
                        if (index == 0)
                            sw.WriteLine("@THIS");
                        else if (index == 1)
                            sw.WriteLine("@THAT");
                        else
                            throw new ArgumentException("Invalid pointer value.");
                        sw.WriteLine("M=D"); break;
                    case "static":
                        sw.WriteLine($"//pop static {index}");
                        AsmPop();
                        sw.Write(
                            $"@{vmFileName}.{index}\n" +
                            "M=D\n"); break;
                    case "temp":
                        sw.WriteLine($"//pop temp {index}");
                        int location = 5 + index;
                        AsmPop();
                        sw.Write(
                            $"@{location}\n" +
                            "M=D\n"); break;
                    default:
                        sw.WriteLine($"//pop arg, lcl, this, that {index}");
                        sw.Write(
                            $"@{vmSegments[segment]}\n" +
                            "D=M\n" +
                            $"@{index}\n" +
                            "D=D+A\n" +
                            "@addr\n" +
                            "M=D\n");
                        AsmPop();
                        sw.Write(
                            "@addr\n" +
                            "A=M\n" +
                            "M=D\n"); break;
                }
            }
            else
                throw new ArgumentException("This is not a C_PUSH or C_POP command.");
        }

        private void AsmPop() =>
            sw.Write(
                "@SP\n" +
                "M=M-1\n" +
                "@SP\n" +
                "A=M\n" +
                "D=M\n");

        private void AsmGet() =>
            sw.Write(
                "@SP\n" +
                "M=M-1\n" +
                "@SP\n" +
                "A=M\n");

        private void AsmPush() =>
            sw.Write(
                "@SP\n" +
                "A=M\n" +
                "M=D\n" +
                "@SP\n" +
                "M=M+1\n");

        private void AsmCompare(string jumpCommand)
        {
            AsmPop();
            AsmGet();
            sw.Write(
                "D=M-D\n" +
                $"@COMPARE{labelCounter}\n" +
                $"{jumpCommand}\n" +
                "@SP\n" +
                "A=M\n" +
                "M=0\n" +
                $"@STOPCOMPARE{labelCounter}\n" +
                "0;JMP\n" +
                $"@COMPARE{labelCounter})\n" +
                "@SP\n" +
                "A=M\n" +
                "M=-1\n" +
                $"(STOPCOMPARE{labelCounter})\n" +
                "@SP\n" +
                "M=M+1\n");
            labelCounter++;
        }
    }
}
