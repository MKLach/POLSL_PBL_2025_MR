using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mkl.lch.parser
{
    public enum TokenType { Type, Identifier, Semicolon, Assign, Number, String, Boolean, Logic, EOF, Print, Character, Null, Sleep, Plus, Minus, Star, FrontSlass, LEFT_NBRACKET, RIGHT_NBRACKET, _IF, LEFT_CBRACKET, RIGHT_CBRACKET, Jump, LOGIC_AND, LOGIC_OR, LOGIC_NOT, 
        
        NUMBER_LOGIC_GT,
        NUMBER_LOGIC_GTE,
        
        NUMBER_LOGIC_LT,
        NUMBER_LOGIC_LTE,

        NUMBER_LOGIC_EQ,
        NUMBER_LOGIC_NEQ,


        LEFT_SBRACKET,
        RIGHT_SBRACKET,
        Break,

        METHOD_CALL,
        FUNCTION_ARG_SEPARATOR,
        _WHILE2,

        RAW_METHOD_CALL,
        EXTERN

    }

    class Token
    {
        public TokenType Type;
        public string Value;

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

    }
}
