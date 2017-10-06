using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameUtility.Display
{
    class Program
    {
        static void Main(string[] args)
        {
            int objectCount = 10000000;
            Console.WriteLine($"Generating {objectCount} objects.");
            List<object> objectList = new List<object>();
            foreach(var i in Enumerable.Range(0, objectCount))
            {
                objectList.Add(new object());
            }
            var names = objectList.Select(o => o.GenerateName()).Distinct().ToList();
            var hashes = objectList.Select(o => o.GetHashCode()).Distinct().ToList();
            Console.WriteLine($"{names.Count} unique names generated, {hashes.Count} hashes generated");
            Console.WriteLine($"Total names available: {NameUtility.GetMaxNameCount()}");
            foreach(var name in names)
            {
                //Console.WriteLine(name);
            }
        }
    }
}
