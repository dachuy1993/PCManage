using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    public class LoginHelper : INotifyPropertyChanged
    {
        public string _ID;
        public string _UserLogin;
        public string _PassLogin;
        public string _RememberLogin;
        public string _TimeLogin;
        public string _TimeLogout;
        public string ID
        {
            get { return this._ID; }
            set
            {
               if(this._ID != value)
                {
                    this._ID = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }
        public string UserLogin
        {
            get { return this._UserLogin; }
            set
            {
                if (this._UserLogin != value)
                {
                    this._UserLogin = value;
                    NotifyPropertyChanged("UserLogin");
                }
            }
        }
        public string PassLogin
        {
            get { return this._PassLogin; }
            set
            {
                if (this._PassLogin != value)
                {
                    this._PassLogin = value;
                    NotifyPropertyChanged("PassLogin");
                }
            }
        }
        public string RememberLogin
        {
            get { return this._RememberLogin; }
            set
            {
                if (this._RememberLogin != value)
                {
                    this._RememberLogin = value;
                    NotifyPropertyChanged("RememberLogin");
                }
            }
        }
        public string TimeLogin
        {
            get { return this._TimeLogin; }
            set
            {
                if (this._TimeLogin != value)
                {
                    this._TimeLogin = value;
                    NotifyPropertyChanged("TimeLogin");
                }
            }
        }
        public string TimeLogout
        {
            get { return this._TimeLogout; }
            set
            {
                if (this._TimeLogout != value)
                {
                    this._TimeLogout = value;
                    NotifyPropertyChanged("TimeLogout");
                }
            }
        }


        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
