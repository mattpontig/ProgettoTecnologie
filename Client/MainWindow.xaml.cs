using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Diagnostics;
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
        int index;
        static String nome;
        List<Chat> chatList;
        Window1 w;
        connessioneTCP tcp;
        public MainWindow()
        {
            InitializeComponent();
            w = new Window1();
            w.ShowDialog();
            nome = w.txtUtente.Text;
            refresh();
            index = -1;
            tcp = new connessioneTCP();
        }

        private void refresh()
        {
            try
            {
                chatList = parseClass.toList(nome, w.record);
                foreach (Chat c in chatList)
                    ListChat.Items.Add(c.ToString());
            }
            catch (Exception ex) { ListChat.SelectedIndex = -1; }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListChat.SelectedIndex != -1)
                 index = ListChat.SelectedIndex;
            try
            {
                if (chatList[index].chatCaricata)
                {

                }
                else
                {
                    //richiedo
                    tcp.send("richiedoChat" + index);
                    tcp.recive();
                }


                ListChat.SelectedIndex = index;
            }
            catch
            {
            }
        }

        public static String getNome()
        {
            return nome;
        }

    }
}
