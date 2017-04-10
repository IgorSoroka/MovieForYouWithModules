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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModuleMainModule.Views
{
    /// <summary>
    /// Логика взаимодействия для Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        public Player()
        {
            InitializeComponent();
            //WebBrowser.Navigate(TextBlock.Text);
            //this.WebBrowser.Source = new Uri("http://www.youtube.com/embed/L8bE5-g8VC0");
        }
    }
}
