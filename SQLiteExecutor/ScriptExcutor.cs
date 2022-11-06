using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteExecutor
{
    internal class ScriptExcutor
    {
        public ScriptExcutor(string databaseFile, List<string> scripts)
        {
            DatabaseFile = databaseFile;
            Scripts = scripts;
        }
        public string DatabaseFile { get; set; }
        public List<string> Scripts { get; set; }


        public void Execute()
        {
            Console.WriteLine("Executing scripts to: {0}", DatabaseFile);
            DateTime start = DateTime.Now;
            using (SQLiteDbContext dbContext = new SQLiteDbContext(DatabaseFile))
            {
                var connection = dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var script in Scripts)
                        {
                            using (var command = connection.CreateCommand())
                            {
                                command.Transaction = transaction;
                                command.CommandTimeout = 0;
                                command.CommandText = script;
                                Console.WriteLine("{0} of rows affected Elapsed: {1}ms", command.ExecuteNonQuery(), (DateTime.Now - start).TotalMilliseconds);
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
        }
    }
}
