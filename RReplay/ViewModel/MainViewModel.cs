using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RReplay.Model;
using System.Collections.ObjectModel;
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

        private RelayCommand<string> _copyDeckCodeCommand;
        public RelayCommand<string> CopyDeckCodeCommand
        {
            get
            {
                return _copyDeckCodeCommand ?? (_copyDeckCodeCommand = new RelayCommand<string>(CopyDeckCodeToClipboard));
            }
        }

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
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
            }
        }

        private void CopyDeckCodeToClipboard(string deckCode)
        {
            Clipboard.SetData(DataFormats.Text, deckCode);
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel( IReplayRepository dataService )
        {
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