using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OdeToFood.Data.DomainClasses
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

    }
}
