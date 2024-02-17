using System;
using System.Windows;
using MySql.Data.MySqlClient;

namespace PlanningSup.Vues
{
    /// <summary>
    /// Logique d'interaction pour formulaireFaculte.xaml
    /// </summary>
    public partial class formulaireFaculte : Window
    {
        private string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";

        public string NomFaculte { get; private set; }

        public formulaireFaculte()
        {
            InitializeComponent();
        }

        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer le nom de la faculté depuis le TextBox
            NomFaculte = NomFaculteTextBox.Text;

            // Vérifier si le nom de la faculté est vide
            if (string.IsNullOrWhiteSpace(NomFaculte))
            {
                MessageBox.Show("Veuillez saisir un nom de faculté.");
                return;
            }

            // Enregistrer la faculté dans la base de données
            EnregistrerFaculteDansBaseDeDonnees(NomFaculte);

            // Fermer la fenêtre après l'enregistrement
            DialogResult = true;
        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            // Fermer la fenêtre sans enregistrer
            DialogResult = false;
        }

        private void EnregistrerFaculteDansBaseDeDonnees(string nomFaculte)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO FaculteSet (Nom) VALUES (@Nom)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nom", nomFaculte);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Faculté enregistrée avec succès dans la base de données.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de l'enregistrement de la faculté dans la base de données : " + ex.Message);
                }
            }
        }
    }
}