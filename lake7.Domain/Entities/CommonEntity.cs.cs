using System;
using System.Collections.Generic;
using System.Text;
namespace lake7.Domain.Entities
{
    public abstract class CommonEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
    }
}

