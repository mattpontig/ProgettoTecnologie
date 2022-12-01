﻿using banco_client;
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
    /// Logica di interazione per WindowRegistrati.xaml
    /// </summary>
    public partial class WindowRegistrati : Window
    {
        Connection c;
        public WindowRegistrati()
        {
            InitializeComponent();
            c = new Connection();
        }

        private void bttRegistrazione_Click(object sender, RoutedEventArgs e)
        {
            c.invia("Register" + ";" + txtUtente.Text + ";" + txtPassword.Text + ";");
            String s = c.recive();
            if (s == "ok")
            {
                this.Close();
            }
        }
    }
}
