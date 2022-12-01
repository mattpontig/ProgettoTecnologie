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
        List<String> tuttiUtenti;
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
            ListChat.Items.Clear();
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
            reloadChat();
        }

        public static String getNome()
        {
            return nome;
        }

        private void bttSend_Click(object sender, RoutedEventArgs e)
        {
            tcp.send("send;" + index + txtMess.Text);
            if (tcp.recive() == "ok")
            {
                Messaggio m = new Messaggio(nome, txtMess.Text);
                chatList[index].messaggi.Add(m);
            }
            reloadChat();
        }

        public void reloadChat()
        {
            ListChatHost.Items.Clear();
            ListChatGuest.Items.Clear();
            try
            {
                if (chatList[index].chatCaricata == false)
                {
                    //richiedo
                    tcp.send("richiedoChat" + index);
                    String chat = tcp.recive();
                    //chatMess = parseClass.toChat(chat);
                    chatList[index].messaggi = parseClass.toChat(chat);
                }

                List<Messaggio> chatMess = chatList[index].messaggi;

                for (int i = 0; i < chatMess.Count; i++)
                {
                    if (nome == chatMess[i].nome)
                    {
                        ListChatHost.Items.Add(chatMess[i].toMessHost());
                        ListChatGuest.Items.Add("");
                    }
                    else
                    {
                        ListChatHost.Items.Add("");
                        ListChatGuest.Items.Add(chatMess[i].toMessGuest());
                    }
                }

                ListChat.SelectedIndex = index;
            }
            catch
            {
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<String> filteredList = tuttiUtenti.Where(x => x.Contains(txtSearch.Text)).ToList();
            ListChat.Items.Clear();
            foreach(String s in tuttiUtenti)
            {
                ListChat.Items.Add(s);
            }

        }
    }
}
