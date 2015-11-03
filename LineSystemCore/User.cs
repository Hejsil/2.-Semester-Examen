using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LineSystemCore
{
    public class User : IComparable<User>
    {
        private static List<int> takenIDs = new List<int>();

        private static int _idGetter = 0;
        private static int IDGetter
        {
            get
            {
                _idGetter++;
                return _idGetter;
            }
        }

        private int _userID;
        public int UserID
        {
            get
            {
                return _userID;
            }
            private set
            {
                if (value < 1)
                    throw new ArgumentException("UserID can't be less than 1");
                else if (takenIDs.Contains(value))
                    throw new ArgumentException("UserID is allready taken");

                takenIDs.Add(value);
                _userID = value;

                //Making sure that _idgetter is allways higher than the user with the highest UserID
                if (_userID > _idGetter)
                    _idGetter = _userID;
            }
        }

        private string _firstName;
        public string FirstName 
        {
            get
            {
                return _firstName;
            }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("FirstName");

                _firstName = value;
            }
        }
        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("LastName");

                _lastName = value;
            }
        }
        private string _userName;
        public string UserName 
        {
            get
            {
                return _userName;
            }
            private set
            {
                //A regex for validating and UserName
                var regex = new Regex("^[a-z0-9_]+$", RegexOptions.IgnoreCase);

                if (value == null)
                    throw new ArgumentNullException("UserName");
                else if (!regex.IsMatch(value))
                    throw new ArgumentException("UserName has invalid characters");
                else
                    _userName = value;
            }
        }
        private string _eMail;
        public string EMail 
        {
            get
            {
                return _eMail;
            }
            private set
            {
                //A regex for validating and EMail
                var regex = new Regex("^[a-z0-9.-]+@[a-z0-9][a-z0-9.-]+[.][a-z0-9.-]+[a-z0-9]$", RegexOptions.IgnoreCase);

                if (value == null)
                    throw new ArgumentNullException("EMail");
                else if (!regex.IsMatch(value))
                    throw new ArgumentException("EMail is invalid");
                else
                    _eMail = value;
            }
        }
        public int Balance { get; set; }

        public User(string firstName, string lastName, string userName, string eMail) : this(firstName, lastName, userName, eMail, 0) { }
        public User(string firstName, string lastName, string userName, string eMail, int balance) : this(firstName, lastName, userName, eMail, balance, IDGetter) { }
        public User(string firstName, string lastName, string userName, string eMail, int balance, int id)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            EMail = eMail;
            Balance = balance;
            UserID = id;
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + FirstName.GetHashCode();
            hash = hash * 23 + LastName.GetHashCode();
            hash = hash * 23 + UserName.GetHashCode();
            hash = hash * 23 + EMail.GetHashCode();

            return hash;
        }

        public int CompareTo(User other)
        {
            return UserID - other.UserID;
        }

        public override string ToString()
        {
            return FirstName + ' ' + LastName + '(' + EMail + ')';
        }
    }
}
