namespace VeterinerSistemi;

public class Kedi : Hayvan
{
    public string TuyTipi { get; set; } // Kisa, Uzun

    public Kedi(string ad, string sahip, int yas, string sikayet, string tuyTipi)
        : base(ad, sahip, yas, sikayet)
    {
        TuyTipi = tuyTipi;
    }

    public override string Tedavi()
        => $"Kedi tedavisi: {TuyTipi} tuylu '{Ad}' icin tirnak bakimi, mantar testi ve solunum kontrolu yapildi.";

    public override string ToString()
        => base.ToString() + $" | Tuy: {TuyTipi}";
}
