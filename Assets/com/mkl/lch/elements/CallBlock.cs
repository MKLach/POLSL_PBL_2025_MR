using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lch.com.mkl.lch.elements;
using lch.com.mkl.lch.variable;

using System.Reflection;


namespace com.mkl.lch.elements
{
    public class MethodCallBlock : AbstractBlock
    {
        Variable instance;
        MethodInfo method;
        object[] args;

        string methodName;
        IVariableReference name;
        List<IVariableReference> arguments;

        public MethodCallBlock(IVariableReference instance, string methodName, List<IVariableReference> arguemnts) {
 
            this.name = instance;
            this.arguments = arguemnts;
            this.methodName = methodName;
        }

        public override void execute()
        {
            if (args == null) {
                args = new object[arguments.Count];

                for (int i = 0; i < arguments.Count; i++)
                {
                    arguments[i].acquire();
                    args[i] = arguments[i].getVariable();
                }
                name.acquire();
                instance = name.getVariable();

                //method = instance.variable.GetType().GetMethod(methodName);

                //foreach (MethodInfo mi in instance.variable.GetType().GetMethods()) {
                //    Console.WriteLine(mi.Name);

                //}

                method = instance.variable.GetType().GetMethods()
                      .Where(m => m.Name == methodName && m.GetParameters().Length == args.Length)
                      .FirstOrDefault();


            }

            method.Invoke(instance.variable, args);
        }
    }

    public class FunctionCallBlock : AbstractBlock
    {
        Variable instance;
        MethodInfo method;
        object[] args;

        string methodName;
        IVariableReference name;
        IVariableReference modifiedValue;
        List<IVariableReference> arguments;
        Variable modValue;


        public FunctionCallBlock(IVariableReference instance, IVariableReference modifiedValue, string methodName, List<IVariableReference> arguemnts)
        {

            this.modifiedValue = modifiedValue;
            this.name = instance;
            this.arguments = arguemnts;
            this.methodName = methodName;
        }

        public override void execute()
        {
            if (args == null)
            {
                args = new object[arguments.Count];

                for (int i = 0; i < arguments.Count; i++)
                {
                    arguments[i].acquire();
                    args[i] = arguments[i].getVariable();
                }
                name.acquire();
                instance = name.getVariable();
                
                //method = instance.variable.GetType().GetMethod(methodName);

                method = instance.variable.GetType().GetMethods()
                    .Where(m => m.Name == methodName && m.GetParameters().Length == args.Length)
                    .FirstOrDefault();


                modifiedValue.acquire();
                modValue = modifiedValue.getVariable();

            }

            object result = method.Invoke(instance.variable, args);

            modValue.set(result);

        }
    }
}
