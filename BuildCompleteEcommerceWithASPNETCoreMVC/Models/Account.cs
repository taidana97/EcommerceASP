using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Models
{
    [Table("Account")]
    public class Account
    {
        public Account()
        {
            RoleAccounts = new HashSet<RoleAccount>();
        }

        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<RoleAccount> RoleAccounts { get; set; }
    }
}
