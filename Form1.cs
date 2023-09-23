using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;
using TextBox = System.Windows.Forms.TextBox; // для упрощение


namespace _2_lesson
{
    public partial class Form1 : Form
    {
        private StringBuilder inputBuffer;
        private bool isResultDisplayed;

        public Form1()
        {
            InitializeComponent();
            textResult1.KeyPress += textResult1_KeyPress;
            inputBuffer = new StringBuilder();
            KeyPreview = true;
            KeyDown += Form1_KeyDown;
            textResult1.TextChanged += TextResult1_TextChanged;
            isResultDisplayed = false;
            buttonequal.Click += Buttonequal_Click;
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.KeyCode;

            if (key >= (int)Keys.D0 && key <= (int)Keys.D9)
            {
                int digit = key - (int)Keys.D0;
                AppendInput(digit.ToString());
                e.Handled = true;
            }
            else if (key == (int)Keys.Oemplus || key == (int)Keys.Add)
            {
                PerformCalculation();
                SetOperator("+");
                textResult1.Text = "";
                e.Handled = true;
            }
            else if (key == (int)Keys.OemMinus || key == (int)Keys.Subtract)
            {
                PerformCalculation();
                SetOperator("-");
                e.Handled = true;
            }
            else if (key == (int)Keys.Multiply)
            {
                PerformCalculation();
                SetOperator("*");
                e.Handled = true;
            }
            else if (key == (int)Keys.Divide)
            {
                PerformCalculation();
                SetOperator("/");
                e.Handled = true;
            }
            else if (key == (int)Keys.Oemcomma || key == (int)Keys.Decimal)
            {
                AppendInput(",");
                e.Handled = true;
            }
            else if (key == (int)Keys.Back)
            {
                RemoveLastInput();
                e.Handled = true;
            }
            else if (key == (int)Keys.Enter)
            {
                PerformCalculation();
                e.Handled = true;
            }
        }

        private void Buttonequal_Click(object sender, EventArgs e)
        {
            PerformCalculation();
        }

        private void AppendInput(string input)
        {
            if (isResultDisplayed)
            {
                ClearInput();
                isResultDisplayed = false;
            }
            inputBuffer.Append(input);
            textResult1.Text = inputBuffer.ToString();
        }

        private void RemoveLastInput()
        {
            if (inputBuffer.Length > 0)
            {
                inputBuffer.Length -= 1;
                textResult1.Text = inputBuffer.ToString();
            }
        }

        private void PerformCalculation()
        {
            try
            {
                DataTable dt = new DataTable();
                var result = dt.Compute(inputBuffer.ToString(), "");
                inputBuffer.Clear();
                inputBuffer.Append(result.ToString());
                textResult1.Text = inputBuffer.ToString();
                isResultDisplayed = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при вычислении: " + ex.Message);
            }
        }

        private void SetOperator(string op)
        {
            inputBuffer.Append(op);
            textResult1.Text = inputBuffer.ToString();
        }

        private void ClearInput()
        {
            inputBuffer.Clear();
            textResult1.Clear();
        }

        private void TextResult1_TextChanged(object sender, EventArgs e)
        {
            inputBuffer.Clear();
            inputBuffer.Append(textResult1.Text);
        }



        // КНОПКА ВЫХОДА
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private bool isNumEntered = false;
        private bool isPoint = false;
        private bool isNum2 = false;

        private string num1 = null;
        private string num2 = null;

        private string currentOperation = "";

        private void SetNum(string txt)
        {
            if (isNum2)
            {
                num2 += txt;
                textResult1.Text = num2;
            }
            else
            {
                if (txt == "," && string.IsNullOrEmpty(num1))
                {
                    num1 = "0" + txt;
                    textResult1.Text = num1;
                }
                else
                {
                    num1 += txt;
                    textResult1.Text = num1;
                }
            }
        }


        private void buttonNumberClick(object obj, EventArgs e)
        {
            var txt = ((Button)obj).Text;

            if (txt == "+/-")
            {
                if (!string.IsNullOrEmpty(num1))
                {
                    num1 = (-double.Parse(num1)).ToString();
                    textResult1.Text = num1;
                }
                return;
            }

            if (isPoint && txt == ",") return;
            if (txt == ",") isPoint = true;

            else if (txt == "," && (textResult1.Text.Length == 0))
            {
                textResult1.Text = "0" + textResult1.Text;
            }

            SetNum(txt);
        }


        private void butttonOperationClick(object obj, EventArgs e)
        {
            isNumEntered = false;
            if (num1 == null)
            {
                if (textResult1.Text.Length > 0)
                    num1 = textResult1.Text;
                else
                    return;
            }

            isNum2 = true;
            currentOperation = ((Button)obj).Text;
            SetResult(currentOperation);
        }

        private void SetResult(string operation)
        {
            string result = null;
            switch (operation)
            {
                case "+": { result = MathOperation.Add(num1, num2); break; }
                case "-": { result = MathOperation.Sub(num1, num2); break; }
                case "*": { result = MathOperation.Mul(num1, num2); break; }
                case "/": { result = MathOperation.Dev(num1, num2); break; }
                case "%": { result = MathOperation.Proc(num1, num2); break; }
                case "√": { result = MathOperation.Sqr(num1); isNum2 = false; break; }
                case "x²": { result = MathOperation.Pow(num1); isNum2 = false; break; }
                case " ⅟x": { result = MathOperation.OneDev(num1); isNum2 = false; break; }
                default: break;
            }
            OutputResult(result, operation);
            if (isNum2) { if (result != null) num1 = result; } else { num1 = null; }
            isPoint = false;
        }

        private void OutputResult(string result, string operation)
        {
            switch (operation)
            {
                case "√": { if (num1 != null) textHistory1.Text = "√" + num1 + " = "; break; }
                case "x²": { if (num1 != null) textHistory1.Text = num1 + "² = "; break; }
                case " ⅟x": { if (num1 != null) textHistory1.Text = "1/" + num1 + " = "; break; }
                default:
                    {
                        if (num2 != null)
                        {
                            textHistory1.Text = num1 + " " + operation + " " + num2 + " = ";
                        }
                        else
                        {
                            if (num1 != null)
                            {
                                textHistory1.Text = num1 + " " + operation + " ";
                                break;
                            }
                        }
                    }
                    break;
            }
            num2 = null;
            if (result != null)
            {
                textResult1.Text = result;
            }
        }
        private void buttonClear(object obj, EventArgs e)
        {
            textResult1.Text = "";
            textHistory1.Text = "";
            isNum2 = false;
            currentOperation = null;
            num1 = null;
            num2 = null;
            isPoint = false;
        }
        private void buttonResultClick(object obj, EventArgs e)
        {
            SetResult(currentOperation);
            isNum2 = false;
            num1 = null;
            num2 = null;
        }
        private void buttonResetNumber(object obj, EventArgs e)
        {
            if (!string.IsNullOrEmpty(num1))
            {
                num1 = num1.Substring(0, num1.Length - 1);
                textResult1.Text = num1;
            }
        }



        private void textResult1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-' && e.KeyChar != '+' && e.KeyChar != '*' && e.KeyChar != '/' && e.KeyChar != '%' && e.KeyChar != '=')
            {
                e.Handled = true;
            }
            else if (e.KeyChar == '-' && textResult1.SelectionStart > 0) // если это знак "-" и он не первый то блокируем
            {
                e.Handled = true;
            }
            else if (e.KeyChar == ',' && textResult1.Text.Contains(",")) // если это запятая "," и ты уже ввел запятую, то больше вводить ее ты не можешь.
            {
                e.Handled = true;
            }
            else if (e.KeyChar == ',' && textResult1.Text.Length == 0) // если ты ввел запятую и она первая, то вместо нее ставим "0,"
            {
                e.Handled = true;
                textResult1.Text = "0,";
                textResult1.SelectionStart = 2; // после изменения, после какого символа пишешь дальше
            }
            else if (textResult1.Text == "-,")
            {
                e.Handled = true;
                textResult1.Text = "-0,";
                textResult1.SelectionStart = 3;
            }
            else if (textResult1.Text == "00")
            {
                e.Handled = true;
                textResult1.Text = "0,0";
                textResult1.SelectionStart = 3;
            }
            else if (textResult1.Text == "0")
            {
                e.Handled = true;
                textResult1.Text = "0,";
                textResult1.SelectionStart = 2;
            }
        }
    }
}
