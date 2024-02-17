using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PlanningSup.Vues
{
    public partial class DetailsCoursWindow : Window
    {
        public CoursPropose Cours { get; set; }

        private string matE;

        private int cid;
        private int ueId; // Variable pour stocker l'ID de l'UE

        // Chaîne de connexion à la base de données
        private string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";

        public DetailsCoursWindow(CoursPropose cours)
        {
            InitializeComponent();
            DataContext = cours;
            Cours = cours;
            AfficherMatriculeEnseignant();
        }

        private void AfficherMatriculeEnseignant()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Récupérer le matricule de l'enseignant
                    string matriculeQuery = @"SELECT ue.Matricule 
                                    FROM UtilisateurSet_Enseignant ue 
                                    INNER JOIN UtilisateurSet u ON ue.Matricule = u.Matricule 
                                    WHERE u.Nom = @Nom;";
                    MySqlCommand matriculeCommand = new MySqlCommand(matriculeQuery, connection);
                    matriculeCommand.Parameters.AddWithValue("@Nom", Cours.NomEnseignant);
                    string matricule = matriculeCommand.ExecuteScalar()?.ToString();
                    matE = matricule;

                    if (!string.IsNullOrEmpty(matricule))
                    {
                        lblMatricule.Content = $"Matricule de l'enseignant : {matricule}";
                    }
                    else
                    {
                        lblMatricule.Content = "Matricule de l'enseignant non trouvé.";
                    }

                    // Récupérer l'ID de l'UE à partir du code de l'UE
                    string ueIdQuery = "SELECT Id FROM UESet WHERE Code = @CodeUE;";
                    MySqlCommand ueIdCommand = new MySqlCommand(ueIdQuery, connection);
                    ueIdCommand.Parameters.AddWithValue("@CodeUE", Cours.CodeUE);
                    ueId = Convert.ToInt32(ueIdCommand.ExecuteScalar());

                    if (ueId <= 0)
                    {
                        MessageBox.Show("Impossible de trouver l'ID de l'UE.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération du matricule de l'enseignant : " + ex.Message);
            }
        }

        private void ValiderLeCours_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    if (!string.IsNullOrEmpty(matE))
                    {
                        // Mettre à jour le champ valide du cours dans CoursSetTemp en utilisant l'ID du cours récupéré
                        string updateQuery = "UPDATE CoursSetTemp SET valide = 1 WHERE Id = @CoursId;";
                        MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@CoursId", cid);
                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cours validé avec succès.");
                        }
                        else
                        {
                            MessageBox.Show("Aucune ligne affectée lors de la validation du cours.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Impossible de trouver le matricule de l'enseignant.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la validation du cours : " + ex.Message);
            }
        }
    }
}