using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace motekarteknologi.Models
{
    public class Product
    {
        public Guid ID { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

    }
}
