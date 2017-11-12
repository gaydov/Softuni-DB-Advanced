using System;
using HospitalDbExtended.Data.Interfaces;

namespace HospitalDbExtended.Core.IO
{
    public class ConsoleReader : IReader
    {
        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        string IReader.ReadLine()
        {
            return ReadLine();
        }
    }
}