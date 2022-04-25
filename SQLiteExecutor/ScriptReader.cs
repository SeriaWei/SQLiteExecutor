using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteExecutor
{
    internal class ScriptReader
    {
        public ScriptReader(string filePath)
        {
            FilePath = filePath;
        }
        public string FilePath { get; set; }
        public List<string> ReadScripts()
        {
            List<string> scripts = new List<string>();
            using (FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    StringBuilder scriptBlock = new StringBuilder();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Equals("BEGIN TRANSACTION;", StringComparison.OrdinalIgnoreCase) ||
                            line.Equals("COMMIT;", StringComparison.OrdinalIgnoreCase))
                        {
                            if (scriptBlock.Length > 0)
                            {
                                scripts.Add(scriptBlock.ToString());
                                scriptBlock.Clear();
                            }
                        }
                        else
                        {
                            scriptBlock.AppendLine(line);
                        }
                    }
                    if (scriptBlock.Length > 0)
                    {
                        scripts.Add(scriptBlock.ToString());
                        scriptBlock.Clear();
                    }
                }
            }
            return scripts;
        }
    }
}
