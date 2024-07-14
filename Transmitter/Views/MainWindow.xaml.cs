using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Transmitter.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {

        public ObservableCollection<SoftwareModel> SoftwareColt { get; set; } = [];
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Top = SystemParameters.PrimaryScreenHeight - 200;
            Left = SystemParameters.PrimaryScreenWidth / 2 - this.Width / 2;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void LbSoftware_Drop(object sender, DragEventArgs e)
        {
            var array = (string[])e.Data.GetData(DataFormats.FileDrop);
            var lnkFilePath = array[0].ToString();
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(lnkFilePath);
            Console.WriteLine("Target path: " + shortcut.TargetPath);
            Console.WriteLine("Working directory: " + shortcut.WorkingDirectory);
            Console.WriteLine("Arguments: " + shortcut.Arguments);
            Console.WriteLine("Description: " + shortcut.Description);
            Console.WriteLine("Icon location: " + shortcut.IconLocation);

            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(System.Drawing.Icon.ExtractAssociatedIcon(lnkFilePath).Handle, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(32, 32));

            SoftwareColt.Add(new SoftwareModel()
            {
                Icon = imageSource,
                IconHeight = imageSource.Height,
                IconWidth = imageSource.Width,
                Name = "1",
                Path = shortcut.TargetPath
            });
        }

        private void Image_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var _model = (e.OriginalSource as System.Windows.Controls.Image).DataContext as SoftwareModel;
            if (_model != null)
                Process.Start(_model.Path);
        }
    }

    public class SoftwareModel
    {
        public ImageSource Icon { get; set; }
        public double IconHeight { get; set; }
        public double IconWidth { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
