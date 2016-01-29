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
using System.ComponentModel;
using ReplayManager;

namespace RDReplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ReplayFolder replayFolder;

        GridViewColumnHeader lstReplays_lastHeaderClicked = null;
        ListSortDirection lstReplays__lastDirection = ListSortDirection.Ascending;

        GridViewColumnHeader lstPlayers_lastHeaderClicked = null;
        ListSortDirection lstPlayers__lastDirection = ListSortDirection.Ascending;

        public MainWindow()
        {
            InitializeComponent();
            replayFolder = new ReplayFolder(Utilities.WarGame3ReplayFolder(), "WarGame3");
            lstReplays.ItemsSource = replayFolder.Replays;
        }

        private void lstReplays_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReplayFiles replayFile = lstReplays.SelectedItem as ReplayFiles;
            lstPlayers.ItemsSource = replayFile.Players;
        }

        void lstReplays_ColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != lstReplays_lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (lstReplays__lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    //string header = (headerClicked.Parent as GridViewColumn).DisplayMemberBinding.ToString();
                    Sort(headerClicked.Name, direction, CollectionViewSource.GetDefaultView(lstReplays.ItemsSource));

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header 
                    if (lstReplays_lastHeaderClicked != null && lstReplays_lastHeaderClicked != headerClicked)
                    {
                        lstReplays_lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    lstReplays_lastHeaderClicked = headerClicked;
                    lstReplays__lastDirection = direction;
                }
            }
        }

        void lstPlayers_ColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != lstPlayers_lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (lstPlayers__lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(header, direction, CollectionViewSource.GetDefaultView(lstPlayers.ItemsSource));

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate = Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header 
                    if (lstPlayers_lastHeaderClicked != null && lstPlayers_lastHeaderClicked != headerClicked)
                    {
                        lstPlayers_lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    lstPlayers_lastHeaderClicked = headerClicked;
                    lstPlayers__lastDirection = direction;
                }
            }
        }
        private void Sort(string sortBy, ListSortDirection direction, ICollectionView dataView)
        {
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        void lstPlayers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Player;
            if (item != null)
            {
                Clipboard.SetData(DataFormats.Text, item.PlayerDeckContent);
            }
        }
    }
}
