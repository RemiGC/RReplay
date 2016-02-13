using System.Windows;
using System.Windows.Controls;

namespace RReplay.View
{
    /// <summary>
    /// Description for ReplayView.
    /// </summary>
    public partial class ReplayView : UserControl
    {
        private enum States { LowWidth, HighWidth };

        private States CurrentStates;
        /// <summary>
        /// Initializes a new instance of the ReplayView class.
        /// </summary>
        public ReplayView()
        {
            InitializeComponent();
            CurrentStates = States.LowWidth;
        }

        private void grid_SizeChanged( object sender, System.Windows.SizeChangedEventArgs e )
        {
            Grid replayGrid = (Grid)sender;
            if ( replayGrid != null )
            {
                if ( replayGrid.ActualWidth > 1280 && CurrentStates == States.LowWidth)
                {
                    VisualStateManager.GoToElementState(replayGrid, "_highWidth", false);
                    CurrentStates = States.HighWidth;
                }
                else if ( replayGrid.ActualWidth < 1280 && CurrentStates == States.HighWidth )
                {
                    VisualStateManager.GoToElementState(replayGrid, "_lowWidth", false);
                    CurrentStates = States.LowWidth;
                }
            }
        }
    }
}