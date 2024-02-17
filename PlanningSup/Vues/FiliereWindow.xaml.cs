using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Effects; // Ajout de l'espace de noms pour les effets de rendu
using MySql.Data.MySqlClient;
using static PlanningSup.Vues.DepartementWindow;

namespace PlanningSup.Vues
{
    public partial class FiliereWindow : Window
    {
        public ObservableCollection<FiliereItem> Filieres { get; set; }
        private readonly string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";
        private BlurEffect blurEffect; // Effet de flou
        public DepartementItem DepartementLam;

        public FiliereWindow(DepartementItem departement)
        {
            InitializeComponent();
            DepartementLam = departement;

            Filieres = new ObservableCollection<FiliereItem>();
            DataContext = this;

            int departementId = GetDepartementId(departement.Nom);
            GetFilieres(departementId);
        }

        private int GetDepartementId(string nomDepartement)
        {
            int departementId = -1;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT Id FROM DepartementSet WHERE Nom = @Nom";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nom", nomDepartement);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        departementId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération de l'ID du département : " + ex.Message);
            }
            return departementId;
        }

        private void GetFilieres(int departementId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT Nom FROM FiliereSet WHERE DepartementId = @DepartementId";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@DepartementId", departementId);

                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nom = reader["Nom"].ToString();
                            Filieres.Add(new FiliereItem { Nom = nom });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des filières : " + ex.Message);
            }
        }

        private void AjouterFiliere_Click(object sender, RoutedEventArgs e)
        {
            // Créer une nouvelle fenêtre pour l'ajout de filière
            AjouterFiliereWindow ajouterFiliereWindow = new AjouterFiliereWindow(DepartementLam);

            // Appliquer l'effet de flou à la fenêtre principale
            blurEffect = new BlurEffect();
            blurEffect.Radius = 10;
            this.Effect = blurEffect;

            // Afficher la nouvelle fenêtre de manière modale
            ajouterFiliereWindow.Owner = this;
            ajouterFiliereWindow.Closed += AjouterFiliereWindow_Closed; // Gérer l'événement de fermeture de la fenêtre d'ajout de filière
            ajouterFiliereWindow.ShowDialog();
        }

        // Gérer l'événement de fermeture de la fenêtre d'ajout de filière
        private void AjouterFiliereWindow_Closed(object sender, EventArgs e)
        {
            // Supprimer l'effet de flou de la fenêtre principale
            this.Effect = null;
        }

        public class FiliereItem
        {
            public string Nom { get; set; }
        }
    }
}