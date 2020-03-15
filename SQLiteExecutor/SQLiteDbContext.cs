using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteExecutor
{
    public class SQLiteDbContext : DbContext
    {
        public SQLiteDbContext(string dbFile)
        {
            FilePath = dbFile;
        }
        public string FilePath { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={FilePath}");
        }
    }
}
