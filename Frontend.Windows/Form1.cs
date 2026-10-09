using Backend;
using System.Globalization;
//using static System.Net.Mime.MediaTypeNames;
//using System.Linq.Expressions;
//using static System.Net.Mime.MediaTypeNames;

namespace Frontend.Windows
{
    public partial class Form1 : Form
    {
        private readonly Label IblDisplay = new();
        private string expression = string.Empty;
        private string lastExpression = string.Empty;
        private bool showingResult;
        private string? errorMessage;

        public Form1()
        {
            InitializeComponent();
            BuildCalculator();

        }

        private void BuildCalculator()

        {
            Text = "Functions Evaluator";
            BackColor = Color.Black;
            ClientSize = new Size(420, 500);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            //IblDisplay.Dock = DockStyle.Top;
            //IblDisplay.Height = 70;
            //IblDisplay.BackColor = Color.Green;
            //IblDisplay.ForeColor = Color.White;
            //IblDisplay.Font = new Font("Segoe UI", 20);
            //IblDisplay.TextAlign = ContentAlignment.MiddleLeft;
            //IblDisplay.Margin = new Padding(0);

           IblDisplay.Dock = DockStyle.Fill;
            IblDisplay.BackColor = Color.Green;
            IblDisplay.ForeColor = Color.White;
            IblDisplay.Font = new Font("Segoe UI", 20);
            IblDisplay.TextAlign = ContentAlignment.MiddleLeft;

       
            var border = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(2)
            };
            border.Controls.Add(IblDisplay);

            var displayHolder = new Panel
            {
                Dock = DockStyle.Top,
                Height = 84,
                BackColor = Color.Black,
                Padding = new Padding(10, 10, 10, 4)
            };
            displayHolder.Controls.Add(border);

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 7,
                RowCount = 4,
                Padding = new Padding(6)
            };

            for (int i = 0; i < 7; i++) table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 7));
            for (int i = 0; i < 4; i++) table.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));

            AddButton(table, "7", 0, 0, 1, false);
            AddButton(table, "8", 0, 1, 1, false);
            AddButton(table, "9", 0, 2, 1, false);
            AddButton(table, "(", 0, 3, 1, true);
            AddButton(table, ")", 0, 4, 1, true);
            AddButton(table, "Delete", 0, 5, 2, true);

            AddButton(table, "4", 1, 0, 1, false);
            AddButton(table, "5", 1, 1, 1, false);
            AddButton(table, "6", 1, 2, 1, false);
            AddButton(table, "*", 1, 3, 1, true);
            AddButton(table, "/", 1, 4, 1, true);
            AddButton(table, "Clear", 1, 5, 2, true);

            AddButton(table, "1", 2, 0, 1, false);
            AddButton(table, "2", 2, 1, 1, false);
            AddButton(table, "3", 2, 2, 1, false);
            AddButton(table, "+", 2, 3, 1, true);
            AddButton(table, "-", 2, 4, 1, true);
            AddButton(table, "^", 2, 5, 2, true);

            AddButton(table, "0", 3, 0, 2, false);
            AddButton(table, ".", 3, 2, 1, false);
            AddButton(table, "=", 3, 3, 4, true);

            Controls.Add(table);
            Controls.Add(displayHolder);

        }


        private void AddButton(TableLayoutPanel table, string text, int row, int col, int colSpan, bool orange)
        {
            var button = new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                Margin = new Padding(4),
                Font = new Font("Segoe UI", text.Length > 1 ? 11 : 18, orange ? FontStyle.Bold : FontStyle.Regular),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Black,
                BackColor = orange ? Color.FromArgb(255, 128, 0) : Color.White
            };

            button.FlatAppearance.BorderColor = Color.White;
            button.FlatAppearance.BorderSize = 2;

            button.Click += (_, _) => OnButtonClick(text);

            table.Controls.Add(button, col, row);
            table.SetColumnSpan(button, colSpan);
        }

        private void OnButtonClick(string text)

        {
            switch (text)
            {
                case "Clear":
                    expression = string.Empty;
                    showingResult = false;
                    errorMessage = null;
                    break;

                case "Delete":
                    errorMessage = null;
                    if (showingResult)
                    {
                        expression = string.Empty;
                        showingResult = false;
                    }
                    else if (expression.Length > 0)
                    {
                        expression = expression[..^1];
                    }
                    break;

                case "=":
                    Calculate();
                    break;

                default:
                    Append(text);
                    break;
            }

            UpdateDisplay();
        }

        private void Append(string text)
        {
            bool isOperator = "+-*/^".Contains(text);

            if (showingResult)
            {

                if (!isOperator)
                {
                    expression = string.Empty;
                }
                showingResult = false;
            }

            errorMessage = null;
            expression += text;
        }

        private void Calculate()
        {
            if (expression.Length == 0 || showingResult)
            {
                return;
            }

            try
            {
                double result = ExpressionEvaluator.Evalute(expression);

                if (double.IsNaN(result) || double.IsInfinity(result))
                {
                    throw new Exception("Invalid result.");
                }

                lastExpression = expression;
                expression = result.ToString(CultureInfo.InvariantCulture);
                showingResult = true;
            }
            catch
            {
                expression = string.Empty;
                showingResult = false;
                errorMessage = "Error";
            }
        }

        private void UpdateDisplay()
        {
            if (errorMessage != null)
            {
                IblDisplay.Text = errorMessage;
            }
            else if (showingResult)
            {
                IblDisplay.Text = $"{lastExpression}={expression}";
            }
            else
            {
                IblDisplay.Text = expression;
            }
            FitDisplayFont();
        }

        private void FitDisplayFont()
        {
            {
                float size = 20f;
                int available =IblDisplay.ClientSize.Width - 10;

                while (size > 8f)
                {
                    using var testFont = new Font("Segoe UI", size);
                    if (TextRenderer.MeasureText(IblDisplay.Text, testFont).Width <= available)
                    {
                        break;
                    }
                    size -= 1f;
                }

                IblDisplay.Font = new Font("Segoe UI", size);
            }
        }
    }
}