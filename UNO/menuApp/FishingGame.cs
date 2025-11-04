namespace menuApp;

public class FishingGame // Implémentationdu  jeu de « pêche »
{

    public event EventHandler<PlayerEventArgs>? ResteUneCarte; //Evénement Uno
    public event EventHandler? PileDePiocheVide; // Evénement pile de pioche vide

    private GameBoard board;

    public async Task Start()
    {
        board = new GameBoard();

        board.VerifierNbJoueurs("3"); //Pré-définition à 3 joueurs, tel que dans l'énoncée
        board.NomJoueurs();
        board.AfficherJoueurs();
        board.VerifierNbCartes();
        board.InitialiserPaquet();
        board.DistribuerCartes();
        board.AfficherToutesMains();
        
        Console.WriteLine("======================================");
        Console.WriteLine("======= DEBUT DE LA PARTIE UNO =======");
        Console.WriteLine("======================================\n");

        // Lancement de la partie
        await board.JouerPartie();

        Console.WriteLine("======================================");
        Console.WriteLine("========== FIN DE LA PARTIE ==========");
        Console.WriteLine("======================================\n");
    }
}

//Evénement pour un joueur (utilisé pour Uno)
public class PlayerEventArgs : EventArgs
{
    public Player Player { get; }

    public PlayerEventArgs(Player player)
    {
        Player = player;
    }
}
public class LogEventArgs : EventArgs
{
    public string Message { get; }
    public LogEventArgs(string message)
    {
        Message = message;
    }
}
public class DrawStackEventArgs : EventArgs
{
    // Vous pouvez ajouter des propriétés spécifiques si nécessaire
    public string Message { get; }
    public DrawStackEventArgs(string message)
    {
        Message = message;
    }
}