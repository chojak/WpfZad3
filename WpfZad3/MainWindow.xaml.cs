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

namespace WpfZad3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum PaperFormat
    {
        A5,
        A4,
        A3,
        A2,
        A1,
        A0
    }
    public partial class MainWindow : Window
    {
        int quantity;
        bool paperColor;
        string paperColorName = "";
        PaperFormat paperFormat = PaperFormat.A5;
        string grammage = "80g/m^2";
        bool printColor = false;
        bool printDoubleSide = false;
        bool printUV = false;
        bool realizationPriority = false;

        public void OrderDisplay()
        {
            double paperFormatCost = (double)(paperFormat == PaperFormat.A5 ? 0.20 :
                paperFormat == PaperFormat.A4 ? 0.50 :
                paperFormat == PaperFormat.A3 ? 1.25 :
                paperFormat == PaperFormat.A2 ? 3.12 :
                paperFormat == PaperFormat.A1 ? 7.18 :
                paperFormat == PaperFormat.A0 ? 19.53 : 0.20);

            double gramFormatMultiplier = (double)(grammage == "80g/m^2" ? 1 : 
                grammage == "120g/m^2" ? 2 :
                grammage == "200g/m^2" ? 2.5 : 
                grammage == "240g/m^2" ? 3 : 1);

            double priceBeforeDiscount = (quantity * paperFormatCost * gramFormatMultiplier);
            priceBeforeDiscount *= printColor == true ? 1.3 : 1;
            priceBeforeDiscount *= printDoubleSide == true ? 1.5 : 1;
            priceBeforeDiscount *= printUV == true ? 1.2 : 1;
            priceBeforeDiscount *= paperColor == true && paperColorName != "" ? 1.5 : 1;
            priceBeforeDiscount += realizationPriority == true ? 15 : 0;

            double discount = ((quantity / 100) > 10 ? 10 : (quantity / 100));
            double priceAfterDiscount = priceBeforeDiscount * (1 - discount/100);


            string printColorString = printColor == true ? "druk kolor, " : "";
            string printDoubleSideString = printDoubleSide == true ? "druk dwustronny, " : "";
            string printUVString = printUV == true ? "druk UV" : "";

            string result = "Przedmiot zamówienia: " + quantity + "szt., " +
                "format " + paperFormat.ToString() + ", " + grammage + ", " + printColorString + printDoubleSideString + printUVString;

            result += (paperColor == true && paperColorName != "") ? "\nKolorowy papier: " + paperColorName : "";
            result += realizationPriority ? "\nEkspresowa realizacja zamówienia" : "";

            result += "\nCena przed rabatem: " + Math.Round(priceBeforeDiscount) + "zł\n" +
                "Naliczony rabat: " + discount + "%\n" + 
                "Cena po rabacie: " + Math.Round(priceAfterDiscount) + "zł"; 
            
            OrderDisplayTextBox.Text = result;
        }
        public MainWindow()
        {
            InitializeComponent();

            DefaultRadiobutton.IsChecked = true;
            DefaultRadiobutton2.IsChecked = true;
        }

        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int parsedValue;
            if (!int.TryParse(QuantityTextBox.Text, out parsedValue) && QuantityTextBox.Text != "")
            {
                MessageBox.Show("This is a number only field");
                return;
            }
            quantity = parsedValue;
            OrderDisplay();
        }

        private void FormatSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)FormatSlider.Value;
            if (value == 0)
            {
                FormatCostLabel.Content = "A5 - cena 20gr/szt.";
            }
            if (value == 1)
            {
                FormatCostLabel.Content = "A4 - cena 50gr/szt.";
            }
            if (value == 2)
            {
                FormatCostLabel.Content = "A3 - cena 1.25zł/szt.";
            }
            if (value == 3)
            {
                FormatCostLabel.Content = "A2 - cena 3.12zł/szt.";
            }
            if (value == 4)
            {
                FormatCostLabel.Content = "A1 - cena 7.81zł/szt.";
            }
            if (value == 5)
            {
                FormatCostLabel.Content = "A0 - cena 19.53zł/szt.";
            }
            paperFormat = (PaperFormat)value;
            OrderDisplay();
        }

        private void ColoredPaperCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            paperColor = ColoredPaperCheckBox.IsChecked == true ? true : false;

            ColorComboBox.IsEnabled = paperColor;
            OrderDisplay();
        }

        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            paperColorName = (ComboBoxItem)ColorComboBox.SelectedItem != null ? ((ComboBoxItem)ColorComboBox.SelectedItem).Content.ToString() : "";
            OrderDisplay();
        }

        private void GrammageRadio_Checked(object sender, RoutedEventArgs e)
        {
            grammage = (sender as RadioButton).Content.ToString().Split(' ')[0];
            OrderDisplay();
        }

        private void PrintColor_Changed(object sender, RoutedEventArgs e)
        {
            printColor = PrintColor.IsChecked == true ? true : false;
            OrderDisplay();
        }

        private void PrintDoubleSide_Changed(object sender, RoutedEventArgs e)
        {
            printDoubleSide = PrintDoubleSide.IsChecked == true ? true : false;
            OrderDisplay();
        }

        private void PrintUV_Changed(object sender, RoutedEventArgs e)
        {
            printUV = PrintUV.IsChecked == true ? true : false;
            OrderDisplay();
        }

        private void RealizationPriority_Checked(object sender, RoutedEventArgs e)
        {
            realizationPriority = (sender as RadioButton).Content.ToString() == "Standard" ? false : true;
            OrderDisplay();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Zamówienie przyjęto do realizacji");

            quantity = 0;
            paperColor = false;
            paperColorName = "";
            paperFormat = PaperFormat.A5;
            grammage = "80g/m^2";
            printColor = false;
            printDoubleSide = false;
            printUV = false;
            realizationPriority = false;

            QuantityTextBox.Text = OrderDisplayTextBox.Text = "";
            FormatSlider.Value = 0;
            PrintColor.IsChecked = PrintDoubleSide.IsChecked = PrintUV.IsChecked = ColoredPaperCheckBox.IsChecked = false;
            ColorComboBox.SelectedIndex = -1;

            DefaultRadiobutton.IsChecked = true;
            DefaultRadiobutton2.IsChecked = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
