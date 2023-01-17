using banco_client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Logica di interazione per WindowRegistrati.xaml
    /// </summary>
    public partial class WindowRegistrati : Window
    {
        Connection c;
        public String s = "";
        public bool close = false;
        public WindowRegistrati()
        {
            InitializeComponent();
            c = new Connection();
            Closing += new System.ComponentModel.CancelEventHandler(WReg_Closing);
        }

        private void WReg_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("ARE YOU WANT TO CLOSE?", "CLOSING", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                s = "ok";
                close= true;
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void bttRegistrazione_Click(object sender, RoutedEventArgs e)
        {
            c.invia("Register" + ";" + txtUtente.Text + ";" + txtPassword.Password.ToString() + ";");
            do
            {
               s = c.recive();
            } while (s == "" || s == null);
            if (s == "ok")
            {
                this.Close();
            }
            else if (s.StartsWith("Utente"))
            {
                txtUtente.Text = s;
            }
        }
    }
}
