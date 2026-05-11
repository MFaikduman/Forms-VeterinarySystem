using System.Drawing.Drawing2D;

namespace VeterinerSistemi;

public partial class Form1 : Form
{
    private static readonly Color Surface = Color.FromArgb(252, 254, 255);
    private static readonly Color SurfaceSoft = Color.FromArgb(238, 247, 250);
    private static readonly Color FieldBack = Color.FromArgb(248, 251, 252);
    private static readonly Color Primary = Color.FromArgb(24, 112, 108);
    private static readonly Color PrimarySoft = Color.FromArgb(218, 241, 238);
    private static readonly Color Accent = Color.FromArgb(222, 104, 76);
    private static readonly Color Success = Color.FromArgb(50, 140, 93);
    private static readonly Color Warning = Color.FromArgb(210, 151, 54);
    private static readonly Color Info = Color.FromArgb(48, 101, 185);
    private static readonly Color Danger = Color.FromArgb(177, 66, 66);
    private static readonly Color TextMain = Color.FromArgb(27, 39, 48);
    private static readonly Color MutedText = Color.FromArgb(88, 103, 117);
    private static readonly Color Line = Color.FromArgb(214, 226, 232);

    private readonly List<Hayvan> hayvanlar = new();
    private readonly List<Hayvan> gorunenHayvanlar = new();
    private readonly List<Veteriner> veterinerler = new()
    {
        new Veteriner("Dr. Elif Yilmaz", "Kucuk Hayvan"),
        new Veteriner("Dr. Burak Demir", "Cerrahi"),
        new Veteriner("Dr. Selin Aksoy", "Genel Kontrol"),
        new Veteriner("Dr. Deniz Kaya", "Acil Bakim")
    };

    private RadioButton rbKopek = null!, rbKedi = null!;
    private TextBox tbAd = null!, tbSahip = null!, tbYas = null!, tbSikayet = null!, tbEkBilgi = null!;
    private TextBox tbAra = null!, tbLog = null!;
    private ComboBox cbDurumFiltre = null!, cbVeteriner = null!;
    private Label lblEkBilgi = null!, lblSecili = null!, lblBosListe = null!;
    private Label lblToplam = null!, lblMuayenede = null!, lblTaburcu = null!;
    private Button btnEkle = null!, btnTemizle = null!;
    private Button btnAtaVeteriner = null!, btnTedaviBaslat = null!, btnTamamla = null!, btnGecmis = null!, btnSil = null!;
    private ListBox lbHayvanlar = null!;
    private Image? backgroundImage;

    public Form1()
    {
        InitializeComponent();
        LoadBackgroundImage();
        BuildUi();
        ListeyiYenile();
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
        Controls.Clear();
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

        var main = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent,
            Padding = new Padding(24),
            ColumnCount = 3,
            RowCount = 3
        };

        main.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 336));
        main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54));
        main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46));
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 96));
        main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 188));

        var header = BuildHeader();
        var intake = BuildIntakePanel();
        var board = BuildPatientBoard();
        var actions = BuildActionPanel();
        var log = BuildLogArea();

        main.Controls.Add(header, 0, 0);
        main.SetColumnSpan(header, 3);
        main.Controls.Add(intake, 0, 1);
        main.Controls.Add(board, 1, 1);
        main.Controls.Add(actions, 2, 1);
        main.Controls.Add(log, 0, 2);
        main.SetColumnSpan(log, 3);

        Controls.Add(main);
    }

    private Control BuildHeader()
    {
        var shell = CreateCard(new Padding(22, 16, 22, 16));
        shell.Margin = new Padding(0, 0, 0, 14);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 520));

        var titlePanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            ColumnCount = 1,
            BackColor = Color.Transparent
        };
        titlePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
        titlePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var title = new Label
        {
            Text = "Veteriner Klinik Paneli",
            Dock = DockStyle.Fill,
            ForeColor = Primary,
            Font = new Font("Segoe UI Semibold", 20f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };

        var subtitle = new Label
        {
            Text = "Hasta kabulu, veteriner atama, muayene ve gecmis takibi tek ekranda.",
            Dock = DockStyle.Fill,
            ForeColor = MutedText,
            Font = new Font("Segoe UI", 10f),
            TextAlign = ContentAlignment.MiddleLeft
        };

        titlePanel.Controls.Add(title, 0, 0);
        titlePanel.Controls.Add(subtitle, 0, 1);

        var stats = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1,
            BackColor = Color.Transparent
        };
        stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));

        stats.Controls.Add(CreateMetric("Toplam Hasta", Primary, out lblToplam), 0, 0);
        stats.Controls.Add(CreateMetric("Muayenede", Warning, out lblMuayenede), 1, 0);
        stats.Controls.Add(CreateMetric("Taburcu", Success, out lblTaburcu), 2, 0);

        layout.Controls.Add(titlePanel, 0, 0);
        layout.Controls.Add(stats, 1, 0);
        shell.Controls.Add(layout);
        return shell;
    }

    private Control BuildIntakePanel()
    {
        var shell = CreateCard(new Padding(18));
        shell.Margin = new Padding(0, 0, 14, 14);
        shell.AutoScroll = true;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            RowCount = 9,
            BackColor = Color.Transparent
        };

        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        layout.Controls.Add(CreateSectionTitle("Yeni Hasta Kaydi", "Kabul formu"), 0, 0);

        var speciesPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent,
            Margin = new Padding(0, 0, 0, 8)
        };
        speciesPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        speciesPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

        rbKopek = CreateSpeciesButton("Kopek", true);
        rbKedi = CreateSpeciesButton("Kedi", false);
        rbKopek.CheckedChanged += (s, e) => UpdateSpeciesUi();
        rbKedi.CheckedChanged += (s, e) => UpdateSpeciesUi();

        speciesPanel.Controls.Add(rbKopek, 0, 0);
        speciesPanel.Controls.Add(rbKedi, 1, 0);
        layout.Controls.Add(speciesPanel, 0, 1);

        tbAd = CreateTextBox("Pamuk");
        tbSahip = CreateTextBox("Ayse Yilmaz");
        tbYas = CreateTextBox("3");
        tbSikayet = CreateTextBox("Kontrol / asi / halsizlik");
        tbEkBilgi = CreateTextBox("Golden, Husky...");

        layout.Controls.Add(CreateField("Hayvan Adi", tbAd), 0, 2);
        layout.Controls.Add(CreateField("Sahip Adi", tbSahip), 0, 3);
        layout.Controls.Add(CreateField("Yas", tbYas), 0, 4);
        layout.Controls.Add(CreateField("Sikayet", tbSikayet), 0, 5);

        var ekPanel = CreateField("Irk", tbEkBilgi);
        lblEkBilgi = (Label)ekPanel.Controls[0];
        layout.Controls.Add(ekPanel, 0, 6);

        var actionRow = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent,
            Margin = new Padding(0, 6, 0, 0)
        };
        actionRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66));
        actionRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));

        btnEkle = CreateButton("Hastayi Kaydet", Success);
        btnEkle.Click += BtnEkle_Click;
        btnTemizle = CreateButton("Temizle", Color.FromArgb(93, 107, 121));
        btnTemizle.Click += (s, e) => TemizleAlanlar();

        actionRow.Controls.Add(btnEkle, 0, 0);
        actionRow.Controls.Add(btnTemizle, 1, 0);
        layout.Controls.Add(actionRow, 0, 7);

        var note = new Label
        {
            Dock = DockStyle.Top,
            Height = 42,
            Text = "Zorunlu alanlar: hayvan adi, sahip adi ve gecerli yas.",
            ForeColor = MutedText,
            Font = new Font("Segoe UI", 9f),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(2, 8, 2, 0)
        };
        layout.Controls.Add(note, 0, 8);

        shell.Controls.Add(layout);
        UpdateSpeciesUi();
        return shell;
    }

    private Control BuildPatientBoard()
    {
        var shell = CreateCard(new Padding(18));
        shell.Margin = new Padding(0, 0, 14, 14);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = Color.Transparent
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        layout.Controls.Add(CreateSectionTitle("Hasta Panosu", "Kayitlar ve filtreler"), 0, 0);

        var filters = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent,
            Margin = new Padding(0, 0, 0, 8)
        };
        filters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58));
        filters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42));

        tbAra = CreateTextBox("Hasta, sahip veya sikayet ara");
        tbAra.TextChanged += (s, e) => ListeyiYenile(SeciliHayvanSilently());

        cbDurumFiltre = new ComboBox
        {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = FieldBack,
            ForeColor = TextMain,
            Font = new Font("Segoe UI", 9.5f),
            IntegralHeight = false,
            Margin = new Padding(8, 0, 0, 0)
        };
        cbDurumFiltre.Items.AddRange(new object[] { "Tum Durumlar", "Kayitli", "Muayenede", "Tedavi Edildi" });
        cbDurumFiltre.SelectedIndex = 0;
        cbDurumFiltre.SelectedIndexChanged += (s, e) => ListeyiYenile(SeciliHayvanSilently());

        filters.Controls.Add(tbAra, 0, 0);
        filters.Controls.Add(cbDurumFiltre, 1, 0);
        layout.Controls.Add(filters, 0, 1);

        var listShell = new RoundedPanel
        {
            Dock = DockStyle.Fill,
            BackColor = FieldBack,
            BorderColor = Line,
            BorderThickness = 1,
            CornerRadius = 14,
            Padding = new Padding(10)
        };

        lbHayvanlar = new ListBox
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.None,
            BackColor = FieldBack,
            ForeColor = TextMain,
            DrawMode = DrawMode.OwnerDrawVariable,
            IntegralHeight = false,
            ItemHeight = 84
        };
        lbHayvanlar.MeasureItem += (s, e) => e.ItemHeight = 84;
        lbHayvanlar.DrawItem += DrawPatientItem;
        lbHayvanlar.SelectedIndexChanged += (s, e) =>
        {
            SeciliBilgiyiGuncelle();
            UpdateActionState();
        };

        lblBosListe = new Label
        {
            Dock = DockStyle.Fill,
            Text = "Henuz hasta kaydi yok.",
            ForeColor = MutedText,
            Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            Visible = false
        };

        listShell.Controls.Add(lbHayvanlar);
        listShell.Controls.Add(lblBosListe);
        layout.Controls.Add(listShell, 0, 2);

        shell.Controls.Add(layout);
        return shell;
    }

    private Control BuildActionPanel()
    {
        var shell = CreateCard(new Padding(18));
        shell.Margin = new Padding(0, 0, 0, 14);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            BackColor = Color.Transparent
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 128));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 86));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 112));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        layout.Controls.Add(CreateSectionTitle("Hasta Islemleri", "Atama ve tedavi"), 0, 0);

        var selectedCard = new RoundedPanel
        {
            Dock = DockStyle.Fill,
            BackColor = SurfaceSoft,
            BorderColor = Line,
            BorderThickness = 1,
            CornerRadius = 14,
            Padding = new Padding(14),
            Margin = new Padding(0, 0, 0, 12)
        };

        lblSecili = new Label
        {
            Dock = DockStyle.Fill,
            Text = "Listeden bir hasta sectiginizde detaylar burada gorunur.",
            ForeColor = MutedText,
            Font = new Font("Segoe UI", 9.5f),
            TextAlign = ContentAlignment.MiddleLeft
        };
        selectedCard.Controls.Add(lblSecili);
        layout.Controls.Add(selectedCard, 0, 1);

        var vetPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            BackColor = Color.Transparent,
            Margin = new Padding(0, 0, 0, 12)
        };
        vetPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
        vetPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        vetPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        vetPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var lblVet = new Label
        {
            Text = "Veteriner secimi",
            Dock = DockStyle.Fill,
            ForeColor = TextMain,
            Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold)
        };

        cbVeteriner = new ComboBox
        {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = FieldBack,
            ForeColor = TextMain,
            Font = new Font("Segoe UI", 9.5f),
            IntegralHeight = false
        };
        cbVeteriner.Items.AddRange(veterinerler.Cast<object>().ToArray());
        cbVeteriner.SelectedIndex = 0;

        btnAtaVeteriner = CreateButton("Ata", Info);
        btnAtaVeteriner.Margin = new Padding(8, 0, 0, 0);
        btnAtaVeteriner.Click += BtnAta_Click;

        vetPanel.Controls.Add(lblVet, 0, 0);
        vetPanel.SetColumnSpan(lblVet, 2);
        vetPanel.Controls.Add(cbVeteriner, 0, 1);
        vetPanel.Controls.Add(btnAtaVeteriner, 1, 1);
        layout.Controls.Add(vetPanel, 0, 2);

        var actionGrid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            BackColor = Color.Transparent,
            Margin = new Padding(0, 0, 0, 10)
        };
        actionGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        actionGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        actionGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        actionGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        btnTedaviBaslat = CreateButton("Tedaviyi Baslat", Accent);
        btnTedaviBaslat.Click += BtnTedaviBaslat_Click;
        btnTamamla = CreateButton("Taburcu Et", Success);
        btnTamamla.Click += BtnTamamla_Click;
        btnGecmis = CreateButton("Gecmisi Yaz", Color.FromArgb(93, 107, 121));
        btnGecmis.Click += BtnGecmis_Click;
        btnSil = CreateButton("Kaydi Sil", Danger);
        btnSil.Click += BtnSil_Click;

        actionGrid.Controls.Add(btnTedaviBaslat, 0, 0);
        actionGrid.Controls.Add(btnTamamla, 1, 0);
        actionGrid.Controls.Add(btnGecmis, 0, 1);
        actionGrid.Controls.Add(btnSil, 1, 1);
        layout.Controls.Add(actionGrid, 0, 3);

        var tip = new Label
        {
            Dock = DockStyle.Top,
            Height = 52,
            Text = "Tedavi baslatmak icin once secili hastaya veteriner atanmalidir.",
            ForeColor = MutedText,
            Font = new Font("Segoe UI", 9f),
            TextAlign = ContentAlignment.TopLeft,
            Padding = new Padding(2, 8, 2, 0)
        };
        layout.Controls.Add(tip, 0, 4);

        shell.Controls.Add(layout);
        return shell;
    }

    private Control BuildLogArea()
    {
        var shell = CreateCard(new Padding(18, 14, 18, 18));
        shell.Margin = new Padding(0);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.Transparent
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 116));

        var title = new Label
        {
            Text = "Islem Akisi",
            Dock = DockStyle.Fill,
            ForeColor = TextMain,
            Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };

        var clear = CreateButton("Gunlugu Sil", Color.FromArgb(93, 107, 121));
        clear.Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold);
        clear.Click += (s, e) => tbLog.Clear();

        header.Controls.Add(title, 0, 0);
        header.Controls.Add(clear, 1, 0);
        layout.Controls.Add(header, 0, 0);

        tbLog = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            ReadOnly = true,
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Consolas", 9f),
            BackColor = FieldBack,
            ForeColor = TextMain
        };
        layout.Controls.Add(tbLog, 0, 1);

        shell.Controls.Add(layout);
        return shell;
    }

    private RoundedPanel CreateCard(Padding padding)
    {
        return new RoundedPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Surface,
            BorderColor = Color.FromArgb(224, 235, 240),
            BorderThickness = 1,
            CornerRadius = 20,
            Padding = padding
        };
    }

    private Control CreateMetric(string caption, Color color, out Label valueLabel)
    {
        var metric = new RoundedPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(247, 251, 252),
            BorderColor = Color.FromArgb(222, 234, 238),
            BorderThickness = 1,
            CornerRadius = 15,
            Padding = new Padding(12, 8, 12, 8),
            Margin = new Padding(8, 0, 0, 0)
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            ColumnCount = 1,
            BackColor = Color.Transparent
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 58));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 42));

        valueLabel = new Label
        {
            Text = "0",
            Dock = DockStyle.Fill,
            ForeColor = color,
            Font = new Font("Segoe UI Semibold", 18f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };

        var captionLabel = new Label
        {
            Text = caption,
            Dock = DockStyle.Fill,
            ForeColor = MutedText,
            Font = new Font("Segoe UI", 8.5f),
            TextAlign = ContentAlignment.MiddleLeft
        };

        layout.Controls.Add(valueLabel, 0, 0);
        layout.Controls.Add(captionLabel, 0, 1);
        metric.Controls.Add(layout);
        return metric;
    }

    private Control CreateSectionTitle(string title, string caption)
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.Transparent,
            Margin = new Padding(0, 0, 0, 8)
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 40));

        panel.Controls.Add(new Label
        {
            Text = title,
            Dock = DockStyle.Fill,
            ForeColor = TextMain,
            Font = new Font("Segoe UI Semibold", 13f, FontStyle.Bold),
            TextAlign = ContentAlignment.BottomLeft
        }, 0, 0);

        panel.Controls.Add(new Label
        {
            Text = caption,
            Dock = DockStyle.Fill,
            ForeColor = MutedText,
            Font = new Font("Segoe UI", 8.8f),
            TextAlign = ContentAlignment.TopLeft
        }, 0, 1);

        return panel;
    }

    private RadioButton CreateSpeciesButton(string text, bool isChecked)
    {
        var radio = new RadioButton
        {
            Text = text,
            Checked = isChecked,
            Dock = DockStyle.Fill,
            Appearance = Appearance.Button,
            FlatStyle = FlatStyle.Flat,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Margin = text == "Kopek" ? new Padding(0, 0, 6, 0) : new Padding(6, 0, 0, 0)
        };
        radio.FlatAppearance.BorderSize = 1;
        return radio;
    }

    private TextBox CreateTextBox(string placeholder)
    {
        return new TextBox
        {
            Dock = DockStyle.Fill,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = FieldBack,
            ForeColor = TextMain,
            Font = new Font("Segoe UI", 9.5f),
            PlaceholderText = placeholder,
            Margin = new Padding(0)
        };
    }

    private Control CreateField(string label, TextBox textBox)
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = Color.Transparent,
            Margin = new Padding(0, 0, 0, 8)
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        panel.Controls.Add(new Label
        {
            Text = label,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
            ForeColor = TextMain,
            TextAlign = ContentAlignment.MiddleLeft
        }, 0, 0);
        panel.Controls.Add(textBox, 0, 1);
        return panel;
    }

    private Button CreateButton(string text, Color color)
    {
        var button = new Button
        {
            Text = text,
            Dock = DockStyle.Fill,
            BackColor = color,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 9.3f, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Margin = new Padding(4)
        };
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(color, 0.08f);
        button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(color, 0.08f);
        return button;
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

    private void UpdateSpeciesUi()
    {
        if (rbKopek == null || rbKedi == null || lblEkBilgi == null || tbEkBilgi == null)
            return;

        lblEkBilgi.Text = rbKopek.Checked ? "Irk" : "Tuy Tipi";
        tbEkBilgi.PlaceholderText = rbKopek.Checked ? "Golden, Husky..." : "Kisa / Uzun";
        StyleSpeciesButton(rbKopek);
        StyleSpeciesButton(rbKedi);
    }

    private void StyleSpeciesButton(RadioButton radio)
    {
        radio.BackColor = radio.Checked ? Primary : Color.White;
        radio.ForeColor = radio.Checked ? Color.White : Primary;
        radio.FlatAppearance.BorderColor = radio.Checked ? Primary : Line;
        radio.FlatAppearance.CheckedBackColor = Primary;
        radio.FlatAppearance.MouseOverBackColor = radio.Checked ? ControlPaint.Light(Primary, 0.08f) : PrimarySoft;
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
            ListeyiYenile(h);
            Log($"[KAYIT] {h.Ad} sisteme eklendi.");
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
        ListeyiYenile(h);
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
        Log($"[MUAYENE] {h.Ad} icin tedavi baslatildi.");
        ListeyiYenile(h);
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
        ListeyiYenile(h);
    }

    private void BtnGecmis_Click(object? sender, EventArgs e)
    {
        var h = SeciliHayvan();
        if (h == null) return;

        Log($"--- {h.Ad} tedavi gecmisi ---");
        if (h.TedaviGecmisi.Count == 0)
            Log("    kayit yok");
        else
            foreach (var k in h.TedaviGecmisi)
                Log("    " + k);
    }

    private void BtnSil_Click(object? sender, EventArgs e)
    {
        var h = SeciliHayvan();
        if (h == null) return;

        var result = MessageBox.Show($"{h.Ad} kaydi silinsin mi?", "Kaydi Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result != DialogResult.Yes)
            return;

        hayvanlar.Remove(h);
        Log($"[SILINDI] {h.Ad} kaydi kaldirildi.");
        ListeyiYenile();
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
        return lbHayvanlar?.SelectedItem as Hayvan;
    }

    private void SeciliBilgiyiGuncelle()
    {
        if (lblSecili == null)
            return;

        var h = SeciliHayvanSilently();
        if (h == null)
        {
            lblSecili.Text = "Listeden bir hasta sectiginizde detaylar burada gorunur.";
            lblSecili.ForeColor = MutedText;
            return;
        }

        var tur = h is Kopek ? "Kopek" : "Kedi";
        var vet = h.AtananVeteriner != null ? h.AtananVeteriner.ToString() : "Atanmadi";
        lblSecili.ForeColor = TextMain;
        lblSecili.Text =
            $"{tur}: {h.Ad}\r\n" +
            $"Sahip: {h.SahipAdi} | Yas: {h.Yas}\r\n" +
            $"Sikayet: {EmptyToDash(h.Sikayet)}\r\n" +
            $"Durum: {StatusText(h.Durum)} | Veteriner: {vet}";

        if (h.AtananVeteriner != null)
            cbVeteriner.SelectedItem = h.AtananVeteriner;
    }

    private void ListeyiYenile(Hayvan? secim = null)
    {
        if (lbHayvanlar == null)
            return;

        secim ??= SeciliHayvanSilently();
        gorunenHayvanlar.Clear();
        lbHayvanlar.BeginUpdate();
        lbHayvanlar.Items.Clear();

        foreach (var h in hayvanlar.Where(MatchesFilter))
        {
            gorunenHayvanlar.Add(h);
            lbHayvanlar.Items.Add(h);
        }

        lbHayvanlar.EndUpdate();

        if (secim != null && gorunenHayvanlar.Contains(secim))
            lbHayvanlar.SelectedItem = secim;
        else if (lbHayvanlar.Items.Count > 0 && secim == null)
            lbHayvanlar.SelectedIndex = 0;

        UpdateEmptyState();
        SeciliBilgiyiGuncelle();
        UpdateSummary();
        UpdateActionState();
        lbHayvanlar.Invalidate();
    }

    private bool MatchesFilter(Hayvan h)
    {
        var search = tbAra?.Text.Trim();
        bool searchOk = string.IsNullOrWhiteSpace(search)
            || h.Ad.Contains(search, StringComparison.OrdinalIgnoreCase)
            || h.SahipAdi.Contains(search, StringComparison.OrdinalIgnoreCase)
            || h.Sikayet.Contains(search, StringComparison.OrdinalIgnoreCase)
            || (h.AtananVeteriner?.Ad.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false);

        bool statusOk = cbDurumFiltre?.SelectedIndex switch
        {
            1 => h.Durum == HastaDurumu.Kayitli,
            2 => h.Durum == HastaDurumu.MuayeneEdiliyor,
            3 => h.Durum == HastaDurumu.TedaviEdildi,
            _ => true
        };

        return searchOk && statusOk;
    }

    private void UpdateEmptyState()
    {
        if (lblBosListe == null)
            return;

        bool empty = lbHayvanlar.Items.Count == 0;
        lblBosListe.Text = hayvanlar.Count == 0 ? "Henuz hasta kaydi yok." : "Filtreye uyan hasta bulunamadi.";
        lblBosListe.Visible = empty;
        lbHayvanlar.Visible = !empty;
        if (empty)
            lblBosListe.BringToFront();
    }

    private void UpdateSummary()
    {
        if (lblToplam == null)
            return;

        int muayenede = hayvanlar.Count(h => h.Durum == HastaDurumu.MuayeneEdiliyor);
        int taburcu = hayvanlar.Count(h => h.Durum == HastaDurumu.TedaviEdildi);
        lblToplam.Text = hayvanlar.Count.ToString();
        lblMuayenede.Text = muayenede.ToString();
        lblTaburcu.Text = taburcu.ToString();
    }

    private void UpdateActionState()
    {
        if (btnAtaVeteriner == null)
            return;

        var h = SeciliHayvanSilently();
        bool secili = h != null;
        btnAtaVeteriner.Enabled = secili;
        btnTedaviBaslat.Enabled = secili && h!.AtananVeteriner != null && h.Durum != HastaDurumu.TedaviEdildi;
        btnTamamla.Enabled = secili && h!.Durum == HastaDurumu.MuayeneEdiliyor;
        btnGecmis.Enabled = secili;
        btnSil.Enabled = secili;
    }

    private void TemizleAlanlar()
    {
        tbAd.Clear();
        tbSahip.Clear();
        tbYas.Clear();
        tbSikayet.Clear();
        tbEkBilgi.Clear();
        rbKopek.Checked = true;
        tbAd.Focus();
    }

    private void Log(string msg)
        => tbLog.AppendText($"{DateTime.Now:HH:mm}  {msg}{Environment.NewLine}");

    private static string EmptyToDash(string value)
        => string.IsNullOrWhiteSpace(value) ? "-" : value;

    private static string StatusText(HastaDurumu durum)
    {
        return durum switch
        {
            HastaDurumu.Kayitli => "Kayitli",
            HastaDurumu.MuayeneEdiliyor => "Muayenede",
            HastaDurumu.TedaviEdildi => "Tedavi Edildi",
            _ => durum.ToString()
        };
    }

    private static Color StatusColor(HastaDurumu durum)
    {
        return durum switch
        {
            HastaDurumu.Kayitli => Info,
            HastaDurumu.MuayeneEdiliyor => Warning,
            HastaDurumu.TedaviEdildi => Success,
            _ => MutedText
        };
    }

    private void DrawPatientItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0 || e.Index >= lbHayvanlar.Items.Count)
            return;

        var h = (Hayvan)lbHayvanlar.Items[e.Index];
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
        var card = new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 5, e.Bounds.Width - 12, e.Bounds.Height - 10);
        FillRoundRectangle(g, selected ? Color.FromArgb(220, 241, 238) : Color.White, card, 14);
        DrawRoundRectangle(g, selected ? Primary : Line, card, 14);

        using var titleFont = new Font("Segoe UI Semibold", 10.2f, FontStyle.Bold);
        using var metaFont = new Font("Segoe UI", 8.8f);
        using var smallFont = new Font("Segoe UI Semibold", 8.2f, FontStyle.Bold);

        var titleRect = new Rectangle(card.Left + 14, card.Top + 10, card.Width - 162, 24);
        var metaRect = new Rectangle(card.Left + 14, card.Top + 35, card.Width - 28, 22);
        var subRect = new Rectangle(card.Left + 14, card.Top + 56, card.Width - 28, 18);
        var badgeRect = new Rectangle(card.Right - 128, card.Top + 13, 108, 25);

        string tur = h is Kopek ? "Kopek" : "Kedi";
        TextRenderer.DrawText(g, $"{h.Ad} - {h.SahipAdi}", titleFont, titleRect, TextMain, TextFormatFlags.EndEllipsis);
        TextRenderer.DrawText(g, $"{tur} | {h.Yas} yas | {EmptyToDash(h.Sikayet)}", metaFont, metaRect, MutedText, TextFormatFlags.EndEllipsis);

        var vet = h.AtananVeteriner != null ? h.AtananVeteriner.Ad : "Veteriner atanmadi";
        TextRenderer.DrawText(g, vet, smallFont, subRect, h.AtananVeteriner != null ? Primary : MutedText, TextFormatFlags.EndEllipsis);

        DrawBadge(g, badgeRect, StatusText(h.Durum), StatusColor(h.Durum), smallFont);
    }

    private static void DrawBadge(Graphics g, Rectangle rect, string text, Color color, Font font)
    {
        FillRoundRectangle(g, Color.FromArgb(235, color), rect, 12);
        TextRenderer.DrawText(g, text, font, rect, color, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    private static void FillRoundRectangle(Graphics g, Color color, Rectangle rect, int radius)
    {
        using var path = CreateRoundPath(rect, radius);
        using var brush = new SolidBrush(color);
        g.FillPath(brush, path);
    }

    private static void DrawRoundRectangle(Graphics g, Color color, Rectangle rect, int radius)
    {
        using var path = CreateRoundPath(rect, radius);
        using var pen = new Pen(color);
        g.DrawPath(pen, path);
    }

    private static GraphicsPath CreateRoundPath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        int diameter = Math.Min(radius * 2, Math.Min(rect.Width, rect.Height));
        var arc = new Rectangle(rect.Location, new Size(diameter, diameter));

        path.AddArc(arc, 180, 90);
        arc.X = rect.Right - diameter;
        path.AddArc(arc, 270, 90);
        arc.Y = rect.Bottom - diameter;
        path.AddArc(arc, 0, 90);
        arc.X = rect.Left;
        path.AddArc(arc, 90, 90);
        path.CloseFigure();
        return path;
    }
}

internal sealed class RoundedPanel : Panel
{
    public int CornerRadius { get; set; } = 16;
    public Color BorderColor { get; set; } = Color.Transparent;
    public int BorderThickness { get; set; }

    public RoundedPanel()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var rect = ClientRectangle;
        rect.Width -= 1;
        rect.Height -= 1;

        using var path = CreateRoundPath(rect, CornerRadius);
        using var brush = new SolidBrush(BackColor);
        e.Graphics.FillPath(brush, path);

        if (BorderThickness > 0)
        {
            using var pen = new Pen(BorderColor, BorderThickness);
            e.Graphics.DrawPath(pen, path);
        }
    }

    protected override void OnResize(EventArgs eventargs)
    {
        base.OnResize(eventargs);
        if (Width <= 0 || Height <= 0)
            return;

        var rect = ClientRectangle;
        using var path = CreateRoundPath(rect, CornerRadius);
        Region = new Region(path);
    }

    private static GraphicsPath CreateRoundPath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        int diameter = Math.Min(radius * 2, Math.Min(rect.Width, rect.Height));
        var arc = new Rectangle(rect.Location, new Size(diameter, diameter));

        path.AddArc(arc, 180, 90);
        arc.X = rect.Right - diameter;
        path.AddArc(arc, 270, 90);
        arc.Y = rect.Bottom - diameter;
        path.AddArc(arc, 0, 90);
        arc.X = rect.Left;
        path.AddArc(arc, 90, 90);
        path.CloseFigure();
        return path;
    }
}
