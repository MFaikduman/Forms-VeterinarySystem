namespace VeterinerSistemi;

public class Veteriner
{
    public string Ad { get; set; }
    public string Uzmanlik { get; set; }

    public Veteriner(string ad, string uzmanlik)
    {
        Ad = ad;
        Uzmanlik = uzmanlik;
    }

    public override string ToString() => $"{Ad} ({Uzmanlik})";
}
