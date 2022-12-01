using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

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
        bool searchM;
        ClientSocket s;

        public MainWindow()
        {
            InitializeComponent();
            w = new Window1();
            w.ShowDialog();
            if (w.txtUtente.Text == "")
            {
                this.Close();
            }
            else
            {
                nome = w.txtUtente.Text;
                refresh();
                index = -1;
                searchM = false;
            }

            s = new ClientSocket("127.0.0.1", 8080);
            Thread t = new Thread(new ThreadStart(s.run));
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
            connessioneTCP inst = connessioneTCP.getInstance();
            if (ListChat.SelectedIndex != -1)
                index = ListChat.SelectedIndex;
            if (searchM == false)
            { }
            else
            {
                inst.send("nuovaChat" + ";" + ListChat.SelectedItem);
            }
            reloadChat();
        }

        public static String getNome()
        {
            return nome;
        }

        private void bttSend_Click(object sender, RoutedEventArgs e)
        {
            connessioneTCP inst = connessioneTCP.getInstance();

            while (!inst.toClose)
            {
                if (inst.getSocket() == null)
                    continue;

                inst.send("send;" + chatList[index].id + txtMess.Text);
                Chat chat = chatList[index];
                chatList.RemoveAt(index);
                chatList.Insert(0, chat);
            }
            if (inst.recive() == "ok")
            {
                Messaggio m = new Messaggio(nome, txtMess.Text);
                chatList[index].messaggi.Add(m);
            }

            reloadChat();

        }

        public void reloadChat()
        {
            connessioneTCP inst = connessioneTCP.getInstance();
            ListChatHost.Items.Clear();
            ListChatGuest.Items.Clear();
            try
            {
                if (chatList[index].chatCaricata == false)
                {
                    //richiedo
                    inst.send("richiedoChat" + index);
                    String chat = inst.recive();
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
            foreach (String s in tuttiUtenti)
            {
                ListChat.Items.Add(s);
                searchM = true;
            }

        }

        private void bttGruppo_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = new CheckBox();
            foreach (String s in tuttiUtenti)
            {
                ListChat.Items.Add(chk + s);
                searchM = true;
            }
        }
    }
}
