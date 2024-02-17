using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace PlanningSup
{
    /// <summary>
    /// Logique d'interaction pour UserControlCours.xaml
    /// </summary>
    public partial class UserControlCours : UserControl
    {
        private string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";
        private string matriculeEnseignant;

        public UserControlCours(string mat)
        {
            InitializeComponent();
            matriculeEnseignant = mat;
            ChargerCours();
        }

        private void ChargerCours()
        {
            List<UE> uesSansCours = new List<UE>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT u.Code, u.Libelle 
                                     FROM UESet u 
                                     LEFT JOIN CoursSet c ON u.Id = c.UE_Id 
                                     WHERE c.Id IS NULL";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            uesSansCours.Add(new UE
                            {
                                Code = reader["Code"].ToString(),
                                Libelle = reader["Libelle"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception, par exemple en affichant un message d'erreur
                MessageBox.Show("Erreur lors du chargement des UE sans cours : " + ex.Message);
            }

            // Mettre à jour la ListBox avec la liste des UE sans cours
            listeCours.ItemsSource = uesSansCours;
        }

        private void listeCours_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (listeCours.SelectedItem != null)
            {
                UE ueSelectionnee = (UE)listeCours.SelectedItem;
                // Ouvrir une fenêtre de proposition de cours avec l'UE sélectionnée
                ProposerCoursWindow fenetreProposerCours = new ProposerCoursWindow(ueSelectionnee, matriculeEnseignant);
                fenetreProposerCours.ShowDialog();
            }
        }
    }

    public class UE
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Libelle { get; set; }
        public int NiveauId { get; set; }
    }
}