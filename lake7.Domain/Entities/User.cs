using System;
using System.Collections.Generic;
using System.Text;

namespace lake7.Domain.Entities
{
    public class User : CommonEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

    }
}
