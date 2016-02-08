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
        public const string SelectedReplayTeam1ViewPropertyName = "SelectedPlayerTeam1View";
        public const string SelectedReplayTeam2ViewPropertyName = "SelectedPlayerTeam2View";
        public const string NameFilterPropertyName = "NameFilter";
        public const string ReplayViewPropertyName = "ReplaysView";

        private ICollectionView _replaysView;
        private ICollectionView _selectedPlayerTeam1View;
        private ICollectionView _selectedPlayerTeam2View;

        private string _nameFilter;

        // All public Command
        public RelayCommand<CancelEventArgs> WindowClosingCommand { get; private set; }

        // The Data service that list of Replays
        private readonly IReplayRepository _dataService;

        // ObservableCollection of all Replays
        private ObservableCollection<Replay> _Replays;

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
        private void ReceiveData( ObservableCollection<Replay> item, List<Tuple<string, string>> parsingError, Exception error )
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

                // Refresh the view for the two teams.
                SelectedPlayerTeam1View = CollectionViewSource.GetDefaultView(SelectedReplay.FirstTeamPlayers);
                SelectedPlayerTeam2View = CollectionViewSource.GetDefaultView(SelectedReplay.SecondTeamPlayers);
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

                    // Refresh the view for the two teams.
                    if ( SelectedReplay != null )
                    {
                        SelectedPlayerTeam1View = CollectionViewSource.GetDefaultView(SelectedReplay.FirstTeamPlayers);
                        SelectedPlayerTeam2View = CollectionViewSource.GetDefaultView(SelectedReplay.SecondTeamPlayers);

                        // If the selected replay doesn't contain the searched name unselect it.
                        if ( SelectedPlayerTeam1View.IsEmpty && SelectedPlayerTeam2View.IsEmpty )
                        {
                            SelectedReplay = null;
                        }
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

        /// <summary>
        /// 
        /// </summary>
        public ICollectionView SelectedPlayerTeam1View
        {
            get
            {
                return _selectedPlayerTeam1View;
            }
            set
            {
                _selectedPlayerTeam1View = value;

                if ( !string.IsNullOrEmpty(NameFilter) )
                {
                    _selectedPlayerTeam1View.Filter = PlayerNameInGameFilter;

                }
                else
                {
                    _selectedPlayerTeam1View.Filter = null;
                }
                RaisePropertyChanged(SelectedReplayTeam1ViewPropertyName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollectionView SelectedPlayerTeam2View
        {
            get
            {
                return _selectedPlayerTeam2View;
            }
            set
            {
                _selectedPlayerTeam2View = value;
                if ( !string.IsNullOrEmpty(NameFilter) )
                {
                    _selectedPlayerTeam2View.Filter = PlayerNameInGameFilter;
                }
                else
                {
                    _selectedPlayerTeam2View.Filter = null;
                }
                RaisePropertyChanged(SelectedReplayTeam2ViewPropertyName);
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