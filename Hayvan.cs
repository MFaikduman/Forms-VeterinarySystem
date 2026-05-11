namespace VeterinerSistemi;

public enum HastaDurumu { Kayitli, MuayeneEdiliyor, TedaviEdildi }

// Soyut sinif: tum hayvanlarin ortak bilgileri ve tedavi sozlesmesi
public abstract class Hayvan : ITedaviEdilebilir
{
    public string Ad { get; set; }
    public string SahipAdi { get; set; }
    public int Yas { get; set; }
    public string Sikayet { get; set; }
    public HastaDurumu Durum { get; set; } = HastaDurumu.Kayitli;
    public Veteriner? AtananVeteriner { get; set; }
    public List<TedaviKaydi> TedaviGecmisi { get; } = new();

    protected Hayvan(string ad, string sahip, int yas, string sikayet)
    {
        Ad = ad;
        SahipAdi = sahip;
        Yas = yas;
        Sikayet = sikayet;
    }

    // Polimorfik metot: her hayvan turu kendi tedavi yontemini saglar
    public abstract string Tedavi();

    public override string ToString()
        => $"[{Ad}] Sahip: {SahipAdi} | {Yas} yas | {Sikayet} ({Durum})";
}
