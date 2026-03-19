using Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Services;
using Domain.Class;
using Domain.Enum;
using Domain.Interface;
using System.Security.Cryptography.X509Certificates;
using Volkswagen_Automobili.Pages;

namespace Volkswagen_Automobili
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string userTB=loginTB.Text.Trim();
            string passwordTB=passwTB.Password.Trim();

            LoginService loginService = new LoginService();
            bool IsLoggedIn;
            UserEnum userenum;
            (IsLoggedIn, userenum) = loginService.LoggingIn(userTB, passwordTB);


            if (IsLoggedIn == true)
            {
                MessageBox.Show("You have successfully logged in!","Attempt Successful");
                TabelaWindow tabelaWindow = new TabelaWindow(userenum);
                tabelaWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Username or password are incorrect.", "Failed to log in!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            loginTB.Text = "";
            passwTB.Password = "";
        }









    //---------------------------------------------------------promena jezika
        private void eng_click(object sender, MouseButtonEventArgs e)
        {
            loginLB.Content = "User:";
            passwordLB.Content= "Password:";
            titleLB.Content= "Volkswagen Database";
            login_button.Content = "Login";
            exit_button.Content = "Exit";

        }
        private void ger_click(object sender, MouseButtonEventArgs e)
        {
            loginLB.Content = "Benutzer:";
            passwordLB.Content = "Passwort:";
            titleLB.Content = "Volkswagen Datenbank";
            login_button.Content = "Einloggen";
            exit_button.Content = "Ausfahrt";
        }
        private void srb_click(object sender, MouseButtonEventArgs e)
        {
            loginLB.Content = "Korisnik:";
            passwordLB.Content = "Lozinka:";
            titleLB.Content = "Volkswagen Baza Podataka";
            login_button.Content = "Prijava";
            exit_button.Content = "Izadji";
        }
    }
}