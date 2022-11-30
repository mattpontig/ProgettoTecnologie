using banco_client;
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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Logica di interazione per Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Connection c;
        public String record;
        public Window1()
        {
            InitializeComponent();
            c = new Connection();
            labelErrato.IsEnabled = false;
        }

        private void bttLogin_Click(object sender, RoutedEventArgs e)
        {
            c.invia("Login" + ";" + txtUtente.Text + ";" + txtPassword.Text);
            String s = c.recive();
            if (s != "0")
            {
                labelErrato.IsEnabled = true;
            }
            else if (s == "1")
            {
                record = c.recive();
                this.Close();
            }
        }

        private void bttRegistrati_Click(object sender, RoutedEventArgs e)
        {
            WindowRegistrati w = new WindowRegistrati();
            w.ShowDialog();
            c.invia("Login" + ";" + w.txtUtente.Text + ";" + w.txtPassword.Text);
            String s = c.recive();
            if (s != "0")
            {
                labelErrato.IsEnabled = true;
            }
            else if (s == "1")
            {
                record = c.recive();
                this.Close();
            }
        }
    }
}
