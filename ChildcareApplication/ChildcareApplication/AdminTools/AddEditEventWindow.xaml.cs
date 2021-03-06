﻿using ChildcareApplication.AdminTools;
using DatabaseController;
using MessageBoxUtils;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdminTools {
    public partial class AddEditEventWindow : Window {
        private bool valueChanged, maxHoursChanged;
        private bool _maxHourPriceChanged;
        private bool _addtlTimeChanged;
        private bool _addtnlRateChanged;
        String oldEventName;
        private bool _windowLoaded = false;

        public AddEditEventWindow() {
            InitializeComponent();
            valueChanged = false;
            maxHoursChanged = false;
            this.MouseDown += WindowMouseDown;
        }

        public AddEditEventWindow(String oldEventName) {
            InitializeComponent();
            LoadData(oldEventName);
            valueChanged = false;
            maxHoursChanged = false;
            if (IsProtected(oldEventName)) {
                ProtectEvents(oldEventName);
            }
            this.oldEventName = oldEventName;
            this.MouseDown += WindowMouseDown;

        }

        private void ProtectEvents(string oldEventName) {
            this.txt_EventName.IsEnabled = false;
            this.cmb_Occurence.IsEnabled = false;
            this.cmb_PriceType.IsEnabled = false;
            this.lbl_EventName.Content = "Event Name (Protected Event)";
            if (oldEventName == "Late Fee") {
                this.txt_MaxHours.IsEnabled = false;
                this.txt_DiscountPrice.IsEnabled = false;
            }
        }

        private bool IsProtected(string eventName) {
            return (eventName == "Regular Childcare" || eventName == "Infant Childcare" || eventName == "Adolescent Childcare" || eventName == "Late Fee");
        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e) {
            if (this.valueChanged)
            {
                ProcessModification();
            } else
            {
                WPFMessageBox.Show("You have not changed any values!  If you would like to return to the previous window without making changes, please hit cancel.");
            }
        }

        private void LoadData(String eventName) {
            EventDB eventDB = new EventDB();
            string[] EventDataT = eventDB.GetEvent(eventName);

            txt_EventName.Text = EventDataT[0];

            SetPriceCombo(EventDataT);
            SetAvailability(EventDataT);
            FillFields(EventDataT);
        }

        private void FillFields(string[] eventData)
        {
            SetMaxHours(eventData);
            SetMaxHourPrice(eventData);
            SetAdditionalRateTime(eventData);
            SetAdditionalRate(eventData);
        }

        private void SetPriceCombo(string[] EventDataT) {
            if (EventDataT[1] != "") { //hourly price
                cmb_PriceType.SelectedIndex = 0;
                txt_Rate.Text = EventDataT[1];
                if (EventDataT[2] != "") {
                    txt_DiscountPrice.Text = EventDataT[2];
                }
            } else if (EventDataT[3] != "") { //daily price
                cmb_PriceType.SelectedIndex = 1;
                txt_Rate.Text = EventDataT[3];
                if (EventDataT[4] != "") {
                    txt_DiscountPrice.Text = EventDataT[4];
                }
            }
        }

        private void SetAvailability(string[] EventDataT) {
            if (EventDataT[5] != "" && EventDataT[6] != "") { //specific day
                cmb_Occurence.SelectedIndex = 1;
                txt_MonthNum.Text = EventDataT[5];
                txt_DayOfMonth.Text = EventDataT[6];
                lbl_DayNum.Visibility = Visibility.Visible;
                lbl_MonthNum.Visibility = Visibility.Visible;
                txt_DayOfMonth.Visibility = Visibility.Visible;
                txt_MonthNum.Visibility = Visibility.Visible;

                lbl_DayName.Visibility = Visibility.Hidden;
                cmb_DayName.Visibility = Visibility.Hidden;
            } else if (EventDataT[7] != "") { //weekday
                String dayName = EventDataT[7];
                cmb_Occurence.SelectedIndex = 2;
                //show day name
                lbl_DayName.Visibility = Visibility.Visible;
                cmb_DayName.Visibility = Visibility.Visible;
                cmb_DayName.SelectedIndex = GetDayIndex(dayName);

                lbl_DayNum.Visibility = Visibility.Hidden;
                lbl_MonthNum.Visibility = Visibility.Hidden;
                txt_DayOfMonth.Visibility = Visibility.Hidden;
                txt_MonthNum.Visibility = Visibility.Hidden;
            } else { //always available
                cmb_Occurence.SelectedIndex = 0;
            }
        }

        private void SetMaxHours(string[] eventInfo)
        {
            if (eventInfo[8] != null)
            {
                txt_MaxHours.Text = eventInfo[8];
            }
        }

        private void SetMaxHourPrice(string[] eventInfo)
        {
            if(eventInfo[10] != null)
            {
                tbx_OvertimeRate.Text = eventInfo[10];
            }
        }

        private void SetAdditionalRateTime(string[] eventInfo)
        {
            if(eventInfo[11] != null)
            {
                Addtl_Rate_Time.Text = eventInfo[11];
            }
        }

        private void SetAdditionalRate(string[] eventInfo)
        {
            if(eventInfo[12] != null)
            {
                Addtl_Rate_Amt.Text = eventInfo[12];
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            EditEvents win = new EditEvents();
            win.Show();
            this.Close();
        }

        private void DataChanged_Event(object sender, TextChangedEventArgs e) {
            this.valueChanged = true;
        }

        private void cmb_Occurence_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            this.valueChanged = true;
            if (cmb_Occurence.SelectedIndex != -1 && ((ComboBoxItem)cmb_Occurence.SelectedItem).Content.ToString() == "Specific Day") {
                lbl_DayNum.Visibility = Visibility.Visible;
                lbl_MonthNum.Visibility = Visibility.Visible;
                txt_DayOfMonth.Visibility = Visibility.Visible;
                txt_MonthNum.Visibility = Visibility.Visible;

                lbl_DayName.Visibility = Visibility.Hidden;
                cmb_DayName.Visibility = Visibility.Hidden;
            } else if (cmb_Occurence.SelectedIndex != -1 && ((ComboBoxItem)cmb_Occurence.SelectedItem).Content.ToString() == "Weekly") {
                lbl_DayName.Visibility = Visibility.Visible;
                cmb_DayName.Visibility = Visibility.Visible;

                lbl_DayNum.Visibility = Visibility.Hidden;
                lbl_MonthNum.Visibility = Visibility.Hidden;
                txt_DayOfMonth.Visibility = Visibility.Hidden;
                txt_MonthNum.Visibility = Visibility.Hidden;
            } else if (cmb_Occurence.SelectedIndex != -1 && ((ComboBoxItem)cmb_Occurence.SelectedItem).Content.ToString() == "Always Available") {
                lbl_DayName.Visibility = Visibility.Hidden;
                cmb_DayName.Visibility = Visibility.Hidden;

                lbl_DayNum.Visibility = Visibility.Hidden;
                lbl_MonthNum.Visibility = Visibility.Hidden;
                txt_DayOfMonth.Visibility = Visibility.Hidden;
                txt_MonthNum.Visibility = Visibility.Hidden;
            }
        }

        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            this.valueChanged = true;
        }

        private void ProcessModification() {
            if (FormDataValid()) {
                if (this.oldEventName == null) {
                    if (txt_DiscountPrice.Text != "") {
                        AddEventDiscount();
                    } else {
                        AddEventNoDiscount();
                    }
                } else if (txt_DiscountPrice.Text != "") {
                    EditEventDiscount();
                } else {
                    EditEventNoDiscount();
                }

                EventDB eventDB = new EventDB();
                string eventName = txt_EventName.Text;

                if (this.maxHoursChanged)
                {
                    
                    if (txt_MaxHours.Text != "")
                    {
                        eventDB.UpdateMaxHours(txt_EventName.Text, "'" + txt_MaxHours.Text + "'");
                    } else {
                        eventDB.UpdateMaxHours(txt_EventName.Text, "null");
                    }
                }

                if(_maxHourPriceChanged)
                {
                    string maxHourRate = tbx_OvertimeRate.Text;
                    float.TryParse(maxHourRate, out float updatedMaxHourRate);

                    if (!string.IsNullOrEmpty(maxHourRate))
                    {                        
                        eventDB.UpdateMaxHourPrice(eventName, updatedMaxHourRate);
                    }
                    else
                    {
                        eventDB.UpdateMaxHourPrice(eventName, updatedMaxHourRate);
                    }
                }

                if (_addtlTimeChanged)
                {
                    string additionalTime = Addtl_Rate_Time.Text;
                    if (!string.IsNullOrEmpty(additionalTime))
                    {
                        float.TryParse(additionalTime, out float updatedAddtlTime);

                        eventDB.UpdateAdditionalTime(eventName, updatedAddtlTime);
                    }
                    else
                    {
                        eventDB.UpdateAdditionalTime(eventName, 0);
                    }
                }

                if (_addtnlRateChanged)
                {
                    string additionalRate = Addtl_Rate_Amt.Text;
                    if (!string.IsNullOrEmpty(additionalRate))
                    {
                        float.TryParse(additionalRate, out float updatedRate);

                        eventDB.UpdateAdditionalRate(eventName, updatedRate);
                    }
                    else
                    {
                        eventDB.UpdateAdditionalRate(eventName, 0);
                    }
                }

                CloseWindow();
            }
        }

        private bool FormDataValid() {
            if (!EventNameValid(txt_EventName.Text)) {
                txt_EventName.Focus();
                return false;
            }
            if (cmb_PriceType.SelectedIndex == -1) {
                WPFMessageBox.Show("You must select a price type from the drop down menu.");
                cmb_PriceType.Focus();
                return false;
            }
            if (!ValidDoubleGreaterThanZero(txt_Rate.Text)) {
                WPFMessageBox.Show("You must enter a valid dollar figure greater than zero in the Rate box.");
                txt_Rate.Focus();
                return false;
            }
            if (txt_DiscountPrice.Text != "" && !ValidDoubleGreaterThanZero(txt_DiscountPrice.Text)) {
                WPFMessageBox.Show("You must enter a valid dollar figure greater than zero in the Discount Price box.");
                txt_DiscountPrice.Focus();
                return false;
            }
            if (cmb_Occurence.SelectedIndex == -1) {
                WPFMessageBox.Show("You must select an event occurence from the drop down menu.");
                cmb_Occurence.Focus();
                return false;
            }
            if (cmb_Occurence.SelectedIndex == 1) {
                if (!DateValid()) {
                    return false;
                }
            }
            if (cmb_Occurence.SelectedIndex == 2 && cmb_DayName.SelectedIndex == -1) {
                WPFMessageBox.Show("You must select a valid day of the week from the drop down menu.");
                cmb_Occurence.Focus();
                return false;
            }
            if (!int.TryParse(txt_MaxHours.Text, out int updatedMaxHour) && txt_MaxHours.Text.Equals("") && !ValidDoubleGreaterThanZero(txt_MaxHours.Text)) {
                WPFMessageBox.Show("You must enter a valid number greater than zero in the maximum hours text box.");
                txt_MaxHours.Focus();
                return false;
            }

            if(!int.TryParse(Addtl_Rate_Time.Text, out int newTime) || !ValidDoubleGreaterThanZero(Addtl_Rate_Time.Text))
            {
                WPFMessageBox.Show("You must enter a valid number greater than 0");
                Addtl_Rate_Time.Focus();
                return false;
            }

            if(!float.TryParse(Addtl_Rate_Amt.Text, out float newAmount) || !ValidDoubleGreaterThanZero(Addtl_Rate_Amt.Text))
            {
                WPFMessageBox.Show("You must enter a valid number greater than 0");
                Addtl_Rate_Amt.Focus();
                return false;
            }

            if(!ValidDoubleGreaterThanZero(tbx_OvertimeRate.Text) || !ValidDoubleGreaterThanZero(tbx_OvertimeRate.Text))
            {
                WPFMessageBox.Show("You must enter a valid dollar figure greater than zero in the Overtime Price box.");
                tbx_OvertimeRate.Focus();
                return false;//This may cause problems?
            }
            if (Addtl_Rate_Time.Text.Equals("") && !ValidDoubleGreaterThanZero(Addtl_Rate_Time.Text)) {
                WPFMessageBox.Show("Enter a valid number greater than zero in the Additional Rate Time field.");
                Addtl_Rate_Time.Focus();
                return false;
            }
            if (!ValidDoubleGreaterThanZero(Addtl_Rate_Amt.Text))
            {
                WPFMessageBox.Show("You must enter a valid dollar figure greater than zero in the  Price box.");
                Addtl_Rate_Amt.Focus();
                return false;//This may cause problems?
            }
            return true;
        }

        private bool EventNameValid(string eventName) {
            EventDB eventDB = new EventDB();
            if (eventName.Length < 1) {
                WPFMessageBox.Show("You must enter an event name.");
                return false;
            }
            if (eventName.Length > 50) {
                WPFMessageBox.Show("Event names may not be longer than 50 characters.");
                return false;
            }
            if (!RegExpressions.RegexValidateEventName(eventName)) {
                WPFMessageBox.Show("Event names may only contain letters and spaces.");
                return false;
            }
            if((eventDB.EventNameExists(eventName) && this.oldEventName == null) || 
                (this.oldEventName != null && this.txt_EventName.Text != this.oldEventName && eventDB.EventNameExists(eventName))) {
                WPFMessageBox.Show("The event name you have entered already exists.");
                return false;
            }
            return true;
        }

        private bool ValidDoubleGreaterThanZero(string rate) {
            double temp;
            if (!Double.TryParse(rate, out temp)) {
                return false;
            }
            if (temp < 0) {
                return false;
            }
            return true;
        }
        private int GetDayIndex(String eventDay) {
            if (eventDay == "Sunday") {
                return 0;
            } else if (eventDay == "Monday") {
                return 1;
            } else if (eventDay == "Tuesday") {
                return 2;
            } else if (eventDay == "Wednesday") {
                return 3;
            } else if (eventDay == "Thursday") {
                return 4;
            } else if (eventDay == "Friday") {
                return 5;
            } else {
                return 6;
            }
        }

        private void AddEventDiscount() {
            //Overtime Time
            var ot_time = Convert.ToInt32(txt_MaxHours.Text);
            //Overtime Rate
            var ot_rate = Convert.ToDouble(tbx_OvertimeRate.Text);
            //Addtl Rate
            var addtl_rate = Convert.ToDouble(Addtl_Rate_Amt.Text);
            //Addtl Rate Time
            var addtl_rate_time = Convert.ToInt32(Addtl_Rate_Time.Text);

            EventDB db = new EventDB();
            if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 0)
            {
                db.HourlyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 0)
            {
                db.DailyPriceAlwaysAvailable(txt_EventName.Text, 
                    Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 1)
            {
                db.HourlyPriceSpecificDay(txt_EventName.Text,
                    Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 1)
            {
                db.DailyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), 
                    Convert.ToDouble(txt_DiscountPrice.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 2)
            {
                db.HourlyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text),
                    Convert.ToDouble(txt_DiscountPrice.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 2)
            {
                db.DailyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), 
                    ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
        }

        private void AddEventNoDiscount() {
            //Overtime Time
            var ot_time = Convert.ToInt32(txt_MaxHours.Text);
            //Overtime Rate
            var ot_rate = Convert.ToDouble(tbx_OvertimeRate.Text);
            //Addtl Rate
            var addtl_rate = Convert.ToDouble(Addtl_Rate_Amt.Text);
            //Addtl Rate Time
            var addtl_rate_time = Convert.ToInt32(Addtl_Rate_Time.Text);
            EventDB db = new EventDB();
            if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 0)
            {
                db.HourlyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 0)
            {
                db.DailyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 1)
            {
                db.HourlyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 1)
            {
                db.DailyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 2)
            {
                db.HourlyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 2)
            {
                db.DailyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
        }

        private void EditEventDiscount() {
            //Overtime Time
            var ot_time = Convert.ToInt32(txt_MaxHours.Text);
            //Overtime Rate
            var ot_rate = Convert.ToDouble(tbx_OvertimeRate.Text);
            //Addtl Rate
            var addtl_rate = Convert.ToDouble(Addtl_Rate_Amt.Text);
            //Addtl Rate Time
            var addtl_rate_time = Convert.ToInt32(Addtl_Rate_Time.Text);
            EventDB db = new EventDB();
            if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 0)
            {
                db.HourlyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), this.oldEventName, addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 0)
            {
                db.DailyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), this.oldEventName, addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 1)
            {
                db.HourlyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), this.oldEventName, addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 1)
            {
                db.DailyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), this.oldEventName, addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 2)
            {
                db.HourlyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), this.oldEventName, addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 2)
            {
                db.DailyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), this.oldEventName, addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
        }

        private void EditEventNoDiscount() {
            //Overtime Time
            var ot_time = Convert.ToInt32(txt_MaxHours.Text);
            //Overtime Rate
            var ot_rate = Convert.ToDouble(tbx_OvertimeRate.Text);
            //Addtl Rate
            var addtl_rate = Convert.ToDouble(Addtl_Rate_Amt.Text);
            //Addtl Rate Time
            var addtl_rate_time = Convert.ToInt32(Addtl_Rate_Time.Text);
            EventDB db = new EventDB();
            if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 0)
            {
                db.HourlyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), this.oldEventName, addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 0) {
                db.DailyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), this.oldEventName, addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 1) {
                db.HourlyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), this.oldEventName, addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 1) {
                db.DailyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), this.oldEventName, addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 2) {
                db.HourlyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), this.oldEventName, addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
            else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 2) {
                db.DailyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), this.oldEventName, addtl_rate, addtl_rate_time, ot_rate, ot_time);
            }
        }

        private void CloseWindow() {
            EditEvents win = new EditEvents();
            win.Show();
            this.Close();
        }

        private void txt_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)((TextBox)sender).SelectAll); //idea found at: http://stackoverflow.com/questions/97459/automatically-select-all-text-on-focus-in-winforms-textbox
        }

        private bool DateValid() {
            int dayNum = 0;
            int monthNum = 0;
            if (!(Int32.TryParse(txt_DayOfMonth.Text, out dayNum))) {
                WPFMessageBox.Show("You must enter a valid number in the Day of Month box.");
                txt_DayOfMonth.Focus();
                return false;
            }
            if (!(Int32.TryParse(txt_MonthNum.Text, out monthNum) && monthNum > 0 && monthNum < 13)) {
                WPFMessageBox.Show("You must enter a valid number in the Month number box.");
                txt_MonthNum.Focus();
                return false;
            }
            GregorianCalendar cal = new GregorianCalendar();
            if (dayNum <= cal.GetDaysInMonth(DateTime.Now.Year, monthNum) && dayNum > 0) {
                return true;
            } else {
                WPFMessageBox.Show("You must enter a valid month number and day number in the month number and day number boxes!");
                txt_DayOfMonth.Focus();
            }
            return false;
        }

        #region TextChanged Listeners

        private void txt_MaxHours_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(_windowLoaded)
            {
                this.valueChanged = true;
                this.maxHoursChanged = true;
            }
        }

        private void tbx_OvertimeRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(_windowLoaded)
            {
                this.valueChanged = true;
                _maxHourPriceChanged = true;
            }
        }

        private void Addtl_Rate_Time_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(_windowLoaded)
            {
                this.valueChanged = true;
                _addtlTimeChanged = true;
            }
        }

        private void Addtl_Rate_Amt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(_windowLoaded)
            {
                this.valueChanged = true;
                _addtnlRateChanged = true;
            }
        }

        

        #endregion

        private void KeyUp_Event(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next); //Found at: http://stackoverflow.com/questions/23008670/wpf-and-mvvm-how-to-move-focus-to-the-next-control-automatically
                request.Wrapped = true;
                ((Control)e.Source).MoveFocus(request);
            }
        }

        private void cmb_PriceType_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                txt_Rate.Focus();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _windowLoaded = true;
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
