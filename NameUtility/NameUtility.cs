using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RichTea.Utilities
{
    /// <summary>
    /// Name utility.
    /// </summary>
    public static class NameUtility
    {
        private const int NameSuffixMax = 1000;

        private static string[] Names;

        /// <summary>
        /// Names that are available.
        /// </summary>
        /// <returns></returns>
        public static string[] FindNames()
        {
            if (null == Names)
            {
                var assembly = typeof(NameUtility).GetTypeInfo().Assembly;
                var resourceName = "RichTea.Utilities.Resources.Names_En.txt";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    Names = result.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            return Names;
        }

        /// <summary>
        /// Creates a name based upon an objects hash code. The name will look something like 'Thomas-897'.
        /// Names are taken from the US census.
        /// </summary>
        /// <param name="obj">Object to name.</param>
        /// <returns>Name for an object.</returns>
        public static string GenerateName(object obj)
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
