namespace menuApp;
public class Person
{
    // Definition des propriétés avec getters et setters
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }

    // Constructeur
    public Person(int id, string nom, string prenom)
    {
        this.Id = id;
        this.Nom = nom;
        this.Prenom = prenom;
    }

    // Surcharge de la m�thode ToString pour afficher les informations de la personne
    public override string ToString() => $"{Nom} {Prenom}";
}