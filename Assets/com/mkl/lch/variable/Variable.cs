using com.mkl.lch.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lch.com.mkl.lch.variable
{
    public abstract class VariableMetadata {
        protected string type;

        public abstract void clone(Variable left, Variable right);

        public string getType() {
            return type;
        }

        public VariableMetadata(string type) {
            this.type = type;
        }

    }

    public class DefaultVariableMetadata : VariableMetadata
    {

        public override void clone(Variable left, Variable right) {

            left.variable = right.variable;
        }

        public DefaultVariableMetadata(string type) : base(type)
        {

        }

    }

    public class StringVariableMetadata : VariableMetadata
    {
        public override void clone(Variable left, Variable right)
        {
            string val = (string)right.variable;

            left.variable = string.Copy(val);
        }

        public StringVariableMetadata(string type) : base(type)
        {

        }

    }

    public interface IVariableReference {

        Variable getVariable();

        object getValue();

        string getAsString();

        int getAsInt();

        void acquire();
    }

    public class VariableReference : IVariableReference {

        string vname;
        Variable variable = null;

        public VariableReference(string name) {
            vname = name;
        }

        public void acquire() {
            if (variable == null) {
                variable=Lch.instance.getVariable(vname);
            }
        }

        public int getAsInt()
        {
        
            return variable.getAsInt();
        }

        public string getAsString()
        {
            return variable.getAsString();
        }

        public object getValue()
        {
            return variable.variable;
        }

        public Variable getVariable()
        {
            return variable;
        }

    }


    public class StringLiteralVariableReference : IVariableReference
    {
        string input;
        Variable var;

        public StringLiteralVariableReference(string str) {
            input = str;
        }

        public void acquire()
        {
            if (var == null)
                var = new Variable("___", input, Lch.intMeta);
        }

        public int getAsInt()
        {

            return 0;
        }

        public string getAsString()
        {
            return var.getAsString();
        }

        public object getValue()
        {
            return var.getVariable();
        }

        public Variable getVariable()
        {
            return var;
        }
    }


    public class IntLiteralVariableReference : IVariableReference
    {
        int input;
        Variable var;
        public IntLiteralVariableReference(int t)
        {
            input = t;
        }

        public void acquire()
        {
            if(var == null)
                var = new Variable("___", input, Lch.intMeta);
        }

        public int getAsInt()
        {
            return var.getAsInt();
        }

        public string getAsString()
        {
            return var.getAsString();
        }

        public object getValue()
        {
            return var.getVariable();
        }

        public Variable getVariable()
        {
            return var;
        }
    }


    public class Variable
    {

        public VariableMetadata meta;
        public byte[] type;
        public object variable;
        public string name;

        public Variable(string name, object variable, VariableMetadata meta) {
            this.name = name;
            this.variable = variable;
            this.meta = meta;
        }

        public virtual object getVariable() {
            return variable;
        }

        public virtual void set(object o) {
            this.variable = o;
        }

        public virtual void set(int i)
        {
            this.variable = i;
        }

        public virtual int getAsInt() {
            return (int)variable;
        }

        public string getAsString()
        {
            return variable.ToString();
        }
    }

    public class LogicConversionVariable : Variable
    {
        string left;
        string right;
        TokenType operand;
        public LogicConversionVariable(string name, string left, TokenType operand, string right, VariableMetadata meta) : base(name, null, meta)
        {
            this.left = left;
            this.right = right;
            this.operand = operand;


        }

        public override object getVariable() {

            switch (operand) {
                case TokenType.NUMBER_LOGIC_EQ:
                    return (int)Lch.instance.getVariable(left).getVariable() == (int)Lch.instance.getVariable(right).getVariable();
                case TokenType.NUMBER_LOGIC_NEQ:
                    return (int)Lch.instance.getVariable(left).getVariable() != (int)Lch.instance.getVariable(right).getVariable();
                case TokenType.NUMBER_LOGIC_GT:
                    return (int)Lch.instance.getVariable(left).getVariable() > (int)Lch.instance.getVariable(right).getVariable();
                case TokenType.NUMBER_LOGIC_GTE:
                    return (int)Lch.instance.getVariable(left).getVariable() >= (int)Lch.instance.getVariable(right).getVariable();
                case TokenType.NUMBER_LOGIC_LT:
                    return (int)Lch.instance.getVariable(left).getVariable() < (int)Lch.instance.getVariable(right).getVariable();
                case TokenType.NUMBER_LOGIC_LTE:
                    return (int)Lch.instance.getVariable(left).getVariable() <= (int)Lch.instance.getVariable(right).getVariable();
            }

            return false;
        }


    }
}
