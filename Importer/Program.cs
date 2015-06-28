using Importer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraImport
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.Title = "Picture Importer";

            var drive = SelectDrive();
            Copier copier = new Copier(drive, @"D:\Pictures", @"D:\Pictures");
            copier.Copy();
            Console.ReadKey();

            foreach (String directory in copier.Directories)
            {
                //LaunchProcess("explorer", directory);
                var newDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), directory.Split(Path.DirectorySeparatorChar).LastOrDefault());
                Directory.CreateDirectory(newDirectory);
                LaunchProcess("explorer", newDirectory);
                System.Threading.Thread.Sleep(1000);
                LaunchProcess("rundll32.exe", String.Format("shimgvw.dll ImageView_Fullscreen {0}", Directory.GetFiles(directory, "*.jpg", SearchOption.TopDirectoryOnly).First()));
                Console.WriteLine("Press [Enter] to continue");
                Console.ReadLine();
            }
        }

        private static MemoryCard SelectDrive()
        {
            List<MemoryCard> drives = GetDriveInfo();
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            while (drives.Count == 0 && cki.Key != ConsoleKey.Escape)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Could not detect any memory cards.");
                Console.WriteLine($"Press any key to try again, or [ESC] to quit.");
                Console.ForegroundColor = ConsoleColor.White;
                cki = Console.ReadKey(true);
                drives = GetDriveInfo();
                Console.Clear();
            }
            if (cki.Key == ConsoleKey.Escape)
                Environment.Exit(0);

            Console.WriteLine("Select the memory card drive:\n\n");
            return GetInput(drives.ToArray());
        }

        private static T GetInput<T>(params T[] choices)
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            for (int idx = 0; idx < choices.Length; idx++)
                Console.WriteLine($"\t{idx + 1}: {choices[idx].ToString()}");
            while (!(cki.Key >= ConsoleKey.D1 && cki.Key < (ConsoleKey.D1 + choices.Length)))
            {
                cki = Console.ReadKey(true);
                // Number keys and Number pad keys don't have the same Key, so if the user used the number pad
                // just convert it to a normal number.
                if (cki.Key >= ConsoleKey.NumPad1 && cki.Key < ConsoleKey.NumPad9)
                    cki = new ConsoleKeyInfo(cki.KeyChar, cki.Key - (ConsoleKey.NumPad1 - ConsoleKey.D1), 
                        cki.Modifiers.HasFlag(ConsoleModifiers.Shift), cki.Modifiers.HasFlag(ConsoleModifiers.Alt), 
                        cki.Modifiers.HasFlag(ConsoleModifiers.Control));
            }
            return choices[cki.Key - (ConsoleKey.D1)];
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

        private static void LaunchProcess(String command, String parameters)
        {
            ProcessStartInfo pInfo = (String.IsNullOrWhiteSpace(parameters) ? 
                new ProcessStartInfo(command) { UseShellExecute = false} :
                new ProcessStartInfo(command, parameters) { UseShellExecute = false });

            Process.Start(pInfo);
        }       
    }
}
