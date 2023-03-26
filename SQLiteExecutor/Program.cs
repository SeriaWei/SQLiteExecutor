using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteExecutor
{
    //dotnet pack -o ./
    [Command(Description = "Execute script to SQLite database.")]
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineApplication.Execute<Program>(args);
        }
        [Option(Description = "Script file path")]
        public string File { get; set; }
        [Option(Description = "Get all *.sqlite files from working directory if [database] is not specified.")]
        public string Database { get; set; }

        [Option(Description = "Get all *.sqlite files from directory.")]
        public string Path { get; set; }
        private int OnExecute()
        {
            if (string.IsNullOrEmpty(File))
            {
                Console.WriteLine("Script file not found.");
                return 1;
            }
            Console.WriteLine("Script file: {0}", File);
            List<string> scripts = new ScriptReader(File).ReadScripts();
            if (!string.IsNullOrWhiteSpace(Database))
            {
                new ScriptExcutor(Database, scripts).Execute();
            }
            else if (!string.IsNullOrEmpty(Path))
            {
                foreach (var item in System.IO.Directory.GetFiles(Path, "*.sqlite"))
                {
                    new ScriptExcutor(item, scripts).Execute();
                }
            }
            else
            {
                foreach (var item in System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory(), "*.sqlite"))
                {
                    new ScriptExcutor(item, scripts).Execute();
                }
            }
            return 0;
        }


    }
}
