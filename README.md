# Veteriner Klinik Sistemi

Veteriner Klinik Sistemi, Windows Forms ile gelistirilmis basit ve okunakli bir hasta takip uygulamasidir. Uygulama; kedi ve kopek hastalarinin kaydini tutar, hastaya veteriner atar, tedavi surecini baslatir ve tedavi gecmisini ekrandaki islem gunlugunde saklar.

## Ozellikler

- Yeni hasta kaydi: hayvan adi, sahip adi, yas, sikayet ve ture ozel bilgi girilir.
- Tur secimi: kopek icin irk, kedi icin tuy tipi tutulur.
- Veteriner atama: hazir veteriner listesinden secim yapilir.
- Tedavi akisi: kayitli hasta once veterinere atanir, sonra tedavisi baslatilir ve taburcu edilir.
- Tedavi gecmisi: her tedavi kaydi tarih, veteriner ve aciklama bilgisiyle listelenir.
- Tek ekranli arayuz: kayit, hasta listesi, islem paneli ve islem gunlugu ayni formda bulunur.

## Kullanilan Teknolojiler

- C#
- .NET 8
- Windows Forms
- Nesne yonelimli programlama

## OOP Tasarimi

Projede ortak hasta davranislari `Hayvan` soyut sinifinda toplanir. `Kedi` ve `Kopek` siniflari bu siniftan tureyerek kendi tedavi aciklamalarini uretir. `ITedaviEdilebilir` arayuzu tedavi sozlesmesini belirtir. `Veteriner` ve `TedaviKaydi` siniflari hasta atama ve gecmis kaydi sorumluluklarini ayirir.

## Proje Yapisi

```text
Soru2_VeterinerSistemi/
|-- Form1.cs                  # Ana Windows Forms arayuzu ve ekran akisi
|-- Program.cs                # Uygulama baslangic noktasi
|-- Hayvan.cs                 # Soyut hasta sinifi ve hasta durumu enum'u
|-- Kedi.cs                   # Kediye ozel tedavi davranisi
|-- Kopek.cs                  # Kopege ozel tedavi davranisi
|-- Veteriner.cs              # Veteriner modeli
|-- TedaviKaydi.cs            # Tedavi gecmisi modeli
|-- ITedaviEdilebilir.cs      # Tedavi arayuzu
|-- docs/PROJECT_PLAN.md      # Tasarim ve gelistirme plani
`-- VeterinerSistemi.csproj   # .NET proje dosyasi
```

## Calistirma

Bu proje Windows Forms kullandigi icin Windows uzerinde calistirilmalidir.

```powershell
dotnet restore
dotnet build
dotnet run --project VeterinerSistemi.csproj
```

## Kullanim Akisi

1. Hasta turunu secin.
2. Hasta ve sahip bilgilerini girip hastayi kaydedin.
3. Listeden hastayi secin.
4. Veteriner atayin.
5. Tedaviyi baslatin.
6. Tedaviyi bitirerek hastayi taburcu edin.
7. Gerektiginde tedavi gecmisini islem gunlugunde goruntuleyin.

## Gelistirme Notlari

Detayli tasarim, sinif diyagrami, ekran akisi ve gelistirme yol haritasi icin [docs/PROJECT_PLAN.md](docs/PROJECT_PLAN.md) dosyasina bakabilirsiniz.
