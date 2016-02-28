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
        private RelayCommand refreshCode;

        /// <summary>
        /// Initializes a new instance of the DeckViewModel class.
        /// </summary>
        public DeckViewModel()
        {
            //unitInfoRepository = repository;
            ReceiveDeckInfo();
            if (IsInDesignMode)
            {
                Deck = new Deck("XPQOsihRZFCjdDOE46kMboULWvgmghBAchlQkDGDNSBVAqgRI0oyUBCk1Kgl6S/qVJhIqsV+T1J6jDw=");
            }
        }

        public RelayCommand DragAndDropCommand
        {
            get
            {
                return dragAndDropCommand ?? (dragAndDropCommand = new RelayCommand(( ) =>
                {
                    // TODO find how to refresh on drag & drop
                    Deck.NewList(UnitsCollection);
                }, () =>
                {
                    return true;
                }));
            }
        }

        public RelayCommand RefreshCode
        {
            get
            {
                return refreshCode ?? (refreshCode = new RelayCommand(() =>
                {
                    Deck.NewList(UnitsCollection);
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
                UnitsCollection = new ObservableCollection<Unit>(value.UnitsList);
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
                Set("UnitsCollection", ref unitsCollection, value);
            }
        }
    }
}