using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CSharpInterviewPractice
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.ExecuteAssembly(
              @"C:\Program Files\NUnit 2.5.5\bin\net-2.0\nunit.exe",
              null,
              new string[] { Assembly.GetExecutingAssembly().Location });
        }
    }
}
