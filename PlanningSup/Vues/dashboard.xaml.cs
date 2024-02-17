using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Windows;

namespace PlanningSup.Vues
{
    /// <summary>
    /// Logique d'interaction pour dashboard.xaml
    /// </summary>
    public partial class dashboard : UserControl
    {
        public ObservableCollection<ActionHistoryItem> ActionHistory { get; set; }

        public dashboard()
        {
            InitializeComponent();
            ActionHistory = new ObservableCollection<ActionHistoryItem>();
            LoadActionHistory();
            DataContext = this; // Définir le contexte de données sur cette instance de dashboard
        }

        private void LoadActionHistory()
        {
            LoadActionHistoryFromTable("Journal");
            LoadActionHistoryFromTable("journal_user");
        }

        private void LoadActionHistoryFromTable(string tableName)
        {
            // Connexion à la base de données et récupération de l'historique des actions
            string connectionString = "Server=localhost;Database=planningsup;Uid=root;Pwd=;";
            string query = $"SELECT Action, Table_concernee, Action_sur_id, Timestamp FROM {tableName} ORDER BY Timestamp DESC";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                try
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ActionHistory.Add(new ActionHistoryItem
                            {
                                Action = reader["Action"].ToString(),
                                TableConcerned = reader["Table_concernee"].ToString(),
                                ActionOnId = reader["Action_sur_id"].ToString(),
                                Timestamp = Convert.ToDateTime(reader["Timestamp"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la récupération de l'historique des actions depuis la table {tableName}: " + ex.Message);
                }
            }
        }
    }

    public class ActionHistoryItem
    {
        public string Action { get; set; }
        public string TableConcerned { get; set; }
        public string ActionOnId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}