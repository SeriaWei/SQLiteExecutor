using McMaster.Extensions.CommandLineUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

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
        private int OnExecute()
        {
            if (string.IsNullOrEmpty(File))
            {
                Console.WriteLine("Script file not found.");
                return 1;
            }
            Console.WriteLine("Script file: {0}", File);
            string script = System.IO.File.ReadAllText(File);
            if (!string.IsNullOrWhiteSpace(Database))
            {
                Execute(Database, script);
            }
            else
            {
                foreach (var item in System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory(), "*.sqlite"))
                {
                    Execute(item, script);
                }
            }
            return 0;
        }

        private void Execute(string dbFile, string script)
        {
            Console.WriteLine("Executing scripts to: {0}", dbFile);
            DateTime start = DateTime.Now;
            using (SQLiteDbContext dbContext = new SQLiteDbContext(dbFile))
            {
                var connection = dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandTimeout = 0;
                            command.CommandText = script;
                            Console.WriteLine("{0} of rows affected Elapsed: {1}ms", command.ExecuteNonQuery(), (DateTime.Now - start).TotalMilliseconds);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }
    }
}
