using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consimple
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
