using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Order_Aggregrate
{
    public class AddressOrder
    {
        public AddressOrder(string firstName, string lastName, string street, string city, string country)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            Countrty = country;

        }
        public AddressOrder()
        {
            
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Countrty { get; set; }
    }
}
