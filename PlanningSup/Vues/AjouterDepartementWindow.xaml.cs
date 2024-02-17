using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace PlanningSup.Vues
{
    public partial class AjoutDepartementWindow : Window
    {
        // Événement déclenché lorsqu'un département est ajouté
        public event EventHandler<string> DepartementAjoute;

        // Chaîne de connexion à la base de données
        private string connectionString;

        // Nom de la faculté associée
        private string nomFaculte;

        public AjoutDepartementWindow(string nomFaculte)
        {
            InitializeComponent();
            this.nomFaculte = nomFaculte;
            connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";
        }

        // Méthode exécutée lors du clic sur le bouton Ajouter
        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier si le champ de texte n'est pas vide
            if (!string.IsNullOrWhiteSpace(DepartementTextBox.Text))
            {
                // Récupérer l'ID de la faculté à partir de son nom
                int faculteId = GetFaculteId(nomFaculte);

                // Ajouter le département à la base de données
                AjouterDepartement(DepartementTextBox.Text, faculteId);

                // Déclencher l'événement de département ajouté avec le nom du département
                DepartementAjoute?.Invoke(this, DepartementTextBox.Text);

                // Fermer la fenêtre
                Close();
            }
            else
            {
                MessageBox.Show("Veuillez entrer un nom de département.");
            }
        }

        // Méthode pour récupérer l'ID de la faculté à partir de son nom
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

        // Méthode pour ajouter un département à la base de données
        private void AjouterDepartement(string nomDepartement, int faculteId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "INSERT INTO DepartementSet (Nom, FaculteId) VALUES (@Nom, @FaculteId)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nom", nomDepartement);
                    command.Parameters.AddWithValue("@FaculteId", faculteId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout du département : " + ex.Message);
            }
        }
    }
}