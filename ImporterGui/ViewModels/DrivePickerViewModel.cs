using ImporterGui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImporterGui.ViewModels
{
    public class DrivePickerViewModel : ViewModelBase
    {
        private List<MemoryCard> _drives = new List<MemoryCard>();
        public List<MemoryCard> Drives
        {
            get { return _drives; }
            set { SetProperty(ref _drives, value); }
        }
    }
}
