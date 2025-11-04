namespace menuApp;
/// <summary>
/// Stratégie anti-UNO, privilégie les cartes spéciales quand un adversaire a UNO.
/// </summary>
public class StrategieAntiUno : IStrategie
{
    // Définition des attributs
    private Random _random;
    public string NomStrategie => "Anti-UNO";

    // Constructeur
    public StrategieAntiUno(Random random)
    {
        _random = random;
    }
    
    public Card? ChoisirCarte(List<Card> cartesJouables, Card carteDuDessus, ContexteDeJeu contexte)
    {
        if (cartesJouables.Count == 0)
            return null;
        
        // Si un adversaire a UNO, prioriser les cartes spéciales.
        if (contexte.AdversaireAUno())
        {
            // Essayer de jouer un As (saute le tour)
            var carteAs = cartesJouables.FirstOrDefault(c => c.Value == CardValue.As);
            if (carteAs.Value != CardValue.As) 
            {
                // Chercher parmi les cartes jouables.
                foreach (var carte in cartesJouables)
                {
                    if (carte.Value == CardValue.As)
                        return carte;
                }
            }
            
            // Sinon essayer un 2 (force à piocher).
            var deux = cartesJouables.FirstOrDefault(c => c.Value == CardValue.Deux);
            if (deux.Value != CardValue.Deux)
            {
                foreach (var carte in cartesJouables)
                {
                    if (carte.Value == CardValue.Deux)
                        return carte;
                }
            }
            
            // Sinon essayer un 10.
            var dix = cartesJouables.FirstOrDefault(c => c.Value == CardValue.Dix);
            if (dix.Value != CardValue.Dix)
            {
                foreach (var carte in cartesJouables)
                {
                    if (carte.Value == CardValue.Dix)
                        return carte;
                }
            }
        }
        
        // Sinon, jouer aléatoirement.
        return cartesJouables[_random.Next(cartesJouables.Count)];
    }
}