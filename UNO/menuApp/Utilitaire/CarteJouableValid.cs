namespace menuApp;

public class CarteJouableValid
{
    public List<Card> ObtenirCartesJouables(List<Card> main, Card carteDuDessus)
    {
        List<Card> cartesJouables = new List<Card>();
        
        foreach (Card carte in main)
        {
            if (EstJouable(carte, carteDuDessus))
            {
                cartesJouables.Add(carte);
            }
        }
        
        return cartesJouables;
    }
    public bool EstJouable(Card carteAJouer, Card carteDuDessus)
    {
        // Le Valet peut être joué sur tout sauf un 2
        if (carteAJouer.Value == CardValue.Valet && carteDuDessus.Value != CardValue.Deux)
        {
            return true;
        }
        
        // Même couleur ou même valeur
        if (carteAJouer.Color.Name == carteDuDessus.Color.Name || 
            carteAJouer.Value == carteDuDessus.Value)
        {
            return true;
        }
        
        return false;
    }
    public bool ValiderNombreJoueurs(int nombre)
    {
        return nombre >= 2 && nombre <= 4;
    }
    public bool LimiteContreAttaque(int penalite)
    {
        return penalite > 8;
    }

    public string ChoisirCouleurAleatoire(Random random)
    {
        string[] couleurs = { CardColor.Coeur, CardColor.Pique, CardColor.Carreau, CardColor.Trefle };
        return couleurs[random.Next(couleurs.Length)];
    }

    public string ChoisirMeilleureCouleur(List<Card> main)
    {
        // Compter les cartes de chaque couleur
        var compteur = new Dictionary<string, int>
        {
            { CardColor.Coeur, 0 },
            { CardColor.Pique, 0 },
            { CardColor.Carreau, 0 },
            { CardColor.Trefle, 0 }
        };
        
        foreach (Card carte in main)
        {
            if (compteur.ContainsKey(carte.Color.Name))
            {
                compteur[carte.Color.Name]++;
            }
        }
        
        // Retourner la couleur la plus fréquente
        return compteur.OrderByDescending(kvp => kvp.Value).First().Key;
    }
}