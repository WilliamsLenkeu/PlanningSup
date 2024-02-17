using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlanningSup.Vues
{
    /// <summary>
    /// Logique d'interaction pour Departement.xaml
    /// </summary>
    public partial class Departement : UserControl
    {
        private readonly BlurEffect blurEffect = new BlurEffect();
        private readonly Window mainWindow;

        public Departement(Window mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow; // Initialisation de la variable mainWindow
        }
    }
}
