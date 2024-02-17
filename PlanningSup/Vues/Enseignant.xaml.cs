using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using MySql.Data.MySqlClient;

namespace PlanningSup.Vues
{
    public partial class Enseignant : UserControl
    {
        private readonly BlurEffect blurEffect = new BlurEffect();
        private readonly Window mainWindow;

        public ObservableCollection<EnseignantItem> Enseignants { get; set; }

        public Enseignant(Window mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow; // Initialisation de la variable mainWindow
            Enseignants = new ObservableCollection<EnseignantItem>();
            LoadEnseignants();
            DataContext = this;
        }

        private void LoadEnseignants()
        {
            // Connexion à la base de données et récupération des enseignants
            string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";
            string query = "SELECT Matricule, Nom, Email FROM UtilisateurSet WHERE Role = 'Enseignant'";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        int index = 1;
                        while (reader.Read())
                        {
                            Enseignants.Add(new EnseignantItem
                            {
                                DisplayIndex = index,
                                Matricule = reader["Matricule"].ToString(),
                                Nom = reader["Nom"].ToString(),
                                Email = reader["Email"].ToString(),
                            });
                            index++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors du chargement des enseignants: " + ex.Message);
                }
            }
        }

        private void OpenTeacherDetails_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer l'enseignant associé au bouton "Plus..."
            var enseignant = ((Button)sender).DataContext as EnseignantItem;
            // Appliquer l'effet de flou à la fenêtre principale
            ApplyBlurEffect();
            // Ouvrir la fenêtre des détails de l'enseignant en passant le matricule
            TeacherDetailsWindow detailsWindow = new TeacherDetailsWindow(enseignant.Matricule);
            detailsWindow.Closed += DetailsWindow_Closed;
            detailsWindow.ShowDialog();
        }

        private void ApplyBlurEffect()
        {
            // Appliquer l'effet de flou à la fenêtre principale
            blurEffect.Radius = 10;
            mainWindow.Effect = blurEffect;
        }

        private void RemoveBlurEffect()
        {
            // Supprimer l'effet de flou de la fenêtre principale
            mainWindow.Effect = null;
        }

        private void DetailsWindow_Closed(object sender, EventArgs e)
        {
            // Désactiver l'effet de flou une fois que la fenêtre de détails est fermée
            RemoveBlurEffect();
        }
    }

    public class EnseignantItem
    {
        public int DisplayIndex { get; set; }
        public string Matricule { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
    }
}