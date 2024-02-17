using MySql.Data.MySqlClient;
using PlanningSup.Vues;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PlanningSup
{
    public partial class home_administrateur : Window
    {
        private enum ActiveTab
        {
            Dashboard,
            PlagesCours,
            Enseignants,
            Etablissements,
            PlagesHoraires
        }

        private string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";

        public string ObtenirNomUtilisateur(string matricule)
        {
            string nomUtilisateur = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT Nom FROM UtilisateurSet WHERE Matricule = @Matricule";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Matricule", matricule);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            nomUtilisateur = result.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Gérer l'exception, par exemple en journalisant l'erreur
                        Console.WriteLine("Erreur lors de la récupération du nom de l'utilisateur : " + ex.Message);
                    }
                }
            }

            return nomUtilisateur;
        }

        private ActiveTab activeTab = ActiveTab.Dashboard;

        public home_administrateur(string mat)
        {
            InitializeComponent();

            string matricule = mat;
            // Mettre à jour le message de bienvenue avec le nom de l'utilisateur
            string nomUtilisateur = ObtenirNomUtilisateur(matricule);
            MessageBienvenue.Text = "Bienvenue, " + nomUtilisateur;

            // Afficher initialement le User Control Dashboard
            ShowDashboard();
            UpdateButtonStyles();

        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            activeTab = ActiveTab.Dashboard;
            ShowDashboard();
            UpdateButtonStyles();
        }

        private void PlagesCours_Click(object sender, RoutedEventArgs e)
        {
            activeTab = ActiveTab.PlagesCours;
            ShowPlagesCours();
            UpdateButtonStyles();
        }

        private void Enseignants_Click(object sender, RoutedEventArgs e)
        {
            activeTab = ActiveTab.Enseignants;
            ShowEnseignants();
            UpdateButtonStyles();
        }

        private void Etablissements_Click(object sender, RoutedEventArgs e)
        {
            activeTab = ActiveTab.Etablissements;
            ShowEtablissements();
            UpdateButtonStyles();
        }

        private void PlagesHoraires_Click(object sender, RoutedEventArgs e)
        {
            activeTab = ActiveTab.PlagesHoraires;
            ShowPlagesHoraires();
            UpdateButtonStyles();
        }

        // Ajoutez des déclencheurs pour les autres boutons
        // ...

        private void ShowDashboard()
        {
            // Afficher le User Control Dashboard
            bordure_Principale.Child = new dashboard();
        }

        private void ShowPlagesCours()
        {
            // Logique pour afficher le User Control des plages de cours
            bordure_Principale.Child = new PlagesCours();
        }

        private void ShowEnseignants()
        {
            // Passer la référence à la fenêtre principale lors de la création de l'instance de Enseignant
            Enseignant enseignantView = new Enseignant(this);
            // Afficher le User Control des enseignants
            bordure_Principale.Child = enseignantView;
        }

        private void ShowEtablissements()
        {
            // Logique pour afficher le User Control des établissements
            Faculte faculteView = new Faculte(this);
            // Afficher le User Control des enseignants
            bordure_Principale.Child = faculteView;
        }

        private void ShowPlagesHoraires()
        {
            // Logique pour afficher le User Control des plages horaires
            bordure_Principale.Child = new PlagesCours();
        }

        private void UpdateButtonStyles()
        {
            // Mettez à jour les styles des boutons en fonction de l'onglet actif
            Home.Style = GetButtonStyle(ActiveTab.Dashboard);
            PlagesCours.Style = GetButtonStyle(ActiveTab.PlagesCours);
            Enseignants.Style = GetButtonStyle(ActiveTab.Enseignants);
            Etablissements.Style = GetButtonStyle(ActiveTab.Etablissements);
            // Mettez à jour les styles des autres boutons si nécessaire
        }

        private Style GetButtonStyle(ActiveTab tab)
        {
            Style style = new Style(typeof(Button));

            Brush backgroundBrush = (tab == activeTab) ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#888")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666"));
            Brush foregroundBrush = (tab == activeTab) ? Brushes.Yellow : Brushes.White;

            style.Setters.Add(new Setter(Button.BackgroundProperty, backgroundBrush));
            style.Setters.Add(new Setter(Button.ForegroundProperty, foregroundBrush));
            style.Setters.Add(new Setter(Button.BorderBrushProperty, null));

            return style;
        }
        private void Deconnexion_Click(object sender, RoutedEventArgs e)
        {
            // Appeler la méthode HandleLogout de MainWindow en passant false
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {   
                mainWindow.HandleLogout(false);
            }
            mainWindow = new MainWindow();
            mainWindow.Show();
            // Fermer la fenêtre actuelle
            this.Close();
        }

    }
}