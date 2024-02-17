using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using MySql.Data.MySqlClient;

namespace PlanningSup.Vues
{
    /// <summary>
    /// Logique d'interaction pour Faculte.xaml
    /// </summary>
    public partial class Faculte : UserControl
    {
        private readonly BlurEffect blurEffect = new BlurEffect();
        private readonly Window mainWindow;

        // Déclaration d'une ObservableCollection pour stocker les facultés
        public ObservableCollection<FaculteItem> Facultes { get; set; }

        // Chaîne de connexion à la base de données
        private string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";

        public Faculte(Window mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow; // Initialisation de la variable mainWindow

            // Initialisation de la liste des facultés
            Facultes = new ObservableCollection<FaculteItem>();

            // Récupérer les données des facultés depuis la base de données et les ajouter à la liste
            GetFacultesFromDatabase();

            // Liaison des données avec le ListBox
            FacultesListBox.ItemsSource = Facultes;
        }

        // Méthode pour récupérer les facultés depuis la base de données
        private void GetFacultesFromDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT Nom FROM FaculteSet";

                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nom = reader["Nom"].ToString();
                            Facultes.Add(new FaculteItem { Nom = nom });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la récupération des données des facultés : " + ex.Message);
                }
            }
        }

        // Méthode exécutée lors du clic sur "Créer Faculté"
        private void CreateFaculte_Click(object sender, RoutedEventArgs e)
        {
            // Exécuter la méthode définie pour créer une faculté
            CreateFaculte();
        }

        // Méthode pour créer une faculté
        private void CreateFaculte()
        {
            // Instancier la fenêtre formulaireFaculte
            formulaireFaculte formulaireFaculte = new formulaireFaculte();

            // Flouter le mainWindow
            blurEffect.Radius = 10;
            mainWindow.Effect = blurEffect;

            // Afficher la fenêtre formulaireFaculte de manière modale
            formulaireFaculte.Owner = mainWindow;
            formulaireFaculte.Closed += (sender, eventArgs) =>
            {
                // Après la fermeture de la fenêtre formulaireFaculte, supprimer l'effet de flou du mainWindow
                mainWindow.Effect = null;
            };
            formulaireFaculte.ShowDialog();
        }

        // Méthode exécutée lors du clic sur une faculté
        private void Faculte_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer la faculté sélectionnée
            FaculteItem faculte = (sender as Button)?.DataContext as FaculteItem;

            if (faculte != null)
            {
                // Créer une instance de la fenêtre DepartementWindow en passant la faculté sélectionnée en paramètre
                DepartementWindow departementWindow = new DepartementWindow(faculte);

                // Flouter le mainWindow
                blurEffect.Radius = 10;
                mainWindow.Effect = blurEffect;

                // Afficher la fenêtre DepartementWindow de manière modale
                departementWindow.Owner = mainWindow;
                departementWindow.Closed += (closedSender, eventArgs) =>
                {
                    // Après la fermeture de la fenêtre des départements, supprimer l'effet de flou du mainWindow
                    mainWindow.Effect = null;
                };
                departementWindow.ShowDialog();
            }
        }
    }

    // Modèle pour représenter une faculté
    public class FaculteItem
    {
        public string Nom { get; set; }
    }
}