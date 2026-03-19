using Domain.Class;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;       // for System.Drawing.Color
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media; // for SolidColorBrush
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;
using static System.Windows.Forms;

namespace Volkswagen_Automobili.Pages
{
    /// <summary>
    /// Interaction logic for AddVehicle.xaml
    /// </summary>
    public partial class AddVehicle : Window
    {
        public AddVehicle()
        {
            InitializeComponent();
            // Učitaj fontove u ComboBox sa preview-om
            foreach (var font in Fonts.SystemFontFamilies.OrderBy(f => f.Source))
            {
                var item = new ComboBoxItem();
                item.Content = font.Source;
                item.FontFamily = font;
                cmbFontFamily.Items.Add(item);
            }
        }
        //=========================================OSNOVNO=====================================================

        private void Cancel_button_Click(object sender, RoutedEventArgs e)
        {
            TabelaWindow tabelaWindow = new TabelaWindow(UserEnum.Admin);
            tabelaWindow.Show();
            this.Close();
        }
        public void SaveToXML(VWautomobili auto)    // za cuvanje tabele
        {
            string path = "automobili.xml";
            List<VWautomobili> lista = new List<VWautomobili>();

            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<VWautomobili>));
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    lista = (List<VWautomobili>)serializer.Deserialize(fs);
                }
            }

            lista.Add(auto);

            XmlSerializer saveSerializer = new XmlSerializer(typeof(List<VWautomobili>));
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                saveSerializer.Serialize(fs, lista);
            }
        }

        private void ADDfinalize_button_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                errorName.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                errorName.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrWhiteSpace(textBoxSlika.Text) || textBoxSlika.Text=="Browse files...")
            {
                errorSlika.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                errorSlika.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrWhiteSpace(textBoxCena.Text))
            {
                errorCena.Visibility = Visibility.Visible;
                hasError = true;
            }
            else 
            {
                errorCena.Visibility = Visibility.Collapsed;
            }
        //errorRTB
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            if (string.IsNullOrWhiteSpace(textRange.Text) || textRange.Text == "\r\n") // extra check for empty paragraph
            {
                errorRTB.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                errorRTB.Visibility = Visibility.Collapsed;
            }
            if (hasError)
                return;

            string rtfPath = $"RTFs\\{textBoxName.Text}_{DateTime.Now.Ticks}.rtf";
            Directory.CreateDirectory("RTFs");

            TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            using (FileStream fs = new FileStream(rtfPath, FileMode.Create))
            {
                range.Save(fs, DataFormats.Rtf);
            }

            VWautomobili auto = new VWautomobili()
            {
                name = textBoxName.Text,
                slikaPath = textBoxSlika.Text,
                RTFpath = rtfPath,
                cena = int.Parse(textBoxCena.Text),
                date = DateTime.Now.ToString("dd.MM.yyyy.")
            };

            SaveToXML(auto);
            MessageBox.Show("Vehicle added!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void imagePathTextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            errorSlika.Content = string.Empty;
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                textBoxSlika.Text = openFileDialog.FileName;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void ToggleFormatting(DependencyProperty formattingProperty, object expectedValue)
        {
            object current = richTextBox.Selection.GetPropertyValue(formattingProperty);
            if (current == DependencyProperty.UnsetValue || !current.Equals(expectedValue))
                richTextBox.Selection.ApplyPropertyValue(formattingProperty, expectedValue);
            else
                richTextBox.Selection.ApplyPropertyValue(formattingProperty, DependencyProperty.UnsetValue);
        }

        //=======================BOLDIRANJE I ITALICOVANJE=============================
        private void btnBold_Checked(object sender, RoutedEventArgs e)
        {
            ToggleBold();
        }

        private void btnItalic_Click(object sender, RoutedEventArgs e)
        {
            ToggleItalic();
        }
        private void ToggleBold()
        {
            TextSelection selection = richTextBox.Selection;
            object current = selection.GetPropertyValue(TextElement.FontWeightProperty);

            if (current != DependencyProperty.UnsetValue && current.Equals(FontWeights.Bold))
                selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            else
                selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }

        private void ToggleItalic()
        {
            TextSelection selection = richTextBox.Selection;
            object current = selection.GetPropertyValue(TextElement.FontStyleProperty);

            if (current != DependencyProperty.UnsetValue && current.Equals(FontStyles.Italic))
                selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            else
                selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
        }

        private void btnUnderline_Checked(object sender, RoutedEventArgs e)
        {
            TextSelection selection = richTextBox.Selection;
            if (selection != null)
            {
                TextDecorationCollection current = (TextDecorationCollection)selection.GetPropertyValue(Inline.TextDecorationsProperty);
                if (current == TextDecorations.Underline)
                    selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                else
                    selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
            {
                FontFamily selectedFont = (FontFamily)cmbFontFamily.SelectedItem;
                richTextBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, selectedFont);
            }
        }

        private void cmbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontSize.SelectedItem is ComboBoxItem item && double.TryParse(item.Content.ToString(), out double size))
            {
                richTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, size);
            }
        }

        private void colorButton_Click(object sender, RoutedEventArgs e)
        {
            //prazno
        }

        private void richTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            string plainText = textRange.Text;
            string[] words = plainText.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int wordCount = words.Length;
            wordCountText.Text = $"Reči: {wordCount}";
        }

//==============================BRISANJE ERROR LABELA SA DODAVANJA VOZILA==========================================
        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            errorName.Visibility = string.IsNullOrWhiteSpace(textBoxName.Text)
            ? Visibility.Visible
            : Visibility.Collapsed;
        }

        private void textBoxCena_TextChanged(object sender, TextChangedEventArgs e)
        {
            errorCena.Visibility = string.IsNullOrWhiteSpace(textBoxCena.Text)
            ? Visibility.Visible
            : Visibility.Collapsed;
        }
    }
}
