using System;
using System.Windows;
using MySql.Data.MySqlClient;
using static PlanningSup.Vues.DepartementWindow;

namespace PlanningSup.Vues
{
    public partial class AjouterFiliereWindow : Window
    {
        private readonly DepartementItem departement;
        private readonly string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";

        public AjouterFiliereWindow(DepartementItem departement)
        {
            InitializeComponent();
            this.departement = departement;
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

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            string nomFiliere = txtNomFiliere.Text;

            if (!int.TryParse(txtNombreNiveaux.Text, out int nombreNiveaux) || nombreNiveaux <= 0)
            {
                MessageBox.Show("Veuillez entrer un nombre valide et supérieur à zéro pour le nombre de niveaux.");
                return;
            }

            int departementId = GetDepartementId(departement.Nom);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO FiliereSet (Nom, DepartementId) VALUES (@Nom, @DepartementId); SELECT LAST_INSERT_ID();";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nom", nomFiliere);
                    command.Parameters.AddWithValue("@DepartementId", departementId);

                    // Exécution de la requête pour ajouter la filière
                    int filiereId = Convert.ToInt32(command.ExecuteScalar());

                    for (int i = 1; i <= nombreNiveaux; i++)
                    {
                        string libelleNiveau = "Niveau " + i;
                        query = "INSERT INTO NiveauSet (Libelle, FiliereId) VALUES (@Libelle, @FiliereId);";
                        command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Libelle", libelleNiveau);
                        command.Parameters.AddWithValue("@FiliereId", filiereId);

                        // Exécution de la requête pour ajouter le niveau
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("La filière a été ajoutée avec succès.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout de la filière : " + ex.Message);
            }
        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}