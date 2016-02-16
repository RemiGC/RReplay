using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using RReplay.MessageInfrastructure;
using RReplay.Model;
using System.Collections.ObjectModel;

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
        public const string UnitsCollectionPropertyName = "UnitsCollection";

        private Deck deck;

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
                unitsCollection = new ObservableCollection<Unit>(value.UnitsList);

                Set("Deck", ref deck, value);
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
                unitsCollection = value;
            }
        }
    }
}