using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Steganografia.EntityFramework.Mappings.Users;
using Steganografia.EntityFramework.Mappings.Sessions;
using Steganografia.EntityFramework.Mappings.Conversations;
using Steganografia.Models.Users;
using System;

namespace Steganografia.EntityFramework
{
    public partial class AppContext : DbContext
    {
        private static AppContext _appContext;

        public IDbSet<User> Users { get; set; }

        public AppContext()
            : base("name=AppContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new SessionMap());
            modelBuilder.Configurations.Add(new ConversationMap());
            modelBuilder.Configurations.Add(new MessageMap());
            modelBuilder.Configurations.Add(new UserUnreadMessageMap());
        }

        internal static AppContext Create()
        {
            return _appContext = _appContext ?? new AppContext();
        }
    }
}
