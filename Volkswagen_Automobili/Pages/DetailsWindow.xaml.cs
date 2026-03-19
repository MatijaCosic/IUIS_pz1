using Domain.Class;
using Domain.Enum;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Volkswagen_Automobili.Pages
{
    public partial class DetailsWindow : Window
    {
        public ObservableCollection<VWautomobili> Automobili { get; set; }
        private VWautomobili automobil;
        private UserEnum currentUser;
        public DataIO serializer = new DataIO();

        public DetailsWindow(VWautomobili auto, UserEnum user, ObservableCollection<VWautomobili> automobiliCollection)
        {
            InitializeComponent();
            automobil = auto;
            currentUser = user;
            Automobili = automobiliCollection ?? throw new ArgumentNullException(nameof(automobiliCollection));

            // Load data into UI
            nameTextBox.Text = automobil.name;
            priceTextBox.Text = "$" + automobil.cena;
            dateTextBox.Text=automobil.date;

            if (File.Exists(automobil.slikaPath))
            {
                vehicleImage.Source = new BitmapImage(new Uri(automobil.slikaPath, UriKind.RelativeOrAbsolute));
            }
            else
            {       // dodato da se izbegne greška ako slika ne postoji, i da se prikaže prazno polje umesto greške
                    // imao sam problem sa tim jer se slika ne nalazi u projektu, već je korisnik dodaje sa svog računara,
                    // pa ako se slika obriše ili premesti, dođe do greške
                vehicleImage.Source = automobil.slikaPath != null ? new BitmapImage(new Uri(automobil.slikaPath, UriKind.RelativeOrAbsolute)) : null;
            }

            if (!string.IsNullOrWhiteSpace(automobil.RTFpath) && File.Exists(automobil.RTFpath))
            {
                TextRange textRange = new TextRange(rtfViewer.Document.ContentStart, rtfViewer.Document.ContentEnd);
                using (FileStream fs = new FileStream(automobil.RTFpath, FileMode.Open))
                {
                    textRange.Load(fs, DataFormats.Rtf);
                }
            }

            // Set controls to read-only if user is not admin
            if (currentUser == UserEnum.User)
            {
                nameTextBox.IsReadOnly = true;
                priceTextBox.IsReadOnly = true;
                rtfViewer.IsReadOnly = true;
                SacuvajBT.Visibility = Visibility.Collapsed;
                ChangeImageBT.Visibility = Visibility.Collapsed;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                automobil.name = nameTextBox.Text;

                if (int.TryParse(priceTextBox.Text.TrimStart('$'), out int cena))
                {
                    automobil.cena = cena;
                }
                else
                {
                    MessageBox.Show("Cena mora biti validan broj.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create RTF directory if needed
                if (string.IsNullOrWhiteSpace(automobil.RTFpath))
                {
                    Directory.CreateDirectory("RTFs");
                    automobil.RTFpath = $"RTFs\\{automobil.name}_{DateTime.Now.Ticks}.rtf";
                }

                // Save RTF content to file
                TextRange range = new TextRange(rtfViewer.Document.ContentStart, rtfViewer.Document.ContentEnd);
                using (FileStream fs = new FileStream(automobil.RTFpath, FileMode.Create))
                {
                    range.Save(fs, DataFormats.Rtf);
                }

                // Update the object inside the ObservableCollection
                var existing = Automobili.IndexOf(automobil);
                if (existing >= 0)
                {
                    Automobili[existing] = automobil; // trigger collection changed
                }

                // Save whole collection to XML
                serializer.SerializeObject(Automobili, "automobili.xml");

                MessageBox.Show("Podaci su sačuvani.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Došlo je do greške pri čuvanju podataka: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LeaveBT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void changeIMG_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                automobil.slikaPath = dialog.FileName;
                vehicleImage.Source = new BitmapImage(new Uri(dialog.FileName));
            }
        }
    }
}
