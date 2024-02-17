using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace PlanningSup.Vues
{
    public partial class DepartementWindow : Window
    {
        public ObservableCollection<DepartementItem> Departements { get; set; }
        private readonly string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";

        public DepartementWindow(FaculteItem faculte)
        {
            InitializeComponent();

            Departements = new ObservableCollection<DepartementItem>();
            DataContext = this;

            // Récupérer l'ID de la faculté à partir de son nom
            int faculteId = GetFaculteId(faculte.Nom);

            // Récupérer les départements liés à la faculté sélectionnée
            GetDepartements(faculteId);
        }

        private int GetFaculteId(string nomFaculte)
        {
            int faculteId = -1; // Valeur par défaut si aucune faculté trouvée
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

        private void AjouterDepartement_Click(object sender, RoutedEventArgs e)
        {
            // Implémentez le code pour ajouter un département
        }

        // Modèle pour représenter un département
        public class DepartementItem
        {
            public string Nom { get; set; }
        }
    }
}