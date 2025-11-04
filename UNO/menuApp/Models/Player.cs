namespace menuApp;

public class Player : Person
{
    // Événements
    public delegate void PlayerEventHandler(object sender, PlayerEventArgs e);
    public event PlayerEventHandler unoHandlers;
    public delegate void LogEventHandler(object sender, LogEventArgs e);
    public event LogEventHandler logHandlers;

    // Taille maximale de la main
    public const int MaxHandSize = 8;

    // Main du joueur
    public List<Card> Hand { get; set; } = new List<Card>();
    
    // Startegie de selection de carte.
    public IStrategie Strategie { get; set; }

    // Constructeur de la classe Player en héritant de Person (base)
    public Player(int id, string nom, string prenom, IStrategie strategie) : base(id, nom, prenom) { Strategie = strategie; }

    // Méthodes pour ajouter et déposer des cartes
    public void AjouterCarte(Card carte)
    {
        // Si comme dans l'énoncé c'est 8 cartes dans la main au plus, alors décocher ces bouts de code.
        // if (Hand.Count >= MaxHandSize)
        // {
        //     // Vérification de la taille maximale de la main
        //     RaiseLog(new LogEventArgs($"{this} ne peut pas piocher plus de cartes, main pleine à capacité {MaxHandSize}."));
        //     throw new InvalidOperationException("Main pleine, impossible de piocher plus de cartes.");
        // }
        Hand.Add(carte);
        RaiseLog(new LogEventArgs($"{this} a pioché la carte {carte}"));
    }    
    public void DeposerCarte(Card carte, DepositStack deposit)
    {
        if (!Hand.Remove(carte))
        {
            // Vérification si la carte est dans la main
            RaiseLog(new LogEventArgs($"{this} a tenté de deposer une carte inexistante:  {carte}"));
            return;
        }
        try // Ajout de la carte à la pile de dépôt et gestion des exceptions éventuelles
        {
            if (EstUno())
            {
                RaiseUno(new PlayerEventArgs(this));
            }
            RaiseLog(new LogEventArgs($"{this} dépose la carte {carte}"));
            deposit.AjouterCartePileDepot(carte);
        } catch (Exception ex) { 
            Console.WriteLine($"Erreur lors de l'ajout de la carte au dépôt: {ex.Message}");
        }        
    }


    // Méthode pour vérifier si le joueur a UNO (une seule carte)
    public bool EstUno()
    {
        return Hand.Count == 1;
    }


    // Méthode appelée lorsqu'un joueur adverse n'a plus qu'une carte (UNO)
    public virtual void RaiseUno(PlayerEventArgs e)
    {
        PlayerEventHandler handlers = unoHandlers;
        if(handlers != null)
        {
            handlers(this, e);
        }
    }


    // Méthode pour lever un événement de journalisation
    public virtual void RaiseLog(LogEventArgs e)
    {
        LogEventHandler handlers = logHandlers;
        if(handlers != null)
        {
            handlers(this, e);
        }
    }
    public Card? ChoisirCarte(List<Card> cartesJouables, Card carteDuDessus, ContexteDeJeu contexte)
    {
        return Strategie.ChoisirCarte(cartesJouables, carteDuDessus, contexte);
    }
    
    // //La méthode CanPlay qu'on peut appeler dans FishingGame pour savoir si le joueur peut jouer.
    // //On vérifie si le joueur a au moins une carte qui peut etre jouée selon la carte posée.
    // public bool CanPlay(Card cartedessus)
    // {
    //     foreach (var carte in Hand)
    //     {
    //         if (carte.Value == cartedessus.Value ||
    //             carte.Color.Name == cartedessus.Color.Name ||
    //             carte.Value == CardValue.Valet)
    //             //Le valet peut etre joué même si la couleur ou la valeur ne correspond pas à la carte du dessus
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }


    // // TODO: Revoir les codes ci-bas
    // public Card ChoisirCarte(Card cartedessus)
    // {
    //     foreach (var carte in Hand)
    //     {
    //         if (carte.Value == cartedessus.Value ||
    //             carte.Color.Name == cartedessus.Color.Name ||
    //             carte.Value == CardValue.Valet)
    //
    //         {
    //             return carte;// on retourne la carte qui sera jouée
    //         }
    //
    //     }
    //
    //     throw new InvalidOperationException("ERREUR: Aucune carte jouable trouvée");
    // }
}