using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineSystemCore
{
    public class Product
    {
        private static List<int> takenIDs = new List<int>();

        protected static int _idGetter = 0;
        protected static int IDGetter
        {
            get
            {
                _idGetter++;
                return _idGetter;
            }
        }

        private int _productID;
        public int ProductID
        {
            get
            {
                return _productID;
            }
            private set
            {
                if (value < 1)
                    throw new ArgumentException("ProductID can't be less than 1");
                else if (takenIDs.Contains(value))
                    throw new ArgumentException("ProductID is allready taken");

                takenIDs.Add(value);
                _productID = value;

                //Making sure that _idgetter is allways higher than the product with the highest ProductID
                if (_productID > _idGetter)
                    _idGetter = _productID;
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("Name");

                _name = value;
            }
        }
        public int Price { get; private set; }
        public virtual bool Active { get; set; }
        public bool CanBeBoughtOnCredit { get; set; }

        public Product(string name, int price, bool active) : this(name, price, active, false) { }
        public Product(string name, int price, bool active, int id) : this(name, price, active, false, id) { }
        public Product(string name, int price, bool active, bool canBeBoughtOnCredit) : this(name, price, active, false, IDGetter) { }
        public Product(string name, int price, bool active, bool canBeBoughtOnCredit, int id)
        {
            Name = name;
            Price = price;
            Active = active;
            CanBeBoughtOnCredit = canBeBoughtOnCredit;
            ProductID = id;
        }
    }
}
