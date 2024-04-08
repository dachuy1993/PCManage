using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcManage
{
    public class Helper_ApplicationInfo 
    {
        private int _id;
        private string _pcno;
        private string _hdd1Serial;
        private string _DisplayName;
        private string _Version;
        private string _InstalledDate;
        private string _Publisher;
        private string _UnninstallCommand;
        private string _ModifyPath;
        private string _monthUpdate;
        private string _insdt;

        public int ID { get { return _id; } set { if (_id != value) { _id = value; NotifyPropertyChanged("ID"); } } }
        public string pcno { get { return _pcno; } set { if (_pcno != value) { _pcno = value; NotifyPropertyChanged("pcno");} } }
        public string hdd1Serial { get { return _hdd1Serial; } set { if (_hdd1Serial != value) { _hdd1Serial = value; NotifyPropertyChanged("hdd1Serial"); } } }
        public string DisplayName { get { return _DisplayName; } set { if (_DisplayName != value) { _DisplayName = value; NotifyPropertyChanged("DisplayName"); } } }
        public string Version { get { return _Version; } set { if (_Version != value) { _Version = value; NotifyPropertyChanged("Version"); } } }
        public string InstalledDate { get { return _InstalledDate; } set { if (_InstalledDate != value) { _InstalledDate = value; NotifyPropertyChanged("InstalledDate"); } } }
        public string Publisher { get { return _Publisher; } set { if (_Publisher != value) { _Publisher = value; NotifyPropertyChanged("Publisher"); } } }
        public string UnninstallCommand { get { return _UnninstallCommand; } set { if (_UnninstallCommand != value) { _UnninstallCommand = value; NotifyPropertyChanged("UnninstallCommand"); } } }
        public string ModifyPath { get { return _ModifyPath; } set { if (_ModifyPath != value) { _ModifyPath = value; NotifyPropertyChanged("ModifyPath"); } } }
        public string monthUpdate { get { return _monthUpdate; } set { if (_monthUpdate != value) { _monthUpdate = value; NotifyPropertyChanged("monthUpdate"); } } }
        public string insdt { get { return _insdt; } set { if (_insdt != value) { _insdt = value; NotifyPropertyChanged("insdt"); } } }
        private void NotifyPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
