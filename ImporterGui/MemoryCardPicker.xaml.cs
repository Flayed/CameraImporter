using ImporterGui.Models;
using ImporterGui.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImporterGui
{
    /// <summary>
    /// Interaction logic for MemoryCardPicker.xaml
    /// </summary>
    public partial class MemoryCardPicker : Page
    {
        DrivePickerViewModel VM = new DrivePickerViewModel();
        public MemoryCardPicker()
        {
            InitializeComponent();
            DataContext = VM;
            VM.Drives = GetDriveInfo();
            ButtonImport.Click += ButtonImport_Click;
        }

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            MemoryCard selectedMemoryCard = ListDriveNames.SelectedItem as MemoryCard;
            NavigationService.Navigate(new ImageImporter(selectedMemoryCard.DriveLetter));
        }

        private static List<MemoryCard> GetDriveInfo()
        {
            List<MemoryCard> drives = new List<MemoryCard>();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Removable)
                {
                    drives.Add(new MemoryCard(drive));
                }
            }

            return drives;
        }
    }
}
