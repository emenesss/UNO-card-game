namespace menuApp;

public interface IStrategie
{
    Card? ChoisirCarte(List<Card> carteJouable, Card carteDuDessus, ContexteDeJeu contexte);
    string NomStrategie { get; }
}
