namespace Steganografia.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using EntityFramework;
    using Models.Users;

    internal sealed class Configuration : DbMigrationsConfiguration<Steganografia.EntityFramework.AppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Steganografia.EntityFramework.AppContext context)
        {
            InsertAdminUser(context);
        }

        private void InsertAdminUser(AppContext context)
        {
            if (!context.Users.Any(x => x.UserName == "Admin"))
            {
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('User', RESEED, 1000)");
                User user = new User("Admin", "Password", "Dawid", "Piekar");
                context.Users.Add(user);
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('User', RESEED, 2000)");
            }
        }
    }
}
