using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lch.com.mkl.lch.elements;
using lch.com.mkl.lch.variable;
using System;
using System.Reflection;


namespace com.mkl.lch.elements
{
    public class RawMethodCallBlock : AbstractBlock
    {
        Variable instance;
        MethodInfo method;
        object[] args;

        string methodName;
        IVariableReference name;
        List<IVariableReference> arguments;

        public RawMethodCallBlock(IVariableReference instance, string methodName, List<IVariableReference> arguemnts) {
 
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
                    args[i] = arguments[i].getVariable().getVariable();
                }
                name.acquire();
                instance = name.getVariable();

                method = instance.variable.GetType().GetMethods()
                    .Where(m => m.Name == methodName && m.GetParameters().Length == args.Length)
                    .FirstOrDefault();

                //foreach (MethodInfo mi in instance.variable.GetType().GetMethods()) {
                //    Console.WriteLine(mi.Name);

                //}




            }

            method.Invoke(instance.variable, args);
        }
    }

    public class RawFunctionCallBlock : AbstractBlock
    {
        Variable instance;
        MethodInfo method;
        object[] args;

        string methodName;
        IVariableReference name;
        IVariableReference modifiedValue;
        List<IVariableReference> arguments;
        Variable modValue;


        public RawFunctionCallBlock(IVariableReference instance, IVariableReference modifiedValue, string methodName, List<IVariableReference> arguemnts)
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
                    args[i] = arguments[i].getVariable().getVariable();
                }
                name.acquire();
                instance = name.getVariable();

               
                modifiedValue.acquire();
                modValue = modifiedValue.getVariable();
                
                method = instance.variable.GetType().GetMethods()
                    .Where(m => m.Name == methodName && m.GetParameters().Length == args.Length)
                    .FirstOrDefault();
            }

            object result = method.Invoke(instance.variable, args);

            modValue.set(result);

        }
    }
}
