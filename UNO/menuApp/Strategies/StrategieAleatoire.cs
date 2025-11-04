namespace menuApp;
/// <summary>
/// Stratégie aléatoire, choisit une carte au hasard.
/// </summary>
public class StrategieAleatoire : IStrategie
{
    // Définition des attributs
    private Random _random;
    public string NomStrategie => "Aléatoire";

    // Constructeur
    public StrategieAleatoire(Random random)
    {
        _random = random;
    }

    // Méthode pour choisir une carte aléatoirement parmi les cartes jouables
    public Card? ChoisirCarte(List<Card> cartesJouables, Card carteDuDessus, ContexteDeJeu contexte)
    {
        if (cartesJouables.Count == 0)
            return null;
            
        return cartesJouables[_random.Next(cartesJouables.Count)];
    }
}