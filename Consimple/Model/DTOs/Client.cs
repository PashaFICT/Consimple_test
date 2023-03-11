using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consimple.Model
{
    public class Client
    {
        public class ClientDto
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public DateTime BirthdayDate { get; set; }
        }
        public class ClientCategoriesDto
        {
            public int ID { get; set; }
            public string Category { get; set; }
            public decimal Count { get; set; }
        }
    }
}
