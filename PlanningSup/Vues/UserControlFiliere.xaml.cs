using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace PlanningSup
{
    /// <summary>
    /// Logique d'interaction pour UserControlFiliere.xaml
    /// </summary>
    public partial class UserControlFiliere : UserControl
    {
        private string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";
        private string matriculeEnseignant;

        public UserControlFiliere(string mat)
        {
            InitializeComponent();
            matriculeEnseignant = mat;
            ChargerFilieres();
        }

        private void ChargerFilieres()
        {
            List<string> filieres = new List<string>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT DISTINCT f.Nom 
                             FROM FiliereSet f 
                             INNER JOIN NiveauSet n ON f.Id = n.FiliereId 
                             INNER JOIN UESet u ON n.Id = u.NiveauId 
                             LEFT JOIN CoursSet c ON u.Id = c.UE_Id 
                             LEFT JOIN UtilisateurSet_Enseignant e ON c.Enseignant_Matricule = e.Matricule 
                             WHERE e.Matricule = @Matricule OR c.Id IS NULL";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Matricule", matriculeEnseignant);
                    connection.Open();  
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            filieres.Add(reader["Nom"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception, par exemple en affichant un message d'erreur
                MessageBox.Show("Erreur lors du chargement des filières : " + ex.Message);
            }

            // Mettre à jour la ListBox avec la liste des filières
            listeFiliere.ItemsSource = filieres;
        }
    }
}