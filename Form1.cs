namespace VeterinerSistemi;

public partial class Form1 : Form
{
    private static readonly Color AppBack = Color.FromArgb(238, 242, 247);
    private static readonly Color Surface = Color.FromArgb(248, 252, 255);
    private static readonly Color Primary = Color.FromArgb(27, 94, 98);
    private static readonly Color PrimaryDark = Color.FromArgb(219, 238, 255);
    private static readonly Color Accent = Color.FromArgb(231, 111, 81);
    private static readonly Color Success = Color.FromArgb(46, 125, 50);
    private static readonly Color Info = Color.FromArgb(37, 99, 235);
    private static readonly Color Neutral = Color.FromArgb(82, 82, 91);
    private static readonly Color TextMain = Color.FromArgb(31, 41, 55);
    private static readonly Color MutedText = Color.FromArgb(91, 104, 124);

    private readonly List<Hayvan> hayvanlar = new();
    private readonly List<Veteriner> veterinerler = new()
    {
        new Veteriner("Dr. Elif Yilmaz", "Kucuk Hayvan"),
        new Veteriner("Dr. Burak Demir", "Cerrahi"),
        new Veteriner("Dr. Selin Aksoy", "Genel")
    };

    private RadioButton rbKopek = null!, rbKedi = null!;
    private TextBox tbAd = null!, tbSahip = null!, tbYas = null!, tbSikayet = null!, tbEkBilgi = null!;
    private Label lblEkBilgi = null!;
    private Button btnEkle = null!;
    private Button btnAtaVeteriner = null!, btnTedaviBaslat = null!, btnTamamla = null!, btnGecmis = null!;
    private ListBox lbHayvanlar = null!;
    private ComboBox cbVeteriner = null!;
    private Label lblSecili = null!, lblOzet = null!;
    private TextBox tbLog = null!;
    private Image? backgroundImage;

    public Form1()
    {
        InitializeComponent();
        LoadBackgroundImage();
        BuildUi();
        UpdateActionState();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            backgroundImage?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void BuildUi()
    {
        var main = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            Padding = new Padding(18),
            ColumnCount = 1,
            RowCount = 3
        };
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 86));
        main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 220));

        var header = BuildHeader();
        var content = BuildContent();
        var log = BuildLogArea();

        main.Controls.Add(header, 0, 0);
        main.Controls.Add(content, 0, 1);
        main.Controls.Add(log, 0, 2);
        Controls.Add(main);
    }

    private Control BuildHeader()
    {
        var header = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Surface,
            Margin = new Padding(0, 0, 0, 12),
            Padding = new Padding(18, 14, 18, 12)
        };

        var title = new Label
        {
            Text = "Veteriner Klinik Sistemi",
            Left = 18,
            Top = 13,
            AutoSize = true,
            ForeColor = Primary,
            Font = new Font("Segoe UI Semibold", 18f, FontStyle.Bold)
        };

        var subtitle = new Label
        {
            Text = "Hasta kaydi, veteriner atama ve tedavi gecmisi tek ekranda yonetilir.",
            Left = 20,
            Top = 50,
            AutoSize = true,
            ForeColor = MutedText,
            Font = new Font("Segoe UI", 9.5f)
        };

        lblOzet = new Label
        {
            Text = "0 hasta | 0 muayenede | 0 taburcu",
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            TextAlign = ContentAlignment.MiddleCenter,
            BackColor = PrimaryDark,
            ForeColor = Primary,
            Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
            Width = 270,
            Height = 34,
            Left = header.Width - 292,
            Top = 24
        };
        lblOzet.Location = new Point(header.Width - lblOzet.Width - 18, 25);
        header.Resize += (s, e) => lblOzet.Left = header.ClientSize.Width - lblOzet.Width - 18;

        header.Controls.AddRange(new Control[] { title, subtitle, lblOzet });
        return header;
    }

    private Control BuildContent()
    {
        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            ColumnCount = 2,
            RowCount = 1,
            Margin = new Padding(0)
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 372));
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var kayit = BuildKayitGroup();
        var right = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            ColumnCount = 1,
            RowCount = 2,
            Margin = new Padding(12, 0, 0, 0)
        };
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 52));
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 48));
        right.Controls.Add(BuildListeGroup(), 0, 0);
        right.Controls.Add(BuildIslemGroup(), 0, 1);

        content.Controls.Add(kayit, 0, 0);
        content.Controls.Add(right, 1, 0);
        return content;
    }

    private GroupBox BuildKayitGroup()
    {
        var grpKayit = CreateGroup("1. ADIM - Yeni Hasta Kaydi");
        grpKayit.Margin = new Padding(0);

        rbKopek = new RadioButton { Text = "Kopek", Left = 18, Top = 34, Checked = true, AutoSize = true };
        rbKedi = new RadioButton { Text = "Kedi", Left = 132, Top = 34, AutoSize = true };
        rbKopek.CheckedChanged += (s, e) =>
        {
            lblEkBilgi.Text = rbKopek.Checked ? "Irk:" : "Tuy Tipi:";
            tbEkBilgi.PlaceholderText = rbKopek.Checked ? "Golden, Husky..." : "Kisa / Uzun";
        };

        tbAd = AddLabelledTextBox(grpKayit, "Hayvan Adi:", 78, "Pamuk");
        tbSahip = AddLabelledTextBox(grpKayit, "Sahip Adi:", 116, "Ayse Yilmaz");
        tbYas = AddLabelledTextBox(grpKayit, "Yas:", 154, "3");
        tbSikayet = AddLabelledTextBox(grpKayit, "Sikayet:", 192, "Kontrol / asi / halsizlik");

        lblEkBilgi = new Label { Text = "Irk:", Left = 18, Top = 235, Width = 112, Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold), ForeColor = TextMain };
        tbEkBilgi = new TextBox { Left = 138, Top = 231, Width = 205, Height = 26, PlaceholderText = "Golden, Husky..." };

        btnEkle = CreateButton("+  Hastayi Kaydet", Success, 18, 292, 325, 42);
        btnEkle.Click += BtnEkle_Click;

        grpKayit.Controls.AddRange(new Control[] { rbKopek, rbKedi, lblEkBilgi, tbEkBilgi, btnEkle });
        return grpKayit;
    }

    private GroupBox BuildListeGroup()
    {
        var grpListe = CreateGroup("2. ADIM - Islem Yapilacak Hastayi Secin");
        grpListe.Margin = new Padding(0, 0, 0, 10);

        lbHayvanlar = new ListBox
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Consolas", 9f),
            BackColor = Color.FromArgb(250, 252, 255),
            ForeColor = TextMain,
            IntegralHeight = false
        };
        lbHayvanlar.SelectedIndexChanged += (s, e) =>
        {
            SeciliBilgiyiGuncelle();
            UpdateActionState();
        };

        var listShell = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 24, 12, 12), BackColor = Surface };
        listShell.Controls.Add(lbHayvanlar);
        grpListe.Controls.Add(listShell);
        return grpListe;
    }

    private GroupBox BuildIslemGroup()
    {
        var grpIslem = CreateGroup("3. ADIM - Secili Hasta Islemleri");
        grpIslem.Margin = new Padding(0);

        lblSecili = new Label
        {
            Text = "Secili hasta: (yok)",
            Left = 18,
            Top = 34,
            Width = 620,
            Height = 24,
            Font = new Font("Segoe UI", 9f, FontStyle.Italic),
            ForeColor = MutedText
        };

        var lblVet = new Label { Text = "Veteriner:", Left = 18, Top = 72, Width = 76, ForeColor = TextMain };
        cbVeteriner = new ComboBox
        {
            Left = 96,
            Top = 68,
            Width = 235,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cbVeteriner.Items.AddRange(veterinerler.Cast<object>().ToArray());
        cbVeteriner.SelectedIndex = 0;

        btnAtaVeteriner = CreateButton("Veteriner Ata", Info, 342, 66, 140, 31);
        btnAtaVeteriner.Click += BtnAta_Click;

        btnTedaviBaslat = CreateButton("Tedaviyi Baslat", Accent, 18, 118, 190, 42);
        btnTedaviBaslat.Click += BtnTedaviBaslat_Click;

        btnTamamla = CreateButton("Tedavi Bitti", Success, 216, 118, 190, 42);
        btnTamamla.Click += BtnTamamla_Click;

        btnGecmis = CreateButton("Tedavi Gecmisi", Neutral, 414, 118, 190, 42);
        btnGecmis.Click += BtnGecmis_Click;

        grpIslem.Controls.AddRange(new Control[] {
            lblSecili, lblVet, cbVeteriner, btnAtaVeteriner,
            btnTedaviBaslat, btnTamamla, btnGecmis
        });

        return grpIslem;
    }

    private GroupBox BuildLogArea()
    {
        var grpLog = CreateGroup("Islem Gunlugu");
        grpLog.Margin = new Padding(0, 14, 0, 0);

        tbLog = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            ReadOnly = true,
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Consolas", 9f),
            BackColor = Color.FromArgb(250, 252, 255),
            ForeColor = TextMain
        };

        var logShell = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 24, 12, 12), BackColor = Surface };
        logShell.Controls.Add(tbLog);
        grpLog.Controls.Add(logShell);
        return grpLog;
    }

    private GroupBox CreateGroup(string text)
    {
        return new GroupBox
        {
            Text = text,
            Dock = DockStyle.Fill,
            BackColor = Surface,
            ForeColor = TextMain,
            Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
            Padding = new Padding(10)
        };
    }

    private Button CreateButton(string text, Color color, int left, int top, int width, int height)
    {
        var button = new Button
        {
            Text = text,
            Left = left,
            Top = top,
            Width = width,
            Height = height,
            BackColor = color,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(color, 0.08f);
        return button;
    }

    private TextBox AddLabelledTextBox(Control parent, string label, int top, string placeholder)
    {
        var lbl = new Label
        {
            Text = label,
            Left = 18,
            Top = top + 4,
            Width = 112,
            Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
            ForeColor = TextMain
        };
        var tb = new TextBox
        {
            Left = 138,
            Top = top,
            Width = 205,
            Height = 26,
            PlaceholderText = placeholder
        };
        parent.Controls.Add(lbl);
        parent.Controls.Add(tb);
        return tb;
    }

    private void LoadBackgroundImage()
    {
        var imagePath = Path.Combine(AppContext.BaseDirectory, "Assets", "clinic-background.png");
        if (!File.Exists(imagePath))
            return;

        backgroundImage = Image.FromFile(imagePath);
        BackgroundImage = backgroundImage;
        BackgroundImageLayout = ImageLayout.Stretch;
    }

    private void BtnEkle_Click(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(tbAd.Text) || string.IsNullOrWhiteSpace(tbSahip.Text))
                throw new Exception("Hayvan adi ve sahip adi zorunlu.");

            if (!int.TryParse(tbYas.Text, out int yas) || yas < 0)
                throw new Exception("Yas 0 veya daha buyuk bir sayi olmali.");

            Hayvan h = rbKopek.Checked
                ? new Kopek(tbAd.Text.Trim(), tbSahip.Text.Trim(), yas, tbSikayet.Text.Trim(), tbEkBilgi.Text.Trim())
                : new Kedi(tbAd.Text.Trim(), tbSahip.Text.Trim(), yas, tbSikayet.Text.Trim(), tbEkBilgi.Text.Trim());

            hayvanlar.Add(h);
            ListeyiYenile(hayvanlar.Count - 1);
            Log($"[KAYIT] {h}");
            TemizleAlanlar();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAta_Click(object? sender, EventArgs e)
    {
        var h = SeciliHayvan();
        if (h == null) return;

        var vet = (Veteriner)cbVeteriner.SelectedItem!;
        h.AtananVeteriner = vet;
        Log($"[ATAMA] {h.Ad} -> {vet}");
        ListeyiYenile(lbHayvanlar.SelectedIndex);
    }

    private void BtnTedaviBaslat_Click(object? sender, EventArgs e)
    {
        var h = SeciliHayvan();
        if (h == null) return;

        if (h.AtananVeteriner == null)
        {
            MessageBox.Show("Once veteriner atayin.", "Eksik Adim", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        h.Durum = HastaDurumu.MuayeneEdiliyor;
        string sonuc = h.Tedavi();
        h.TedaviGecmisi.Add(new TedaviKaydi(h.AtananVeteriner, sonuc));
        Log($"[MUAYENEDE] {h.Ad} -> {sonuc}");
        ListeyiYenile(lbHayvanlar.SelectedIndex);
    }

    private void BtnTamamla_Click(object? sender, EventArgs e)
    {
        var h = SeciliHayvan();
        if (h == null) return;

        if (h.Durum != HastaDurumu.MuayeneEdiliyor)
        {
            MessageBox.Show("Once 'Tedaviyi Baslat' butonuna basin.", "Eksik Adim", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        h.Durum = HastaDurumu.TedaviEdildi;
        Log($"[TABURCU] {h.Ad} tedavi edildi.");
        ListeyiYenile(lbHayvanlar.SelectedIndex);
    }

    private void BtnGecmis_Click(object? sender, EventArgs e)
    {
        var h = SeciliHayvan();
        if (h == null) return;

        Log($"--- {h.Ad} tedavi gecmisi ---");
        if (h.TedaviGecmisi.Count == 0)
            Log("    (kayit yok)");
        else
            foreach (var k in h.TedaviGecmisi)
                Log("    " + k);
    }

    private Hayvan? SeciliHayvan()
    {
        var h = SeciliHayvanSilently();
        if (h == null)
        {
            MessageBox.Show("Once listeden bir hasta secin.", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        return h;
    }

    private Hayvan? SeciliHayvanSilently()
    {
        if (lbHayvanlar.SelectedIndex < 0 || lbHayvanlar.SelectedIndex >= hayvanlar.Count)
            return null;

        return hayvanlar[lbHayvanlar.SelectedIndex];
    }

    private void SeciliBilgiyiGuncelle()
    {
        var h = SeciliHayvanSilently();
        if (h == null)
        {
            lblSecili.Text = "Secili hasta: (yok)";
            return;
        }

        string atanan = h.AtananVeteriner != null ? h.AtananVeteriner.Ad : "Atanmadi";
        lblSecili.Text = $"Secili: [{h.Ad}] {h.SahipAdi} - {h.Sikayet} | Durum: {h.Durum} | Veteriner: {atanan}";
    }

    private void ListeyiYenile(int? secim = null)
    {
        int sec = secim ?? lbHayvanlar.SelectedIndex;
        lbHayvanlar.Items.Clear();

        foreach (var h in hayvanlar)
        {
            string atanan = h.AtananVeteriner != null ? $" | Vet: {h.AtananVeteriner.Ad}" : "";
            lbHayvanlar.Items.Add(h + atanan);
        }

        if (sec >= 0 && sec < lbHayvanlar.Items.Count)
            lbHayvanlar.SelectedIndex = sec;

        SeciliBilgiyiGuncelle();
        UpdateSummary();
        UpdateActionState();
    }

    private void UpdateSummary()
    {
        int muayenede = hayvanlar.Count(h => h.Durum == HastaDurumu.MuayeneEdiliyor);
        int taburcu = hayvanlar.Count(h => h.Durum == HastaDurumu.TedaviEdildi);
        lblOzet.Text = $"{hayvanlar.Count} hasta | {muayenede} muayenede | {taburcu} taburcu";
    }

    private void UpdateActionState()
    {
        if (btnAtaVeteriner == null) return;

        var h = SeciliHayvanSilently();
        bool secili = h != null;
        btnAtaVeteriner.Enabled = secili;
        btnTedaviBaslat.Enabled = secili && h!.AtananVeteriner != null && h.Durum != HastaDurumu.TedaviEdildi;
        btnTamamla.Enabled = secili && h!.Durum == HastaDurumu.MuayeneEdiliyor;
        btnGecmis.Enabled = secili;
    }

    private void TemizleAlanlar()
    {
        tbAd.Clear();
        tbSahip.Clear();
        tbYas.Clear();
        tbSikayet.Clear();
        tbEkBilgi.Clear();
        tbAd.Focus();
    }

    private void Log(string msg)
        => tbLog.AppendText(msg + Environment.NewLine);
}
