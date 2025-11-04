namespace menuApp;

public struct Card
{
    //D�finition des attributs d'une carte (avec setters et getters)
    public CardValue Value { get;  set; }
    public CardColor Color { get; set; }

    //Constructeur de la carte
    public Card(CardValue value, CardColor color)
    {
        this.Value = value;
        this.Color = color;
    }

    // Retourne les points d'une carte individuelle
    public int CalculerPointsCarte()
    {
        switch (this.Value)
        {
            case CardValue.As:
                return 11;
            case CardValue.Valet:
            case CardValue.Dame:
            case CardValue.Roi:
                return 2;
            //Ici, le valet, la dame et le roi valent 2 points chacun.
            default:
                return (int)this.Value;
                // Les cartes numériques valent leur valeur faciale en points.
        }
    }

    //Red�finition de la méthode ToString pour afficher la carte
    public override string ToString()
    {
        return Value + " de " + Color;
    }
}