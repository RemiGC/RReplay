using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RReplay.Model;
using RReplay.Properties;
using RReplay.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace RReplay.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IReplayRepository _dataService;
        ObservableCollection<Replay> _Replays;


        public const string SelectedReplayPropertyName = "SelectedReplay";
        private Replay _selectedReplay;

        public RelayCommand<CancelEventArgs> WindowClosingCommand { get; private set; }


        private RelayCommand<string> _copyDeckCodeCommand;
        public RelayCommand<string> CopyDeckCodeCommand
        {
            get
            {
                return _copyDeckCodeCommand ?? (_copyDeckCodeCommand = new RelayCommand<string>( (value) =>
                {
                    Clipboard.SetData(DataFormats.Text, value);
                }));
            }
        }

        private RelayCommand<string> _browseToReplayFileCommand;
        public RelayCommand<string> BrowseToReplayFileCommand
        {
            get
            {
                return _browseToReplayFileCommand ?? (_browseToReplayFileCommand = new RelayCommand<string>(( value ) =>
                {
                    Process.Start("explorer.exe", string.Format("/select,\"{0}\"", value));
                }));
            }
        }

        private RelayCommand _openReplayJSONView;
        public RelayCommand OpenReplayJSONView
        {
            get
            {
                return _openReplayJSONView ?? (_openReplayJSONView = new RelayCommand( () =>
                {
                    ReplayJSONView replayJSONView = new ReplayJSONView();
                    replayJSONView.Show();
                }));
            }
        }

        public ObservableCollection<Replay> Replays
        {
            get
            {
                return _Replays;
            }
            set
            {
                Set(ref _Replays, value);
            }
        }

        public MainViewModel( IReplayRepository dataService )
        {
            // Window Closing
            WindowClosingCommand = new RelayCommand<CancelEventArgs>( (args) =>
            {
                Settings.Default.Save();
            });

            _dataService = dataService;
            _dataService.GetData(
                ( item, error ) =>
                {
                    if ( error != null )
                    {
                        // Report error here
                        return;
                    }

                    _Replays = item;
                });

            if ( IsInDesignMode )
            {
                SelectedReplay = Replays[0];
            }
        }


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

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}