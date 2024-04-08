using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcManageServer
{
    public class Helper_PcProcessData
    {
        private string _biosSerial;
        private string _upFile;
        private string _dowFile;
        private string _listFolder;
        private string _listFile;
        private string _sourcePath;
        private string _destPath;
        private string _fodlerPath;
        private string _filePath;
        private string _fileName;
        private string _fileType;
        private string _fileSize;
        private string _run;
        private string _etc1;
        private string _etc2;
        private string _hdd1Serial;
        private string _etc4;
        private string _etc5;
        private string _etc6;
        private string _etc7;
        private string _etc8;
        private string _etc9;
        private string _etc10;
        private string _checkXLS;
        private string _insdt;
        public string checkXLS { get { return _checkXLS; } set { if (_checkXLS != value) { _checkXLS = value; NotifyPropertyChanged("checkXLS"); } } }

        public string biosSerial { get { return _biosSerial; } set { if (_biosSerial != value) { _biosSerial = value; NotifyPropertyChanged("biosSerial"); } } }
        public string upFile { get { return _upFile; } set { if (_upFile != value) { _upFile = value; NotifyPropertyChanged("upFile"); } } }
        public string dowFile { get { return _dowFile; } set { if (_dowFile != value) { _dowFile = value; NotifyPropertyChanged("dowFile"); } } }
        public string listFolder { get { return _listFolder; } set { if (_listFolder != value) { _listFolder = value; NotifyPropertyChanged("listFolder"); } } }
        public string listFile { get { return _listFile; } set { if (_listFile != value) { _listFile = value; NotifyPropertyChanged("listFile"); } } }
        public string sourcePath { get { return _sourcePath; } set { if (_sourcePath != value) { _sourcePath = value; NotifyPropertyChanged("sourcePath"); } } }
        public string destPath { get { return _destPath; } set { if (_destPath != value) { _destPath = value; NotifyPropertyChanged("destPath"); } } }
        public string fodlerPath { get { return _fodlerPath; } set { if (_fodlerPath != value) { _fodlerPath = value; NotifyPropertyChanged("fodlerPath"); } } }
        public string filePath { get { return _filePath; } set { if (_filePath != value) { _filePath = value; NotifyPropertyChanged("filePath"); } } }
        public string fileName { get { return _fileName; } set { if (_fileName != value) { _fileName = value; NotifyPropertyChanged("fileName"); } } }
        public string fileType { get { return _fileType; } set { if (_fileType != value) { _fileType = value; NotifyPropertyChanged("fileType"); } } }
        public string fileSize { get { return _fileSize; } set { if (_fileSize != value) { _fileSize = value; NotifyPropertyChanged("fileSize"); } } }
        public string run { get { return _run; } set { if (_run != value) { _run = value; NotifyPropertyChanged("run"); } } }
        public string etc1 { get { return _etc1; } set { if (_etc1 != value) { _etc1 = value; NotifyPropertyChanged("etc1"); } } }
        public string etc2 { get { return _etc2; } set { if (_etc2 != value) { _etc2 = value; NotifyPropertyChanged("etc2"); } } }
        public string hdd1Serial { get { return _hdd1Serial; } set { if (_hdd1Serial != value) { _hdd1Serial = value; NotifyPropertyChanged("hdd1Serial"); } } }
        public string etc4 { get { return _etc4; } set { if (_etc4 != value) { _etc4 = value; NotifyPropertyChanged("etc4"); } } }
        public string etc5 { get { return _etc5; } set { if (_etc5 != value) { _etc5 = value; NotifyPropertyChanged("etc5"); } } }
        public string etc6 { get { return _etc6; } set { if (_etc6 != value) { _etc6 = value; NotifyPropertyChanged("etc6"); } } }
        public string etc7 { get { return _etc7; } set { if (_etc7 != value) { _etc7 = value; NotifyPropertyChanged("etc7"); } } }
        public string etc8 { get { return _etc8; } set { if (_etc8 != value) { _etc8 = value; NotifyPropertyChanged("etc8"); } } }
        public string etc9 { get { return _etc9; } set { if (_etc9 != value) { _etc9 = value; NotifyPropertyChanged("etc9"); } } }
        public string etc10 { get { return _etc10; } set { if (_etc10 != value) { _etc10 = value; NotifyPropertyChanged("etc10"); } } }
        public string insdt { get { return _insdt; } set { if (_insdt != value) { _insdt = value; NotifyPropertyChanged("insdt"); } } }

        private void NotifyPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
