using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.BusinessObjects
{
    public class Guest
    {
        private int guestID;
        private string firstName;
        private string lastName;

        public int GuestID { get => guestID; set => guestID = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }

        public Guest(int guestID, string firstName, string lastName)
        {
            this.GuestID = guestID;
            this.FirstName = firstName;
            this.LastName = lastName;
        }


        public Guest() { 
        }


        public override string ToString()
        {
            return FirstName + " " + LastName;
        }


    }
}
