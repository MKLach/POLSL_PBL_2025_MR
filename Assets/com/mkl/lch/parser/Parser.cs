using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lch.com.mkl.lch.elements;
using lch.com.mkl.lch;
using com.mkl.lch.elements;
using lch.com.mkl.lch.variable;
using com.mkl.lch.instruction;

namespace com.mkl.lch.parser
{
    class Parser
    {
        private const TokenType SYS_FUNCS_BRACKET = TokenType.LEFT_NBRACKET;
        private const TokenType SYS_FUNCS_CLOSING_BRACKET = TokenType.RIGHT_NBRACKET;

        private const TokenType METHOD_CALL_BRACKET = TokenType.LEFT_NBRACKET;
        private const TokenType METHOD_CALL_CLOSING_BRACKET = TokenType.RIGHT_NBRACKET;

        private const TokenType FUNCTION_CALL_BRACKET = TokenType.LEFT_SBRACKET;
        private const TokenType FUNCTION_CALL_CLOSING_BRACKET = TokenType.RIGHT_SBRACKET;


        static int cb_next_level = 0;
        static int unique_nest_level = 0;

        private readonly Lexer lexer;
        private Token currentToken;
        private Dictionary<string, string> declaredVariables;

        public Parser(Lexer lexer, Dictionary<string, string> declaredVariables)
        {
            this.declaredVariables = declaredVariables;
            this.lexer = lexer;
            currentToken = lexer.GetNextToken();
        }


        public List<IExecutableBlock> parseConcatAssignment(string name, string type)
        {
            List<IExecutableBlock> blocks = new List<IExecutableBlock>();
            List<String> variableNames = new List<string>();

            while (currentToken.Type != TokenType.Semicolon && currentToken.Type != TokenType.EOF)
            {
                Console.WriteLine(currentToken.Type + " " + currentToken.Value);

                if (currentToken.Type == TokenType.Identifier)
                {

                    if (!declaredVariables.TryGetValue(currentToken.Value, out string sourceType))
                        throw new Exception($"Zmienna '{currentToken.Value}' nie została zadeklarowana");

                    if (type == "object")
                        throw new Exception($"Nie można przypisać {sourceType} do {type}");

                    variableNames.Add(currentToken.Value);

                }
                else if (currentToken.Type == TokenType.String || currentToken.Type == TokenType.Number)
                {
                    string v = Lch.instance.getNextLiteralVariableName();

                    LiteralCreateVariableBlock cvb1 = new LiteralCreateVariableBlock(v, currentToken.Value, null, Lch.instance);



                    blocks.Add(cvb1);
                    variableNames.Add(v);
                    Console.WriteLine(v);
                }

                currentToken = lexer.GetNextToken();
            }


            blocks.Add(new ConcatAssignBlock(name, variableNames));



            Eat(TokenType.Semicolon);

            return blocks;
        }



        public String evaluateComparison()
        {


            if (currentToken.Type == TokenType.Identifier)
            {

                String left = currentToken.Value;

                Eat(TokenType.Identifier);

                TokenType operation = currentToken.Type;

                Eat(operation);

                String right = currentToken.Value;

                Eat(TokenType.Identifier);

                string v = Lch.instance.getNextLogicEvalVariableName();

                Lch.instance.addVariable(v, new LogicConversionVariable(v, left, operation, right, Lch.logicConvMeta));


                return v;

            }

            throw new Exception("Nie zmienna!");

        }

        public List<IExecutableBlock> parseLogicExpression(string name, string type, TokenType last = TokenType.Semicolon)
        {
            List<IExecutableBlock> blocks = new List<IExecutableBlock>();
            List<String> variableNames = new List<string>();
            List<int> conns = new List<int>();

            int nextType = 0;


            while (currentToken.Type != TokenType.Semicolon && currentToken.Type != TokenType.EOF && currentToken.Type != last)
            {
                Console.WriteLine(currentToken.Type);

                if (currentToken.Type == TokenType.Logic || (currentToken.Type == TokenType.Identifier && declaredVariables[currentToken.Value].Equals("boolean")))
                {
                    conns.Add(nextType);
                    variableNames.Add(currentToken.Value);
                }
                else if (currentToken.Type == TokenType.LOGIC_NOT)
                {
                    nextType |= 1;

                }
                else if (currentToken.Type == TokenType.LOGIC_OR)
                {
                    nextType |= 2;

                }
                else if (currentToken.Type == TokenType.LOGIC_AND)
                {
                    nextType |= 4;

                }
                else if (currentToken.Type == TokenType.Identifier)
                {

                    String convVariable = evaluateComparison();
                    conns.Add(nextType);
                    nextType = 0;
                    variableNames.Add(convVariable);

                    //if (currentToken.Type == last) {
                    continue;
                    //}

                }


                currentToken = lexer.GetNextToken();
            }


            blocks.Add(new LogicEvalBlock(name, variableNames, conns));


            Eat(last);
            return blocks;
        }

        public List<IExecutableBlock> parseNumbericAssignment(string name, string type)
        {
            List<IExecutableBlock> blocks = new List<IExecutableBlock>();

            object value = null;

            if (currentToken.Type == TokenType.Number)
            {
                if (type == "int")
                {
                    if (currentToken.Type != TokenType.Number)
                        throw new Exception("Oczekiwano liczby");

                    value = int.Parse(currentToken.Value);

                }
                else if (type == "float")
                {
                    if (currentToken.Type != TokenType.Number)
                        throw new Exception("Oczekiwano liczby");


                    value = float.Parse(currentToken.Value);

                }

                Eat(TokenType.Number);
                blocks.Add(new LiteralAssignBlock(name, value));

            }
            else if (currentToken.Type == TokenType.Identifier)
            {

                blocks.Add(new AssignBlock(name, currentToken.Value));
                Eat(TokenType.Identifier);
            }

            Eat(TokenType.Semicolon);


            return blocks;
        }


        public List<IExecutableBlock> ParseDeclaration()
        {
            bool assign_found = false;
            bool isLiteral = true;
            bool isNull = false;

            if (currentToken.Type != TokenType.Type)
                throw new Exception("Oczekiwano typu");

            string type = currentToken.Value;
            Eat(TokenType.Type);

            if (currentToken.Type != TokenType.Identifier)
                throw new Exception("Oczekiwano identyfikatora");

            string name = currentToken.Value;
            Eat(TokenType.Identifier);

            object value = null;

            if (declaredVariables.ContainsKey(name))
            {
                throw new Exception($"Zmienna '{name}' jest już zadeklarowana");
            }


            List<IExecutableBlock> blocks = new List<IExecutableBlock>();


            if (currentToken.Type == TokenType.Assign)
            {
                assign_found = true;
                Eat(TokenType.Assign);

                if (type == "string")
                {

                    LiteralCreateVariableBlock cvb = new LiteralCreateVariableBlock(name, "", type, Lch.instance);
                    blocks.Add(cvb);


                    var a = parseConcatAssignment(name, type);
                    blocks.AddRange(a);

                    declaredVariables.Add(name, type);

                    return blocks;
                }
                else if (type == "int")
                {

                    LiteralCreateVariableBlock cvb = new LiteralCreateVariableBlock(name, 0, type, Lch.instance);
                    blocks.Add(cvb);


                    var a = parseNumbericAssignment(name, type);

                    blocks.AddRange(a);

                    declaredVariables.Add(name, type);

                    return blocks;
                }
                else if (type == "boolean")
                {
                    LiteralCreateVariableBlock cvb = new LiteralCreateVariableBlock(name, "", type, Lch.instance);
                    blocks.Add(cvb);

                    var a = parseLogicExpression(name, type);


                    blocks.AddRange(a);

                    declaredVariables.Add(name, type);

                    return blocks;

                }

            }

            Eat(TokenType.Semicolon);
            declaredVariables.Add(name, type);



            if (isLiteral)
            {
                LiteralCreateVariableBlock cvb;
                if (isNull)
                {
                    cvb = new LiteralCreateVariableBlock(name, null, type, Lch.instance);
                }
                else
                {
                    cvb = new LiteralCreateVariableBlock(name, assign_found ? value : null, type, Lch.instance);
                }

                blocks.Add(cvb);

            }
            else
            {
                var cvb = new LiteralCreateVariableBlock(name, null, type, Lch.instance);
                blocks.Add(cvb);

                if (assign_found)
                {
                    var ab = new AssignBlock(name, (string)value);
                    blocks.Add(ab);
                }


            }


            Console.WriteLine($" {type} {name}" + (value != null ? $" = {value}" : "") + ";");
            return blocks;
        }




        public List<IExecutableBlock> ParseAssignmentV2()
        {

            bool isLiteral = true;
            bool isNull = false;
            string name = currentToken.Value;
            Eat(TokenType.Identifier);

            Eat(TokenType.Assign);

            if (!declaredVariables.TryGetValue(name, out string expectedType))
                throw new Exception($"Zmienna '{name}' nie została zadeklarowana");

            if (currentToken.Type == TokenType.Identifier)
            {
                isLiteral = false;
                if (!declaredVariables.TryGetValue(currentToken.Value, out string sourceType))
                    throw new Exception($"Zmienna '{currentToken.Value}' nie została zadeklarowana");

                if (sourceType != expectedType)
                    throw new Exception($"Nie można przypisać {sourceType} do {expectedType}");
            }
            else if (expectedType == "int" || expectedType == "float")
            {
                if (currentToken.Type != TokenType.Number)
                    throw new Exception($"Oczekiwano liczby dla typu {expectedType}");
            }
            else if (expectedType == "string")
            {
                if (currentToken.Type == TokenType.Null)
                {
                    isNull = true;
                }
                else
                if (currentToken.Type != TokenType.String)
                    throw new Exception($"Oczekiwano napisu dla typu string");
            }
            else
            {
                throw new Exception("Niepoprawna wartość po '='");
            }

            string value = currentToken.Value;
            Eat(currentToken.Type);

            Eat(TokenType.Semicolon);

            Console.WriteLine($"Przypisanie: {name} = {value};");

            List<IExecutableBlock> blocks = new List<IExecutableBlock>();

            if (isNull)
            {
                blocks.Add(new NullAssignBlock(name));

            }
            else

            if (isLiteral)
            {

                blocks.Add(new LiteralAssignBlock(name, value));

            }
            else
            {

                var ab = new AssignBlock(name, value);
                blocks.Add(ab);

            }

            return blocks;
        }

        public List<IExecutableBlock> ParsePrint()
        {
            Eat(TokenType.Print);
            Eat(SYS_FUNCS_BRACKET);

            if (currentToken.Type == TokenType.String)
            {
                string text = currentToken.Value;
                Eat(TokenType.String);
                Eat(SYS_FUNCS_CLOSING_BRACKET);
                Eat(TokenType.Semicolon);

                Console.WriteLine($" print {text}");

                List<IExecutableBlock> blocks = new List<IExecutableBlock>();
                string v = Lch.instance.getNextLiteralVariableName();

                blocks.Add(new LiteralCreateVariableBlock(v, text, null, Lch.instance));
                blocks.Add(new PrintBlock(v));
                return blocks;
            }
            else if (currentToken.Type == TokenType.Identifier)
            {
                string name = currentToken.Value;

                if (!declaredVariables.TryGetValue(name, out _))
                    throw new Exception($"Zmienna '{name}' nie została zadeklarowana");

                Eat(TokenType.Identifier);

                Eat(SYS_FUNCS_CLOSING_BRACKET);

                Eat(TokenType.Semicolon);
                Console.WriteLine($" print  {name}");
                List<IExecutableBlock> blocks = new List<IExecutableBlock>();
                blocks.Add(new PrintBlock(name));
                return blocks;
            }
            else
            {
                throw new Exception("Oczekiwano identyfikatora lub tekstu w cudzysłowie po 'println'");
            }
        }

        public List<IExecutableBlock> ParseSleep()
        {
            Eat(TokenType.Sleep);
            Eat(SYS_FUNCS_BRACKET);
            if (currentToken.Type == TokenType.Number)
            {
                string text = currentToken.Value;
                Eat(TokenType.Number);
                Eat(SYS_FUNCS_CLOSING_BRACKET);
                Eat(TokenType.Semicolon);
                Console.WriteLine($" sleep {text}");

                List<IExecutableBlock> blocks = new List<IExecutableBlock>();

                string v = Lch.instance.getNextLiteralVariableName();

                blocks.Add(new LiteralCreateVariableBlock(v, int.Parse(text), null, Lch.instance));
                blocks.Add(new SleepBlock(v));
                return blocks;
            }
            else if (currentToken.Type == TokenType.Identifier)
            {
                string name = currentToken.Value;

                if (!declaredVariables.TryGetValue(name, out _))
                    throw new Exception($"Zmienna '{name}' nie została zadeklarowana");

                Eat(TokenType.Identifier);
                Eat(SYS_FUNCS_CLOSING_BRACKET);
                Eat(TokenType.Semicolon);
                Console.WriteLine($" sleep  {name}");
                List<IExecutableBlock> blocks = new List<IExecutableBlock>();
                blocks.Add(new SleepBlock(name));
                return blocks;
            }
            else
            {
                throw new Exception("Oczekiwano identyfikatora lub tekstu w cudzysłowie po 'print'");
            }
        }

        public List<IExecutableBlock> ParseJump()
        {
            Eat(TokenType.Jump);
            Eat(SYS_FUNCS_BRACKET);
            if (currentToken.Type == TokenType.Number)
            {
                string text = currentToken.Value;
                Eat(TokenType.Number);
                Eat(SYS_FUNCS_CLOSING_BRACKET);
                Eat(TokenType.Semicolon);
                Console.WriteLine($" jump {text}");

                List<IExecutableBlock> blocks = new List<IExecutableBlock>();

                blocks.Add(new LiteralRelativeJumpBlock(int.Parse(text)));
                return blocks;
            }
            else if (currentToken.Type == TokenType.Identifier)
            {
                string name = currentToken.Value;

                if (!declaredVariables.TryGetValue(name, out _))
                    throw new Exception($"Zmienna '{name}' nie została zadeklarowana");

                Eat(TokenType.Identifier);
                Eat(SYS_FUNCS_CLOSING_BRACKET);
                Eat(TokenType.Semicolon);
                Console.WriteLine($" jump  {name}");
                List<IExecutableBlock> blocks = new List<IExecutableBlock>();
                blocks.Add(new RelativeJumpBlock(name));
                return blocks;
            }
            else
            {
                throw new Exception("Oczekiwano identyfikatora lub liczby po 'jump'");
            }
        }

        public List<IExecutableBlock> ParseBreak()
        {
            Eat(TokenType.Break);
            Eat(TokenType.Semicolon);

            List<IExecutableBlock> blocks = new List<IExecutableBlock>();

            blocks.Add(new LiteralRelativeJumpBlock(-1));

            return blocks;

        }

        private List<IExecutableBlock> ParseIf()
        {

            List<IExecutableBlock> blocks = new List<IExecutableBlock>();

            Eat(TokenType._IF);

            Eat(SYS_FUNCS_BRACKET);

            string v = Lch.instance.getNextLogicEvalVariableName();


            LiteralCreateVariableBlock cvb = new LiteralCreateVariableBlock(v, "", "boolean", Lch.instance);
            blocks.Add(cvb);

            var a = parseLogicExpression(v, "boolean", SYS_FUNCS_CLOSING_BRACKET);




            blocks.AddRange(a);

            var eval = blocks[blocks.Count - 1];

            blocks.RemoveAt(blocks.Count - 1);

            var _if = new IfBlock((LogicEvalBlock)eval);

            blocks.Add(_if);


            return blocks;
        }

        private List<IExecutableBlock> ParseWhile()
        {

            List<IExecutableBlock> blocks = new List<IExecutableBlock>();

            Eat(TokenType._WHILE2);

            Eat(SYS_FUNCS_BRACKET);

            string v = Lch.instance.getNextLogicEvalVariableName();


            LiteralCreateVariableBlock cvb = new LiteralCreateVariableBlock(v, "", "boolean", Lch.instance);
            blocks.Add(cvb);

            var a = parseLogicExpression(v, "boolean", SYS_FUNCS_CLOSING_BRACKET);

            blocks.AddRange(a);

            var eval = blocks[blocks.Count - 1];

            blocks.RemoveAt(blocks.Count - 1);

            var _while = new WhileBlock((LogicEvalBlock)eval);

            blocks.Add(_while);

            // blocks.Add(new LiteralRelativeJumpBlock(-2));

            return blocks;
        }



        private (string value, string type) ParseTerm()
        {
            if (currentToken.Type == TokenType.Number)
            {
                string val = currentToken.Value;
                Eat(TokenType.Number);
                return (val, val.Contains('.') ? "float" : "int");
            }
            else if (currentToken.Type == TokenType.Identifier)
            {
                string name = currentToken.Value;

                if (!declaredVariables.TryGetValue(name, out string type))
                    throw new Exception($"Zmienna '{name}' nie została zadeklarowana");

                Eat(TokenType.Identifier);
                return (name, type);
            }
            else
            {
                throw new Exception("Oczekiwano liczby lub identyfikatora");
            }
        }

        public List<IExecutableBlock> ParseAssignment(string name)
        {

            bool isLiteral = true;
            bool isNull = false;
            //string name = currentToken.Value;

            Eat(TokenType.Assign);
            List<IExecutableBlock> blocks = new List<IExecutableBlock>();

            if (!declaredVariables.TryGetValue(name, out string expectedType))
                throw new Exception($"Zmienna '{name}' nie została zadeklarowana");

            if (expectedType == "string")
            {
                var a = parseConcatAssignment(name, expectedType);
                blocks.AddRange(a);
                return blocks;
            }
            else if (expectedType == "boolean")
            {

                var a = parseLogicExpression(name, expectedType);
                blocks.AddRange(a);
                return blocks;
            }
            else if (expectedType == "int")
            {
                var a = parseNumbericAssignment(name, expectedType);
                blocks.AddRange(a);
                return blocks;
            }



            throw new Exception($"Zmienna nie jest stringiem");
        }

        private List<IVariableReference> parseArgs(TokenType closing)
        {
            List<IVariableReference> args = new List<IVariableReference>();
            while (currentToken.Type != closing)
            {


                string val = currentToken.Value;

                switch (currentToken.Type)
                {
                    case TokenType.Identifier:
                        args.Add(new VariableReference(val));
                        break;
                    case TokenType.Number:
                        args.Add(new IntLiteralVariableReference(int.Parse(val)));
                        break;
                    case TokenType.String:
                        args.Add(new StringLiteralVariableReference(val));
                        break;
                    case TokenType.FUNCTION_ARG_SEPARATOR:
                        break;

                }

                currentToken = lexer.GetNextToken();
            }

            return args;

        }


        private List<IExecutableBlock> parseMethodCall(string name, string methodName, bool raw)
        {
            List<IExecutableBlock> blocks = new List<IExecutableBlock>();

            Eat(METHOD_CALL_BRACKET);

            List<IVariableReference> args = new List<IVariableReference>();

            if (currentToken.Type == METHOD_CALL_CLOSING_BRACKET)
            {

            }
            else
            {
                args.AddRange(parseArgs(METHOD_CALL_CLOSING_BRACKET));
            }

            Eat(METHOD_CALL_CLOSING_BRACKET);
            Eat(TokenType.Semicolon);

            IExecutableBlock fcb;

            if (raw)
                fcb = new RawMethodCallBlock(new VariableReference(name), methodName, args);
            else 
                fcb = new MethodCallBlock(new VariableReference(name), methodName, args);

            
            blocks.Add(fcb);

            return blocks;
        }

        private List<IExecutableBlock> parseFunctionCall(string name, string methodName, bool raw)
        {
            List<IExecutableBlock> blocks = new List<IExecutableBlock>();

            Eat(FUNCTION_CALL_BRACKET);

            List<IVariableReference> args = new List<IVariableReference>();

            string modValueName = currentToken.Value;
            Eat(TokenType.Identifier);
            

            if (currentToken.Type == METHOD_CALL_CLOSING_BRACKET)
            {

            }
            else
            {
                args.AddRange(parseArgs(FUNCTION_CALL_CLOSING_BRACKET));
            }


            Eat(FUNCTION_CALL_CLOSING_BRACKET);

            Eat(TokenType.Semicolon);

            IExecutableBlock fcb;

            if (raw)
                fcb = new RawFunctionCallBlock(new VariableReference(name), new VariableReference(modValueName), methodName, args);
            else
                fcb = new FunctionCallBlock(new VariableReference(name), new VariableReference(modValueName), methodName, args);

            blocks.Add(fcb);

            return blocks;
        }




        private List<IExecutableBlock> parseCall(string name, bool raw)
        {

            Eat(raw ? TokenType.RAW_METHOD_CALL : TokenType.METHOD_CALL);

            string methodName = currentToken.Value;

            Eat(TokenType.Identifier);


            if (currentToken.Type == METHOD_CALL_BRACKET)
            {
                return parseMethodCall(name, methodName, raw);
            }
            else if (currentToken.Type == FUNCTION_CALL_BRACKET)
            {
                return parseFunctionCall(name, methodName, raw);
            }

            return null;
        }



        private IVariableReference getValueFromIdentifier(string vname)
        {
            return new VariableReference(vname);
        }

        private List<IExecutableBlock> parseIdentifier()
        {
            string name = currentToken.Value;
            Eat(TokenType.Identifier);


            if (currentToken.Type == TokenType.Assign)
            {
                return ParseAssignment(name);
            }
            else if (currentToken.Type == TokenType.METHOD_CALL)
            {
                return parseCall(name, false);
            }
            else if (currentToken.Type == TokenType.RAW_METHOD_CALL)
            {
                return parseCall(name, true);
            }


            return null;

        }

        private List<IExecutableBlock> ParseExtern() {
            
            Eat(TokenType.EXTERN);
            string name = currentToken.Value;
            Eat(TokenType.Identifier);

            declaredVariables.Add(name, "object");

            Eat(TokenType.Semicolon);

            return new List<IExecutableBlock>();
        }

        private void Eat(TokenType type)
        {
            if (currentToken.Type == type)
                currentToken = lexer.GetNextToken();
            else
                throw new Exception($"Błąd składni – oczekiwano {type}, otrzymano {currentToken.Type}");
        }


        public (List<IExecutableBlock>, int) ParseInstruction2()
        {
            List<IExecutableBlock> instructions = new List<IExecutableBlock>();
            int type = 0;
            if (currentToken.Type == TokenType.Type)
                instructions = ParseDeclaration();
            else if (currentToken.Type == TokenType.Identifier)
                instructions = parseIdentifier();
            else if (currentToken.Type == TokenType.Print)
                instructions = ParsePrint();
            else if (currentToken.Type == TokenType.Sleep)
                instructions = ParseSleep();
            else if (currentToken.Type == TokenType.Jump)
                instructions = ParseJump();
            else if (currentToken.Type == TokenType.EXTERN)
                instructions = ParseExtern();
            else if (currentToken.Type == TokenType._IF)
            {
                instructions = ParseIf();
                type = 1;
            }
            else if (currentToken.Type == TokenType._WHILE2)
            {
                instructions = ParseWhile();
                type = 2;
            }
            else if (currentToken.Type == TokenType.Break)
                instructions = ParseBreak();


            return (instructions, type);
        }

        public IInstruction ParseInstruction()
        {
            (List<IExecutableBlock>, int) t = ParseInstruction2();


            if (t.Item1.Count == 0) return null;

            if (t.Item2 == 0)
            {
                return new Instruction(t.Item1);
            }
            else if (t.Item2 == 1) {
                return new Instruction(t.Item1);
            } else if (t.Item2 == 2)
            {
                return new WhileInstruction(t.Item1);
            }

            return null;

        }
    }
}
