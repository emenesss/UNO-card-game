namespace menuApp;
/// <summary>
/// Stratégie de minimisation de points, joue les cartes avec le plus de points en premier.
/// </summary>
public class StrategieMinimisationDePoint : IStrategie
{
    // Définition des attributs
    public string NomStrategie => "Minimisation de points";
    
    public Card? ChoisirCarte(List<Card> cartesJouables, Card carteDuDessus, ContexteDeJeu contexte)
    {
        if (cartesJouables.Count == 0)
            return null;
        
            
        return cartesJouables
            .OrderByDescending(c => c.CalculerPointsCarte()) // Ordonne par points décroissants et prendre la première.
            .First();
    }
}
