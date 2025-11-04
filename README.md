## Introduction

Développé par Yacine HAMMOUCHE, Marc-André GRETRY, Imane ESSAHLI.

Le Devoir Consiste a crée un projet de jeu de cartes inspiré de jeux 
de type pêche (UNO, 8 americain) où plusieurs joueurs jouent à tour de rôle en tentant 
de gagner. Le jeu inclut une logique pour distribuer 
et jouer des cartes, gérer le score, appliquer des règles spéciales 
pour certaines cartes et observer les événements de jeu grâce à un modèle 
d'observateur.

## Que contient le projet

Fonctionnalités principales
•	Distribution des cartes : Chaque joueur reçoit un nombre initial de cartes au début de la partie.
•	Règles de jeu personnalisées : Certaines cartes déclenchent des effets spéciaux :
•	As → Passe le tour du joueur suivant
•	2 → Le joueur suivant pioche deux cartes
•	10 → Inverse le sens du jeu
•	Valet → Permet de changer la couleur en jeu
•	Système d’observation : Le modèle d’observateur est utilisé pour notifier les événements importants, tels que :
•	Le joueur qui joue à chaque tour
•	La carte jouée
•	La dernière carte sur la pile de dépôt
•	La fin de la partie et le vainqueur
•	Stratégie de minimisation : Un joueur est choisi aléatoirement pour jouer avec une stratégie visant à minimiser son nombre de points.
•	Gestion du score : Le score final est calculé selon la valeur des cartes restantes en main.

⸻

Structure du projet

Le projet est organisé en plusieurs classes et interfaces :
•	Card : Représente une carte avec une couleur et une valeur.
•	CardColor : Définit les différentes couleurs possibles (Trèfle, Cœur, Carreau, Pique).
•	CardValue : Enum représentant la valeur des cartes (As, 2–10, Valet, Dame, Roi).
•	CardPair : Gère la création d’un jeu complet de 52 cartes.
•	Person : Contient les informations d’une personne (nom, prénom, identifiant).
•	Player : Hérite de Person, et représente un joueur avec une main.
•	DepositStack : Pile de dépôt (les cartes jouées).
•	DrawStack : Pile de pioche.
•	GameBoard : Orchestration complète de la partie — initialise paquets et piles, crée les joueurs/stratégies, 
    distribue les cartes, maintient le sens et le joueur courant, gère/relaye les événements 
    (sens, passer tour, pénalités, UNO, pioche vide avec recyclage), délègue la logique d’un tour à TourDeJeu, 
    et déclenche le bilan des scores en fin de jeu.
•   TourDeJeu : Gère la logique d’un tour complet — vérifie les cartes jouables, applique les règles spéciales 
    (pioches, contres, valet, victoires), et orchestre les actions du joueur pendant sa manche.
•	FishingGame : Point d’entrée principal, initialise et lance la partie.
•	IStrategie : Interface définissant le comportement des stratégies.
•	StrategieMinimisationDePoint / StrategieAleatoire / StrategieAntiUno : Implémentations concrètes des stratégies de jeu.
•	Score : Calcule le score final de chaque joueur à la fin de la partie.
•	ObservateurConcret / IObservateur : Gèrent les notifications entre les différentes entités du jeu.

⸻

Technologies et concepts utilisés
•	Langage : C#
•	Paradigme : Programmation orientée objet
•	Patrons de conception :
•	Observateur (Observer Pattern)
•	Stratégie (Strategy Pattern)
•	Respect des principes SOLID
•	Gestion des événements avec event et delegate
•	Asynchrone : Utilisation de async/await et de délais pour simuler le déroulement réel d’une partie

⸻

Exécution

Le programme démarre avec 3 joueurs, distribue aléatoirement les cartes et affiche, à chaque tour :
•	Le joueur qui joue
•	La carte jouée
•	La dernière carte sur la pile de dépôt

Le jeu se termine lorsqu’un joueur n’a plus de cartes, le vainqueur est alors affiché, ainsi que le score final de chaque joueur.

## Developing with Gitpod

This template repository also has a fully-automated dev setup for [Gitpod](https://docs.gitlab.com/ee/integration/gitpod.html).

The `.gitpod.yml` ensures that, when you open this repository in Gitpod, you'll get a cloud workspace with .NET Core pre-installed, and your project will automatically be built and start running.