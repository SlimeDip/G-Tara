using Microsoft.Web.WebView2.WinForms;
using System.Text.Json;

namespace G_Tara
{
    public class MapPickerForm : Form
    {
        private const string TileReferer = "https://g-tara.local/";
        private readonly WebView2 _webView;
        private readonly Label _lblCoords;
        private readonly Button _btnConfirm;
        private readonly Button _btnCancel;

        public double SelectedLatitude { get; private set; }
        public double SelectedLongitude { get; private set; }

        public MapPickerForm(double initialLatitude = 14.5995, double initialLongitude = 120.9842)
        {
            SelectedLatitude = initialLatitude;
            SelectedLongitude = initialLongitude;

            this.Text = "Pick Location on Map";
            this.StartPosition = FormStartPosition.CenterParent;
            Text = "Pick Location on Map";
            StartPosition = FormStartPosition.CenterParent;
            Width = 900;
            Height = 650;
            this.BackColor = Color.FromArgb(242, 215, 217); 
            this.Font = new Font("Segoe UI Semibold", 9.5F); 
            MinimumSize = new Size(700, 500);

            var container = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent
            };
            container.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            container.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));

            _webView = new WebView2
            {
                Dock = DockStyle.Fill
            };

            var bottomPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                Padding = new Padding(15, 8, 15, 8),
                BackColor = Color.FromArgb(189, 126, 131) 
            };
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _lblCoords = new Label
            {
                AutoSize = true,
                Text = $"Lat: {SelectedLatitude:F6}, Lon: {SelectedLongitude:F6}",
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F),
                Anchor = AnchorStyles.Left,
                TextAlign = ContentAlignment.MiddleLeft
            };


            _btnConfirm = new Button { Text = "Use This Location", Width = 140, Height = 32 };

            _btnConfirm.Click += (s, e) =>
            {
                DialogResult = DialogResult.OK;
                Close();
            };

            _btnCancel = new Button { Text = "Cancel", Width = 100, Height = 32 };

            _btnCancel.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            bottomPanel.Controls.Add(_lblCoords, 0, 0);
            bottomPanel.Controls.Add(_btnConfirm, 1, 0);
            bottomPanel.Controls.Add(_btnCancel, 2, 0);

            container.Controls.Add(_webView, 0, 0);
            container.Controls.Add(bottomPanel, 0, 1);
            Controls.Add(container);

            ApplyGaraTheme();
            Shown += OnShownAsync;
        }

        private async void OnShownAsync(object? sender, EventArgs e)
        {
            try
            {
                await _webView.EnsureCoreWebView2Async();
                _webView.CoreWebView2.Settings.IsWebMessageEnabled = true;
                _webView.CoreWebView2.AddWebResourceRequestedFilter("https://*.tile.openstreetmap.org/*", Microsoft.Web.WebView2.Core.CoreWebView2WebResourceContext.Image);
                _webView.CoreWebView2.WebResourceRequested += OnWebResourceRequested;
                _webView.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
                _webView.NavigateToString(BuildMapHtml(SelectedLatitude, SelectedLongitude));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to initialize map: {ex.Message}", "Map Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnWebMessageReceived(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                var json = e.TryGetWebMessageAsString();
                if (string.IsNullOrWhiteSpace(json))
                {
                    json = e.WebMessageAsJson;
                }

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.String)
                {
                    var innerJson = root.GetString();
                    if (string.IsNullOrWhiteSpace(innerJson))
                    {
                        return;
                    }

                    using var innerDoc = JsonDocument.Parse(innerJson);
                    var innerRoot = innerDoc.RootElement;
                    if (!innerRoot.TryGetProperty("lat", out var innerLat) || !innerRoot.TryGetProperty("lon", out var innerLon))
                    {
                        return;
                    }

                    SelectedLatitude = innerLat.GetDouble();
                    SelectedLongitude = innerLon.GetDouble();
                    _lblCoords.Text = $"Lat: {SelectedLatitude:F6}, Lon: {SelectedLongitude:F6}";
                    return;
                }

                if (root.TryGetProperty("lat", out var latProp) && root.TryGetProperty("lon", out var lonProp))
                {
                    SelectedLatitude = latProp.GetDouble();
                    SelectedLongitude = lonProp.GetDouble();
                    _lblCoords.Text = $"Lat: {SelectedLatitude:F6}, Lon: {SelectedLongitude:F6}";
                }
            }
            catch
            {
            }
        }

        private void OnWebResourceRequested(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebResourceRequestedEventArgs e)
        {
            try
            {
                var headers = e.Request.Headers;
                headers.SetHeader("Referer", TileReferer);
            }
            catch
            {
            }
        }

        private static string BuildMapHtml(double lat, double lon)
        {
            return $@"<!doctype html>
<html>
<head>
  <meta charset='utf-8' />
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css' />
  <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
  <style>
    html, body, #map {{ height: 100%; margin: 0; padding: 0; }}
  </style>
</head>
<body>
  <div id='map'></div>
  <script>
    const initialLat = {lat.ToString(System.Globalization.CultureInfo.InvariantCulture)};
    const initialLon = {lon.ToString(System.Globalization.CultureInfo.InvariantCulture)};

    const map = L.map('map').setView([initialLat, initialLon], 13);

    L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{
      maxZoom: 19,
      attribution: '&copy; OpenStreetMap contributors'
    }}).addTo(map);

    let marker = L.marker([initialLat, initialLon]).addTo(map);

    function send(lat, lon) {{
      if (window.chrome && window.chrome.webview) {{
        window.chrome.webview.postMessage(JSON.stringify({{ lat: lat, lon: lon }}));
      }}
    }}

    send(initialLat, initialLon);
    marker.bindPopup(`Lat: ${{initialLat.toFixed(6)}}<br>Lon: ${{initialLon.toFixed(6)}}`).openPopup();

    map.on('click', function(e) {{
      const lat = e.latlng.lat;
      const lon = e.latlng.lng;
      marker.setLatLng([lat, lon]);
      marker.bindPopup(`Lat: ${{lat.toFixed(6)}}<br>Lon: ${{lon.toFixed(6)}}`).openPopup();
      send(lat, lon);
    }});
  </script>
</body>
</html>";
        }
        private void ApplyGaraTheme()
        {
            // Palette for buttons[cite: 2]
            Color btnBack = Color.FromArgb(248, 230, 231);
            Color btnBorder = Color.FromArgb(210, 170, 175);
            Color btnHover = Color.FromArgb(235, 190, 195);
            Color btnDown = Color.FromArgb(200, 150, 155);

            ApplyRoundedButtonStyle(_btnConfirm, btnBack, btnBorder, btnHover, btnDown, null);
            ApplyRoundedButtonStyle(_btnCancel, btnBack, btnBorder, btnHover, btnDown, null);
        }

        private static void ApplyRoundedButtonStyle(Button btn, Color back, Color border, Color hover, Color down, System.Media.SoundPlayer? hoverSound)
        {
            if (btn.Tag is RoundedButtonStyleState) return;
            var state = new RoundedButtonStyleState { Back = back, Border = border, Hover = hover, Down = down };
            btn.Tag = state;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Region = new Region(CreateRoundedRectPath(new Rectangle(0, 0, btn.Width, btn.Height), 12));

            state.AnimationTimer = new System.Windows.Forms.Timer { Interval = 15 };
            state.AnimationTimer.Tick += (s, e) => {
                var delta = state.HoverTarget - state.HoverProgress;
                if (Math.Abs(delta) < 0.01f) { state.HoverProgress = state.HoverTarget; state.AnimationTimer.Stop(); }
                else { state.HoverProgress += delta * 0.22f; }
                btn.Invalidate();
            };

            btn.MouseEnter += (s, e) => { state.IsHover = true; state.HoverTarget = 1f; state.AnimationTimer.Start(); };
            btn.MouseLeave += (s, e) => { state.IsHover = false; state.HoverTarget = 0f; state.AnimationTimer.Start(); };
            btn.MouseDown += (s, e) => { state.IsDown = true; btn.Invalidate(); };
            btn.MouseUp += (s, e) => { state.IsDown = false; btn.Invalidate(); };

            btn.Paint += (s, e) => {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Color current = state.IsDown ? state.Down : Interpolate(state.Back, state.Hover, state.HoverProgress);
                using (var path = CreateRoundedRectPath(new Rectangle(0, 0, btn.Width - 1, btn.Height - 1), 12))
                {
                    using (var brush = new SolidBrush(current)) e.Graphics.FillPath(brush, path);
                    using (var pen = new Pen(state.Border, 1f)) e.Graphics.DrawPath(pen, path);
                }
                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, Color.FromArgb(64, 64, 64), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
        }

        private static Color Interpolate(Color b, Color t, float p) =>
            Color.FromArgb((int)(b.R + (t.R - b.R) * p), (int)(b.G + (t.G - b.G) * p), (int)(b.B + (t.B - b.B) * p));

        private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedRectPath(Rectangle r, int rad)
        {
            var p = new System.Drawing.Drawing2D.GraphicsPath();
            int d = rad * 2;
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        public class RoundedButtonStyleState
        {
            public Color Back, Border, Hover, Down;
            public float HoverProgress, HoverTarget;
            public bool IsHover, IsDown;
            public System.Windows.Forms.Timer AnimationTimer = new();
        }
    }
}
