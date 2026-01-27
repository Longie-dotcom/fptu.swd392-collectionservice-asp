using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationException
{
    public class CollectionTaskNotFound : Exception
    {
        public CollectionTaskNotFound(string message) : base(message) { }
    }
}
