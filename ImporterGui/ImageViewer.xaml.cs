using ImporterGui.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : Page
    {
        public ImageViewerViewModel VM;
        public ImageViewer(IEnumerable<string> images)
        {
            InitializeComponent();
            VM = new ImageViewerViewModel(images);
            DataContext = VM;
            ButtonPrevious.Click += ButtonPrevious_Click;
            ButtonNext.Click += ButtonNext_Click;
            ButtonUnkeep.Click += ButtonKeep_Toggle;
            ButtonKeep.Click += ButtonKeep_Toggle;
            ButtonNext.GotFocus += ImageViewer_ReturnFocus;
            ButtonPrevious.GotFocus += ImageViewer_ReturnFocus;
            ButtonKeep.GotFocus += ImageViewer_ReturnFocus;
            ButtonUnkeep.GotFocus += ImageViewer_ReturnFocus;
            Focus();
            KeyUp -= ImageViewer_KeyUp;
            KeyUp += ImageViewer_KeyUp;
        }

        private void ImageViewer_ReturnFocus(object sender, RoutedEventArgs e)
        {
            Focus();
            e.Handled = true;
        }

        private void ImageViewer_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                    VM.GetNextImage();
                    break;
                case Key.Left:
                    VM.GetPreviousImage();
                    break;
                case Key.Return:
                    VM.ToggleImageKeep();
                    break;
            }                           
        }

        private void ButtonKeep_Toggle(object sender, RoutedEventArgs e)
        {
            VM.ToggleImageKeep();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            VM.GetNextImage();
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            VM.GetPreviousImage();
        }

    }
}
