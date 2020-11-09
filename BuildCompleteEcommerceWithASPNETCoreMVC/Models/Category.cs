using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Models
{
    [Table("Category")]
    public class Category
    {
        public Category()
        {
            InverseParents = new HashSet<Category>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public bool Status { get; set; }
        
        public virtual Category Parent { get; set; }

        public virtual ICollection<Category> InverseParents { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
