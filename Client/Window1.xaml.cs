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
        public String nomeUtente = "";

        public Window1()
        {
            InitializeComponent();
            c = new Connection();
        }

        private void bttLogin_Click(object sender, RoutedEventArgs e)
        {
            nomeUtente = txtUtente.Text;
            c.invia("Login" + ";" + nomeUtente + ";" + txtPassword.Text + ";");
            String s = c.recive();
            //MessageBox.Show(s);
            if (s == "0")
            {
                txtUtente.Text = "Login Errato";
            }
            else if (Connection.online == false)
            {
                txtUtente.Text = "Server down";
            }
            else if (s == "1")
            {
                this.Close();
            }
        }

        private void bttRegistrati_Click(object sender, RoutedEventArgs e)
        {
            WindowRegistrati w = new WindowRegistrati();
            w.ShowDialog();
            while(w.s != "ok") { }
            nomeUtente = w.txtUtente.Text;
            c.invia("Login" + ";" + nomeUtente + ";" + w.txtPassword.Text + ";");
            String s = c.recive();
            if (s == "0")
            {
                txtUtente.Text = "Login Errato";
            }
            else if (Connection.online == false)
            {
                txtUtente.Text = "Server down";
            }
            else if (s == "1")
            {
                this.Close();
            }
        }
    }
}
