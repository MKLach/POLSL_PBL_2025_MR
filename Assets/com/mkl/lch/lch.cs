using lch.com.mkl.lch.variable;
using lch.com.mkl.lch.script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using lch.com.mkl.lch.elements;
using com.mkl.lch.events;
using System.Collections;

namespace lch.com.mkl.lch
{
    public class Lch : VariableManager
    {
        private static readonly SHA256 Sha256 = SHA256.Create();

        public static Dictionary<string, Variable> variables;
        public static Dictionary<string, ExecutableScript> scripts;

        public readonly string[] builtinTypes = { "int", "float", "string", "char", "double", "long", "short", "boolean", "object", "list" };

        public static Lch instance;
        public int current_instruction_index = 0;

        public LchRuntimeEnvironment env;
        public EventMgr eventManager;

        public void executeScript(ExecutableScript script)
        {
            for (current_instruction_index = 0; current_instruction_index < script.blocks.Count; )
            {

                //Console.WriteLine(script.getInstruction(current_instruction_index));
                current_instruction_index += script.getInstruction(current_instruction_index).execute(current_instruction_index);

               

            }
        }

        public Dictionary<string, string> getDefaultVariablesSet() {

            return variables.ToDictionary(kv => kv.Key, kv => kv.Value.meta.getType());
        }
        
        public static readonly byte[] BUILTIN_LONG;
        public static readonly byte[] BUILTIN_INTEGER;
        public static readonly byte[] BUILTIN_FLOAT;
        public static readonly byte[] BUILTIN_DOUBLE;
        public static readonly byte[] BUILTIN_CHAR;
        public static readonly byte[] BUILTIN_STRING;

        public static VariableMetadata stringMeta;
        public static VariableMetadata intMeta;
        public static VariableMetadata charMeta;
        public static VariableMetadata floatMeta;
        public static VariableMetadata logicMeta;
        public static VariableMetadata logicConvMeta;
        public static VariableMetadata objectMeta;
        public static VariableMetadata listMeta;

        static Lch() {
            BUILTIN_INTEGER = ComputeSHA256("int");
            BUILTIN_LONG = ComputeSHA256("long");
            BUILTIN_FLOAT = ComputeSHA256("float");
            BUILTIN_DOUBLE = ComputeSHA256("double");
            BUILTIN_CHAR = ComputeSHA256("char");
            BUILTIN_STRING = ComputeSHA256("string");

            stringMeta = new StringVariableMetadata("string");
            intMeta = new DefaultVariableMetadata("int");
            charMeta = new DefaultVariableMetadata("char");
            floatMeta = new DefaultVariableMetadata("float");
            logicMeta = new DefaultVariableMetadata("boolean");
            logicConvMeta = new DefaultVariableMetadata("conv");
            objectMeta = new DefaultVariableMetadata("object");
            listMeta = new DefaultVariableMetadata("list");
        }

        public Lch() {
            instance = this;
            variables = new Dictionary<string, Variable>();

            variables.Add("true", new Variable("true", true, logicMeta));
            variables.Add("false", new Variable("false", false, logicMeta));

            env = new LchRuntimeEnvironment();
            variables.Add("env", new Variable("env", env, objectMeta));
            
            eventManager = new EventMgr();
            variables.Add("eventManager", new Variable("eventManager", eventManager, objectMeta));

        }

        public Variable getVariable(string variableName) {

            return variables[variableName];
        }

        public void addVariable(string name, Variable value) {
            if(!variables.ContainsKey(name))
                variables.Add(name, value);
            
        }

        public void addOverrideVariable(string name, Variable value)
        {
            if (!variables.ContainsKey(name))
            {
                variables.Add(name, value);
            }
            else {
                variables[name].set(value.getVariable());
            }
            

        }

        public VariableMetadata getType(string type) {
            switch (type) {
                case "string":
                    return stringMeta;
                case "boolean":
                    return logicMeta;
                case "int":
                    return intMeta;
                case "float":
                    return floatMeta;
                case "char":
                    return charMeta;
                case "object":
                    return objectMeta;
                case "list":
                    return listMeta;
                default:
                    return objectMeta;
            }
        }

        public static byte[] ComputeSHA256(string input)
        {
            
            return Sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            
        }

        int c=0;
        public string getNextLiteralVariableName() {

            return "?_" + (c++);
        }

        int cl = 0;
        public string getNextLogicEvalVariableName()
        {

            return "?le_" + (cl++);
        }

        public void resetVariables()
        {
            List<string> validKeys = new List<string> { "env", "true", "false" };


            var keysToRemove = new List<string>();

            // Find keys to remove
            foreach (var key in variables.Keys)
            {
                if (!validKeys.Contains(key))
                {
                    keysToRemove.Add(key);
                }
            }

            // Remove the keys from the dictionary
            foreach (var key in keysToRemove)
            {
                variables.Remove(key);
            }
        }
    }
}
