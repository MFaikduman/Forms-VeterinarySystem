namespace VeterinerSistemi;

public class TedaviKaydi
{
    public Veteriner Veteriner { get; }
    public DateTime Tarih { get; }
    public string Aciklama { get; }

    public TedaviKaydi(Veteriner veteriner, string aciklama)
    {
        Veteriner = veteriner;
        Tarih = DateTime.Now;
        Aciklama = aciklama;
    }

    public override string ToString()
        => $"{Tarih:dd.MM.yyyy HH:mm} - {Veteriner.Ad}: {Aciklama}";
}
