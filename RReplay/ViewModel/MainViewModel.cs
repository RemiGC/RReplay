using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using RReplay.MessageInfrastructure;
using RReplay.Model;
using RReplay.Properties;
using RReplay.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace RReplay.ViewModel
{
    /// <summary>
    /// MainViewModel of the project. Keep all the replays files
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        // property Name
        public const string SelectedReplayPropertyName = "SelectedReplay";
        public const string NameFilterPropertyName = "NameFilter";
        public const string ReplayViewPropertyName = "ReplaysView";
        public const string ReplayViewVisiblePropertyName = "ReplayViewVisible";
        public const string AllPlayersDeckVisiblePropertyName = "AllPlayersDeckVisible";

        // The filtered replays View for the main view
        private ICollectionView replaysView;

        // The filtered players view that appear when the name filter is active
        private ICollectionView playersView;

        private string nameFilter;

        private bool replayViewVisible;
        private bool allPlayersDeckViewVisible;

        // All Command
        private RelayCommand<CancelEventArgs> windowClosingCommand;
        private RelayCommand refreshReplays;
        private RelayCommand<string> copyDeckCodeCommand;
        private RelayCommand<Player> openSteamCommunityPageCommand;
        private RelayCommand browseToReplayFileCommand;
        private RelayCommand openReplayJSONView;
        private RelayCommand<Deck> openDeckViewCommand;

        // The Data service that collect and return the Replays
        private readonly IReplayRepository dataService;

        // ObservableCollection of all Replays
        private ObservableCollection<Replay> replaysCollection;

        // The List of all players with the decks for that games
        private List<Player> playersList;

        // The Replay currently selected
        private Replay selectedReplay;

        public MainViewModel( IReplayRepository dataService )
        {
            this.dataService = dataService;
            this.dataService.GetData(this.ReceiveData);

            if ( IsInDesignMode )
            {
                SelectedReplay = Replays[0];
            }
            ReplayViewVisible = true;
            nameFilter = "";

        }

        /// <summary>
        /// Fonction that receive the replays from the Repository
        /// </summary>
        /// <param name="replaysCollection">An ObservableCollection of replays</param>
        /// <param name="playersList">A list of all players with their deck</param>
        /// <param name="parseErrorList">A list of replays that couldn't be parsed from Json</param>
        /// <param name="error">An exception if the repository couldn't get the replays</param>
        private void ReceiveData( ObservableCollection<Replay> replaysCollection, List<Player> playersList, List<Tuple<string, string>> parseErrorList, Exception error )
        {
            if ( error != null )
            {
                if ( error is EmptyReplaysPathException )
                {
                    // set the empty list anyway
                    this.replaysCollection = replaysCollection;
                    string newPath;
                    if ( ReplayFolderPicker.GetNewReplayFolder(Settings.Default.replaysFolder, out newPath) )
                    {
                        Settings.Default.replaysFolder = newPath;
                        dataService.GetData(ReceiveData);
                    }
                    else
                    {
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
            else
            {
                Replays = replaysCollection;
                Players = playersList;
                if ( parseErrorList.Count > 0 )
                {
                    foreach ( var fileNotParsed in parseErrorList )
                    {
                        MessageBox.Show(String.Format("The replay {0} couldn't be parsed. Error is:\n{1}", fileNotParsed.Item1, fileNotParsed.Item2));
                    }
                }
            }
        }

        #region Property

        /// <summary>
        /// ObservableCollection of all the replays 
        /// </summary>
        public ObservableCollection<Replay> Replays
        {
            get
            {
                return replaysCollection;
            }
            set
            {
                Set(ref replaysCollection, value);
                ReplaysView = CollectionViewSource.GetDefaultView(replaysCollection);
            }
        }

        public List<Player> Players
        {
            get
            {
                return playersList;
            }
            set
            {
                Set(ref playersList, value);
                PlayersView = CollectionViewSource.GetDefaultView(playersList);
            }
        }

        /// <summary>
        /// Currently selected Replay
        /// </summary>
        public Replay SelectedReplay
        {
            get
            {
                return selectedReplay;
            }
            set
            {
                Set(SelectedReplayPropertyName, ref selectedReplay, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ReplayViewVisible
        {
            get
            {
                return replayViewVisible;
            }
            set
            {
                Set(ReplayViewVisiblePropertyName, ref replayViewVisible, value);
                Set(AllPlayersDeckVisiblePropertyName, ref allPlayersDeckViewVisible, !value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllPlayersDeckVisible
        {
            get
            {
                return allPlayersDeckViewVisible;
            }
            set
            {
                Set(AllPlayersDeckVisiblePropertyName, ref allPlayersDeckViewVisible, value);
                Set(ReplayViewVisiblePropertyName, ref replayViewVisible, !value);
            }
        }


        /// <summary>
        /// The PlayerName that will be filtered
        /// </summary>
        public string NameFilter
        {
            get
            {
                return nameFilter;
            }
            set
            {
                if ( nameFilter == value )
                {
                    return;
                }
                else
                {
                    nameFilter = value;

                    ReplaysView = CollectionViewSource.GetDefaultView(Replays);
                    PlayersView = CollectionViewSource.GetDefaultView(PlayersView);

                    // If filter is active, show all the deck for the result
                    // otherwise show by team for the selected replay
                    if(string.IsNullOrEmpty(nameFilter))
                    {
                        ReplayViewVisible = true;
                    }
                    else
                    {
                        AllPlayersDeckVisible = true;
                    }

                    RaisePropertyChanged(NameFilterPropertyName);
                }
            }
        }


        /// <summary>
        /// The View, filtered on filterName, that will be displayed in the MainList
        /// </summary>
        public ICollectionView ReplaysView
        {
            get
            {
                return replaysView;
            }
            set
            {
                replaysView = value;

                if ( !string.IsNullOrEmpty(NameFilter) )
                {
                    replaysView.Filter = PlayerNameInReplayFilter;
                }
                else
                {
                    replaysView.Filter = null;
                }
                RaisePropertyChanged(ReplayViewPropertyName);
            }
        }

        /// <summary>
        /// All the players view filtered on filterName that will be displayed in the search result
        /// </summary>
        public ICollectionView PlayersView
        {
            get
            {
                return playersView;
            }
            set
            {
                playersView = value;
                if ( !string.IsNullOrEmpty(NameFilter) )
                {
                    playersView.Filter = PlayerNameInGameFilter;

                }
                else
                {
                    playersView.Filter = null;
                }
                RaisePropertyChanged("PlayersView");
            }
        }

        #endregion

        #region Public Command

        public RelayCommand<CancelEventArgs> WindowClosingCommand
        {
            get
            {
                return windowClosingCommand ?? (windowClosingCommand = new RelayCommand<CancelEventArgs>(( value ) =>
                {
                    Settings.Default.Save();
                }));
            }
        }

        public RelayCommand<Deck> OpenDeckViewCommand
        {
            get
            {
                return openDeckViewCommand ?? (openDeckViewCommand = new RelayCommand<Deck>(( value ) =>
                {
                    DeckView dv = new DeckView();
                    
                    Messenger.Default.Send<MessageCommunicator>(new MessageCommunicator()
                    {
                        deck = value
                    });

                    dv.Show();
                }));
            }
        }

        /// <summary>
        /// Command to copy the deck code to the clipboard
        /// </summary>
        public RelayCommand<string> CopyDeckCodeCommand
        {
            get
            {
                return copyDeckCodeCommand ?? (copyDeckCodeCommand = new RelayCommand<string>(( value ) =>
                {
                    Clipboard.SetData(DataFormats.Text, value);
                }));
            }
        }

        /// <summary>
        /// Command to open the default browser to the Steam Community Page for that SteamID
        /// </summary>
        public RelayCommand<Player> OpenSteamCommunityPageCommand
        {
            get
            {
                return openSteamCommunityPageCommand ?? (openSteamCommunityPageCommand = new RelayCommand<Player>(( value ) =>
                {
                    Player player = value as Player;
                    string uri = @"http://steamcommunity.com/profiles/" + player.SteamID.ToString();
                    System.Diagnostics.Process.Start(uri);
                },
                ( value ) =>
                {
                    if ( value is Player )
                    {
                        Player player = value as Player;
                        if ( player.IsAI )
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }));
            }
        }

        /// <summary>
        /// Command to open explorer to the current file
        /// </summary>
        public RelayCommand BrowseToReplayFileCommand
        {
            get
            {
                return browseToReplayFileCommand ?? (browseToReplayFileCommand = new RelayCommand(() =>
                {
                    Process.Start("explorer.exe", string.Format("/select,\"{0}\"", SelectedReplay.CompletePath));
                }));
            }
        }

        /// <summary>
        /// Command to open a ReplayJSONView of the SelectedReplay
        /// </summary>
        public RelayCommand OpenReplayJSONView
        {
            get
            {
                return openReplayJSONView ?? (openReplayJSONView = new RelayCommand(() =>
                {
                    ReplayJSONView replayJSONView = new ReplayJSONView();
                    replayJSONView.Show();
                }));
            }
        }

        /// <summary>
        /// Command to refresh the list of Replays
        /// </summary>
        public RelayCommand RefreshReplay
        {
            get
            {
                return refreshReplays ?? (refreshReplays = new RelayCommand(() =>
                {
                    dataService.GetData(ReceiveData);
                }));
            }
        }
        #endregion

        #region filter

        /// <summary>
        /// Filter for an invididual Replay depending on the players name in nameFilter
        /// </summary>
        /// <param name="item">A replay obect</param>
        /// <returns></returns>
        private bool PlayerNameInReplayFilter( object item )
        {
            Replay replay = item as Replay;
            return replay.Players.Exists(x => x.PlayerName.IndexOf(nameFilter, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Filter for player depending on the name in nameFilter
        /// </summary>
        /// <param name="item">A player object</param>
        /// <returns></returns>
        private bool PlayerNameInGameFilter( object item )
        {
            Player player = item as Player;
            return player.PlayerName.IndexOf(nameFilter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        #endregion  

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}