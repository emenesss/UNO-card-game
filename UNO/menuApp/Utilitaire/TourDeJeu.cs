namespace menuApp;

public class TourDeJeu
{
    private DrawStack _drawStack;
    private DepositStack _depositStack;
    private CarteJouableValid _carteJouableValid;
    private Random _random;
    
    public TourDeJeu(DrawStack drawStack, DepositStack depositStack, Random random)
    {
        _drawStack = drawStack;
        _depositStack = depositStack;
        _random = random;
        _carteJouableValid = new CarteJouableValid();
    }
    
    public List<Card> ObtenirCartesJouables(Player joueur)
    {
        if (!_depositStack.CarteDuDessus.HasValue)
        {
            return new List<Card>();
        }
        
        return _carteJouableValid.ObtenirCartesJouables(joueur.Hand, _depositStack.CarteDuDessus.Value);
    }
    public void PiocherMultiplesCartes(Player joueur, int nombre)
    {
        Console.WriteLine($"{joueur.Prenom} doit piocher {nombre} cartes!");
        
        for (int i = 0; i < nombre; i++)
        {
            Card? cartePiochee = _drawStack.PiocherCartePilePioche();
            if (cartePiochee.HasValue)
            {
                joueur.AjouterCarte(cartePiochee.Value);
            }
            else
            {
                Console.WriteLine("Plus de cartes disponibles dans la pioche!");
                break;
            }
        }
        
        Console.WriteLine($"{joueur.Prenom} a fini de piocher.");
    }
    public bool PiocherUneCarte(Player joueur)
    {
        Console.WriteLine($"{joueur.Prenom} ne peut pas jouer et pioche une carte.");
        
        Card? cartePiochee = _drawStack.PiocherCartePilePioche();
        if (cartePiochee.HasValue)
        {
            joueur.AjouterCarte(cartePiochee.Value);
            return true;
        }
        else
        {
            Console.WriteLine($"La pile de pioche est vide! {joueur.Prenom} ne peut pas piocher.");
            return false;
        }
    }
    public void JouerCarteNormale(Player joueur, Card carte)
    {
        joueur.DeposerCarte(carte, _depositStack);
    }
    
    public void JouerValet(Player joueur, Card valet, ContexteDeJeu contexte)
    {
        // Choisir la couleur selon la stratégie du joueur
        string nouvelleCouleur;
        
        // Stratégie intelligente: choisir la couleur la plus présente dans la main
        if (joueur.Strategie is StrategieCombinantToutesStrategies || joueur.Strategie is StrategieMinimisationDePoint)
        {
            nouvelleCouleur = _carteJouableValid.ChoisirMeilleureCouleur(joueur.Hand);
        }
        else
        {
            nouvelleCouleur = _carteJouableValid.ChoisirCouleurAleatoire(_random);
        }
        
        Console.WriteLine($"{joueur.Prenom} joue un {valet} et change vers {nouvelleCouleur}!");
        
        // Retirer le Valet de la main
        joueur.Hand.Remove(valet);
        
        // Créer un nouveau Valet avec la couleur choisie
        Card valetAvecNouvelleCouleur = new Card(CardValue.Valet, new CardColor(nouvelleCouleur));
        
        // L'ajouter à la pile de dépôt
        _depositStack.AjouterCartePileDepot(valetAvecNouvelleCouleur);
        
        // Vérifier UNO
        if (joueur.Hand.Count == 1)
        {
            joueur.RaiseUno(new PlayerEventArgs(joueur));
        }
    }
    public bool PeutContrer(Player joueur)
    {
        return joueur.Hand.Any(c => c.Value == CardValue.Deux);
    }
    public void ContrerAttaque(Player joueur, ref int penalite)
    {
        Card carte2 = joueur.Hand.First(c => c.Value == CardValue.Deux);
        
        joueur.DeposerCarte(carte2, _depositStack);
        Console.WriteLine($"{joueur.Prenom} contre l'attaque avec un 2!");
        penalite *= 2;
        
        Console.WriteLine($"La pénalité passe à {penalite} cartes!");
    }
    public bool VerifierVictoire(Player joueur)
    {
        if (joueur.Hand.Count == 0)
        {
            Console.WriteLine($"\n*** {joueur.Prenom} a gagné la partie! ***\n");
            return true;
        }
        return false;
    }
}