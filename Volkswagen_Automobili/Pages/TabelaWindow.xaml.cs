using Domain.Class;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Shapes;
using System.Xml.Serialization;
using Volkswagen_Automobili.Pages;
using System.Diagnostics;
using System.Windows.Navigation;
using Domain.Class;

namespace Volkswagen_Automobili.Pages
{
    /// <summary>
    /// Interaction logic for TabelaWindow.xaml
    /// </summary>
    public partial class TabelaWindow : Window
    {

        public ObservableCollection<VWautomobili> Automobili { get; set; }
        private UserEnum constUEnum;
        public DataIO serializer = new DataIO();

        public TabelaWindow(UserEnum userenum)
        {
            InitializeComponent();
            Automobili = serializer.DeSerializeObject<ObservableCollection<VWautomobili>>("automobili.xml");
            if (Automobili == null)
            {
                Automobili=new ObservableCollection<VWautomobili>();
            }
            constUEnum = userenum;

            if (userenum == UserEnum.User)
            {
                delete_button.Visibility = Visibility.Hidden;
                add_button.Visibility = Visibility.Hidden;
                delete_button.IsEnabled = false;
                add_button.IsEnabled = false;
            }
                DataContext = this;
        }

        

        private void logout_button_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to log out?", "Logging out", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                this.Close();

        }

        
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            if (Automobili.Count > 0)
            {
                var selectedItems = Automobili.Where(a => a.IsSelected).ToList();
                if (selectedItems.Count == 0)
                {
                    MessageBox.Show("No vehicles were selected.","Failed!",MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to delete the selected vehicle/s?", "Confirmation needed", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    foreach (var item in selectedItems)
                    {
                        Automobili.Remove(item);
                    }
                }
                serializer.SerializeObject<ObservableCollection<VWautomobili>>(Automobili, "automobili.xml");
            }
            else
            {
                MessageBox.Show("Table is empty!");
                serializer.SerializeObject<ObservableCollection<VWautomobili>>(Automobili, "automobili.xml");
            }
        }

        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddVehicle addVehicle = new AddVehicle();
                addVehicle.Show();
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("There's been an issue adding a new vehicle."); }
        }

        private void SelectAllBoxes_Click(object sender, RoutedEventArgs e)
        {
            if (Automobili == null) return;

            bool newState = (sender as CheckBox)?.IsChecked == true;

            foreach (var item in Automobili)
            {
                item.IsSelected = newState;
            }
            
        }

        private void HyperLink_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink link && link.DataContext is VWautomobili auto)
            {
                DetailsWindow dw = new DetailsWindow(auto, constUEnum, this.Automobili);
                dw.ShowDialog();
            }
        }
    }
}
