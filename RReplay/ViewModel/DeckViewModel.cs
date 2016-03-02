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
        public const string UnitsCollectionPropertyName = "UnitsCollection";
        public const string UnitsGroupedViewPropertyName = "UnitsGroupedView";
        public const string PlayerPropertyName = "Player";

        private Deck deck;
        private Player player;

        private ObservableCollection<Unit> unitsCollection;

        private ICollectionView unitsView;

        private RelayCommand dragAndDropCommand;
        private RelayCommand refreshCode;

        /// <summary>
        /// Initializes a new instance of the DeckViewModel class.
        /// </summary>
        public DeckViewModel()
        {
            if ( IsInDesignMode )
            {
                this.Player = new Player()
                {
                    Deck = new Deck("XPQOsihRZFCjdDOE46kMboULWvgmghBAchlQkDGDNSBVAqgRI0oyUBCk1Kgl6S/qVJhIqsV+T1J6jDw="),
                    PlayerName = "My Super Name",
                    PlayerDeckName = "The name of the deck"
                };
            }
        }


        public Player Player
        {
            get
            {
                return player;
            }
            set
            {
                Set(PlayerPropertyName, ref player, value);
                Deck = new Deck(value.Deck.DeckCode);
            }
        }

        public string Title
        {
            get { return string.Format("{0} by {1}", Player.PlayerDeckName, Player.PlayerName); }
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