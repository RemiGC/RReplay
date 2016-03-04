using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Practices.ServiceLocation;
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
        public const string UnitsCollectionPropertyName = "UnitsCollection";
        public const string UnitsGroupedViewPropertyName = "UnitsGroupedView";
        public const string PlayerPropertyName = "Player";

        // The Main deck for this model
        private Deck deck;

        // The player that created the deck
        private Player player;

        // The collection of units from the deck
        private ObservableCollection<Unit> unitsCollection;

        // The grouped collectionview of the units
        private ICollectionView unitsView;

        private RelayCommand dragAndDropCommand;
        private RelayCommand refreshCode;

        private readonly IDeckInfoRepository deckInfoRepository;

        /// <summary>
        /// Initializes a new instance of the DeckViewModel class.
        /// </summary>
        public DeckViewModel(IDeckInfoRepository repository)
        {
            this.deckInfoRepository = repository;
            if ( IsInDesignMode )
            {
                this.Player = new Player()
                {
                    Deck = new Deck("XPQOsihRZFCjdDOE46kMboULWvgmghBAchlQkDGDNSBVAqgRI0oyUBCk1Kgl6S/qVJhIqsV+T1J6jDw=", this.deckInfoRepository),
                    PlayerName = "My Super Name",
                    PlayerDeckName = "The name of the deck"
                };
            }
        }

        /// <summary>
        /// The player who own the deck
        /// </summary>
        public Player Player
        {
            get
            {
                return player;
            }
            set
            {
                Set(PlayerPropertyName, ref player, value);
                Deck = new Deck(value.Deck.DeckCode, deckInfoRepository);
            }
        }

        /// <summary>
        /// The Title for this viewmodel
        /// </summary>
        public string Title
        {
            get { return string.Format("{0} by {1}", Player.PlayerDeckName, Player.PlayerName); }
        }

        /// <summary>
        /// Drag & Drop command to move units within the collection
        /// </summary>
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

        /// <summary>
        /// The CollectionView of all the units grouped on factory
        /// </summary>
        public ICollectionView UnitsGroupedView
        {
            get
            {
                return unitsView;
            }
            set
            {
                unitsView = value;
                RaisePropertyChanged(UnitsGroupedViewPropertyName);
            }
        }

        /// <summary>
        /// Refresh the deck code based on the new order of the units collection
        /// </summary>
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

        /// <summary>
        /// The main Deck for this viewmodel
        /// </summary>
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

        /// <summary>
        /// The units collection of the Deck
        /// </summary>
        public ObservableCollection<Unit> UnitsCollection
        {
            get
            {
                return unitsCollection;
            }

            set
            {
                Set("UnitsCollection", ref unitsCollection, value);
                ICollectionView cv = CollectionViewSource.GetDefaultView(value);
                // Keep the same order as in game
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Factory");
                groupDescription.GroupNames.Add(3);
                groupDescription.GroupNames.Add(6);
                groupDescription.GroupNames.Add(13);
                groupDescription.GroupNames.Add(9);
                groupDescription.GroupNames.Add(10);
                groupDescription.GroupNames.Add(8);
                groupDescription.GroupNames.Add(11);
                groupDescription.GroupNames.Add(7);
                groupDescription.GroupNames.Add(12);
                cv.GroupDescriptions.Add(groupDescription);
                UnitsGroupedView = cv;
            }
        }
    }
}