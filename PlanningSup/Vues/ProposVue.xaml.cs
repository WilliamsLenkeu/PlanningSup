using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace PlanningSup.Vues
{
    /// <summary>
    /// Logique d'interaction pour ProposVue.xaml
    /// </summary>
    public partial class ProposVue : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<CoursPropose> coursProposes;
        public ObservableCollection<CoursPropose> CoursProposes
        {
            get { return coursProposes; }
            set
            {
                coursProposes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CoursProposes"));
            }
        }

        private string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";

        private string nomA;

        public ProposVue(string nomAdmin)
        {
            InitializeComponent();
            DataContext = this;
            ChargerCoursProposes();
            nomA = nomAdmin;
        }

        private void ChargerCoursProposes()
        {
            CoursProposes = new ObservableCollection<CoursPropose>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT c.Jour, c.Heure_debut, c.Heure_fin, u.Code AS CodeUE, u.Libelle AS LibelleUE, 
                                    us.Nom AS NomEnseignant
                                    FROM CoursSetTemp c
                                    INNER JOIN UESet u ON c.UE_Id = u.Id
                                    INNER JOIN UtilisateurSet_Enseignant ue ON c.Enseignant_Matricule = ue.Matricule
                                    INNER JOIN UtilisateurSet us ON ue.Matricule = us.Matricule
                                    WHERE c.valide = 0"; // Sélectionne uniquement les cours non validés

                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        CoursProposes.Add(new CoursPropose
                        {
                            Jour = reader.GetString("Jour"),
                            HeureDebut = reader.GetTimeSpan("Heure_debut"),
                            HeureFin = reader.GetTimeSpan("Heure_fin"),
                            CodeUE = reader.GetString("CodeUE"),
                            LibelleUE = reader.GetString("LibelleUE"),
                            NomEnseignant = reader.GetString("NomEnseignant")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des cours proposés : " + ex.Message);
            }
        }

        private void ValiderCours_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer le cours correspondant au bouton cliqué
            if (sender is Button button && button.DataContext is CoursPropose cours)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // Récupérer l'ID de l'UE correspondant au code de l'UE
                        string ueIdQuery = "SELECT Id FROM UESet WHERE Code = @CodeUE;";
                        MySqlCommand ueIdCommand = new MySqlCommand(ueIdQuery, connection);
                        ueIdCommand.Parameters.AddWithValue("@CodeUE", cours.CodeUE);
                        int ueId = Convert.ToInt32(ueIdCommand.ExecuteScalar());

                        // Récupérer le matricule de l'enseignant correspondant à son nom
                        string matriculeQuery = "SELECT Matricule FROM UtilisateurSet WHERE Nom = @Nom AND Role = 'Enseignant';";
                        MySqlCommand matriculeCommand = new MySqlCommand(matriculeQuery, connection);
                        matriculeCommand.Parameters.AddWithValue("@Nom", cours.NomEnseignant);
                        string matricule = matriculeCommand.ExecuteScalar()?.ToString();

                        if (!string.IsNullOrEmpty(matricule))
                        {
                            // Mettre à jour le champ valide du cours dans la base de données
                            string updateQuery = "UPDATE CoursSetTemp SET valide = 1 WHERE Jour = @Jour AND Heure_debut = @HeureDebut AND Heure_fin = @HeureFin AND UE_Id = @UEId AND Enseignant_Matricule = @Matricule;";
                            MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                            updateCommand.Parameters.AddWithValue("@Jour", cours.Jour);
                            updateCommand.Parameters.AddWithValue("@HeureDebut", cours.HeureDebut);
                            updateCommand.Parameters.AddWithValue("@HeureFin", cours.HeureFin);
                            updateCommand.Parameters.AddWithValue("@UEId", ueId);
                            updateCommand.Parameters.AddWithValue("@Matricule", matricule);
                            int rowsAffected = updateCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Cours validé avec succès.");
                                // Mettre à jour la liste des cours après validation
                                ChargerCoursProposes();
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

    public class CoursPropose
    {
        public string Jour { get; set; }
        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }
        public string CodeUE { get; set; }
        public string LibelleUE { get; set; }
        public string NomEnseignant { get; set; }
    }
}