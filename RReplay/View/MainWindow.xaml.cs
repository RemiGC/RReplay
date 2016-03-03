using RReplay.ViewModel;
using System.Windows;
using MahApps.Metro.Controls;

namespace RReplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += ( s, e ) => ViewModelLocator.Cleanup();
        }
    }
}