using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcManage
{
    public class Helper_PcInfomation
    {
        private int _id;
        private string _cmpcode;
        private string _bizdiv;
        private string _pcno;
        private string _biosSerial;
        private string _mainSerial;
        private string _pcName;
        private string _ipAddress;
        private string _macAddress;
        private string _hdd1Model;
        private string _hdd1Serial;
        private string _hdd2Model;
        private string _hdd2Serial;
        private string _ramTotal;
        private string _ramWorking;
        private string _cpuSerial;
        private string _idNumber;
        private string _fullName;
        private string _depatment;
        private string _run;
        private string _diskTotal_C;
        private string _diskTotal_D;
        private string _diskTotal_E;
        private string _diskFree_C;
        private string _diskFree_D;
        private string _diskFree_E;
        private string _monitorNo;
        private string _monitorModel;
        private string _version;
        private string _monitorNo2;
        private string _monitorModel2;
        private string _monitorNo3;
        private string _monitorModel3;
        private string _idNumber2;
        private string _fullName2;
        private string _depatment2;
        private string _monthUpdate;
        private string _imsempcode;
        private string _insdt;
        private string _updempcode;
        private string _upddt;
        private string _checkXLS;
        private string _lockUSB;
        public string checkXLS { get { return _checkXLS; } set { if (_checkXLS != value) { _checkXLS = value; NotifyPropertyChanged("checkXLS"); } } }       
        public string lockUSB { get { return _lockUSB; } set { if (_lockUSB != value) { _lockUSB = value; NotifyPropertyChanged("lockUSB"); } } }
        public int ID { get { return _id; } set { if (_id != value) { _id = value; NotifyPropertyChanged("ID"); } } }
        public string cmpcode { get { return _cmpcode; } set { if (_cmpcode != value) { _cmpcode = value; NotifyPropertyChanged("cmpcode"); } } }
        public string bizdiv { get { return _bizdiv; } set { if (_bizdiv != value) { _bizdiv = value; NotifyPropertyChanged("bizdiv"); } } }
        public string pcno { get { return _pcno; } set { if (_pcno != value) { _pcno = value; NotifyPropertyChanged("pcno"); } } }
        public string biosSerial { get { return _biosSerial; } set { if (_biosSerial != value) { _biosSerial = value; NotifyPropertyChanged("biosSerial"); } } }
        public string mainSerial { get { return _mainSerial; } set { if (_mainSerial != value) { _mainSerial = value; NotifyPropertyChanged("mainSerial"); } } }
        public string pcName { get { return _pcName; } set { if (_pcName != value) { _pcName = value; NotifyPropertyChanged("pcName"); } } }
        public string ipAddress { get { return _ipAddress; } set { if (_ipAddress != value) { _ipAddress = value; NotifyPropertyChanged("ipAddress"); } } }
        public string macAddress { get { return _macAddress; } set { if (_macAddress != value) { _macAddress = value; NotifyPropertyChanged("macAddress"); } } }
        public string hdd1Model { get { return _hdd1Model; } set { if (_hdd1Model != value) { _hdd1Model = value; NotifyPropertyChanged("hdd1Model"); } } }
        public string hdd1Serial { get { return _hdd1Serial; } set { if (_hdd1Serial != value) { _hdd1Serial = value; NotifyPropertyChanged("hdd1Serial"); } } }
        public string hdd2Model { get { return _hdd2Model; } set { if (_hdd2Model != value) { _hdd2Model = value; NotifyPropertyChanged("hdd2Model"); } } }
        public string hdd2Serial { get { return _hdd2Serial; } set { if (_hdd2Serial != value) { _hdd2Serial = value; NotifyPropertyChanged("hdd2Serial"); } } }
        public string ramTotal { get { return _ramTotal; } set { if (_ramTotal != value) { _ramTotal = value; NotifyPropertyChanged("ramTotal"); } } }
        public string ramWorking { get { return _ramWorking; } set { if (_ramWorking != value) { _ramWorking = value; NotifyPropertyChanged("ramWorking"); } } }
        public string cpuSerial { get { return _cpuSerial; } set { if (_cpuSerial != value) { _cpuSerial = value; NotifyPropertyChanged("cpuSerial"); } } }
        public string idNumber { get { return _idNumber; } set { if (_idNumber != value) { _idNumber = value; NotifyPropertyChanged("idNumber"); } } }
        public string fullName { get { return _fullName; } set { if (_fullName != value) { _fullName = value; NotifyPropertyChanged("fullName"); } } }
        public string depatment { get { return _depatment; } set { if (_depatment != value) { _depatment = value; NotifyPropertyChanged("depatment"); } } }
        public string run { get { return _run; } set { if (_run != value) { _run = value; NotifyPropertyChanged("run"); } } }
        public string diskTotal_C { get { return _diskTotal_C; } set { if (_diskTotal_C != value) { _diskTotal_C = value; NotifyPropertyChanged("diskTotal_C"); } } }
        public string diskTotal_D { get { return _diskTotal_D; } set { if (_diskTotal_D != value) { _diskTotal_D = value; NotifyPropertyChanged("diskTotal_D"); } } }
        public string diskTotal_E { get { return _diskTotal_E; } set { if (_diskTotal_E != value) { _diskTotal_E = value; NotifyPropertyChanged("diskTotal_E"); } } }
        public string diskFree_C { get { return _diskFree_C; } set { if (_diskFree_C != value) { _diskFree_C = value; NotifyPropertyChanged("diskFree_C"); } } }
        public string diskFree_D { get { return _diskFree_D; } set { if (_diskFree_D != value) { _diskFree_D = value; NotifyPropertyChanged("diskFree_D"); } } }
        public string diskFree_E { get { return _diskFree_E; } set { if (_diskFree_E != value) { _diskFree_E = value; NotifyPropertyChanged("diskFree_E"); } } }
        public string monitorNo { get { return _monitorNo; } set { if (_monitorNo != value) { _monitorNo = value; NotifyPropertyChanged("monitorNo"); } } }
        public string monitorModel { get { return _monitorModel; } set { if (_monitorModel != value) { _monitorModel = value; NotifyPropertyChanged("monitorModel"); } } }
        public string version { get { return _version; } set { if (_version != value) { _version = value; NotifyPropertyChanged("version"); } } }
        public string monitorNo2 { get { return _monitorNo2; } set { if (_monitorNo2 != value) { _monitorNo2 = value; NotifyPropertyChanged("monitorNo2"); } } }
        public string monitorModel2 { get { return _monitorModel2; } set { if (_monitorModel2 != value) { _monitorModel2 = value; NotifyPropertyChanged("monitorModel2"); } } }
        public string monitorNo3 { get { return _monitorNo3; } set { if (_monitorNo3 != value) { _monitorNo3 = value; NotifyPropertyChanged("monitorNo3"); } } }
        public string monitorModel3 { get { return _monitorModel3; } set { if (_monitorModel3 != value) { _monitorModel3 = value; NotifyPropertyChanged("monitorModel3"); } } }
        public string idNumber2 { get { return _idNumber2; } set { if (_idNumber2 != value) { _idNumber2 = value; NotifyPropertyChanged("idNumber2"); } } }
        public string fullName2 { get { return _fullName2; } set { if (_fullName2 != value) { _fullName2 = value; NotifyPropertyChanged("fullName2"); } } }
        public string depatment2 { get { return _depatment2; } set { if (_depatment2 != value) { _depatment2 = value; NotifyPropertyChanged("depatment2"); } } }
        public string monthUpdate { get { return _monthUpdate; } set { if (_monthUpdate != value) { _monthUpdate = value; NotifyPropertyChanged("monthUpdate"); } } }
        public string imsempcode { get { return _imsempcode; } set { if (_imsempcode != value) { _imsempcode = value; NotifyPropertyChanged("imsempcode"); } } }
        public string insdt { get { return _insdt; } set { if (_insdt != value) { _insdt = value; NotifyPropertyChanged("insdt"); } } }
        public string updempcode { get { return _updempcode; } set { if (_updempcode != value) { _updempcode = value; NotifyPropertyChanged("updempcode"); } } }
        public string upddt { get { return _upddt; } set { if (_upddt != value) { _upddt = value; NotifyPropertyChanged("upddt"); } } }


        private void NotifyPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
