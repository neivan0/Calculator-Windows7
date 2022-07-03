using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;



namespace калькулятор
{  
    public partial class MainWindow : Window
    {
        string number = "", number_history, num_1=null, num_2=null, operation=null, result=null;
        bool is_operation=true;
        public MainWindow()
        {
            InitializeComponent();
            foreach (UIElement el in win_content.Children)
            {
                if (el is Button)
                {
                    if (((Button)el).Tag.ToString() == "count") ((Button)el).Click += NumberClick;
                    if (((Button)el).Tag.ToString() == "operation_1") ((Button)el).Click += OperationClick_1;
                    if (((Button)el).Tag.ToString() == "operation_2") ((Button)el).Click += OperationClick_2;
                    if (((Button)el).Tag.ToString() == "memory") ((Button)el).Click += MemoryClick;
                }
            }
        }
        private void KeyDown_button(object sender, KeyEventArgs e)
        {
            //MessageBox.Show((((e.Key))).ToString());
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
            {
                if (e.Key == Key.OemPlus) OperationClick_1(plus, null);
                if (e.Key == Key.D8) OperationClick_1(mult, null);
            } 
            else
            {
                if (e.Key == Key.OemQuestion) OperationClick_1(del, null);
                if (e.Key == Key.OemMinus) OperationClick_1(minus, null);
                if (e.Key == Key.OemPlus) ShowResult(equal, null);

                if (e.Key == Key.Back) DeleteSymbol(back, null);

                if (e.Key == Key.D0) NumberClick(num0, null);
                if (e.Key == Key.D1) NumberClick(num1, null);
                if (e.Key == Key.D2) NumberClick(num2, null);
                if (e.Key == Key.D3) NumberClick(num3, null);
                if (e.Key == Key.D4) NumberClick(num4, null);
                if (e.Key == Key.D5) NumberClick(num5, null);
                if (e.Key == Key.D6) NumberClick(num6, null);
                if (e.Key == Key.D7) NumberClick(num7, null);
                if (e.Key == Key.D8) NumberClick(num8, null);
                if (e.Key == Key.D9) NumberClick(num9, null);

            }

        }
        private void SetHistory(string symbol)
        {
            symbol = (symbol == "√") ? "sqrt" : (symbol == "1/x") ? "reciprock" : (symbol == "+-") ? "negative" : symbol;
            if (symbol == "+" || symbol == "-" || symbol == "/" || symbol == "*")
            {
                if (!is_input & label_history.Content.ToString()!= "")
                {
                    label_history.Content = label_history.Content.ToString().Remove(label_history.Content.ToString().Length - 1, 1);
                    label_history.Content += symbol;
                    
                   // MessageBox.Show("0");
                }
                else if (number_history == null)
                {
                    
                    number_history = (num_2 == null) ? num_1 : num_2;
                    if (number_history == "0") number_history = label_op.Content.ToString();
                    label_history.Content += number_history + symbol;
                    
                   // MessageBox.Show(number_history);
                }
                else
                {
                    label_history.Content += symbol;
                }
                number_history = null;
                is_input = false;
                //is_change = true;
            }
            else
            {
                //удаляю sqrt(num), заменяю на sqrt(sqrt(num)) и т.д.
                //потом сделать адекватную проверку
                is_input = true;
                if (number_history!=null & label_history.Content.ToString().Length>=6) label_history.Content = label_history.Content.ToString().Remove(label_history.Content.ToString().Length-number_history.Length, number_history.Length);
               number_history = (number_history != null) ? $"{symbol}({number_history})" : $"{symbol}({label_op.Content.ToString()})";
               label_history.Content += number_history;
                
                //number_history = null;
            }
            
        }
        bool is_input = false;
        private void InputNumbers(string num)
        {
            is_input = true;
            if (!(number.Contains(num) & num == "," | num == "0" & label_op.Content.ToString() == "0") & number.Length<=16)
            {
                if (num == "," & number == "") number += "0";
                number += num;
                label_op.Content = number;
            }
        }
        private void SetNumber()
        {
            // if (number == "") number = "0";
            if (num_1 == null & number != "") num_1 = number;
            else if (number != "" & num_1!=null) num_2 = number;
            else if (number == "" & num_1 != null) num_2 = "0";
            else if (number == "" & num_1 == null) { num_1 = "0";  }
            number = "";
           // is_input = false;
        }
        private void SetOperation(string symbol)
        {
            //MessageBox.Show(num_1);
            if (!is_operation)num_2 = null;

            if (is_operation) { SetNumber(); }
           // if (num_1 == null) MessageBox.Show(num_1); ;

            if (num_1 != null & num_2 != null) result = Operation(num_1, num_2, operation);

            if (result != null)
            {
                SetResult();
            }

            operation = symbol;
        }
        private string Operation(string a, string b = null, string op = null)
        {
            string answer = null;
            switch (op)
            {
                case "+":
                    answer = MathOperations.Add(a, b);
                    break;
                case "-":
                    answer = MathOperations.Sub(a, b);
                    break;
                case "*":
                    answer = MathOperations.Mult(a, b);
                    break;
                case "/":
                    answer = MathOperations.Del(a, b);
                    break;
                case "√":
                    answer = MathOperations.Sqr(a);
                    break;
                case "1/x":
                    answer = MathOperations.Fract(a);
                    break;
                case "+-":
                    answer = MathOperations.Neg(a);
                    break;
            }
            return answer;
        }
        private void SetResult()
        {
            label_op.Content = result;
            num_1 = result;
            //num_2 = null;
        }
        string repeat_operation;
        private void ShowResult(object sender, RoutedEventArgs e)
        {
            if (operation != null)
            {
                repeat_operation = operation;
               // SetNumber();
            }
            if (is_operation) { SetNumber(); num_2 = label_op.Content.ToString(); }
            result = Operation(num_1, num_2, repeat_operation);
            SetResult();
            is_operation = false;            
            label_history.Content = "";
            number_history = null;
            operation = null;
            SetNormalSize();
           // SetNumber();
        } 
        private void NumberClick(object sender, RoutedEventArgs e)
        {
           // is_operation = false;
            if (!MathOperations.is_error) InputNumbers(((Button)sender).Content.ToString());
            SetNormalSize();
        }
        private void OperationClick_1(object sender, RoutedEventArgs e)
        {
            if (!MathOperations.is_error)
            {
              //  if (num_1 == null & num_2 == null) is_operation = true;
                SetOperation(((Button)sender).Content.ToString());
                SetHistory(operation);
                is_operation = true;
            }
            SetNormalSize();
        }
        private void OperationClick_2(object sender, RoutedEventArgs e)
        {
            
            if (!MathOperations.is_error)
            {
                SetHistory(((Button)sender).Content.ToString());
                number = Operation(label_op.Content.ToString(), op: ((Button)sender).Content.ToString());

                label_op.Content = number;
                if (operation == null)
                {
                    num_1 = number;
                    number = "";
                    result = num_1;
                   // MessageBox.Show(num_2);
                }
                else
                {
                    SetNumber();
                    // num_2 = number;
                    //number = "";
                   // MessageBox.Show($"{num_1}  {num_2}");
                    result = Operation(num_1, num_2, operation);
                }
                //SetResult();
                is_operation = false;
               // is_input = true;
            }
            SetNormalSize();
        }
        private void DeleteSymbol(object sender, RoutedEventArgs e)
        {
            if (is_input)
            {
                if (number.Length > 0) number = number.Remove(number.Length - 1);
                label_op.Content = (number.Length == 0) ? "0" : number;
                SetNormalSize();
            }
        }
        private void DeleteNumber(object sender, RoutedEventArgs e)
        {
            number = "";
            SetNumber();
            label_op.Content = "0";
            SetNormalSize();
        }
        private void DeleteAll(object sender, RoutedEventArgs e)
        {
            MathOperations.is_error = false;
            number = "";
            num_1 = null;
            num_2 = null;
            label_op.Content = "0";
            label_history.Content = "";
            number_history = null;
            operation = null;
            is_operation = true;
            result = null;
            SetNormalSize();

        }

        string save_number="0";
        private void MemoryClick(object sender, RoutedEventArgs e)
        {
            string function = ((Button)sender).Content.ToString();
            if (!MathOperations.is_error) {
                switch (function)
                {
                    case "MS":
                        save_number = label_op.Content.ToString();
                        Label_memory.Content = "M";
                        break;
                    case "MR":
                        label_op.Content = save_number;
                        number = save_number;
                        SetNumber();
                        break;
                    case "MC":
                        save_number = "0";
                        Label_memory.Content = "";
                        break;
                    case "M+":
                        Label_memory.Content = "M";
                        save_number = Operation(save_number, label_op.Content.ToString(), "+");
                        break;
                    case "M-":
                        Label_memory.Content = "M";
                        save_number = Operation(save_number, label_op.Content.ToString(), "-");
                        break;
                }
            }
        }
        private void SetNormalSize()
        {
            if (label_op.Content == null) label_op.Content = "0";
            if (label_op.Content.ToString().Length > 14) label_op.FontSize = 18;
            
            else label_op.FontSize = 23;
            if (label_op.Content.ToString().Length > 17) label_op.FontSize = 15;
            //  if (label_op.Content.ToString() == null) label_op.Content = "0";
        }
    }
}
