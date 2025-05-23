﻿using Microsoft.EntityFrameworkCore;

namespace RealEstate.DAL.Entities
{
    public class ReviewEntity : BaseEntity
    {
        public short Rating { get; set; }
        public string Comment { get; set; } = null!;
        public Guid? AuthorId { get; set; }
        public Guid RecipientId { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public UserEntity? Author { get; set; }
        public UserEntity Recipient { get; set; } = null!;
    }
}
