using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Importer
{
    public class Copier
    {
        private string SourceLocation { get; set; }
        private string VaultDirectory { get; set; }
        private string PictureDirectory { get; set; }
        private string FileType { get; set; }

        public List<string> Directories { get; private set;} = new List<string>();
        public double TotalBytes { get; private set; }
        public double TotalKilobytes { get { return Math.Round((double)(TotalBytes / 1024), 2); } }
        public double TotalMegabytes { get { return Math.Round((double)(TotalBytes / Math.Pow(1024,2)), 2); } }
        public double TotalGigabytes { get { return Math.Round((double)(TotalBytes / Math.Pow(1024,3)), 2); } }
        public double CopiedBytes { get; private set; }
        public double CopiedKilobytes { get { return Math.Round((double)(CopiedBytes / 1024), 2); } }
        public double CopiedMegabytes { get { return Math.Round((double)(CopiedBytes / Math.Pow(1024, 2)), 2); } }
        public double CopiedGigabytes { get { return Math.Round((double)(CopiedBytes / Math.Pow(1024, 3)), 2); } }
        public int TotalFiles { get; private set; }
        public int FilesCopied { get; private set; }
        public int FilesSkipped { get; private set; }
        public int Errors { get; private set; }

        /// <summary>
        /// Copies pictures from the provided memory card to the vault directory and creates the picture directory.
        /// </summary>
        /// <param name="memoryCard">Source Media</param>
        /// <param name="vaultDirectory">Destination for storing all images</param>
        /// <param name="pictureDirectory">Destination for storing selected ("good") images</param>
        public Copier(MemoryCard memoryCard, string vaultDirectory, string pictureDirectory, string fileType = ".jpg")
        {
            SourceLocation = memoryCard.DriveLetter;
            VaultDirectory = vaultDirectory;
            PictureDirectory = pictureDirectory;
            FileType = fileType;
        }

        public void Copy()
        {
            var imageFilePaths = Directory.EnumerateFiles(SourceLocation, $"*{FileType}", SearchOption.AllDirectories);
            TotalFiles = imageFilePaths.Count();
            foreach (var imageFilePath in imageFilePaths)
            {
                try
                {
                    FileInfo imageFile = new FileInfo(imageFilePath);
                    TotalBytes += imageFile.Length;
                    var destinationDirectory = Path.Combine(VaultDirectory, imageFile.CreationTime.ToString("yyyyMMdd"));
                    if (!Directory.Exists(destinationDirectory)) Directory.CreateDirectory(destinationDirectory);
                    if (!Directories.Any(d => d.Equals(destinationDirectory, StringComparison.InvariantCultureIgnoreCase)))
                        Directories.Add(destinationDirectory);
                    var destinationPath = Path.Combine(destinationDirectory, imageFile.Name);
                    if (!File.Exists(destinationPath))
                    {
                        CopiedBytes += imageFile.Length;
                        Console.WriteLine($"Copying {imageFile.Name} ({Math.Round(((double)imageFile.Length / (1024 * 1024)), 2)} MB)");
                        File.Copy(imageFilePath, destinationPath);
                        FilesCopied++;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"Skipping {imageFile.Name}, already exists in the destination");
                        Console.ForegroundColor = ConsoleColor.White;
                        FilesSkipped++;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error copying file: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Errors++;
                }
            }
            Console.WriteLine($"\n----------\n{ToString()}");
        }

        public override string ToString()
        {
            return $"{FilesCopied} / {TotalFiles} ({CopiedMegabytes} MB) copied ({FilesSkipped} skipped, {Errors} errors.";
        }
    }
}
