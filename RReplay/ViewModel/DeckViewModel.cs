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
        public const string TwoTransportsCollectionPropertyName = "TwoTransportsCollection";
        public const string OneTransportsCollectionPropertyName = "OneTransportsCollection";
        public const string UnitsCollectionPropertyName = "UnitsCollection";

        private Deck deck;

        private ObservableCollection<TwoTransportUnit> twoTransportsCollection;
        private ObservableCollection<OneTransportUnit> oneTransportsCollection;
        private ObservableCollection<Unit> unitsCollection;

        private RelayCommand dragAndDropCommand;

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
                return dragAndDropCommand ?? (dragAndDropCommand = new RelayCommand(( ) =>
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
                return deck;
            }
            set
            {
                Set("Deck", ref deck, value);

                twoTransportsCollection = new ObservableCollection<TwoTransportUnit>();

                foreach ( var unit in deck.TwoTransportUnitsList )
                {
                    twoTransportsCollection.Add(unit);
                }

                oneTransportsCollection = new ObservableCollection<OneTransportUnit>();

                foreach ( var unit in deck.OneTransportUnitsList )
                {
                    oneTransportsCollection.Add(unit);
                }

                unitsCollection = new ObservableCollection<Unit>();

                foreach ( var unit in deck.UnitsList )
                {
                    unitsCollection.Add(unit);
                }
            }
        }

        public ObservableCollection<TwoTransportUnit> TwoTransportsCollection
        {
            get
            {
                return twoTransportsCollection;
            }
            set
            {
                Set(TwoTransportsCollectionPropertyName, ref twoTransportsCollection, value);
            }
        }

        public ObservableCollection<OneTransportUnit> OneTransportsCollection
        {
            get
            {
                return oneTransportsCollection;
            }
            set
            {
                Set(OneTransportsCollectionPropertyName, ref oneTransportsCollection, value);
            }
        }


        public ObservableCollection<Unit> UnitsCollection
        {
            get
            {
                return unitsCollection;
            }
            set
            {
                Set(UnitsCollectionPropertyName, ref unitsCollection, value);
            }
        }
    }
}