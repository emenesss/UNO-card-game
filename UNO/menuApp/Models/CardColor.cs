namespace menuApp;

//Represnete la couleur d'une carte
//Couleurs : Trèfle, Carreau, Cœur, Pique.
public struct CardColor
{
    //Définition des attributs d'une couleur de carte (avec setters et getters)
    public String Name { get; set; }

    // Constantes représentant les couleurs des cartes
    public const string Coeur = "Coeur";
    public const string Pique = "Pique";
    public const string Carreau = "Carreau";
    public const string Trefle = "Trèfle";

    public CardColor(String name)
    {
        this.Name = name;
    }
    public override string ToString()
    {
        return Name;
    }
}


