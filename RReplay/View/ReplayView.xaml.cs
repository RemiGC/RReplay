using System;
using System.Windows;
using System.Windows.Controls;

namespace RReplay.View
{
    /// <summary>
    /// Description for ReplayView.
    /// </summary>
    public partial class ReplayView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the ReplayView class.
        /// </summary>
        public ReplayView()
        {
            InitializeComponent();
        }

        private void grid_SizeChanged( object sender, System.Windows.SizeChangedEventArgs e )
        {
            Grid replayGrid = (Grid)sender;
            if ( replayGrid != null )
            {
                if ( replayGrid.ActualWidth > 1280 )
                {
                    VisualStateManager.GoToElementState(replayGrid, "_highWidth", false);
                }
                else
                {
                    VisualStateManager.GoToElementState(replayGrid, "_lowWidth", false);
                }
            }
        }
    }
}