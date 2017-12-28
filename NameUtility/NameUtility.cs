using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NameUtility
{
    public static class NameUtility
    {
        private const int NameSuffixMax = 1000;

        private static string[] Names;

        public static string[] FindNames()
        {
            if (null == Names)
            {
                var assembly = typeof(NameUtility).GetTypeInfo().Assembly;
                var resourceName = "NameUtility.Resources.Names_En.txt";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    Names = result.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            return Names;
        }

        public static string GenerateName(this object obj)
        {
            FindNames();
            var hash = Math.Abs(obj.GetHashCode());
            var nameIndex = hash % Names.Count();
            string name = Names[nameIndex];

            var counter = (hash % 1000) + 1;
            var fullName = $"{name}-{counter}";
            return fullName;
        }

        /// <summary>
        /// Calculate how many unique names can be generated.
        /// </summary>
        /// <returns></returns>
        public static int GetMaxNameCount()
        {
            return FindNames().Count() * NameSuffixMax;
        }
    }
}
