using Steganografia.Models.Users;
using System;

namespace Steganografia.Models.Sessions
{
    public class Session : EntityBase
    {
        public virtual Guid SesssionId { get; set; }

        public virtual int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual DateTime Expires { get; set; }

        protected Session()
        {

        }

        public Session(Guid sesssionId, int userId, DateTime expires) : base(1000)
        {
            SesssionId = sesssionId;
            UserId = userId;
            Expires = expires;
        }
    }
}