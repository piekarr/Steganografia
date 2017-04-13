using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Steganografia.Models.Users;
using System.Data.Entity.ModelConfiguration.Conventions;
using Steganografia.EntityFramework.Mappings.Users;
using Steganografia.EntityFramework.Mappings.Sessions;

namespace Steganografia.EntityFramework
{
    public partial class AppContext : DbContext
    {
        private static AppContext _appContext;
        public virtual IDbSet<User> Users { get; set; }

        public AppContext()
            : base("name=AppContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new SessionMap());
        }

        public static AppContext Create()
        {
            return (_appContext = _appContext ?? new AppContext());
        }
    }
}
