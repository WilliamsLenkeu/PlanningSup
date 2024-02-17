using System.Windows;
using static PlanningSup.Vues.FiliereWindow;

namespace PlanningSup.Vues
{
    public partial class AfficherFiliereWindow : Window
    {
        public AfficherFiliereWindow(FiliereItem filiere)
        {
            InitializeComponent();
            DataContext = filiere;
        }

        private void Fermer_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Fermer la fenêtre
        }
    }
}