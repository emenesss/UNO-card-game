namespace menuApp;

public class Score
{
    // Calcule et retourne le score total d'une main de cartes
    public int CalculerScore(List<Card> main)
    {
        int total = 0;
        foreach (Card carte in main)
        {
            total += carte.CalculerPointsCarte();
        }
        return total;
    }

    // Affiche le bilan final de la partie avec les scores des joueurs
    public void AfficherBilan(List<Player> joueurs)
    {
        Console.WriteLine("\n=== BILAN DE LA PARTIE ===");
        
        // Créer une liste avec joueurs et leurs scores
        var resultats = joueurs.Select(joueur => new
        {
            Joueur = joueur,
            Score = CalculerScore(joueur.Hand)
        }).OrderBy(r => r.Score).ToList();
        
        // Afficher les résultats
        foreach (var resultat in resultats)
        {
            Console.WriteLine($"{resultat.Joueur.Prenom}: {resultat.Score} points et " +
                                $"({resultat.Joueur.Hand.Count} cartes restantes)");
        }
        
        Console.WriteLine("=========================\n");
    }

    // // Retourne le joueur avec le score le plus bas (le gagnant) 
    // // L'utiliser si on veut faire un systeme de 1er, 2ieme, 3ieme..
    // public Player ObtenirGagnantParScore(List<Player> joueurs)
    // {
    //     return joueurs.OrderBy(j => CalculerScore(j.Hand)).First();
    // }
}
