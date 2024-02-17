using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using PlanningSup.Vues;

namespace PlanningSup
{
    /// <summary>
    /// Logique d'interaction pour ProposerCoursWindow.xaml
    /// </summary>
    public partial class ProposerCoursWindow : Window
    {
        private UE ueSelectionnee;
        private string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";
        private string matricule;

        public ProposerCoursWindow(UE ue, string mat)
        {
            InitializeComponent();
            ueSelectionnee = ue;
            matricule = mat;

            // Récupérer l'ID de l'UE à partir du code de l'UE
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT Id, NiveauId FROM UESet WHERE Code = @CodeUE";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CodeUE", ueSelectionnee.Code);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        ueSelectionnee.Id = reader.GetInt32("Id");
                        ueSelectionnee.NiveauId = reader.GetInt32("NiveauId");
                    }
                    else
                    {
                        MessageBox.Show("Impossible de récupérer l'UE.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération de l'UE : " + ex.Message);
            }
        }

        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs sélectionnées par l'utilisateur
            string jour = cmbJour.Text;
            string heureDebut = cmbHeureDebut.Text;
            string heureFin = cmbHeureFin.Text;

            // Vérifier si les champs sont vides
            if (string.IsNullOrWhiteSpace(jour) || string.IsNullOrWhiteSpace(heureDebut) || string.IsNullOrWhiteSpace(heureFin))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            // Vérifier si les heures sélectionnées sont déjà prises par un cours valide
            if (HeuresDejaPrises(jour, heureDebut, heureFin))
            {
                MessageBox.Show("Les heures sélectionnées sont déjà prises par un cours valide.");
                return;
            }

            // Insérer les données dans la table CoursSetTemp
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    int enseignantId = GetEnseignantId(matricule);
                    string query = @"INSERT INTO CoursSetTemp (NiveauId, EnseignantId, Jour, Heure_debut, Heure_fin, valide, UE_Id, Enseignant_Matricule) 
                             VALUES (@NiveauId, @EnseignantId, @Jour, @HeureDebut, @HeureFin, @Valide, @UE_Id, @Enseignant_Matricule)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NiveauId", ueSelectionnee.NiveauId);
                    command.Parameters.AddWithValue("@EnseignantId", enseignantId);
                    command.Parameters.AddWithValue("@Jour", jour);
                    command.Parameters.AddWithValue("@HeureDebut", heureDebut);
                    command.Parameters.AddWithValue("@HeureFin", heureFin);
                    command.Parameters.AddWithValue("@Valide", false); // Valeur par défaut : false
                    command.Parameters.AddWithValue("@UE_Id", ueSelectionnee.Id); // Utiliser l'ID de l'UE récupéré
                    command.Parameters.AddWithValue("@Enseignant_Matricule", matricule);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cours proposé avec succès.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la proposition du cours. Veuillez réessayer.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la proposition du cours : " + ex.Message);
            }
        }

        private bool HeuresDejaPrises(string jour, string heureDebut, string heureFin)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT COUNT(*) FROM CoursSetTemp 
                             WHERE Jour = @Jour AND valide = 0 
                             AND ((Heure_debut BETWEEN @HeureDebut AND @HeureFin) 
                             OR (Heure_fin BETWEEN @HeureDebut AND @HeureFin))";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Jour", jour);
                    command.Parameters.AddWithValue("@HeureDebut", heureDebut);
                    command.Parameters.AddWithValue("@HeureFin", heureFin);

                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la vérification des heures : " + ex.Message);
                return true; // Considérer que les heures sont déjà prises en cas d'erreur
            }
        }

        public int GetEnseignantId(string matricule)
        {
            int enseignantId = -1; // Valeur par défaut au cas où rien ne serait trouvé

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT Id FROM UtilisateurSet_Enseignant WHERE Matricule = @Matricule";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Matricule", matricule);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out enseignantId))
                    {
                        // L'ID de l'enseignant a été récupéré avec succès
                    }
                    else
                    {
                        MessageBox.Show("Aucun enseignant trouvé avec ce matricule.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération de l'ID de l'enseignant : " + ex.Message);
            }

            return enseignantId;
        }
    }
}