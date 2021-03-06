﻿using DatabaseController;
using MessageBoxUtils;
using System;

namespace GuardianTools {
    class TransactionCharge {

        private GuardianToolsSettings settings;
        private string guardianID;
        private ConnectionsDB db;
        private string allowanceID;
        internal double lateTime;
        internal string eventName;
        internal bool isLate = false;

        public TransactionCharge(String guardianID, String allowanceID) {
            this.settings = new GuardianToolsSettings();
            this.guardianID = guardianID;
            this.allowanceID = allowanceID;
            this.db = new ConnectionsDB();
        }

        public void setLateTime(double lateTime) {
            this.lateTime = lateTime;
        }

        public void setIsLate(bool isLate) {
            this.isLate = isLate;
        }

        internal bool PrepareTransaction(string childID, string guardianID) {

            TransactionDB transDB = new TransactionDB();
            EventDB eventDB = new EventDB();

            string[] transaction = transDB.FindTransaction(this.allowanceID);
            if (transaction == null || this.allowanceID == null) {
                WPFMessageBox.Show("Unable to check out child. Please log out then try again.");
                return false;
            }
            this.eventName = transaction[1];
            string transactionDate = transaction[3];
            string checkInTime = transaction[4];

            checkInTime = Convert.ToDateTime(checkInTime).ToString("HH:mm:ss");
            string checkOutTime = DateTime.Now.ToString("HH:mm:ss");
            double eventFee = FindEventFee(guardianID, eventName);
            this.lateTime = settings.CheckIfPastClosing(DateTime.Now.DayOfWeek.ToString(), TimeSpan.Parse(checkOutTime));
            eventFee = CalculateTransaction(checkInTime, checkOutTime, eventName, eventFee);
            string eventFeeRounded = eventFee.ToString("f2");
            db.CheckOut(DateTime.Now.ToString("HH:mm:ss"), eventFeeRounded, this.allowanceID);
            CompleteTransaction(eventFee, this.eventName, DateTime.Now.ToString("yyyy-MM-dd"));
            return true;
        }

        internal double CalculateTransaction(string checkInTime, string checkOutTime, string eventName, double eventFee) {
            EventDB eventDB = new EventDB();
            //
            string[] eventData = eventDB.FindEventData(this.eventName);
            int overHrs;
            int.TryParse(eventData[8], out overHrs);
            double overRate;
            Double.TryParse(eventData[10], out overRate);
            int addTime;
            int.TryParse(eventData[11], out addTime);
            double addRate;
            Double.TryParse(eventData[12], out addRate);
            WPFMessageBox.Show("overHrs: " + overHrs.ToString() + "\noverRate: " + overRate.ToString() + "\naddTime: " + addTime.ToString() + "\naddRate: " + addRate.ToString());
            //
            TimeSpan TimeSpanCheckOut = TimeSpan.Parse(DateTime.Parse(checkOutTime).ToString("HH:mm:ss"));
            TimeSpan TimeSpanCheckIn = TimeSpan.Parse(DateTime.Parse(checkInTime).ToString("HH:mm:ss"));
            double totalCheckedInHours = (TimeSpanCheckOut.Hours - TimeSpanCheckIn.Hours) + ((TimeSpanCheckOut.Minutes - TimeSpanCheckIn.Minutes) / 60.0);
            //double lateMaximum = eventDB.GetEventHourCap(eventName);
            totalCheckedInHours = 6;//
            int hrs = 0;
            if(totalCheckedInHours > overHrs)
            {
                hrs = overHrs;
                double overTime = totalCheckedInHours - overHrs;
                hrs += (int)Math.Ceiling(overTime * 6); 
            }
            if(totalCheckedInHours < overHrs)
            {
                hrs = (int)Math.Floor(totalCheckedInHours);
                double minutes = totalCheckedInHours - Math.Floor(totalCheckedInHours);
                if (minutes > (1 / 6)) // greater than 10 minutes
                    hrs++;
            }


            /*
            if (totalCheckedInHours < addTime)
            {
                addRate = 0;
            }
            if (totalCheckedInHours < overHrs)
            {
                overRate = 0;
            }
            */
            
            /*
             * if (totalCheckedInHours > lateMaximum && eventName.CompareTo("Late Fee") != 0) {
                double timeDifference = totalCheckedInHours - lateMaximum;                
                if (timeDifference > this.lateTime) {
                    this.lateTime = timeDifference;
                    totalCheckedInHours = lateMaximum;
                } else {
                    totalCheckedInHours = totalCheckedInHours - this.lateTime;
                }
                this.isLate = true;
            } else if (this.lateTime > 0 && eventName.CompareTo("Late Fee") != 0) {
                totalCheckedInHours = totalCheckedInHours - this.lateTime;
                this.isLate = true;
            }
            */

            return getCharge(eventFee, eventName, totalCheckedInHours, overHrs, overRate, addTime, addRate, hrs);
        }

        internal double getCharge(double eventFee, string eventName, double totalCheckedInHours, int overHrs, double overRate, int addTime, double addRate, int hrs) {
            int i = 0;
            double charge = 0;
            if (CheckIfHourly(eventName))
            {
                while (i < hrs && i < addTime)
                {
                    charge += eventFee;
                    i++;
                }
                while (i < hrs && i < overHrs)
                {
                    charge += addRate;
                    i++;
                } while (i < hrs)
                {
                    charge += overRate;
                    i++;
                }
            }
            else
            {
                charge = eventFee;
            }            
            charge = Math.Round(charge, 2, MidpointRounding.AwayFromZero);
            charge = charge - GetBillingCap(eventName, guardianID, charge);
            if (totalCheckedInHours < 0)
            {
                charge = 0;
            }
                /*
                if (CheckIfHourly(eventName)) {
                    eventFee = eventFee * totalCheckedInHours;
                    eventFee = Math.Round(eventFee, 2, MidpointRounding.AwayFromZero);
                }
                eventFee = eventFee - GetBillingCap(eventName, guardianID, eventFee);
                if (totalCheckedInHours < 0) {
                    eventFee = 0;
                }
                */
                return charge;
        }

        public void CompleteTransaction(double eventFee, string name, string date) {
            TransactionDB transDB = new TransactionDB();
            AddToBalance(name, eventFee);
            if (this.isLate && name.CompareTo("Late Fee") != 0) {
                double lateFee = CalculateLateFee(date);
            }
        }

        //Below method should be helpful in adding additional late fees retrieved from the EventDB
        internal double CalculateLateFee(string date) {
            TransactionDB transDB = new TransactionDB();
            EventDB eventDB = new EventDB();
            String name = "Late Fee";
            double lateFee = eventDB.GetLateFee(name);
            lateFee = lateFee * this.lateTime;
            string maxTransactionID = transDB.GetNextTransID();
            transDB.AddLateFee(maxTransactionID, name, this.allowanceID, date, lateFee);
            transDB.UpdateBalances(guardianID, lateFee, "MiscTotal");
            return lateFee;
        }

        internal bool CheckIfHourly(string eventName) {
            EventDB eventDB = new EventDB();
            string[] EventDataT = eventDB.GetEvent(eventName);
            if (EventDataT == null) {
                return false;
            }
            if (String.IsNullOrWhiteSpace(EventDataT[1])) {
                return false;
            }
            return true;
        }

        internal double FindEventFee(string guardianID, string eventName) {
            EventDB eventDB = new EventDB();
            bool discount = false;
            int childrenCheckedIn = db.NumberOfCheckedIn(guardianID);
            string[] EventDataT = eventDB.GetEvent(eventName);
            if ((childrenCheckedIn > 1) && (!String.IsNullOrWhiteSpace(EventDataT[2]) || !String.IsNullOrWhiteSpace(EventDataT[4]))) {
                discount = true;
            }
            if (EventDataT == null) {
                return 0.0;
            }
            if (discount) {
                if (String.IsNullOrWhiteSpace(EventDataT[2])) {
                    return Convert.ToDouble(EventDataT[4]);
                } else {
                    return Convert.ToDouble(EventDataT[2]);
                }
            } else {
                if (String.IsNullOrWhiteSpace(EventDataT[1])) {
                    return Convert.ToDouble(EventDataT[3]);
                } else {
                    return Convert.ToDouble(EventDataT[1]);
                }
            }
        }

        internal double GetStrictEventFee(string eventName) {
            EventDB eventDB = new EventDB();
            string[] EventDataT = eventDB.GetEvent(eventName);
            if (EventDataT == null) {
                return 0.0;
            }
            if (String.IsNullOrWhiteSpace(EventDataT[1])) {
                return Convert.ToDouble(EventDataT[3]);
            } else {
                return Convert.ToDouble(EventDataT[1]);
            }
        }

        public double GetBillingCap(string eventName, string guardianID, double eventFee) {
            string familyID = guardianID.Remove(guardianID.Length - 1);
            int billingStart = settings.GetBillingStart();
            int billingEnd = settings.GetBillingEnd();
            DateTime DTStart;
            DateTime DTEnd;
            if (DateTime.Now.Day > billingEnd) {
                DTStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, billingStart);
                DTEnd = FindBillingEnd(DTStart, billingEnd);
            } else {
                DTEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, billingEnd);
                DTStart = FindBillingStart(DTEnd, billingStart);
            }
            return BillingCapCalc(DTStart, DTEnd, eventName, familyID, eventFee);
        }

        public double BillingCapCalc(DateTime DTStart, DateTime DTEnd, string eventName, string familyID, double eventFee) {
            TransactionDB transDB = new TransactionDB();
            double cap = settings.GetBillingCap();
            string start = DTStart.ToString("yyyy-MM-dd");
            string end = DTEnd.ToString("yyyy-MM-dd");
            if (eventName.CompareTo("Regular Childcare") == 0 || eventName.CompareTo("Infant Childcare") == 0 || eventName.CompareTo("Adolescent Childcare") == 0) {
                object recordFound = transDB.SumRegularCare(start, end, familyID);
                double sum;
                if (recordFound == DBNull.Value || recordFound == null) {
                    return 0;
                } else {
                    sum = Convert.ToDouble(recordFound);
                }
                double total = sum + eventFee;
                double capdiff = total - cap;
                if (capdiff > 0 && capdiff < eventFee) {
                    return capdiff;
                } else if (capdiff >= eventFee) {
                    return eventFee;
                }
            }
            return 0.0;
        }

        internal DateTime FindBillingEnd(DateTime DTStart, int billingEnd) {
            DateTime DTEnd;
            int endMonth = DTStart.Month + 1;
            if (endMonth == 13) {
                int endYear = DTStart.Year + 1;
                DTEnd = new DateTime(endYear, 1, billingEnd);
            } else {
                DTEnd = new DateTime(DateTime.Now.Year, endMonth, billingEnd);
            }
            return DTEnd;
        }

        public DateTime FindBillingStart(DateTime DTEnd, int billingStart) {
            DateTime DTStart;
            int startMonth = DTEnd.Month - 1;
            if (startMonth == 0) {
                int startYear = DTEnd.Year - 1;
                DTStart = new DateTime(startYear, 12, billingStart);
            } else {
                DTStart = new DateTime(DateTime.Now.Year, startMonth, billingStart);
            }
            return DTStart;
        }

        public void AddToBalance(String name, Double eventFee) {
            TransactionDB transDB = new TransactionDB();
            if (name.CompareTo("Regular Childcare") == 0 || name.CompareTo("Infant Childcare") == 0 || name.CompareTo("Adolescent Childcare") == 0) {
                transDB.UpdateBalances(guardianID, eventFee, "RegularTotal");
            } else if (name.ToUpper().Contains("CAMP")) {
                transDB.UpdateBalances(guardianID, eventFee, "CampTotal");
            } else {
                transDB.UpdateBalances(guardianID, eventFee, "MiscTotal");
            }
        }
    }
}
