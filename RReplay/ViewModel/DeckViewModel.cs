using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using RReplay.MessageInfrastructure;
using RReplay.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace RReplay.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DeckViewModel : ViewModelBase
    {
        private Deck _deck;

        private ICollectionView _twoTransportsUnits;
        private ICollectionView _oneTransportsUnits;
        private ICollectionView _units;

        private ObservableCollection<TwoTransportUnit> _twoTransportsCollection;
        private ObservableCollection<OneTransportUnit> _oneTransportsCollection;
        private ObservableCollection<Unit> _unitsCollection;

        private RelayCommand _dragAndDropCommand;

        /// <summary>
        /// Initializes a new instance of the DeckViewModel class.
        /// </summary>
        public DeckViewModel()
        {
            ReceiveDeckInfo();
            if (IsInDesignMode)
            {
                Deck = new Deck("WfgQSknJN2XKKV+HmurrM22ZVau2zMtlPsg0QziTKRTIijQak9GKY+FYyQ5H6f2WodaUAqf0XBgRLyLbOwGzxbp1MA==");
            }


        }

        public RelayCommand DragAndDropCommand
        {
            get
            {
                return _dragAndDropCommand ?? (_dragAndDropCommand = new RelayCommand(( ) =>
                {
                    // drag and drop
                }, () =>
                {
                    return true;
                }));
            }
        }

        void ReceiveDeckInfo()
        {
            if ( Deck == null )
            {
                Messenger.Default.Register<MessageCommunicator>(this, ( deckMsg ) =>
                {
                    Deck = deckMsg.deck;
                });
            }
        }

        public Deck Deck
        {
            get
            {
                return _deck;
            }
            set
            {
                Set("Deck", ref _deck, value);

                TwoTransportsUnits = CollectionViewSource.GetDefaultView(_deck.TwoTransportUnitsList);
                OneTransportsUnits = CollectionViewSource.GetDefaultView(_deck.OneTransportUnitsList);
                Units = CollectionViewSource.GetDefaultView(_deck.UnitsList);

                _twoTransportsCollection = new ObservableCollection<TwoTransportUnit>();

                foreach ( var unit in _deck.TwoTransportUnitsList )
                {
                    _twoTransportsCollection.Add(unit);
                }

                _oneTransportsCollection = new ObservableCollection<OneTransportUnit>();

                foreach ( var unit in _deck.OneTransportUnitsList )
                {
                    _oneTransportsCollection.Add(unit);
                }

                _unitsCollection = new ObservableCollection<Unit>();

                foreach ( var unit in _deck.UnitsList )
                {
                    _unitsCollection.Add(unit);
                }
            }
        }

        public ObservableCollection<TwoTransportUnit> TwoTransports
        {
            get
            {
                return _twoTransportsCollection;
            }
            set
            {
                Set("TwoTransports", ref _twoTransportsCollection, value);
            }
        }

        public ObservableCollection<OneTransportUnit> OneTransports
        {
            get
            {
                return _oneTransportsCollection;
            }
            set
            {
                Set("OneTransports", ref _oneTransportsCollection, value);
            }
        }


        public ObservableCollection<Unit> UnitsCollection
        {
            get
            {
                return _unitsCollection;
            }
            set
            {
                Set("UnitsCollection", ref _unitsCollection, value);
            }
        }


        public ICollectionView TwoTransportsUnits
        {
            get
            {
                return _twoTransportsUnits;
            }
            set
            {
                Set("TwoTransportsUnits", ref _twoTransportsUnits, value);
            }
        }

        public ICollectionView OneTransportsUnits
        {
            get
            {
                return _oneTransportsUnits;
            }
            set
            {
                Set("OneTransportUnits", ref _oneTransportsUnits, value);
            }
        }

        public ICollectionView Units
        {
            get
            {
                return _units;
            }
            set
            {
                Set("Units", ref _units, value);
            }
        }
    }
}