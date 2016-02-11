using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using RReplay.MessageInfrastructure;

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

        private ICollectionView _replaysView;
        private ICollectionView _playersView;

        private string _nameFilter;

        private bool _replayViewVisible;
        private bool _allPlayersDeckVisible;
        private DeckView _deckView;

        // All Command
        private RelayCommand<CancelEventArgs> _windowClosingCommand;
        private RelayCommand _refreshReplays;
        private RelayCommand<string> _copyDeckCodeCommand;
        private RelayCommand<ulong> _openSteamCommunityPageCommand;
        private RelayCommand _browseToReplayFileCommand;
        private RelayCommand _openReplayJSONView;
        private RelayCommand<Deck> _openDeckViewCommand;

        // The Data service that collect and return the Replays
        private readonly IReplayRepository _dataService;

        // ObservableCollection of all Replays
        private ObservableCollection<Replay> _Replays;

        // The List of all players with the decks for that games
        private List<Player> _players;

        // The Replay currently selected
        private Replay _selectedReplay;

        public MainViewModel( IReplayRepository dataService )
        {
            ReplayViewVisible = true;

            _dataService = dataService;
            _dataService.GetData(ReceiveData);

            if ( IsInDesignMode )
            {
                SelectedReplay = Replays[1];
            }

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
                    _Replays = replaysCollection;
                    string newPath;
                    if ( ReplayFolderPicker.GetNewReplayFolder(Settings.Default.replaysFolder, out newPath) )
                    {
                        Settings.Default.replaysFolder = newPath;
                        _dataService.GetData(ReceiveData);
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
                return _Replays;
            }
            set
            {
                Set(ref _Replays, value);
                ReplaysView = CollectionViewSource.GetDefaultView(_Replays);
            }
        }

        public List<Player> Players
        {
            get
            {
                return _players;
            }
            set
            {
                Set(ref _players, value);
                PlayersView = CollectionViewSource.GetDefaultView(_players);
            }
        }

        /// <summary>
        /// Currently selected Replay
        /// </summary>
        public Replay SelectedReplay
        {
            get
            {
                return _selectedReplay;
            }
            set
            {
                Set(SelectedReplayPropertyName, ref _selectedReplay, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ReplayViewVisible
        {
            get
            {
                return _replayViewVisible;
            }
            set
            {
                Set(ReplayViewVisiblePropertyName, ref _replayViewVisible, value);
                Set(AllPlayersDeckVisiblePropertyName, ref _allPlayersDeckVisible, !value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllPlayersDeckVisible
        {
            get
            {
                return _allPlayersDeckVisible;
            }
            set
            {
                Set(AllPlayersDeckVisiblePropertyName, ref _allPlayersDeckVisible, value);
                Set(ReplayViewVisiblePropertyName, ref _replayViewVisible, !value);
            }
        }


        /// <summary>
        /// The PlayerName that will be filtered
        /// </summary>
        public string NameFilter
        {
            get
            {
                return _nameFilter;
            }
            set
            {
                if ( _nameFilter == value )
                {
                    return;
                }
                else
                {
                    _nameFilter = value;

                    ReplaysView = CollectionViewSource.GetDefaultView(Replays);
                    PlayersView = CollectionViewSource.GetDefaultView(PlayersView);

                    // If filter is active, show all the deck for the result
                    // otherwise show by team for the selected replay
                    if(string.IsNullOrEmpty(_nameFilter))
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
        /// The View, filtered on _filterName, that will be displayed in the MainList
        /// </summary>
        public ICollectionView ReplaysView
        {
            get
            {
                return _replaysView;
            }
            set
            {
                _replaysView = value;

                if ( !string.IsNullOrEmpty(NameFilter) )
                {
                    _replaysView.Filter = PlayerNameInReplayFilter;
                }
                else
                {
                    _replaysView.Filter = null;
                }
                RaisePropertyChanged(ReplayViewPropertyName);
            }
        }

        /// <summary>
        /// All the players view filtered on _filterName that will be displayed in the search result
        /// </summary>
        public ICollectionView PlayersView
        {
            get
            {
                return _playersView;
            }
            set
            {
                _playersView = value;
                if ( !string.IsNullOrEmpty(NameFilter) )
                {
                    _playersView.Filter = PlayerNameInGameFilter;

                }
                else
                {
                    _playersView.Filter = null;
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
                return _windowClosingCommand ?? (_windowClosingCommand = new RelayCommand<CancelEventArgs>(( value ) =>
                {
                    Settings.Default.Save();
                }));
            }
        }

        public RelayCommand<Deck> OpenDeckViewCommand
        {
            get
            {
                return _openDeckViewCommand ?? (_openDeckViewCommand = new RelayCommand<Deck>(( value ) =>
                {

                    /*if(_deckView == null )
                    {
                        _deckView = new DeckView();
                        _deckView.Show();
                    }*/

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
                return _copyDeckCodeCommand ?? (_copyDeckCodeCommand = new RelayCommand<string>(( value ) =>
                {
                    Clipboard.SetData(DataFormats.Text, value);
                }));
            }
        }

        /// <summary>
        /// Command to open the default browser to the Steam Community Page for that SteamID
        /// </summary>
        public RelayCommand<ulong> OpenSteamCommunityPageCommand
        {
            get
            {
                return _openSteamCommunityPageCommand ?? (_openSteamCommunityPageCommand = new RelayCommand<ulong>(( value ) =>
                {
                    string uri = @"http://steamcommunity.com/profiles/" + value.ToString();
                    System.Diagnostics.Process.Start(uri);
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
                return _browseToReplayFileCommand ?? (_browseToReplayFileCommand = new RelayCommand(() =>
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
                return _openReplayJSONView ?? (_openReplayJSONView = new RelayCommand(() =>
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
                return _refreshReplays ?? (_refreshReplays = new RelayCommand(() =>
                {
                    _dataService.GetData(ReceiveData);
                }));
            }
        }
        #endregion

        #region filter

        /// <summary>
        /// Filter for an invididual Replay depending on the players name in _nameFilter
        /// </summary>
        /// <param name="item">A replay obect</param>
        /// <returns></returns>
        private bool PlayerNameInReplayFilter( object item )
        {
            Replay replay = item as Replay;
            return replay.Players.Exists(x => x.PlayerName.IndexOf(_nameFilter, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Filter for player depending on the name in _nameFilter
        /// </summary>
        /// <param name="item">A player object</param>
        /// <returns></returns>
        private bool PlayerNameInGameFilter( object item )
        {
            Player player = item as Player;
            return player.PlayerName.IndexOf(_nameFilter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        #endregion  

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}