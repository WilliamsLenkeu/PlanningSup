using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects; // Ajout de l'espace de noms pour les effets visuels

namespace PlanningSup.Vues
{
    public partial class DepartementWindow : Window
    {
        public ObservableCollection<DepartementItem> Departements { get; set; }
        private readonly string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";

        private FaculteItem Faculte { get; set; }
        private BlurEffect blurEffect; // Effet de flou

        public DepartementWindow(FaculteItem faculte)
        {
            InitializeComponent();

            Faculte = faculte;
            Departements = new ObservableCollection<DepartementItem>();
            DataContext = this;

            int faculteId = GetFaculteId(faculte.Nom);
            GetDepartements(faculteId);

            // Initialisation de l'effet de flou
            blurEffect = new BlurEffect();
        }

        private int GetFaculteId(string nomFaculte)
        {
            int faculteId = -1;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT Id FROM FaculteSet WHERE Nom = @Nom";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nom", nomFaculte);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        faculteId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération de l'ID de la faculté : " + ex.Message);
            }
            return faculteId;
        }

        private void GetDepartements(int faculteId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT Nom FROM DepartementSet WHERE FaculteId = @FaculteId";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FaculteId", faculteId);

                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nom = reader["Nom"].ToString();
                            Departements.Add(new DepartementItem { Nom = nom });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des départements : " + ex.Message);
            }
        }

        private void Departement_Click(object sender, RoutedEventArgs e)
        {
            DepartementItem departement = (sender as Button)?.DataContext as DepartementItem;

            if (departement != null)
            {
                // Ouvrir une nouvelle fenêtre affichant les filières du département sélectionné
                FiliereWindow filiereWindow = new FiliereWindow(departement);

                // Appliquer un effet de flou à la fenêtre précédente
                blurEffect.Radius = 10;
                Effect = blurEffect;

                filiereWindow.ShowDialog();

                // Réinitialiser l'effet de flou après la fermeture de la fenêtre modale
                Effect = null;
            }
        }

        private void AjouterDepartement_Click(object sender, RoutedEventArgs e)
        {
            // Créer une instance de la fenêtre d'ajout de département et lui passer la faculté sélectionnée en paramètre
            AjoutDepartementWindow ajoutDepartementWindow = new AjoutDepartementWindow(Faculte.Nom);

            // Appliquer un effet de flou à la fenêtre précédente
            blurEffect.Radius = 10;
            Effect = blurEffect;

            ajoutDepartementWindow.ShowDialog();

            // Réinitialiser l'effet de flou après la fermeture de la fenêtre modale
            Effect = null;

            // Recharger la liste des départements après l'ajout
            Departements.Clear();
            GetDepartements(GetFaculteId(Faculte.Nom));
        }

        public class DepartementItem
        {
            public string Nom { get; set; }
        }
    }
}