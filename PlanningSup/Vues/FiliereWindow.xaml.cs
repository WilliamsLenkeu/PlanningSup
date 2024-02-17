using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
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

        private void Filiere_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer la filière sélectionnée
            FiliereItem filiere = (sender as Button)?.DataContext as FiliereItem;

            if (filiere != null)
            {
                // Appliquer l'effet de flou à la fenêtre parente
                blurEffect = new BlurEffect();
                blurEffect.Radius = 10;
                this.Effect = blurEffect;

                // Ouvrir une nouvelle fenêtre pour afficher les informations de la filière
                AfficherFiliereWindow afficherFiliereWindow = new AfficherFiliereWindow(filiere);
                afficherFiliereWindow.Closed += (s, args) =>
                {
                    // Supprimer l'effet de flou de la fenêtre parente après la fermeture de la nouvelle fenêtre
                    this.Effect = null;
                };
                afficherFiliereWindow.ShowDialog();
            }
        }

        public class FiliereItem
        {
            public string Nom { get; set; }
        }
    }
}