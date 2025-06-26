using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lch.com.mkl.lch.variable;

namespace com.mkl.lch.list
{
    public class LchList
    {
        List<Variable> listIn;

        public LchList() {
            listIn = new List<Variable>();
        }

        public void init(List<Variable> range) {
            listIn.AddRange(range);
        }

        public void add(Variable variable) {
            listIn.Add(variable);
        }

        public object get(Variable index) {

            if (index.getAsInt() >= size()) {
                return null;
            }

            return listIn[index.getAsInt()].getVariable();
        }

        public void remove(Variable index)
        {
            listIn.RemoveAt(index.getAsInt());
        }

        public int size() {
            return listIn.Count;
        }
    }

    public class CustomType {

        string title;
        string desc;
        
        public CustomType() { 
            
        }

        public void setTitle(string title) {
            this.title = title;
        }

        public string getTitle(string title)
        {
            return this.title;
        }

        public void setDesc(string desc)
        {
            this.desc = desc;

        }

        public void show() {

            Console.WriteLine(title);
            Console.WriteLine(desc);
        }

    }

}
