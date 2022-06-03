using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace UserDataBase

{
    public class UserFilesDb : DbContext
    {
        public DbSet<UserFile> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=usersdbfiles;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        private void ConstructorBody()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public UserFilesDb(DbContextOptions<UserFilesDb> options) : base(options)
        {
            ConstructorBody();
        }
        public UserFilesDb()
        {
            ConstructorBody();
        }
    }
}
