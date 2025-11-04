namespace menuApp
{
    // Représente la pile de dépôt (cartes défaussées)
    public class DepositStack
    {
        private List<Card> _cards = new List<Card>();
        // Evement déclenché quand un 10 est joué.
        public event EventHandler? SensChange;
        // Evenement déclenché quand un As est joué.
        public event EventHandler? PasserTour;
        //Evenement déclenché quand un 2 est joué.
        public event EventHandler<DrawCardsEventArgs>? ForceLaPioche;

        // Obtient la carte du dessus de la pile de dépôt ou null si la pile est vide comme vu en cours
        public Card? CarteDuDessus
        {
            get
            {
                if (_cards.Count > 0)
                {
                    return _cards[_cards.Count - 1];
                }
                else
                {
                    return null;
                }
            }
        }

        // Obtient le nombre de cartes dans la pile de dépôt
        public int Count
        {
            get
            {
                return _cards.Count;
            }
            
        }

        // Pose une carte sur la pile de dépôt
        public void AjouterCartePileDepot(Card card)
        {
            _cards.Add(card);

            // Gérer les effets des cartes spéciales
            switch ((int)card.Value)
            {
                case 1: // L'As fait sauter le tour du joueur suivant.
                    OnUn(EventArgs.Empty);
                    break;
                case 2: // Le 2 fait piocher 2 cartes au prochain joueur.
                    OnDeux(new DrawCardsEventArgs(2));
                    break; 
                case 10: // le 10 permet de changer le sens du jeu.
                    OnDix(EventArgs.Empty); 
                    break;
                default:
                    break;
            }
        }

        // Retire toutes les cartes sauf la dernière et retourne la liste des cartes retirées.
        public List<Card> RetirerToutesCarteSaufTop()
        {
            List<Card> removed = new List<Card>();
            if (_cards.Count > 1)
            {
                // Conserver la dernière carte et retirer toutes les autres
                removed.AddRange(_cards.GetRange(0, _cards.Count - 1));
                _cards.RemoveRange(0, _cards.Count - 1);
            }
            return removed;
        }

        // Méthodes protégées pour déclencher les événements
        protected virtual void OnDix(EventArgs e)
        {
            SensChange?.Invoke(this, e);
        }

        protected virtual void OnUn(EventArgs e)
        {
            PasserTour?.Invoke(this, e);
        }

        protected virtual void OnDeux(DrawCardsEventArgs e)
        {
            ForceLaPioche?.Invoke(this, e);
        }

        // Classe pour les arguments de l'événement de pioche forcée
        public class DrawCardsEventArgs : EventArgs
        {
            public int NombreDeCarte { get; }

            public DrawCardsEventArgs(int nombreDeCarte)
            {
                NombreDeCarte = nombreDeCarte;
            }
        }
    }
}