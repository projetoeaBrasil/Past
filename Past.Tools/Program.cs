﻿using Past.Tools.Ele;
using Past.Tools.Ele.Subtypes;
using System;

namespace Past.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            var ele = new ElementsReader().ReadEle(@"C:\Users\skeee\Desktop\Dofus 2 Online\content\maps\elements.ele");
            foreach (var elements in ele.ElementsMap)
            {
                if (elements.Value is NormalGraphicalElementData)
                {
                    var test = elements.Value as NormalGraphicalElementData;
                    Console.WriteLine($"{test.Id}, {test.GfxId}");
                }
            }
            while (true)
            {
                Console.Read();
            }
        }
    }
}
