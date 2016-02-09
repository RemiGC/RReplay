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

        // All public Command
        public RelayCommand<CancelEventArgs> WindowClosingCommand { get; private set; }

        // The Data service that list of Replays
        private readonly IReplayRepository _dataService;

        // ObservableCollection of all Replays
        private ObservableCollection<Replay> _Replays;
        private List<Player> _players;

        // The Replay currently selected
        private Replay _selectedReplay;

        // Private Command
        private RelayCommand _refreshReplays;
        private RelayCommand<string> _copyDeckCodeCommand;
        private RelayCommand<ulong> _openSteamCommunityPageCommand;
        private RelayCommand _browseToReplayFileCommand;
        private RelayCommand _openReplayJSONView;


        public MainViewModel( IReplayRepository dataService )
        {
            // Window Closing
            WindowClosingCommand = new RelayCommand<CancelEventArgs>(( args ) =>
           {
               Settings.Default.Save();
           });
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
        /// <param name="item">An ObservableCollection of replays</param>
        /// <param name="parsingError">A list of replays that couldn't be parsed from Json</param>
        /// <param name="error">An exception if the repository couldn't get the replays</param>
        private void ReceiveData( ObservableCollection<Replay> item, List<Player> lstPlayers, List<Tuple<string, string>> parsingError, Exception error )
        {
            if ( error != null )
            {
                if ( error is EmptyReplaysPathException )
                {
                    // set the empty list anyway
                    _Replays = item;
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
                Replays = item;
                Players = lstPlayers;
                if ( parsingError.Count > 0 )
                {
                    foreach ( var fileNotParsed in parsingError )
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

        private bool MapFilter( object item )
        {
            Replay replay = item as Replay;
            return replay.Map.Contains("Conquete");
        }

        private bool PlayerNameInReplayFilter( object item )
        {
            Replay replay = item as Replay;
            return replay.Players.Exists(x => x.PlayerName.IndexOf(_nameFilter, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private bool PlayerNameInGameFilter( object item )
        {
            Player player = item as Player;
            return player.PlayerName.IndexOf(_nameFilter, StringComparison.OrdinalIgnoreCase) >= 0;
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
                //_selectedReplay = value;
                //RaisePropertyChanged(SelectedReplayPropertyName);
            }
        }

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

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}