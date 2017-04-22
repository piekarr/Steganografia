using Steganografia.Models.Users;
using System;

namespace Steganografia.Models
{
    public abstract class EntityBase
    {
        public virtual int Id { get; protected set; }
        public virtual DateTime CreatedDate { get; protected set; }
        public virtual int CreatedByUserId { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual DateTime? UpdatedDate { get; set; }
        public virtual int? UpdatedByUserId { get; set; }
        public virtual User UpdatedByUser { get; set; }
        public virtual bool IsActive { get; protected set; }

        protected EntityBase()
        {
            IsActive = true;
            CreatedDate = DateTime.UtcNow;
        }

        public EntityBase(int createdByUserId)
        {
            CreatedByUserId = createdByUserId;
        }
    }
}