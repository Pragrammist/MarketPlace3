using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Linq;
using System;
namespace UserDataBase
{
    public class UserDb : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=usersdb;Trusted_Connection=True;");
            //optionsBuilder.EnableSensitiveDataLogging();
        }
        public UserDb(DbContextOptions<UserDb> options) : base(options)
        {
            ConstructorBody();
        }
        private void ConstructorBody()
        {
            
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            //DeleteBooks();
        }
        private void DeleteBooks()
        {
            foreach (var id in Books.Select(e => e.Id))
            {
                var entity = new Book { Id = id };
                Books.Attach(entity);
                Books.Remove(entity);
            }
            SaveChanges();
        }
        public UserDb()
        {
            ConstructorBody();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            User admin = new User
            {
                BirthDate = new DateTime(2003, 1, 29),
                Email = "ag@p",
                Id = 24,
                NickName = "ProvoslavieVpered",
                PhoneNumber = "83005553434",
                Role = Role.admin,
                SmallDiscription = "аккаунт садомазы - создателя сайта, данные не действительные, админ если угодно",
                Money = 1000
            };
            admin.Password = admin.GetHashCode("clrutchBrihor1");
            modelBuilder.Entity<ShoppingCard>();
            modelBuilder.Entity<History>();
            modelBuilder.Entity<Comment>().HasOne(c => c.History).WithMany(u => u.Wrote); //
            modelBuilder.Entity<Comment>().HasOne(c => c.Book).WithMany(u => u.Comments);
            modelBuilder.Entity<User>().HasOne(u => u.Reading).WithMany(b => b.Users);
            modelBuilder.Entity<Book>().HasMany(b => b.ShoppingCards).WithMany(c => c.Books).UsingEntity(t => t.ToTable("BookInShoppingCards"));
            modelBuilder.Entity<Book>().HasMany(b => b.HistoryBought).WithMany(u => u.Bought);//
            modelBuilder.Entity<Book>().HasMany(b => b.HistoryRead).WithMany(u => u.Read);
            modelBuilder.Entity<Book>().HasMany(b => b.HistoryRated).WithMany(u => u.Reated);
            modelBuilder.Entity<User>().HasData(admin);
            base.OnModelCreating(modelBuilder);
        }
        
       
    }
}
