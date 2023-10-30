using System;
using System.IO;
using System.Linq; // to be able to use count for input file
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary; //to use serialization

namespace MainProject_Serialization
{
    [Serializable]
    class Customers
    {
        private string Name;
        private string Address;
        private int Periodicity;
        private DateTime LastVisitDate;
        private DateTime NextVisitDate;

        //method to create a new object(customer)
        public Customers(string NewName, string NewAdress, DateTime NewLastVisitDate, int NewPeriodicity)
        {
            // Uses the default calendar of the InvariantCulture.
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;

            if (NewName == null || NewAdress == null) // a int value can never be null
            {
                throw new ArgumentNullException("Null names not accepted.");
            }

            if ((NewName == "") || (NewAdress == ""))
            {
                throw new Exception("Empty names not accepted.");
            }
            this.Name = NewName;
            this.Address = NewAdress;
            this.LastVisitDate = NewLastVisitDate;
            this.Periodicity = NewPeriodicity;
            this.NextVisitDate = myCal.AddMonths(this.LastVisitDate, NewPeriodicity);
        }
        public int getNextVisitMonth()
        {
            return this.NextVisitDate.Month;
        }
        public int getNextVisitYear()
        {
            return this.NextVisitDate.Year;
        }
        public void setNextVisitDate()
        {
            // Uses the default calendar of the InvariantCulture.
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            int P = this.Periodicity; //periodicity of the customer
            DateTime LastVisitDate = this.LastVisitDate;
            int LastVisitWeekDay = (int)LastVisitDate.DayOfWeek;
            DateTime NewNextVisitDate = myCal.AddMonths(LastVisitDate, P);
            //if weekend
            if (LastVisitWeekDay == 6) //saturday
            {
                NewNextVisitDate = myCal.AddDays(NewNextVisitDate, 2);
            }
            if (LastVisitWeekDay == 7) //sunday
            {
                NewNextVisitDate = myCal.AddDays(NewNextVisitDate, 1);
            }
            this.NextVisitDate = NewNextVisitDate;
        }
        public DateTime getNextVisitDate()
        {
            return this.NextVisitDate;
        }
        public String getLastVisitDateString()
        {
            string asString = this.LastVisitDate.ToString("D",CultureInfo.CreateSpecificCulture("en-US"));
            return asString;
        }
        public String getNextVisitDateString()
        {
            string asString = this.NextVisitDate.ToString("D", CultureInfo.CreateSpecificCulture("en-US"));
            return asString;
        }
        public void setNewName(string aName)
        {
            this.Name = aName;
        }
        public void setNewAddress(string aAddress)
        {
            this.Address = aAddress;
        }
        public void setNewPeriodicity(int aPeriodicity)
        {
            this.Periodicity = aPeriodicity;
        }
        public void setNewLastVisitDate(int day, int month, int year)
        {
            DateTime NewLastVisitDate = new DateTime(year, month, day, new GregorianCalendar());
            this.LastVisitDate = NewLastVisitDate;

        }
        public string getName()
        {
            return this.Name;
        }
        public string getAddress()
        {
            return this.Address;
        }
        public int getPeriodicity()
        {
            return this.Periodicity;
        }
    }
}
