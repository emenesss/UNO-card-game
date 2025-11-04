namespace menuApp;
/// <summary>
/// Stratégie qui combine plusieurs tactiques.
/// </summary>
public class StrategieCombinantToutesStrategies : IStrategie
{
    //Définition des attributs
    private Random _random;
    private StrategieMinimisationDePoint _minimisationDePoints;
    private StrategieAntiUno _antiUno;
    public string NomStrategie => "Combination des strategies";

    //Constructeur
    public StrategieCombinantToutesStrategies(Random random)
    {
        _random = random;
        _minimisationDePoints = new StrategieMinimisationDePoint();
        _antiUno = new StrategieAntiUno(random);
    }
    
    public Card? ChoisirCarte(List<Card> cartesJouables, Card carteDuDessus, ContexteDeJeu contexte)
    {
        if (cartesJouables.Count == 0)
            return null;
        
        // Si un adversaire a UNO, utiliser la stratégie anti-UNO.
        if (contexte.AdversaireAUno())
        {
            return _antiUno.ChoisirCarte(cartesJouables, carteDuDessus, contexte);
        }
        
        // Si on a beaucoup de cartes (>5), minimiser les points. //TODO si on a le temps essayer de faire par rapport au nombre de carte de chaque joueur.
        if (contexte.JoueurActuel.Hand.Count > 5)
        {
            return _minimisationDePoints.ChoisirCarte(cartesJouables, carteDuDessus, contexte);
        }
        
        // Sinon, privilégier les cartes spéciales pour garder le contrôle.
        var cartesSpeciales = cartesJouables
            .Where(c => c.Value == CardValue.As || 
                        c.Value == CardValue.Deux || 
                        c.Value == CardValue.Dix ||
                        c.Value == CardValue.Valet)
            .ToList();
        
        if (cartesSpeciales.Count > 0)
        {
            return cartesSpeciales[_random.Next(cartesSpeciales.Count)];
        }
        
        // Par défaut, jouer aléatoirement.
        return cartesJouables[_random.Next(cartesJouables.Count)];
    }
}
