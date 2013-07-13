using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtoV
{
    struct MyValue
    {
        public MyValue(string myString, DateTime myDate)
        {
            MyString = myString;
            MyDate = myDate;
        }

        public string MyString;
        public DateTime MyDate;
    }

    class MyClass : IWetMyself
    {
        static int count;

        static MyClass()
        {
            Console.WriteLine("Statically Constructing!!");
        }

        public MyClass() : this("Default String", DateTime.MaxValue)
        {
        }

        public MyClass(string myString, DateTime myDate)
        {
            this.myString = myString;
            MyDate = myDate;
            Console.WriteLine("Making MyClass Number {0}", ++count);
        }

        public string myString;
        public DateTime MyDate;

        public string DoubleMyString
        {
            get
            {
                return myString + myString;
            }
            set
            {
                if (value[0] != 'a')
                    myString = "Oops";
                else
                    myString = value;
            }
        }

        public void DoALittleDance()
        {
            Console.WriteLine("Dance!");
        }

        public string GetAString(int i)
        {
            if(i > 0)
                return i.ToString();
            return "Oops";
        }

        public static void DOABetterDance()
        {
            Console.WriteLine("Weeeeee!");
        }

        #region IWetMyself Members

        public void WetMySelf()
        {
        }

        #endregion
    }

    public interface IWetMyself
    {
        void WetMySelf();
    }
}
