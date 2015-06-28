using ImporterGui.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace ImporterGui
{
    /// <summary>
    /// Interaction logic for ImageImporter.xaml
    /// </summary>
    public partial class ImageImporter : Page
    {
        private const string VaultDirectory = @"D:\Pictures";
        public DriveImporterViewModel VM = new DriveImporterViewModel();
        private string ImportDrive { get; set; }
        public ImageImporter(string importDrive)
        {
            InitializeComponent();
            ImportDrive = importDrive;
            DataContext = VM;
            Task.Run(() => { ImportImages(); });
        }

        public object Directories { get; private set; }
               
        public void ImportImages()
        {
            var imageFiles = Directory.EnumerateFiles(ImportDrive, "*.jpg", SearchOption.AllDirectories);
            VM.TotalFiles = imageFiles.Count();
            var c = 0;
            foreach (var imageFile in imageFiles)
            {
                c++;
                VM.PercentComplete = ((double)c / (double)VM.TotalFiles);
                try
                {
                    FileInfo fileInfo = new FileInfo(imageFile);
                    var destinationDirectory = Path.Combine(VaultDirectory, fileInfo.CreationTime.ToString("yyyyMMdd"));
                    if (!Directory.Exists(destinationDirectory)) Directory.CreateDirectory(destinationDirectory);
                    var destinationPath = Path.Combine(destinationDirectory, fileInfo.Name);
                    if (!File.Exists(destinationPath))
                    {
                        File.Copy(imageFile, destinationPath);
                        VM.CopiedMegabytes = Math.Round(VM.CopiedMegabytes + (double)(fileInfo.Length / Math.Pow(1024, 2)), 2);
                        VM.CopiedFiles++;
                        VM.CopiedFileList.Add(destinationPath);
                    }
                    else
                    {
                        VM.SkippedMegabytes = Math.Round(VM.SkippedMegabytes + (double)(fileInfo.Length / Math.Pow(1024, 2)), 2);
                        VM.SkippedFiles++;
                    }
                }
                catch { }
            }
            Dispatcher.Invoke(() =>
            {
                NavigationService.Navigate(new ImageViewer(VM.CopiedFileList));
            });
        }
    }
}
