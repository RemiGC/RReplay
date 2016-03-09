using System.Windows;
using MahApps.Metro.Controls;
using RReplay.ViewModel;

namespace RReplay.View
{
    /// <summary>
    /// Description for DeckView.
    /// </summary>
    public partial class DeckView : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the DeckView class.
        /// </summary>
        public DeckView(DeckViewModel dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }
    }
}