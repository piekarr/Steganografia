using Steganografia.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Steganografia.EntityFramework.Mappings
{
    public abstract class EntityBaseMap<T> : EntityTypeConfiguration<T> where T : EntityBase
    {
        public EntityBaseMap()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CreatedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.CreatedByUserId);
            Property(x => x.UpdatedDate);
            Property(x => x.UpdatedByUserId);
            Property(x => x.IsActive);
            HasRequired(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedByUserId);
            HasOptional(x => x.UpdatedByUser).WithMany().HasForeignKey(x => x.UpdatedByUserId);
        }
    }
}