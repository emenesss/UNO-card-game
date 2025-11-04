using System;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using static menuApp.Player;

namespace menuApp
{

    class GameBoard
    {
        // Définir les propriétés et champs nécessaires pour le plateau de jeu
        private int _nbJoueurs { get; set; } = -1;

        private int iCpj; // Cpj = Carte par joueur.
        private int iSens = 1; // 1 = sens horaire, -1 = sens anti-horaire.
        private int iJoueurCourant = 0; //Index du joueur actuel.
        private int iPenaliteCartes = 0; //Nombre de carte a piocher.
        private bool bSauterTour = false; //Indique si le tour doit être sauté.

        private List<Player> _players = new List<Player>(); //Liste des joueurs.

        private CardPair? _cardPack; // paquet de 52 cartes
        private DepositStack _depositStack; // pile de dépôt
        private DrawStack _drawStack; // pile de pioche
        private Random _random; // Générateur de nombres aléatoires

        private CarteJouableValid _carteJouableValid; // Validateur
        private Score _score; // Gestionnaire de score
        private TourDeJeu _tourDeJeu; //Gestionnaire des tours de jeu pour les joueurs

        public List<Player> Players => _players; // Liste des joueurs de la partie

        public delegate void LogEventHandler(object sender, LogEventArgs e);
        public event LogEventHandler logHandlers;

        public GameBoard()
        {
            // Initialisation des composants du jeu
            _depositStack = new DepositStack();
            _depositStack.SensChange += HandleChangementOrientation;
            _depositStack.PasserTour += (s, e) => { bSauterTour = true; };
            _depositStack.ForceLaPioche += (s, e) => { iPenaliteCartes = e.NombreDeCarte; };
            _random = new Random();
            
            _carteJouableValid = new CarteJouableValid();
            _score = new Score();
            this.logHandlers += HandleLog;
        }

        //Cette méthode permet de demande aux joueurs combien sont-ils
        //Une partie de jeu de pêche peut se faire avec 2, 3 ou 4 joueurs.
        public void VerifierNbJoueurs(string? iPreDefined)
        {
            string saisie;
            while (true)
            {
                if (iPreDefined != null)
                {
                    saisie = iPreDefined;
                }
                else
                {
                    Console.WriteLine("=== Veuillez choisir le nombre de joueurs, entre 2 et 4: ===");
                    saisie = Console.ReadLine();
                }
                

                try
                {
                    _nbJoueurs = int.Parse(saisie);
                    // Utilisation du validateur pour vérifier si le nombre est dans la plage valide
                    if (_carteJouableValid.ValiderNombreJoueurs(_nbJoueurs))
                    {
                        Console.WriteLine($"- Le nombre de joueurs que vous avez choisis est: {_nbJoueurs} -");
                        break;
                    }
                    else
                    {
                        // Gestion du cas où le nombre n'est pas dans la plage valide
                        Console.WriteLine("ERREUR. Veuillez entrer un nombre de 2 à 4 ");
                    }
                }
                catch (FormatException)
                {
                    // Gestion de l'exception si la saisie n'est pas un nombre valide
                    Console.WriteLine("ERREUR, vous devez entrer un chiffre");
                }
            }
        }

        //Cette méthode permet de demander le nom de chaque joueur
        //Les joueurs sont des personnes avec nom et prénom et un identifiant unique.
        public void NomJoueurs()
        {
            if (_nbJoueurs == -1)
            {
                //Ici, j'ajoute une vérification afin que le nombre de joueur ait bien été défini d'avance
                Console.WriteLine("ERREUR: Vous devez définir le nombre de joueur d'abord.");
                VerifierNbJoueurs("3");
            }
            // Choisir aléatoirement un joueur qui utilisera la stratégie de minimisation de points
            int joueurAvecMinimisation = _random.Next(0, _nbJoueurs);

            for (int i = 1; i <= _nbJoueurs; i++)
            {
                string nom = DemanderNom(i);
                string prenom = DemanderPrenom(i);

                // Assigner une stratégie selon le joueur
                IStrategie strategie;
                if (i - 1 == joueurAvecMinimisation)
                {
                    strategie = new StrategieMinimisationDePoint();
                }
                else
                {
                    // Mélange de stratégies pour les autres joueurs
                    int typeStrategie = _random.Next(0, 3);
                    strategie = typeStrategie switch
                    {
                        0 => new StrategieAleatoire(_random),
                        1 => new StrategieAntiUno(_random),
                        _ => new StrategieCombinantToutesStrategies(_random)
                    };
                }

                Player player = new Player(i, nom, prenom, strategie);
                _players.Add(player);
                // Ajout des gestionnaires d'événements aux joueurs
                player.unoHandlers += HandleUno;
                player.logHandlers += HandleLog;

                if (i - 1 == joueurAvecMinimisation)
                {
                    RaiseLog(new LogEventArgs($"\n{player.Prenom} utilisera la stratégie: {strategie.NomStrategie}\n"));
                }
            }
        }

        // Méthode pour demander le nom d'un joueur
        private string DemanderNom(int numeroJoueur)
        {
            while (true)
            {
                Console.Write("=== Entrer le nom du joueur " + numeroJoueur + ": ===");
                string nom = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(
                        nom)) // IsNullOrWhiteSpace au lieu de IsNullOrEmpty car les espaces compte comme des caracteres.
                {
                    return nom;
                }
                Console.WriteLine("ERREUR: Le nom ne peut pas etre vide");
            }
        }

        // Méthode pour demander le prénom d'un joueur
        private string DemanderPrenom(int numeroJoueur)
        {
            while (true)
            {
                Console.Write("=== Entrer le prenom du joueur " + numeroJoueur + ": ===");
                string prenom = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(prenom))
                {
                    return prenom;
                }

                Console.WriteLine("ERREUR: Le prenom ne peut pas etre vide");
            }
        }

        // Définit le nombre de cartes par joueur (entre 5 et 8)
        public void VerifierNbCartes()
        {
            iCpj = _random.Next(5, 9); //5 minvalue inclus, 9 maxvalue exclus 
            Console.WriteLine($"Vous jouerez avec {iCpj} cartes par joueur.");
        }

        // Affiche la liste des joueurs avec leurs identifiants, noms et prénoms
        public void AfficherJoueurs()
        {
            Console.WriteLine("Voici la liste des joueurs: ");
            foreach (var player in _players)
            {
                RaiseLog(new LogEventArgs($"Joueur {player.Id}: {player.Nom.ToUpper()}, {player.Prenom}"));
            }
            Console.WriteLine();
        }

        //initialise le paquet de 52 cartes.
        public void InitialiserPaquet()
        {
            _cardPack = new CardPair();
        }

        // Distribue les cartes aux joueurs et initialise les piles.
        public void DistribuerCartes()
        {
            Console.WriteLine("Distribution des cartes en cours...\n");
            foreach (Player joueur in _players)
            {
                // Distribuer iCpj(Carte Par Joueur) cartes à chaque joueur
                for (int i = 0; i < iCpj; i++)
                {
                    joueur.Hand.Add(_cardPack.Piger());
                }
            }

            //Premiere carte sur la pile de dépot.
            Card premiereCarte = _cardPack.Piger();
            _depositStack.AjouterCartePileDepot(premiereCarte);
            // Réinitialiser immédiatement les effets dans le cas ou la game débute sur une carte spéciale.
            iPenaliteCartes = 0;
             bSauterTour = false;
            Console.WriteLine($"Premiere carte sur la pile de dépot: {premiereCarte}\n");

            //Le reste dans la pile de pioche
            _drawStack = new DrawStack(_cardPack.ViderPaquet());
            _drawStack.handlers += HandleDrawStack;
            _drawStack.Empty += HandleStackEmpty;
            RaiseLog(new LogEventArgs($"Pile de pioche initialisée avec {_drawStack.Count} cartes\n"));
            
            _tourDeJeu = new TourDeJeu(_drawStack, _depositStack, _random);
        }

        public void AfficherToutesMains()
        {
            // Affiche la main de chaque joueur
            foreach (Player joueur in _players)
            {
                AfficherMain(joueur);
            }
        }

        public void AfficherMain(Player joueur)
        {
            // Affiche la main d'un joueur
            RaiseLog(new LogEventArgs($"Main du joueur {joueur.Id} : {joueur.Prenom}:"));
            foreach (Card carte in joueur.Hand)
            {
                Console.WriteLine($"{carte}");
            }
            Console.WriteLine("");
        }

        // Boucle principale du jeu
        public async Task JouerPartie()
        {
            // Choisir aléatoirement le premier joueur
            iJoueurCourant = _random.Next(0, _nbJoueurs);
            Console.WriteLine($"Le joueur {_players[iJoueurCourant].Prenom} commence la partie!\n");

            bool partieEnCours = true;

            while (partieEnCours)
            {
                Player joueurActuel = _players[iJoueurCourant];
                RaiseLog(new LogEventArgs($"--- Tour de {joueurActuel.Prenom}({joueurActuel.Strategie.NomStrategie}) ---"));
                RaiseLog(new LogEventArgs($"Carte sur la pile de dépôt: {_depositStack.CarteDuDessus}"));

                // Délai pour simulation
                await Task.Delay(1500);

                partieEnCours = await TourJoueur(joueurActuel);

                if (!partieEnCours)
                {
                    break;
                }

                // Passer au joueur suivant
                ProchainJoueur();
                Console.WriteLine();
            }
            // Calculer et afficher les scores
            _score.AfficherBilan(_players);
        }

        // Gère le tour d'un joueur via le comportement et les vérifications nécessaires
        private async Task<bool> TourJoueur(Player joueur)
        {
            // Vérifier si la pile de dépôt a au moins une carte
            if (!_depositStack.CarteDuDessus.HasValue)
            {
                Console.WriteLine("ERREUR: Aucune carte sur la pile de dépôt!");
                return false;
            }

            // Vérifier si le tour doit être sauté à cause d'un As
            if (bSauterTour)
            {
                RaiseLog(new LogEventArgs($"Le tour de {joueur.Prenom} est sauté à cause d'un As!"));
                bSauterTour = false;
                return true;
            }

            // Vérifier si le joueur doit piocher à cause d'un 2
            if (iPenaliteCartes > 0)
            {
                return await GererPenalitePioche(joueur);
            }

            ContexteDeJeu contexte = new ContexteDeJeu(_players, joueur, iSens, iPenaliteCartes);

            // Trouver les cartes jouables
            List<Card> cartesJouables = _tourDeJeu.ObtenirCartesJouables(joueur);

            // Selon le contexte, le joueur choisit une carte à jouer
            Card? carteAJouer = joueur.ChoisirCarte(cartesJouables, _depositStack.CarteDuDessus.Value, contexte);

            // Jouer la carte choisie ou piocher si aucune carte n'est jouable
            if (carteAJouer.HasValue)
            {
                return await JouerCarte(joueur, carteAJouer.Value, contexte);
            }
            else
            {
                _tourDeJeu.PiocherUneCarte(joueur);
                return true;
            }
        }

        // Gère la pénalité de pioche pour le joueur
        private async Task<bool> GererPenalitePioche(Player joueur)
        {
            // Vérifier la limite de contres
            if (_carteJouableValid.LimiteContreAttaque(iPenaliteCartes))
            {
                RaiseLog(new LogEventArgs($"Limite de contres atteinte! {joueur.Prenom} doit piocher {iPenaliteCartes} cartes!"));
                _tourDeJeu.PiocherMultiplesCartes(joueur, iPenaliteCartes);
                iPenaliteCartes = 0;
                return true;
            }
            
            // Vérifier si le joueur peut contrer
            if (_tourDeJeu.PeutContrer(joueur))
            {
                _tourDeJeu.ContrerAttaque(joueur, ref iPenaliteCartes);
                
                // Vérifier si le joueur a gagné après avoir contré
                if (_tourDeJeu.VerifierVictoire(joueur))
                {
                    return false;
                }
                return true;
            }
            else
            {
                // Le joueur ne peut pas contrer, il doit piocher
                _tourDeJeu.PiocherMultiplesCartes(joueur, iPenaliteCartes);
                iPenaliteCartes = 0;
                return true;
            }
        }

        // Gère le jeu d'une carte par le joueur
        private async Task<bool> JouerCarte(Player joueur, Card carte, ContexteDeJeu contexte)
        {
            // Gestion spéciale du Valet
            if (carte.Value == CardValue.Valet)
            {
                _tourDeJeu.JouerValet(joueur, carte, contexte);
            }
            else
            {
                _tourDeJeu.JouerCarteNormale(joueur, carte);
            }
            
            // Vérifier victoire
            if (_tourDeJeu.VerifierVictoire(joueur))
            {
                return false; // Fin de la partie
            }
            
            return true; // La partie continue
        }
        private void ProchainJoueur()
        {
            // Met à jour l'index du joueur courant en fonction du sens du jeu
            iJoueurCourant = (iJoueurCourant + iSens + _nbJoueurs) % _nbJoueurs;
        }

        // Méthode pour lever un événement de journalisation
        public virtual void RaiseLog(LogEventArgs e)
        {
            LogEventHandler handlers = logHandlers;
            if (handlers != null)
            {
                handlers(this, e);
            }
        }

        //Handling functions for events
        private void HandleChangementOrientation(object sender, EventArgs e)
        {
            // Inverser le sens
            iSens *= -1;
            string direction = iSens == 1 ? "horaire" : "anti-horaire";
            RaiseLog(new LogEventArgs($"Le sens du jeu change vers : {direction}"));
        }
        private void HandleStackEmpty(object sender, EventArgs e)
        {
            RaiseLog(new LogEventArgs("\n*** La pile de pioche est vide! Recyclage de la pile de dépôt... ***"));
            
            // Récupérer toutes les cartes sauf la dernière
            List<Card> cartesARecycler = _depositStack.RetirerToutesCarteSaufTop();
            
            // Les ajouter à la pile de pioche et mélanger
            _drawStack.AjouterCartePilePioche(cartesARecycler);
            _drawStack.MelangerCartePilePioche(_random);
            
            RaiseLog(new LogEventArgs($"La pile de pioche contient maintenant {_drawStack.Count} cartes.\n"));
        }
        void HandleUno(object sender, PlayerEventArgs e)
        {
            Console.WriteLine($"Le joueur {e.Player.Prenom} a fait UNO!");
        }
        void HandleLog(object sender, LogEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        void HandleDrawStack(object sender, DrawStackEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}