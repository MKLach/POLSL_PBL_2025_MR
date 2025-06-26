using lch.com.mkl.lch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mkl.lch.parser
{
    class Lexer
    {
        private readonly string[] types;

        private readonly string input;
        private int pos = 0;

        public Lexer(string input)
        {
            this.types = Lch.instance.builtinTypes;
            this.input = input.Trim();
        }

        public Token GetNextToken()
        {
            SkipWhitespace();

            if (pos >= input.Length)
                return new Token(TokenType.EOF, "");

            char current = input[pos];

            if (current == ';')
            {
                pos++;
                return new Token(TokenType.Semicolon, ";");
            }

            if (current == '=')
            {
                pos++;
                return new Token(TokenType.Assign, "=");
            }

            if (current == '+')
            {
                pos++;
                return new Token(TokenType.Plus, "+");
            }

            

            if (current == '*')
            {
                pos++;
                return new Token(TokenType.Star, "*");
            }

            if (current == '/')
            {
                pos++;
                return new Token(TokenType.FrontSlass, "/");
            }

            if (current == '(')
            {
                pos++;
                return new Token(TokenType.LEFT_NBRACKET, "(");
            }

            if (current == ')')
            {
                pos++;
                return new Token(TokenType.RIGHT_NBRACKET, ")");
            }

            if (current == '{')
            {
                pos++;
                return new Token(TokenType.LEFT_CBRACKET, "{");
            }

            if (current == '}')
            {
                pos++;
                return new Token(TokenType.RIGHT_CBRACKET, "}");
            }

            if (current == '[')
            {
                pos++;
                return new Token(TokenType.LEFT_SBRACKET, "[");
            }

            if (current == ']')
            {
                pos++;
                return new Token(TokenType.RIGHT_SBRACKET, "]");
            }

            if (current == ',')
            {
                pos++;
                return new Token(TokenType.FUNCTION_ARG_SEPARATOR, ",");
            }

            if (current == '"')
            {
                string str = ReadString();
                return new Token(TokenType.String, str);
            }

            if (current == '-') {
                
                if(char.IsDigit(input[pos + 1])){
                    pos++;
                    string number = ReadNumber();
                    return new Token(TokenType.Number, '-' + number);
                }

                if(input[pos + 1] == '>') {

                    pos++;
                    pos++;
                    return new Token(TokenType.METHOD_CALL, "->");
                }


                pos++;
                return new Token(TokenType.Minus, "-");

            }


            if (current == ':') {

                if (input[pos + 1] == ':') {
                    pos++;
                    pos++;
                    return new Token(TokenType.RAW_METHOD_CALL, "::");
                }
                pos++;
            }


            if (char.IsDigit(current))
            {
                string number = ReadNumber();
                return new Token(TokenType.Number, number);
            }

            if (current == '\'') {
                string _char = ReadChar();
                //Console.WriteLine(_char);
                return new Token(TokenType.Character, _char);
            }

            if (char.IsLetter(current) || current == '_')
            {
                string word = ReadWord();

                //if (word == "println")
                //    return new Token(TokenType.Print, word);
                
                
                if (word == "and")
                    return new Token(TokenType.LOGIC_AND, word);
                else if (word == "or")
                    return new Token(TokenType.LOGIC_OR, word);
                else if (word == "not")
                    return new Token(TokenType.LOGIC_NOT, word);
                
                
                else if (word == "sleep")
                    return new Token(TokenType.Sleep, word);
                else if (word == "jump")
                    return new Token(TokenType.Jump, word);
                else if (word == "break")
                    return new Token(TokenType.Break, word);
                else if (word == "extern")
                    return new Token(TokenType.EXTERN, word);

                else if (word == "true" || word == "false")
                    return new Token(TokenType.Logic, word);
               
                
                else if (word == "eq")
                    return new Token(TokenType.NUMBER_LOGIC_EQ, word);
                else if (word == "neq")
                    return new Token(TokenType.NUMBER_LOGIC_NEQ, word);
                else if (word == "gt")
                    return new Token(TokenType.NUMBER_LOGIC_GT, word);
                else if (word == "gte")
                    return new Token(TokenType.NUMBER_LOGIC_GTE, word);
                else if (word == "lt")
                    return new Token(TokenType.NUMBER_LOGIC_LT, word);
                else if (word == "lte")
                    return new Token(TokenType.NUMBER_LOGIC_LTE, word);

                else if (word == "if")
                    return new Token(TokenType._IF, word);

                else if (word == "while")
                    return new Token(TokenType._WHILE2, word);

               
                else if (Array.Exists(types, t => t == word))
                    return new Token(TokenType.Type, word);
                else
                    return new Token(TokenType.Identifier, word);
            }

            throw new Exception($"Nieznany znak: '{current}'");
        }

        private void SkipWhitespace()
        {
            while (pos < input.Length && char.IsWhiteSpace(input[pos]))
                pos++;
        }

        private string ReadWord()
        {
            int start = pos;
            while (pos < input.Length && (char.IsLetterOrDigit(input[pos]) || input[pos] == '_'))
                pos++;
            return input.Substring(start, pos - start);
        }

        private string ReadChar()
        {
            pos++; // skip opening '
            int start = pos;

            while (pos < input.Length && input[pos] != '\'')
                pos++;

            if (pos >= input.Length)
                throw new Exception("Niezamknięty pojedynczy cudzysłów w napisie");

            if (pos - start != 1) {
                throw new Exception("Wartość typu char musi mieć dokładnie jeden znak");
            }

            Console.WriteLine(pos-start);
            string result = input.Substring(start, pos - start);
            pos++; // skip closing '
            return result;
        }

        private string ReadNumber()
        {
            int start = pos;
            bool dotSeen = false;

            while (pos < input.Length && (char.IsDigit(input[pos]) || (!dotSeen && input[pos] == '.')))
            {
                if (input[pos] == '.')
                    dotSeen = true;

                pos++;
            }

            return input.Substring(start, pos - start);
        }

        private string ReadString()
        {
           
            pos++; // skip opening "
            int start = pos;

            while (pos < input.Length && input[pos] != '"')
                pos++;

            if (pos >= input.Length)
                throw new Exception("Niezamknięty cudzysłów w napisie");

            string result = input.Substring(start, pos - start);
            pos++; // skip closing "
            return result;
        }
    }
}
