using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImporterGui.Models
{
    public class MemoryCard
    {
        private DriveInfo Drive { get; set; }
        public MemoryCard(DriveInfo driveInfo)
        {
            Drive = driveInfo;
        }

        public string DriveLetter { get { return Drive.Name; } }
        public string Name
        {
            get
            {
                return string.IsNullOrWhiteSpace(Drive.VolumeLabel) ?
                    Drive.DriveType.ToString() : Drive.VolumeLabel;
            }
        }
        public double Capacity
        {
            get
            {
                return Math.Round(BytesToGigaBytes(Drive.TotalSize), 2);
            }
        }
        public double FreeSpace
        {
            get
            {
                return Math.Round(BytesToGigaBytes(Drive.TotalSize - Drive.AvailableFreeSpace), 2);
            }
        }
        public double PercentFree
        {
            get
            {
                return Math.Round(100 * Math.Abs(1 - (double)FreeSpace / (double)Capacity), 2);
            }
        }

        private double BytesToMegabytes(long bytes)
        {
            return Math.Round(((double)bytes / 1048576.0), 2);
        }
        private double BytesToGigaBytes(long bytes)
        {
            return Math.Round(((double)bytes / 1073741824.0), 2);
        }

        public override string ToString()
        {
            return $"{DriveLetter} - {Name} ({FreeSpace} GB\\{Capacity} GB  {PercentFree}% Free)";
        }
    }
}
