using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace G_Tara
{
    public class CalendarPickerForm : Form
    {
        private readonly WebView2 _webView;
        private readonly Button _btnConfirm;
        private readonly Button _btnCancel;
        private readonly Button _btnClear;
        private readonly bool _multiSelect;

        public DateTime? SelectedDate { get; private set; }
        public List<DateTime> SelectedDates { get; private set; } = new();

        public CalendarPickerForm(DateTime? initialDate = null, IEnumerable<DateTime>? initialDates = null, bool multiSelect = false)
        {
            _multiSelect = multiSelect;

            Text = multiSelect ? "Pick Available Dates" : "Pick Date";
            StartPosition = FormStartPosition.CenterParent;
            Width = 520;
            Height = 520;
            MinimumSize = new Size(420, 420);

            var container = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            container.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            container.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));

            _webView = new WebView2
            {
                Dock = DockStyle.Fill
            };

            var bottomPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                Padding = new Padding(10, 8, 10, 8)
            };
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _btnClear = new Button
            {
                Text = "Clear",
                Width = 80,
                Height = 28,
                Anchor = AnchorStyles.Right
            };
            _btnClear.Click += (s, e) =>
            {
                SelectedDates.Clear();
                SelectedDate = null;
                _webView.CoreWebView2?.PostWebMessageAsString("{\"action\":\"clear\"}");
            };

            _btnConfirm = new Button
            {
                Text = multiSelect ? "Use Dates" : "Use Date",
                Width = 110,
                Height = 28,
                Anchor = AnchorStyles.Right
            };
            _btnConfirm.Click += (s, e) =>
            {
                DialogResult = DialogResult.OK;
                Close();
            };

            _btnCancel = new Button
            {
                Text = "Cancel",
                Width = 90,
                Height = 28,
                Anchor = AnchorStyles.Right
            };
            _btnCancel.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            bottomPanel.Controls.Add(new Label { AutoSize = true }, 0, 0);
            bottomPanel.Controls.Add(_btnClear, 1, 0);
            bottomPanel.Controls.Add(_btnConfirm, 2, 0);
            bottomPanel.Controls.Add(_btnCancel, 3, 0);

            container.Controls.Add(_webView, 0, 0);
            container.Controls.Add(bottomPanel, 0, 1);
            Controls.Add(container);

            var dateList = initialDates?.Select(d => d.Date).Distinct().OrderBy(d => d).ToList() ?? new List<DateTime>();
            if (initialDate.HasValue)
            {
                SelectedDate = initialDate.Value.Date;
                if (!dateList.Any())
                {
                    dateList.Add(SelectedDate.Value);
                }
            }

            SelectedDates = dateList;
            Shown += (s, e) => OnShownAsync(dateList, SelectedDate);
        }

        private async void OnShownAsync(List<DateTime> initialDates, DateTime? initialDate)
        {
            try
            {
                await _webView.EnsureCoreWebView2Async();
                _webView.CoreWebView2.Settings.IsWebMessageEnabled = true;
                _webView.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
                _webView.NavigateToString(BuildCalendarHtml(initialDates, initialDate, _multiSelect));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to initialize calendar: {ex.Message}", "Calendar Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                if (root.TryGetProperty("dates", out var datesElement) && datesElement.ValueKind == JsonValueKind.Array)
                {
                    var dates = new List<DateTime>();
                    foreach (var item in datesElement.EnumerateArray())
                    {
                        if (item.ValueKind != JsonValueKind.String) continue;
                        if (DateTime.TryParse(item.GetString(), out var parsed))
                        {
                            dates.Add(parsed.Date);
                        }
                    }

                    SelectedDates = dates.OrderBy(d => d).ToList();
                    SelectedDate = SelectedDates.Count > 0 ? SelectedDates[0] : null;
                }
            }
            catch
            {
            }
        }

        private static string BuildCalendarHtml(List<DateTime> initialDates, DateTime? initialDate, bool multiSelect)
        {
            var dateStrings = initialDates.Select(d => d.ToString("yyyy-MM-dd")).Distinct().ToList();
            var datesJson = JsonSerializer.Serialize(dateStrings);
            var initialSingle = initialDate?.ToString("yyyy-MM-dd") ?? string.Empty;

            return $@"<!doctype html>
<html>
<head>
  <meta charset='utf-8' />
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <style>
    :root {{
      --bg: #f7f4ee;
      --text: #2e2a24;
      --accent: #b36a1f;
      --accent-strong: #8c4f12;
      --card: #ffffff;
      --muted: #8b8176;
      --grid: #e7dfd3;
      --selected: #f5d3a1;
      --selected-text: #3d2b15;
    }}
    * {{ box-sizing: border-box; font-family: 'Segoe UI', Tahoma, sans-serif; }}
    html, body {{ height: 100%; margin: 0; background: var(--bg); color: var(--text); }}
    .wrap {{ padding: 16px; height: 100%; display: flex; flex-direction: column; gap: 12px; }}
    .header {{ display: flex; align-items: center; gap: 8px; }}
    .month {{ flex: 1; text-align: center; font-size: 18px; font-weight: 600; }}
    .nav-btn {{ border: 1px solid var(--grid); background: var(--card); padding: 6px 10px; border-radius: 6px; cursor: pointer; }}
    .weekday {{ display: grid; grid-template-columns: repeat(7, 1fr); gap: 6px; font-size: 12px; color: var(--muted); }}
    .grid {{ display: grid; grid-template-columns: repeat(7, 1fr); gap: 6px; flex: 1; }}
    .cell {{ border: 1px solid var(--grid); background: var(--card); padding: 10px 6px; border-radius: 8px; text-align: center; cursor: pointer; user-select: none; }}
    .cell.out {{ color: var(--muted); background: #f3efe7; }}
    .cell.selected {{ background: var(--selected); color: var(--selected-text); border-color: var(--accent); }}
    .legend {{ font-size: 12px; color: var(--muted); }}
  </style>
</head>
<body>
  <div class='wrap'>
    <div class='header'>
      <button class='nav-btn' id='prev' type='button'>&lt;</button>
      <div class='month' id='monthLabel'></div>
      <button class='nav-btn' id='next' type='button'>&gt;</button>
    </div>
    <div class='weekday'>
      <div>Sun</div><div>Mon</div><div>Tue</div><div>Wed</div><div>Thu</div><div>Fri</div><div>Sat</div>
    </div>
    <div class='grid' id='grid'></div>
    <div class='legend' id='legend'></div>
  </div>
  <script>
    const multiSelect = {multiSelect.ToString().ToLowerInvariant()};
    const initialDates = {datesJson};
    const initialSingle = '{initialSingle}';
    let selected = new Set(initialDates);
    const today = new Date();
    let view = initialSingle ? new Date(initialSingle + 'T00:00:00') : new Date();

    function startOfMonth(date) {{
      return new Date(date.getFullYear(), date.getMonth(), 1);
    }}

    function formatDate(date) {{
      const y = date.getFullYear();
      const m = String(date.getMonth() + 1).padStart(2, '0');
      const d = String(date.getDate()).padStart(2, '0');
      return y + '-' + m + '-' + d;
    }}

    function postSelection() {{
      const dates = Array.from(selected.values()).sort();
      if (window.chrome && window.chrome.webview) {{
        window.chrome.webview.postMessage(JSON.stringify({{ dates }}));
      }}
      const legend = document.getElementById('legend');
      legend.textContent = dates.length > 0 ? (dates.length + ' date(s) selected') : 'No dates selected';
    }}

    function render() {{
      const grid = document.getElementById('grid');
      grid.innerHTML = '';
      const label = document.getElementById('monthLabel');
      label.textContent = view.toLocaleString('en-US', {{ month: 'long', year: 'numeric' }});

      const first = startOfMonth(view);
      const startDay = first.getDay();
      const daysInMonth = new Date(view.getFullYear(), view.getMonth() + 1, 0).getDate();

      const prevMonthDays = new Date(view.getFullYear(), view.getMonth(), 0).getDate();

      const cells = 42;
      for (let i = 0; i < cells; i++) {{
        const cell = document.createElement('div');
        cell.className = 'cell';
        let dayNum = i - startDay + 1;
        let cellDate = new Date(view.getFullYear(), view.getMonth(), dayNum);
        if (dayNum <= 0) {{
          cell.classList.add('out');
          cellDate = new Date(view.getFullYear(), view.getMonth() - 1, prevMonthDays + dayNum);
        }} else if (dayNum > daysInMonth) {{
          cell.classList.add('out');
          cellDate = new Date(view.getFullYear(), view.getMonth() + 1, dayNum - daysInMonth);
        }}

        const dateStr = formatDate(cellDate);
        cell.textContent = cellDate.getDate();
        if (selected.has(dateStr)) {{
          cell.classList.add('selected');
        }}

        cell.addEventListener('click', () => {{
          if (!multiSelect) {{
            selected = new Set([dateStr]);
          }} else {{
            if (selected.has(dateStr)) {{
              selected.delete(dateStr);
            }} else {{
              selected.add(dateStr);
            }}
          }}
          render();
          postSelection();
        }});

        grid.appendChild(cell);
      }}
    }}

    document.getElementById('prev').addEventListener('click', () => {{
      view = new Date(view.getFullYear(), view.getMonth() - 1, 1);
      render();
    }});

    document.getElementById('next').addEventListener('click', () => {{
      view = new Date(view.getFullYear(), view.getMonth() + 1, 1);
      render();
    }});

    window.chrome && window.chrome.webview && window.chrome.webview.addEventListener('message', event => {{
      try {{
        const data = JSON.parse(event.data);
        if (data.action === 'clear') {{
          selected = new Set();
          render();
          postSelection();
        }}
      }} catch (err) {{
      }}
    }});

    render();
    postSelection();
  </script>
</body>
</html>";
        }
    }
}
