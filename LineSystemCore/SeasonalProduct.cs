using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineSystemCore
{
    //TODO: Use this somewhere
    public class SeasonalProduct : Product
    {
        public DateTime? SeasonStartDate { get; private set; }
        public DateTime? SeasonEndDate { get; private set; }
        public override bool Active
        {
            get
            {
                //Seasonal product is only active, if the product is active, and the current date is between SeasonStartDate and  SeasonEndDate
                if (base.Active == true)
                {
                    if ((SeasonStartDate == null || DateTime.Now.CompareTo(SeasonStartDate) >= 0) && (SeasonEndDate == null || DateTime.Now.CompareTo(SeasonEndDate) <= 0))
                    {
                        return true;
                    }
                }

                return false;
            }
            set
            {
                base.Active = value;
            }
        }

        public SeasonalProduct(string name, int price, bool active, DateTime? seasonStartDate, DateTime? seasonEndDate) : this(name, price, active, seasonStartDate, seasonEndDate, false) { }
        public SeasonalProduct(string name, int price, bool active, DateTime? seasonStartDate, DateTime? seasonEndDate, int id) : this(name, price, active, seasonStartDate, seasonEndDate, false, id) { }
        public SeasonalProduct(string name, int price, bool active, DateTime? seasonStartDate, DateTime? seasonEndDate, bool canBeBoughtOnCredit) : this(name, price, active, seasonStartDate, seasonEndDate, canBeBoughtOnCredit, IDGetter) { }
        public SeasonalProduct(string name, int price, bool active, DateTime? seasonStartDate, DateTime? seasonEndDate, bool canBeBoughtOnCredit, int id)
            : base(name, price, active, canBeBoughtOnCredit, id)
        {
            SeasonStartDate = seasonStartDate;
            SeasonEndDate = seasonEndDate;
        }
    }
}
