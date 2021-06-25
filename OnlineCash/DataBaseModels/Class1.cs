using System;
using System.Collections.Generic;

namespace DataBase
{
    public class Class1
    {
        public string GetHello() => "Hello world";
        public List<Class2> Persons = new List<Class2>()
        {
            new Class2{Name="Никульшин",Age=32},
            new Class2{Name="Никульшин 1",Age=33},
            new Class2{Name="Никульшин 2",Age=34},
        };
    }
}
