using System.Text.RegularExpressions;
using System;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using PlanningSup.Vues;
using System.Windows.Media.Effects;
using System.Windows.Media;

namespace PlanningSup
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";
        private MySqlConnection connection;
        public string mat;

        private Enseignant enseignantControl;

        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabaseConnection();

            // Créez une instance de Enseignant en passant la référence de la fenêtre principale
            enseignantControl = new Enseignant(this);
        }

        public void HandleLogout(bool isLoggedOut)
        {
            // Gérer la déconnexion ici en fonction de la valeur booléenne reçue
            if (isLoggedOut)
            {
                // Code à exécuter lorsque l'utilisateur est déconnecté
            }
        }

        // Méthode pour appliquer l'effet de flou à la fenêtre principale
        public void ApplyBlurEffect()
        {
            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 10;
            Effect = blurEffect;
        }

        // Méthode pour retirer l'effet de flou de la fenêtre principale
        public void RemoveBlurEffect()
        {
            Effect = null;
        }
        private void InitializeDatabaseConnection()
        {
            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Connected to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void Inscription_Click(object sender, RoutedEventArgs e)
        {
            string username = NtbUsername.Text;
            string email = tbEmail.Text;
            string password = NpbPassword.Password;
            string matricule = tbMatricule.Text;
            string role = (cbRole.SelectedItem as ComboBoxItem).Content.ToString();

            // Vérifier si les champs ne sont pas vides
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(matricule))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            // Vérifier le format de l'email
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Format d'email invalide.");
                return;
            }

            // Vérifier le format du matricule
            if (!IsValidMatricule(matricule))
            {
                MessageBox.Show("Format de matricule invalide. Le matricule doit être sous le format XXYXXXX avec X un entier et Y une lettre.");
                return;
            }

            // Vérifier si un rôle est sélectionné
            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Veuillez sélectionner un rôle.");
                return;
            }

            // Insérer dans UtilisateurSet
            InsertIntoUtilisateurSet(matricule, username, email, password, role);

            // Si le rôle est Administrateur, insérer également dans UtilisateurSet_Administrateur
            if (role == "Administrateur")
            {
                InsertIntoAdministrateurTable(matricule);
            }
            else if (role == "Enseignant")
            {
                // Insérer dans UtilisateurSet_Enseignant
                string speciality = tbSpeciality.Text;
                if (string.IsNullOrEmpty(speciality))
                {
                    MessageBox.Show("Veuillez entrer une spécialité pour l'enseignant.");
                    return;
                }

                InsertIntoEnseignantTable(matricule, speciality);
            }
            else if (role == "Étudiant")
            {
                // Vérifier s'il existe une filière et un niveau
                if (!IsFiliereAndNiveauAvailable())
                {
                    MessageBox.Show("Impossible pour un étudiant de s'inscrire. Filière ou niveau manquant.");
                    return;
                }

                // Récupérer les valeurs sélectionnées dans les combobox de filière et de niveau
                int filiereId = (cbFiliere.SelectedItem as Filiere).Id; // Modifier Filiere avec le type approprié
                int niveauId = (cbNiveau.SelectedItem as Niveau).Id; // Modifier Niveau avec le type approprié

                // Insérer dans UtilisateurSet_Etudiant
                InsertIntoEtudiantTable(matricule, filiereId, niveauId);
            }

            MessageBox.Show("Inscription réussie!");
        }

        // Méthode pour insérer dans UtilisateurSet_Administrateur
        private void InsertIntoAdministrateurTable(string matricule)
        {
            string query = "INSERT INTO UtilisateurSet_Administrateur (Matricule) VALUES (@Matricule)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Matricule", matricule);
            command.ExecuteNonQuery();
        }

        // Méthode pour insérer dans UtilisateurSet_Enseignant
        private void InsertIntoEnseignantTable(string matricule, string speciality)
        {
            string query = "INSERT INTO UtilisateurSet_Enseignant (Matricule, Specialite) VALUES (@Matricule, @Specialite)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Matricule", matricule);
            command.Parameters.AddWithValue("@Specialite", speciality);
            command.ExecuteNonQuery();
        }

        // Méthode pour insérer dans UtilisateurSet_Etudiant
        private void InsertIntoEtudiantTable(string matricule, int filiereId, int niveauId)
        {
            string query = "INSERT INTO UtilisateurSet_Etudiant (Matricule, FiliereId, NiveauId) VALUES (@Matricule, @FiliereId, @NiveauId)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Matricule", matricule);
            command.Parameters.AddWithValue("@FiliereId", filiereId);
            command.Parameters.AddWithValue("@NiveauId", niveauId);
            command.ExecuteNonQuery();
        }

        private void Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Affiche les champs spécifiques aux utilisateurs en fonction du rôle sélectionné
            if (cbRole.SelectedItem != null)
            {
                string role = (cbRole.SelectedItem as ComboBoxItem).Content.ToString();
                if (role == "Étudiant")
                {
                    studentFields.Visibility = Visibility.Visible;
                    teacherFields.Visibility = Visibility.Collapsed;
                }
                else if (role == "Enseignant")
                {
                    studentFields.Visibility = Visibility.Collapsed;
                    teacherFields.Visibility = Visibility.Visible;
                }
                else if (role == "Administrateur")
                {
                    // Cacher tous les champs spéciaux
                    studentFields.Visibility = Visibility.Collapsed;
                    teacherFields.Visibility = Visibility.Collapsed;

                    // Effacer le texte dans les champs de saisie, s'il y a lieu
                    ClearTextFields();
                }
            }
        }

        // Méthode pour insérer dans UtilisateurSet
        private void InsertIntoUtilisateurSet(string matricule, string username, string email, string password, string role)
        {
            string query = "INSERT INTO UtilisateurSet (Matricule, Nom, Email, MotDePasse, Role) VALUES (@Matricule, @Nom, @Email, @MotDePasse, @Role)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Matricule", matricule);
            command.Parameters.AddWithValue("@Nom", username);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@MotDePasse", password);
            command.Parameters.AddWithValue("@Role", role);
            command.ExecuteNonQuery();
        }

        // Vérifie le format du matricule
        private bool IsValidMatricule(string matricule)
        {
            Regex regex = new Regex(@"^\d{2}[A-Za-z]\d{4}$");
            return regex.IsMatch(matricule);
        }

        // Vérifie le format de l'email
        private bool IsValidEmail(string email)
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        // Efface le texte dans les champs de saisie
        private void ClearTextFields()
        {
            // Efface le texte dans les champs de saisie
            cbFiliere.SelectedIndex = -1;
            cbNiveau.SelectedIndex = -1;
            tbSpeciality.Clear();
        }

        // Vérifie s'il existe une filière et un niveau
        private bool IsFiliereAndNiveauAvailable()
        {
            try
            {
                // Ouvrir une nouvelle connexion
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier s'il existe au moins une filière
                    string queryFiliere = "SELECT COUNT(*) FROM FiliereSet";
                    MySqlCommand commandFiliere = new MySqlCommand(queryFiliere, connection);
                    int countFiliere = Convert.ToInt32(commandFiliere.ExecuteScalar());
                    if (countFiliere == 0)
                    {
                        return false; // Aucune filière n'est disponible
                    }

                    // Vérifier s'il existe au moins un niveau
                    string queryNiveau = "SELECT COUNT(*) FROM NiveauSet";
                    MySqlCommand commandNiveau = new MySqlCommand(queryNiveau, connection);
                    int countNiveau = Convert.ToInt32(commandNiveau.ExecuteScalar());
                    if (countNiveau == 0)
                    {
                        return false; // Aucun niveau n'est disponible
                    }
                }

                // Si au moins une filière et un niveau sont disponibles, retourner true
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la vérification de la disponibilité de filière et de niveau: " + ex.Message);
                return false;
            }
        }

        private void Connexion_Click(object sender, RoutedEventArgs e)
        {
            string usernameOrEmail = tbUsername.Text;
            string password = pbPassword.Password;

            // Vérifier si les champs ne sont pas vides
            if (string.IsNullOrEmpty(usernameOrEmail) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            string query = "SELECT Matricule, Role FROM UtilisateurSet WHERE (Nom = @UsernameOrEmail OR Email = @UsernameOrEmail) AND MotDePasse = @MotDePasse";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UsernameOrEmail", usernameOrEmail);
                    command.Parameters.AddWithValue("@MotDePasse", password);

                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string role = reader.GetString("Role");
                        mat = reader.GetString("Matricule");

                        // Ouvrir la fenêtre appropriée en fonction du rôle
                        switch (role)
                        {
                            case "Administrateur":
                                home_administrateur home_administrateur = new home_administrateur(mat);
                                home_administrateur.Show();
                                break;
                            case "Enseignant":
                                home_enseignant home_enseignant = new home_enseignant();
                                home_enseignant.Show();
                                break;
                            case "Étudiant":
                                home_etudiant home_etudiant = new home_etudiant();
                                home_etudiant.Show();
                                break;
                        }

                        // Fermer la fenêtre de connexion
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Nom d'utilisateur, email ou mot de passe incorrect.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la connexion: " + ex.Message);
            }
        }


        private void ClearText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                ((TextBox)sender).Text = "";
            }
            else if (sender is PasswordBox)
            {
                ((PasswordBox)sender).Password = "";
            }
        }

        private void BasculeInscription_Click(object sender, RoutedEventArgs e)
        {
            if (borderConnexion.Visibility == Visibility.Visible)
            {
                borderConnexion.Visibility = Visibility.Collapsed;
                borderInscription.Visibility = Visibility.Visible;
            }
            else
            {
                borderConnexion.Visibility = Visibility.Visible;
                borderInscription.Visibility = Visibility.Collapsed;
            }
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            BasculeInscription_Click(sender, e);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            BasculeInscription_Click(sender, e);
        }

        public class Filiere
        {
            public int Id { get; set; }
            public int DepartementId { get; set; }
            public string Nom { get; set; }
        }

        public class Niveau
        {
            public int Id { get; set; }
            public int FiliereId { get; set; }
            public string Libelle { get; set; }
        }
    }
}