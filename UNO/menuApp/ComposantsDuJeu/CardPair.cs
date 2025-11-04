namespace menuApp;

// Implémentation la paire de 52 cartes
public class CardPair
{
    // Définit la liste des cartes représentant le paquet
    public List<Card> Cards { get; }

    // Constructeur qui initialise le paquet de cartes
    public CardPair()
    {
        // Initialiser la liste des cartes
        Cards = new List<Card>();
        
        // Générer toutes les combinaisons des 52 cartes.
        string[] suits = { CardColor.Trefle, CardColor.Carreau, CardColor.Coeur, CardColor.Pique };
        foreach (string suit in suits)
        {
            foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
            {
                Cards.Add(new Card(value, new CardColor(suit)));
            }
        }
        MelangerPaquet();
    }

    // Méthode pour compter le nombre de cartes restantes dans le paquet
    public int CompterCartes()
    {
        return Cards.Count;
    }

    // Méthode pour piger une carte du sommet du paquet
    public Card Piger()
    {
        Card carte = Cards[0];
        Cards.RemoveAt(0);
        return carte;
    }

    // Méthode pour mélanger le paquet de cartes
    public void MelangerPaquet()
    {
        Random random = new Random();
        int n = Cards.Count;
        for (int i = 0; i < n; i++)
        {
            int j = random.Next(i, n);
            (Cards[j], Cards[i]) = (Cards[i], Cards[j]);

        }
    }

    // Méthode pour vider le paquet de cartes et retourner les cartes retirées (utilisé pour "peupler" le DrawStack)
    public List<Card> ViderPaquet()
    {
        var old = new List<Card>(Cards);   
        Cards.Clear();
        return old;
    }
}