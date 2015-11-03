using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineSystemCore
{
    [Serializable()]
    public class NotActiveException : Exception
    {
        public Product Product { get; private set; }

        public NotActiveException(Product product)
            : base(product.Name + " is not active")
        {
            Product = product;
        }

        protected NotActiveException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
