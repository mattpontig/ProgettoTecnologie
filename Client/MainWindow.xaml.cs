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

namespace Client
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static String nome;
        List<Chat> chatList;
        Window1 w;

        public MainWindow()
        {
            InitializeComponent();
            w = new Window1();
            w.ShowDialog();
            nome = w.txtUtente.Text;
            refresh();
        }

        private void refresh()
        {
            chatList = w.
            foreach (Chat c in chatList)
                ListChat.Items.Add(c.ToString());
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public static String getNome()
        {
            return nome;
        }
    }
}
