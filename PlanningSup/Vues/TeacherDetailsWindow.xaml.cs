using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace PlanningSup.Vues
{
    /// <summary>
    /// Logique d'interaction pour TeacherDetailsWindow.xaml
    /// </summary>
    public partial class TeacherDetailsWindow : Window
    {
        // Propriété pour stocker la liste des niveaux où l'enseignant est affecté
        public List<NiveauInfo> NiveauInfoList { get; set; }

        // Champ pour stocker le matricule de l'enseignant
        private readonly string Matricule;

        // Constructeur prenant le view model de l'enseignant comme argument
        public TeacherDetailsWindow(string matricule)
        {
            InitializeComponent();
            Matricule = matricule; // Stocker le matricule
            // Utilisez le view model pour initialiser la fenêtre des détails de l'enseignant
            // Par exemple, récupérez les niveaux correspondants à partir de la base de données
            NiveauInfoList = GetNiveauInfoList(Matricule);
            DataContext = this; // Définir le contexte de données sur cette fenêtre
        }

        // Méthode pour obtenir les niveaux où l'enseignant est affecté avec les informations de filière, département et faculté
        private List<NiveauInfo> GetNiveauInfoList(string matricule)
        {
            List<NiveauInfo> niveauInfos = new List<NiveauInfo>();

            try
            {
                string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";
                string query = $"SELECT NiveauSet.Libelle, FiliereSet.Nom AS Filiere, DepartementSet.Nom AS Departement, FaculteSet.Nom AS Faculte " +
                               $"FROM NiveauSet " +
                               $"INNER JOIN FiliereSet ON NiveauSet.FiliereId = FiliereSet.Id " +
                               $"INNER JOIN DepartementSet ON FiliereSet.DepartementId = DepartementSet.Id " +
                               $"INNER JOIN FaculteSet ON DepartementSet.FaculteId = FaculteSet.Id " +
                               $"INNER JOIN UESet ON NiveauSet.Id = UESet.NiveauId " +
                               $"WHERE UESet.Enseignant_Matricule = '{matricule}'";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NiveauInfo niveauInfo = new NiveauInfo
                            {
                                Niveau = reader["Libelle"].ToString(),
                                Filiere = reader["Filiere"].ToString(),
                                Departement = reader["Departement"].ToString(),
                                Faculte = reader["Faculte"].ToString()
                            };
                            niveauInfos.Add(niveauInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des niveaux : " + ex.Message);
            }

            return niveauInfos;
        }

        // Méthode exécutée lors du clic sur "Nommer Administrateur"
        private void NommerAdministrateur_Click(object sender, RoutedEventArgs e)
        {
            // Utiliser le matricule stocké dans le champ Matricule
            NommerEnseignantAdministrateur(Matricule);
        }

        // Méthode exécutée lors du clic sur "Enlever Administrateur"
        private void EnleverAdministrateur_Click(object sender, RoutedEventArgs e)
        {
            // Utiliser le matricule stocké dans le champ Matricule
            EnleverRoleAdministrateur(Matricule);
        }

        // Méthode exécutée lors du clic sur "Supprimer Enseignant"
        private void SupprimerEnseignant_Click(object sender, RoutedEventArgs e)
        {
            // Utiliser le matricule stocké dans le champ Matricule
            SupprimerEnseignant(Matricule);
        }

        // Méthode pour nommer un enseignant en tant qu'administrateur
        private void NommerEnseignantAdministrateur(string matricule)
        {
            try
            {
                string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";
                string query = $"UPDATE UtilisateurSet SET Role = 'Administrateur' WHERE Matricule = '{matricule}'";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Enseignant nommé administrateur avec succès.");
                    }
                    else
                    {
                        MessageBox.Show("Aucun enseignant trouvé avec ce matricule.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du nommage de l'enseignant en administrateur : " + ex.Message);
            }
        }

        // Méthode pour enlever le rôle administrateur d'un enseignant
        private void EnleverRoleAdministrateur(string matricule)
        {
            try
            {
                string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";
                string query = $"UPDATE UtilisateurSet SET Role = 'Enseignant' WHERE Matricule = '{matricule}'";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Rôle administrateur enlevé avec succès de l'enseignant.");
                    }
                    else
                    {
                        MessageBox.Show("Aucun enseignant trouvé avec ce matricule.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression du rôle administrateur de l'enseignant : " + ex.Message);
            }
        }

        // Méthode pour supprimer un enseignant
        private void SupprimerEnseignant(string matricule)
        {
            try
            {
                string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";
                string query = $"DELETE FROM UtilisateurSet WHERE Matricule = '{matricule}'";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Enseignant supprimé avec succès.");
                    }
                    else
                    {
                        MessageBox.Show("Aucun enseignant trouvé avec ce matricule.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression de l'enseignant : " + ex.Message);
            }
        }

        // Modèle de données pour les informations sur le niveau
        public class NiveauInfo
        {
            public string Niveau { get; set; }
            public string Filiere { get; set; }
            public string Departement { get; set; }
            public string Faculte { get; set; }
        }
    }
}
