using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using MySql.Data.MySqlClient;

namespace PlanningSup.Vues
{
    public partial class ueView : UserControl
    {
        // Collection pour stocker les UE
        public ObservableCollection<UEItem> UEItems { get; set; }
        private readonly BlurEffect blurEffect = new BlurEffect();
        private readonly Window mainWindow;

        // Chaîne de connexion à la base de données
        private string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";

        public ueView()
        {
            InitializeComponent();
            DataContext = this;

            // Initialisation de la collection des UE
            UEItems = new ObservableCollection<UEItem>();

            // Récupération des UE depuis la base de données
            GetUEFromDatabase();

            // Assignation de la fenêtre principale
            mainWindow = Application.Current.MainWindow;
        }

        // Méthode pour récupérer les UE depuis la base de données
        private void GetUEFromDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT Libelle, Code FROM UESet";

                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string libelle = reader["Libelle"].ToString();
                            string code = reader["Code"].ToString();
                            UEItems.Add(new UEItem { Libelle = libelle, Code = code });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la récupération des données des UE : " + ex.Message);
                }
            }
        }

        // Méthode exécutée lors du clic sur "Ajouter UE"
        private void AjouterUE_Click(object sender, RoutedEventArgs e)
        {
            // Exécuter la méthode définie pour ajouter une UE
            AjouterUE();
        }

        // Méthode pour ajouter une UE
        private void AjouterUE()
        {
            // Création de la nouvelle fenêtre
            AjouterUEWindow ajouterUEWindow = new AjouterUEWindow();

            // Rendre la fenêtre principale floue
            blurEffect.Radius = 10;
            mainWindow.Effect = blurEffect;

            ajouterUEWindow.Owner = mainWindow;
            ajouterUEWindow.Closed += (closedSender, eventArgs) =>
            {
                // Après la fermeture de la fenêtre des départements, supprimer l'effet de flou du mainWindow
                mainWindow.Effect = null;
            };
            ajouterUEWindow.ShowDialog();
        }
    }

    // Modèle pour représenter une UE
    public class UEItem
    {
        public string Libelle { get; set; }
        public string Code { get; set; }
    }
}