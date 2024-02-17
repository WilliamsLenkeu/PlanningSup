using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace PlanningSup.Vues
{
    public partial class AjouterUEWindow : Window
    {
        private string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";

        public AjouterUEWindow()
        {
            InitializeComponent();
            ChargerFacultes();
        }

        private void ChargerFacultes()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT Nom FROM FaculteSet";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbFaculte.Items.Add(reader["Nom"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des facultés : " + ex.Message);
            }
        }

        private void CmbFaculte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFaculte.SelectedItem != null)
            {
                string faculteSelectionnee = cmbFaculte.SelectedItem.ToString();
                ChargerDepartements(faculteSelectionnee);
            }
        }

        private void ChargerDepartements(string faculte)
        {
            cmbDepartement.IsEnabled = true;
            cmbDepartement.Items.Clear();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT d.Nom FROM DepartementSet d INNER JOIN FaculteSet f ON d.FaculteId = f.Id WHERE f.Nom = @Faculte";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Faculte", faculte);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbDepartement.Items.Add(reader["Nom"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des départements : " + ex.Message);
            }
        }

        private void CmbDepartement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDepartement.SelectedItem != null)
            {
                string departementSelectionne = cmbDepartement.SelectedItem.ToString();
                ChargerFilieres(departementSelectionne);
            }
        }

        private void ChargerFilieres(string departement)
        {
            cmbFiliere.IsEnabled = true;
            cmbFiliere.Items.Clear();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT f.Nom FROM FiliereSet f INNER JOIN DepartementSet d ON f.DepartementId = d.Id WHERE d.Nom = @Departement";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Departement", departement);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbFiliere.Items.Add(reader["Nom"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des filières : " + ex.Message);
            }
        }

        private void CmbFiliere_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFiliere.SelectedItem != null)
            {
                cmbNiveau.IsEnabled = true;
                cmbNiveau.Items.Clear();
                string filiere = cmbFiliere.SelectedItem.ToString();
                ChargerNiveaux(filiere);
            }
        }

        private void ChargerNiveaux(string filiere)
        {
            cmbNiveau.IsEnabled = true;
            cmbNiveau.Items.Clear();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT Libelle FROM NiveauSet WHERE FiliereId IN (SELECT Id FROM FiliereSet WHERE Nom = @Filiere)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Filiere", filiere);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbNiveau.Items.Add(reader["Libelle"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des niveaux : " + ex.Message);
            }
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Récupérer les valeurs sélectionnées dans les ComboBox
                string faculte = cmbFaculte.SelectedItem.ToString();
                string departement = cmbDepartement.SelectedItem.ToString();
                string filiere = cmbFiliere.SelectedItem.ToString();
                string niveau = cmbNiveau.SelectedItem.ToString();
                string libelleUE = txtLibelleUE.Text;

                // Générer le code de l'UE selon le format spécifié
                string codeUE = GenererCodeUE(filiere, niveau);

                // Ajouter ici le code pour insérer l'UE dans la base de données
                // Utiliser les valeurs récupérées et insérées dans les ComboBox
                // Utiliser également le code de l'UE généré

                // Connexion à la base de données
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Requête SQL pour l'insertion d'une nouvelle UE
                    string query = "INSERT INTO UESet (Code, Libelle, Semestre, NiveauId) VALUES (@Code, @Libelle, @Semestre, @NiveauId)";

                    // Création de la commande SQL
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Ajout des paramètres
                    command.Parameters.AddWithValue("@Code", codeUE);
                    command.Parameters.AddWithValue("@Libelle", libelleUE);
                    // Hardcoded semestre to 1 as example
                    command.Parameters.AddWithValue("@Semestre", 1);

                    // Récupération de l'identifiant du niveau
                    int niveauId = RecupererNiveauId(filiere, niveau);
                    command.Parameters.AddWithValue("@NiveauId", niveauId);

                    // Ouverture de la connexion
                    connection.Open();

                    // Exécution de la commande SQL
                    int rowsAffected = command.ExecuteNonQuery();

                    // Vérification si l'insertion a réussi
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("UE ajoutée avec succès !");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout de l'UE : Aucune ligne affectée.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout de l'UE : " + ex.Message);
            }
        }

        private int RecupererNiveauId(string filiere, string niveau)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT n.Id FROM NiveauSet n INNER JOIN FiliereSet f ON n.FiliereId = f.Id WHERE f.Nom = @Filiere AND n.Libelle = @Niveau";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Filiere", filiere);
                    command.Parameters.AddWithValue("@Niveau", niveau);
                    connection.Open();
                    int niveauId = Convert.ToInt32(command.ExecuteScalar());
                    return niveauId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération de l'identifiant du niveau : " + ex.Message);
                return 0;
            }
        }

        private string GenererCodeUE(string filiere, string niveau)
        {
            try
            {
                // Récupérer les 3 premières lettres de la filière
                string prefixeFiliere = string.Join("", filiere.Take(3));

                // Récupérer le chiffre du libellé du niveau
                int chiffreNiveau = int.Parse(new string(niveau.Where(char.IsDigit).ToArray()));

                // Récupérer le nombre d'UE déjà existantes pour ce niveau
                int nombreUE = RecupererNombreUE(filiere, niveau);

                // Formater le code de l'UE selon le format XXXYZZ
                string codeUE = $"{prefixeFiliere}{chiffreNiveau:0}{nombreUE + 1:00}";

                return codeUE;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la génération du code de l'UE : " + ex.Message);
                return null;
            }
        }

        private int RecupererNombreUE(string filiere, string niveau)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM UESet ue INNER JOIN NiveauSet n ON ue.NiveauId = n.Id INNER JOIN FiliereSet f ON n.FiliereId = f.Id WHERE f.Nom = @Filiere AND n.Libelle = @Niveau";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Filiere", filiere);
                    command.Parameters.AddWithValue("@Niveau", niveau);
                    connection.Open();
                    int nombreUE = Convert.ToInt32(command.ExecuteScalar());
                    return nombreUE;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération du nombre d'UE : " + ex.Message);
                return 0;
            }
        }
    }
}