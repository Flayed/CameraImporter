using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace ImporterGui.ViewModels
{
    public class ImageViewerViewModel : ViewModelBase
    {
        public ImageViewerViewModel() { }
        public ImageViewerViewModel(IEnumerable<string> images)
        {
            Images = images.Select(i => new Uri(i, UriKind.Absolute));
            ImageCount = Images.Count();
            if (ImageCount == 0) return;
            ShownImage = LoadImage(Images.ElementAt(0));
            if (Images.Count() > 1)
            {
                NextImage = LoadImage(Images.ElementAt(1));
                HasNextImage = true;
            }
        }

        private ImageSource _previousImage;
        public ImageSource PreviousImage
        {
            get { return _previousImage; }
            private set { SetProperty(ref _previousImage, value); }
        }

        private ImageSource _nextImage;
        public ImageSource NextImage
        {
            get { return _nextImage; }
            private set { SetProperty(ref _nextImage, value); }
        }

        private ImageSource _shownImage;
        public ImageSource ShownImage
        {
            get { return _shownImage; }
            private set { SetProperty(ref _shownImage, value); }
        }
        
        private int _imageCount = 0;
        public int ImageCount
        {
            get { return _imageCount; }
            set { SetProperty(ref _imageCount, value); }
        }

        private int _currentImageIndex;
        public int CurrentImageIndex
        {
            get { return _currentImageIndex; }
            private set { SetProperty(ref _currentImageIndex, value); }
        }

        private bool _hasPreviousImage = false;
        public bool HasPreviousImage
        {
            get { return _hasPreviousImage; }
            private set { SetProperty(ref _hasPreviousImage, value); }
        }

        private bool _hasNextImage = false;
        public bool HasNextImage
        {
            get { return _hasNextImage; }
            private set { SetProperty(ref _hasNextImage, value); }
        }

        private bool _isKept;
        public bool IsKept
        {
            get { return _isKept; }
            set { SetProperty(ref _isKept, value); }
        }


        public void GetPreviousImage()
        {
            if (!HasPreviousImage) return;
            CurrentImageIndex--;
            NextImage = ShownImage;
            ShownImage = PreviousImage;
            HasNextImage = true;
            IsKept = KeptIndexes.Any(k => k.Equals(CurrentImageIndex));
            if (CurrentImageIndex > 0)
            {
                PreviousImage = LoadImage(Images.ElementAt(CurrentImageIndex - 1));
                HasPreviousImage = true;
            }
            else
            {
                HasPreviousImage = false;
                PreviousImage = null;
            }
        }

        public void GetNextImage()
        {
            if (!HasNextImage) return;
            CurrentImageIndex++;
            PreviousImage = ShownImage;
            ShownImage = NextImage;
            HasPreviousImage = true;
            IsKept = KeptIndexes.Any(k => k.Equals(CurrentImageIndex));
            if (CurrentImageIndex < (ImageCount - 1))
            {
                NextImage = LoadImage(Images.ElementAt(CurrentImageIndex + 1));
                HasNextImage = true;
            }
            else
            {
                NextImage = null;
                HasNextImage = false;
            }
        }

        public void ToggleImageKeep()
        {
            if (IsKept)
            {
                KeptIndexes.RemoveAll(k => k.Equals(CurrentImageIndex));
                IsKept = false;
            }
            else
            {
                KeptIndexes.Add(CurrentImageIndex);
                IsKept = true;
            }
        }
        
        private IEnumerable<Uri> Images { get; set; }
        private List<int> KeptIndexes { get; set; } = new List<int>();

        private ImageSource LoadImage(Uri imageUri)
        {
            var bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.UriSource = imageUri;
            bmi.CacheOption = BitmapCacheOption.OnLoad;
            bmi.EndInit();
            return bmi;
            
        }
    }
}
