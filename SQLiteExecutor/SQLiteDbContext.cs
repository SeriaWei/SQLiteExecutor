using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteExecutor
{
    internal class SQLiteDbContext
    {
        public SQLiteDbContext(string dbFile)
        {
            FilePath = dbFile;
        }
        public string FilePath { get; set; }

        public SqliteConnection GetDbConnection()
        {
            return new SqliteConnection($"Data Source={FilePath};");
        }
    }
}
