using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Tests.TestServices
{
    public class RandomGenerator
    {
        public static int Integer()
        {
            return new Random().Next(1, int.MaxValue);
        }

        public static string String()
        {
            return Guid.NewGuid().ToString();
        }

        public static Restaurant Restaurant()
        {
            return new Restaurant
            {
                Id = Integer(),
                Name = String(),
                City = String(),
                Country = String()
            };
        }
    }
}
