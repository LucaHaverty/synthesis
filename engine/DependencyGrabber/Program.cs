using System;
using System.Xml;

namespace DependencyGrabber {
    public static class Program {

        

        private static readonly string NUGET_DIRECTORY =
            $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}{Path.AltDirectorySeparatorChar}.nuget{Path.AltDirectorySeparatorChar}packages{Path.AltDirectorySeparatorChar}";
        private static readonly string OUTPUT_DIRECTORY =
            "../Assets/Packages/";

        public static void Main(string[] args) {

            var deps = GetDependencies();

            foreach (DepDetails dep in deps) {

                Console.WriteLine($"{dep.Name}, {dep.Version}");

                string srcDir = NUGET_DIRECTORY + $"{dep.Name.ToLower()}{Path.AltDirectorySeparatorChar}{dep.Version}{Path.AltDirectorySeparatorChar}lib{Path.AltDirectorySeparatorChar}netstandard2.0";
                if (!Directory.Exists(srcDir)) {
                    Console.WriteLine("Skipping...");
                    continue;
                }
                string outDir = OUTPUT_DIRECTORY + $"{dep.Name}.{dep.Version}{Path.AltDirectorySeparatorChar}netstandard2.0{Path.AltDirectorySeparatorChar}";
                if (Directory.Exists(outDir))
                    Directory.Delete(outDir, true);
                var info = Directory.CreateDirectory(outDir);
                foreach (string file in Directory.EnumerateFiles(srcDir)) {
                    File.Copy(file, outDir + Path.GetFileName(file));
                }
            }
        }

        private static List<DepDetails> GetDependencies() {
            var deps = new List<DepDetails>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(File.ReadAllText($"NetStandardLib{Path.AltDirectorySeparatorChar}NetStandardLib.csproj"));

            XmlNodeList packages = xml.GetElementsByTagName("PackageReference");

            foreach (XmlNode node in packages) {
                deps.Add(new DepDetails(node.Attributes!.GetNamedItem("Include")!.InnerText, node.Attributes!.GetNamedItem("Version")!.InnerText));
            }

            return deps;
        }

        private struct DepDetails {

            public DepDetails(string name, string version) {
                Name = name;
                Version = version;
            }

            public string Name;
            public string Version;
        }
    }
}
