namespace VeterinerSistemi;

public class Kopek : Hayvan
{
    public string Irk { get; set; } // Golden, Husky, Labrador vb.

    public Kopek(string ad, string sahip, int yas, string sikayet, string irk)
        : base(ad, sahip, yas, sikayet)
    {
        Irk = irk;
    }

    public override string Tedavi()
        => $"Kopek tedavisi: {Irk} irki '{Ad}' icin asi yapildi, kalp ritmi dinlendi, parazit kontrolu tamamlandi.";

    public override string ToString()
        => base.ToString() + $" | Irk: {Irk}";
}
