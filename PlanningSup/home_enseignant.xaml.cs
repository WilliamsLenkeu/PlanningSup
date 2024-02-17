using MySql.Data.MySqlClient;
using System;
using PlanningSup.Vues;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlanningSup
{
    /// <summary>
    /// Logique d'interaction pour home_enseignant.xaml
    /// </summary>
    public partial class home_enseignant : Window
    {
        private string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";

        private string matTmp;

        public home_enseignant(string mat)
        {
            InitializeComponent();

            string matricule = mat;
            matTmp = mat;
            // Mettre à jour le message de bienvenue avec le nom de l'utilisateur
            string nomUtilisateur = ObtenirNomUtilisateur(matricule);
            MessageBienvenue.Text = "Bienvenue, " + nomUtilisateur;
        }

        public string ObtenirNomUtilisateur(string matricule)
        {
            string nomUtilisateur = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT Nom FROM UtilisateurSet WHERE Matricule = @Matricule";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Matricule", matricule);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            nomUtilisateur = result.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Gérer l'exception, par exemple en journalisant l'erreur
                        Console.WriteLine("Erreur lors de la récupération du nom de l'utilisateur : " + ex.Message);
                    }
                }
            }

            return nomUtilisateur;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            // Afficher le UserControl des filières où l'enseignant enseigne
            UserControlFiliere userControlFiliere = new UserControlFiliere(matTmp);
            bordure_Principale.Child = userControlFiliere;
        }

        private void Cours_Click(object sender, RoutedEventArgs e)
        {
            // Afficher le UserControl de la liste des cours sans enseignant
            UserControlCours userControlCours = new UserControlCours(matTmp);
            bordure_Principale.Child = userControlCours;
        }

        private void Deconnexion_Click(object sender, RoutedEventArgs e)
        {
            // Appeler la méthode HandleLogout de MainWindow en passant false
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.HandleLogout(false);
            }
            mainWindow = new MainWindow();
            mainWindow.Show();
            // Fermer la fenêtre actuelle
            this.Close();
        }
    }
}
