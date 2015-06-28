using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImporterGui.ViewModels
{
    public class DriveImporterViewModel : ViewModelBase
    {
        private double _percentComplete;
        public double PercentComplete
        {
            get { return _percentComplete; }
            set { SetProperty(ref _percentComplete, value); }
        }

        private double _copiedMegabytes;
        public double CopiedMegabytes
        {
            get { return _copiedMegabytes; }
            set { SetProperty(ref _copiedMegabytes, value); }
        }

        private double _skippedMegabytes;
        public double SkippedMegabytes
        {
            get { return _skippedMegabytes; }
            set { SetProperty(ref _skippedMegabytes, value); }
        }

        private int _totalFiles;
        public int TotalFiles
        {
            get { return _totalFiles; }
            set { SetProperty(ref _totalFiles, value); }
        }

        private int _copiedFiles;
        public int CopiedFiles
        {
            get { return _copiedFiles; }
            set { SetProperty(ref _copiedFiles, value); }
        }

        private int _skippedFiles;
        public int SkippedFiles
        {
            get { return _skippedFiles; }
            set { SetProperty(ref _skippedFiles, value); }
        }

        public List<string> CopiedFileList { get; set; } = new List<string>();
    }
}
