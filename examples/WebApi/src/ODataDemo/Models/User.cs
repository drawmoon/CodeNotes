﻿using System.Collections.Generic;

namespace ODataDemo.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool LockoutEnabled { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
