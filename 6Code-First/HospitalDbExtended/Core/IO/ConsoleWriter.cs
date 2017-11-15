using System;
using HospitalDbExtended.Data.Interfaces;

namespace HospitalDbExtended.Core.IO
{
    public class ConsoleWriter : IWriter
    {
        public static void WriteLine(object textLine)
        {
            Console.WriteLine(textLine);
        }

        public static void Write(object text)
        {
            Console.Write(text);
        }

        void IWriter.WriteLine(string textLine)
        {
            WriteLine(textLine);
        }

        void IWriter.Write(string text)
        {
            Write(text);
        }
    }
}