namespace menuApp
{
    // Représente la pile de pioche (cartes face cachée restantes à piocher)
    public class DrawStack
    {
        // Liste des cartes dans la pile de pioche
        private List<Card> _cards = new List<Card>();

        // Evénement déclenché lorsque la pile de pioche devient vide
        public event EventHandler? Empty; //TODO: Revoir grammaire des événements (ref. Cours)
        public delegate void DrawStackEventHandler(object sender, DrawStackEventArgs e);
        public event DrawStackEventHandler handlers;

        // Obtient le nombre de cartes dans la pile de pioche
        public int Count
        {
            get
            {
                return _cards.Count;
            }
        }

        // Constructeur à partir d'une liste de cartes initiale (déjà mélangée).
        public DrawStack(IEnumerable<Card> cards) 
        {
            //J'utilise IEnumerable au lieu de List pour accepter toutes collection confondu
            _cards = new List<Card>(cards);
        }

        // Pioche une carte du sommet de la pile. Renvoie la carte piochée ou null si plus de cartes
        public Card? PiocherCartePilePioche()
        {
            if (_cards.Count == 0)
            {
                // Déclenche l'événement si la pioche est vide
                Empty?.Invoke(this, EventArgs.Empty);
                return null;
            }

            // Extraire la dernière carte de la liste (sommet de la pioche)
            int lastIndex = _cards.Count - 1;
            Card card = _cards[lastIndex];
             _cards.RemoveAt(lastIndex);
            return card;
        }

        // Ajoute une liste de cartes à la pioche (ex: après mélange de la défausse)
        public void AjouterCartePilePioche(IEnumerable<Card> cards)
        {
            _cards.AddRange(cards);
        }

        // Mélange les cartes restantes dans la pioche
        public void MelangerCartePilePioche(Random random)
        {
            for (int i = _cards.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                // TODO: Échanger _cards[i] et _cards[j]
                Card temp = _cards[i];
                _cards[i] = _cards[j];
                _cards[j] = temp;
            }
        }
    }
}