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

        private Deck deck;

        private ObservableCollection<Unit> unitsCollection;

        private ICollectionView unitsView;

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
                ICollectionView cv = CollectionViewSource.GetDefaultView(value);
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