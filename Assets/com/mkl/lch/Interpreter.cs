using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lch.com.mkl.lch;
using lch.com.mkl.lch.variable;
using lch.com.mkl.lch.script;
using com.mkl.lch.elements;
using com.mkl.lch.instruction;
using com.mkl.lch.parser;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;

namespace com.mkl.lch
{
    public class Interpreter
    {

        bool error = false;

        IInstruction parseBlock(string line, string[] lines, int i, out int out_i, Dictionary<string, string> declaredVariables, List<IInstruction> previous)
        {
            i++;
            line = lines[i];
            List<IInstruction> instructions = new List<IInstruction>();

            ScopedInstruction si = new ScopedInstruction();


            while (!line.Contains("}"))
            {
                Console.WriteLine($"\nPrzetwarzanie w bloku: \"{line}\"");



                var lexer = new Lexer(line);
                var parser = new Parser(lexer, declaredVariables);



                try
                {
                    var ins = parser.ParseInstruction();
                    if (ins != null)
                        instructions.Add(ins);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Błąd: " + ex.Message);
                    error = true;
                }

                if (isThereABlockStart(line))
                {
                    if (instructions.Last() is WhileInstruction wi)
                    {
                        wi.setNestedScope(parseBlock(line, lines, i, out i, declaredVariables, instructions));
                    }
                    else
                    {
                        instructions.Add(parseBlock(line, lines, i, out i, declaredVariables, instructions));

                    }
                }

                i++;
                line = lines[i];

            }

            si.setInstructions(instructions);
            out_i = i;
            return si;

        }

        string message;
        


        public ExecutableScript parseScript(string[] lines) {
            Dictionary<string, string> declaredVariables = Lch.instance.getDefaultVariablesSet();
            //declaredVariables.Add("env", "object");

            List<IInstruction> instructions = new List<IInstruction>();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];


                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
                {
                    Console.WriteLine($"\nPuste lub komentarz");
                    continue;
                }


                Console.WriteLine($"\nPrzetwarzanie: \"{line}\"");


                var lexer = new Lexer(line);
                var parser = new Parser(lexer, declaredVariables);



                try
                {
                    var ins = parser.ParseInstruction();


                    if (ins != null)
                        instructions.Add(ins);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Błąd: " + ex.Message);
                    message = ex.Message + "\nAt" + i + ": " + line;
                    error = true;
                }

                if (isThereABlockStart(line))
                {
                    if (instructions.Last() is WhileInstruction wi)
                    {

                        wi.setNestedScope(parseBlock(line, lines, i, out i, declaredVariables, instructions));



                    }
                    else
                    {
                        instructions.Add(parseBlock(line, lines, i, out i, declaredVariables, instructions));

                    }
                }
            }

            if (error) {

                throw new Exception("Parsing failed!\n" + message);
            }

            ExecutableScript scriptOne = new ExecutableScript();
            scriptOne.blocks = instructions;

            return scriptOne;

        }
        




        private static readonly Regex whitespace = new Regex(@"\s+");
        static bool isThereABlockStart(string line)
        {

            string copy = new string(line.ToCharArray());

            whitespace.Replace(copy, "");

            Console.WriteLine("DATA " + copy);

            if (copy.StartsWith("{"))
                return true;

            if (copy.EndsWith("{"))
            {
                return true;
            }

            return false;

        }
    }
}
