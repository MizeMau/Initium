﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Table.Backend
{
    public class User
    {
        [Table("User", Schema = "backend")]
        public class Model
        {
            [Key]
            public long BackendUserID { get; set; }
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Salt { get; set; } = string.Empty;
        }

        public class Service : Table.Service<Model>
        {
            // Example: Add User-specific query
            public Model? GetByUsername(string username)
            {
                return GetQuery().SingleOrDefault(s => s.Username == username);
            }
        }
    }
}
