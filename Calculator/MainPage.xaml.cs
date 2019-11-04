using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Calculator
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        List<String> selectedButtons;

        //memory string or list
        string memoryString = "";
        double memoryValue = 0;

        //Variabel to evaluate x^y
        Boolean xyExponent = false;
        double firstXY = 0;

        //Public constructor intializes the list containing the selected buttons and the picker
        public MainPage()
        {
            InitializeComponent();
            selectedButtons = new List<string>();
            setUpPicker();
          
            
        }

        //Creates the picker with a default value of 3
        public void setUpPicker()
        {
            //Adding 0-14 to picker and setting starting round number to 3
            for (int i = 0; i < 15; i++)
            {
                picker.Items.Add(i.ToString());
            }
            picker.SelectedIndex = 3;
        }

        //Event handler if a button is a memory button
        public void MemoryOnClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            string buttonText = button.Text;
        
            if (!mainLabel.Text.Equals("0"))
            {
                switch (buttonText)
                {
                    case "M+":
                        memoryValue = memoryValue + Double.Parse(mainLabel.Text);
                        memoryString = memoryValue.ToString();
                        break;
                    case "M-":
                        memoryValue = memoryValue - Double.Parse(mainLabel.Text);
                        memoryString = memoryValue.ToString();
                        break;
                    case "MR":
                        if (!memoryString.Equals(""))
                        {
                            mainLabel.Text = memoryString;
                            selectedButtons.Clear();
                            selectedButtons.Add(memoryString);
                        }
                        break;
                    case "MS":
                        //method to see if operator
                        memoryString = mainLabel.Text;
                        memoryValue = Double.Parse(memoryString);
                        break;
                    case "MC":
                        memoryString = "";
                        memoryValue = 0;
                        break;

                    default:
                        break;
                }
            }

        }

        //Event handler if a button with a number on it is clicked
        public void NumberOnClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            string buttonText = button.Text;
            double num = 0;
            selectedButtons.Add(buttonText);
            if (xyExponent)
            {
                selectedButtons.Clear();
                num = Math.Pow(firstXY, Double.Parse(buttonText));
                num = Math.Round(num, Int32.Parse(picker.SelectedIndex.ToString()));
                mainLabel.Text = num.ToString();
                selectedButtons.Add(num.ToString());
                xyExponent = false;
            }
           else if (mainLabel.Text.Equals("0") | !memoryString.Equals(""))
            {
                mainLabel.Text = buttonText;
            }
            else
            {
                mainLabel.Text = mainLabel.Text + buttonText;
            }
        }
        //Event handler for when one of the yellow buttons are clicked
        public void MathOnClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            string buttonText = button.Text;
            selectedButtons.Remove(mainLabel.Text);
            double num = 0;
            switch (buttonText)
            {
                case "sin":
                    num = Math.Sin(Double.Parse(mainLabel.Text));
                    break;
                case "asin":
                    num = Math.Asin(Double.Parse(mainLabel.Text));
                    break;
                case "cos":
                    num = Math.Cos(Double.Parse(mainLabel.Text));
                    break;
                case "acos":
                    num = Math.Acos(Double.Parse(mainLabel.Text));
                    break;
                case "tan":
                    num = Math.Tan(Double.Parse(mainLabel.Text));
                    break;
                case "atan":
                    num = Math.Atan(Double.Parse(mainLabel.Text));
                    break;
                case "√":
                    num = Math.Sqrt(Double.Parse(mainLabel.Text));
                    break;
                case "x^2":
                    num = Math.Pow(Double.Parse(mainLabel.Text), 2);
                    break;
                case "10^x":
                    num = Math.Pow(10, Double.Parse(mainLabel.Text));
                    break;
                case "ln":
                    num = Math.Log(Double.Parse(mainLabel.Text));
                    break;
                case "log":
                    num = Math.Log10(Double.Parse(mainLabel.Text));
                    break;
                case "1/x":
                    num = 1/(Double.Parse(mainLabel.Text));
                    break;
                case "x!":
                    double value = Double.Parse(mainLabel.Text);
                   for(double i = value-1; i!=0; i--)
                    {
                        value = value * i;
                    }
                    num = value;
                    break;
                default:
                    break;
            }
            num = Math.Round(num, Int32.Parse(picker.SelectedIndex.ToString()));
            mainLabel.Text = num.ToString();
            selectedButtons.Add(num.ToString());

        }
        //Computers the expression in the window
        //Returns a double which is the evaluated expression value
        public double EvaluateExpression()
        {
            string firstNumber = "";
            string operatorr = "";
            string lastNumber = "";
            double evaluation = 0;
            if (!isLastAnOperator() && (selectedButtons.Count >= 3 | isOperator(selectedButtons[0])))
            {
                int i = 0;
                if (isOperator(selectedButtons[0]))
                {
                    if (selectedButtons[0].Equals("-"))
                    {
                       firstNumber = firstNumber + "-";
                        i++;
                    }
                    else
                    {
                        firstNumber = "0";
                    }
                   
                }
                while (!isOperator(selectedButtons[i]))
                {
                    firstNumber = firstNumber + selectedButtons[i];
                    i++;
                }
                if (isOperator(selectedButtons[i]))
                {
                    operatorr = selectedButtons[i];
                    i++;
                }

                while (i < selectedButtons.Count)
                {
                    if (isOperator(selectedButtons[i]))
                    {
                        mainLabel.Text = "-1";
                        selectedButtons.Clear();
                        selectedButtons.Add("-1");
                        return -1;
                    }
                    lastNumber = lastNumber + selectedButtons[i];
                    i++;
                }

                switch (operatorr)
                {
                    case "+":
                        evaluation = Double.Parse(firstNumber) + Double.Parse(lastNumber);
                        break;
                    case "-":
                        evaluation = Double.Parse(firstNumber) - Double.Parse(lastNumber);
                        break;
                    case "x":
                        evaluation = Double.Parse(firstNumber) * Double.Parse(lastNumber);
                        break;
                    case "÷":
                        evaluation = Double.Parse(firstNumber) / Double.Parse(lastNumber);
                        break;
                    case "exp":
                        evaluation = Double.Parse(firstNumber) * Math.Pow(10, Double.Parse(lastNumber));
                        break;
                    default:
                        break;
                }
                evaluation = Math.Round(evaluation, Int32.Parse(picker.SelectedIndex.ToString()));
               
            }
            return evaluation;

        }
        //Event Handler for when the "=" is clicked
        public void EqualOnClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            string buttonText = button.Text;
            double evaluation = EvaluateExpression();
            mainLabel.Text = evaluation.ToString();
            selectedButtons.Clear();
            char[] eval = mainLabel.Text.ToCharArray();
            for(int i = 0; i < eval.Length; i++)
            {
                selectedButtons.Add(eval[i].ToString());
                
            }

        }
        //Checks whether an expression contains an opertator
        //Returns true if expression contains an opertator and false otherwise
        public Boolean containsOperator(string value)
        {
            if(value.Contains("+")|value.Contains("e") |value.Contains("/")
                | value.Contains("*"))
            {
                return true;
            }
            return false;
        }
        //Checks to see if a string is an operator
        //Return true if string is an operator and false otherwise
        //Takes in a string
        public Boolean isOperator(string value)
        {
            if (value.Equals("x"))
                return true;
            if (value.Equals("+"))
                return true;
            if (value.Equals("-"))
                return true;
            if (value.Equals("÷"))
                return true;
            if (value.Equals("exp"))
                return true;

            return false;
        }
     //Checks to see if last entered button is an operator
     //Returns true if the last thing entered was an operator
        public Boolean isLastAnOperator()
        {
            if (mainLabel.Text.Equals("0"))
            {
                return false;
            }
            if (mainLabel.Text.Equals("0-"))
                return false;
            if (mainLabel.Text.Equals("0+"))
                return false;
            if (mainLabel.Text.Equals("0/"))
                return false;
            if (mainLabel.Text.Equals("0*"))
                return false;
            if (mainLabel.Text.Equals("-"))
                return false;
            if (mainLabel.Text.Equals("+"))
                return false;

            if (selectedButtons[selectedButtons.Count - 1].Equals("x"))
                return true;
            if (selectedButtons[selectedButtons.Count - 1].Equals("-"))
                return true;
            if (selectedButtons[selectedButtons.Count - 1].Equals("+"))
                return true;
            if (selectedButtons[selectedButtons.Count - 1].Equals("÷"))
                return true;
            if (selectedButtons[selectedButtons.Count - 1].Equals("exp"))
                return true;
            if (selectedButtons[selectedButtons.Count - 1].Equals("."))
                return true;
            return false;
        }
        //Event handler for when an operator is clicked
        public void OperatorOnClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            string buttonText = button.Text;
            //if last index of is equal to * / + - do nothing
            if (!isLastAnOperator())
            {
                switch (buttonText)
                {
                    case "÷":
                        if (mainLabel.Text.Equals("-") | mainLabel.Text.Equals("+") |
                        mainLabel.Text.Equals("0-") | mainLabel.Text.Equals("0+") | mainLabel.Text.Equals("0*"))
                        {
                            mainLabel.Text = "0/";
                        }
                        else
                        {
                            mainLabel.Text = mainLabel.Text + "/";
                        }
                        break;
                    case "+":
                        if (mainLabel.Text.Equals("-") | mainLabel.Text.Equals("+") | mainLabel.Text.Equals("0-") |
                            mainLabel.Text.Equals("0/") | mainLabel.Text.Equals("0*"))
                        {
                            mainLabel.Text = "0+";
                        }
                        else if (mainLabel.Text.Equals("0"))
                        {
                            mainLabel.Text = "+";
                        }
                        else if (mainLabel.Text.Equals("0+"))
                        {
                            //doesn't allow for multiple + signs to be entered
                        }
                        else
                        {
                            mainLabel.Text = mainLabel.Text + "+";
                        }
                        break;
                    case "x":
                        if (mainLabel.Text.Equals("-") | mainLabel.Text.Equals("+") |
                        mainLabel.Text.Equals("0-") | mainLabel.Text.Equals("0+") | mainLabel.Text.Equals("0/"))
                        {
                            mainLabel.Text = "0*";
                        }
                        else
                        {
                            mainLabel.Text = mainLabel.Text + "*";
                        }
                        break;
                    case "-":
                        if (mainLabel.Text.Equals("-") | mainLabel.Text.Equals("+") | mainLabel.Text.Equals("0+") |
                            mainLabel.Text.Equals("0/") | mainLabel.Text.Equals("0*"))
                        {
                            mainLabel.Text = "0-";
                        }
                        else if (mainLabel.Text.Equals("0"))
                        {
                            mainLabel.Text = "-";
                        }
                        else if (mainLabel.Text.Equals("0-"))
                        {
                            //doesn't allow for multiple - signs to be entered
                        }
                        else
                        {
                            mainLabel.Text = mainLabel.Text + "-";
                        }
                        break;
                    default:
                        break;
                }
                selectedButtons.Add(buttonText);
                
            } 
            
        }
        //Event handler for buttons who have unique resulting actions
        public void OnClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            string buttonText = button.Text;
          
            switch (buttonText)
            {
                case "DEL":
                    if (!mainLabel.Text.Equals("0") && mainLabel.Text.Length>0)
                    {
                        mainLabel.Text = mainLabel.Text.Substring(0, mainLabel.Text.Length - 1);
                        selectedButtons.Remove(selectedButtons[selectedButtons.Count-1]);
                       
                        if (mainLabel.Text.Length == 0)
                        {
                            mainLabel.Text = "0";
                        }
                    }
                    else
                    {
                        mainLabel.Text = "0";
                    }
                   
                    break;
                case "C":
                    mainLabel.Text = "0";
                    selectedButtons.Clear();
                   
                    break;
               
                case "±":
                    if (!containsOperator(mainLabel.Text))
                    {
                        char[] temp = mainLabel.Text.ToCharArray();
                        for(int i =0; i < temp.Length; i++)
                        {
                            selectedButtons.Remove(temp[i].ToString());
                        }
                       
                        double num = (-1) * Double.Parse(mainLabel.Text);
                        mainLabel.Text = num.ToString();
                        temp = num.ToString().ToCharArray();
                        for(int j=0; j < temp.Length; j++)
                        {
                            selectedButtons.Add(temp[j].ToString());
                        }
                      
                    }
                    break;
                case "exp":
                    if (!mainLabel.Text.Equals("0"))
                    {
                        mainLabel.Text = mainLabel.Text + "e";
                    }
                    selectedButtons.Add(buttonText);
                    break;
 
                case ".":
                    mainLabel.Text = mainLabel.Text + ".";
                    selectedButtons.Add(buttonText);
                    break;
                case "π":
                    mainLabel.Text = mainLabel.Text + Math.Round(Math.PI, picker.SelectedIndex).ToString();
                    selectedButtons.Add(Math.PI.ToString());
                    break;
                case "x^y":
                    firstXY = Double.Parse(mainLabel.Text);
                    xyExponent = true;
                    break;
                default:
                    break;
            }

        }

    }//end class
}//end namespace
