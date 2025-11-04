namespace menuApp;
    public class ContexteDeJeu
    {
        public List<Player> Joueurs { get; set; }
        public Player JoueurActuel { get; set; }
        public int SensJeu { get; set; }
        public int PenaliteCartes { get; set; }

        public ContexteDeJeu(List<Player> joueurs, Player joueurActuel, int sensJeu, int penaliteCartes)
        {
            Joueurs = joueurs;
            JoueurActuel = joueurActuel;
            SensJeu = sensJeu;
            PenaliteCartes = penaliteCartes;
        }

        public bool AdversaireAUno() // Verifie si un adversaire à une carte.
        {
            return Joueurs.Any(p => p.Id != JoueurActuel.Id && p.Hand.Count == 1);
        }
        // // Je voulais l'utiliser pour complexifier une strategie, focus adversaire qui a probablement UNOOOO.(a voir si j'ai le temps)
        // public Player? AdversaireAvecMoinsDeCartes() // Renvoie le joueur avec le moins de cartes.
        // {
        //     return Joueurs
        //         .Where(p => p.Id != JoueurActuel.Id) //exclue le joueur actuel.
        //         .OrderBy(p => p.Hand.Count) // Ordonne les joueurs par le nb de carte en decroissant et prend le premier ou si null lui par default
        //         .FirstOrDefault();
        // }
    }