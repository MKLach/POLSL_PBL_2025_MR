using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lch.com.mkl.lch.variable;
using com.mkl.lch.list;
using System.Reflection;
using UnityEngine;

namespace lch.com.mkl.lch
{
    class writer {

        public void println(object arg) {
            //Console.WriteLine(arg.ToString());
            Debug.Log(arg);
        }

        public void print(object arg)
        {
            //Console.Write(arg.ToString());
            Debug.Log(arg);
        }


    }

    public class LchRuntimeEnvironment
    {
        writer writer = new writer();

        System.Random r = new System.Random();

        public string readLine() {
           
            return Console.ReadLine();
        }

        public bool isNull(Variable var) {
            return var.getVariable() == null;
        }

        public void println(Variable arg) {
            writer.println(arg.variable);
        }

        public void print(Variable arg)
        {
            writer.print(arg.variable);
        }

        public void println2( Variable arg)
        {
            writer.println(arg.name + " = " + arg.variable);
        }

        public string name() {
            return "lch Runtime Environment - Unity";
        }


        public void addInt(Variable left, Variable right) {
            left.set(left.getAsInt() + right.getAsInt());
        }
        public void subtractInt(Variable left, Variable right)
        {
            left.set(left.getAsInt() - right.getAsInt());
        }

        public void multiplyInt(Variable left, Variable right)
        {
            left.set(left.getAsInt() * right.getAsInt());
        }

        public void divideInt(Variable left, Variable right)
        {
            left.set(left.getAsInt() / right.getAsInt());
        }


        public int addIntF(Variable left, Variable right)
        {
            return left.getAsInt() + right.getAsInt();
        }

        public string version() {
            return "0.1";
        }

        public void pversion() {
            writer.println(version());
        }

        public int randomInt() {
            return r.Next(int.MinValue, int.MaxValue);
        }

        public string present()
        {
            return name() + ", version " + version();
        }

        public override string ToString() {


            return present();
        }

        public object createList(Variable typ)
        {
            string typeName = typ.getAsString();
            
            Type elementType = Type.GetType(typeName)
                               ?? AppDomain.CurrentDomain
                                           .GetAssemblies()
                                           .SelectMany(a => a.GetTypes())
                                           .FirstOrDefault(t => t.FullName == typeName || t.Name == typeName);

            if (elementType == null)
                throw new ArgumentException($"Type '{typeName}' not found.");

            
            Type listType = typeof(List<>).MakeGenericType(elementType);

            return Activator.CreateInstance(listType);
        }

        public object createLchList()
        {
            LchList list = new LchList();
            return list;
        }

        public int variablescount() {
            return Lch.variables.Count;
        }


        public object createObject(Variable typ)
        {
            string typeName = typ.getAsString();

            if (string.IsNullOrWhiteSpace(typeName))
                throw new ArgumentException("Type name must not be null or empty.", nameof(typeName));

            Type type = AppDomain.CurrentDomain
                                 .GetAssemblies()
                                 .SelectMany(a => a.GetTypes())
                                 .FirstOrDefault(t => t.Name == typeName || t.FullName == typeName);

            if (type == null)
                throw new TypeLoadException($"Type '{typeName}' not found.");

            return Activator.CreateInstance(type);
        }


        public bool loadLib(Variable name) {
            try
            {
                Assembly ass = Assembly.Load(name.getAsString());

                return true;
            }
            catch (Exception e) {
                return false;
            }
            
        }

    }
}
