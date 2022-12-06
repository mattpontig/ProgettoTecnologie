using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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
        List<String> tuttiUtenti = null;
        Window1 w;
        bool searchM;
        ClientSocket s;

        public MainWindow()
        {
            InitializeComponent();
            bttGruppoConfirm.Visibility = Visibility.Hidden;
            labelGruppo.Visibility = Visibility.Hidden;
            txtNomeGruppo.Visibility = Visibility.Hidden;
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

            s = new ClientSocket(8080);
            Thread t = new Thread(new ThreadStart(s.run));
            t.Start();
            //prova();
        }
    }

        void prova()
        {
            connessioneTCP inst = connessioneTCP.getInstance();
            inst.send("prova");
        }
        private void refresh()
        {
            ListChat.Items.Clear();
            try
            {
                chatList = parseClass.toList(nome, w.record);
                foreach (Chat c in chatList)
                    ListChat.Items.Add(c.toString());
            }
            catch (Exception ex) { ListChat.SelectedIndex = -1; }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            connessioneTCP inst = connessioneTCP.getInstance();
            if (ListChat.SelectedIndex != -1)
                index = ListChat.SelectedIndex;
            if (searchM == false)
            {
                reloadChat();
            }
            else
            {
                inst.send("nuovaChat" + ";" + ListChat.SelectedIndex);
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
            /*connessioneTCP inst = connessioneTCP.getInstance();

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
            }*/

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
                    inst.send("richiedoChat;" + chatList[index].id);
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
                chk = new CheckBox();
                chk.Click += aggiuntaGruppo;
                chk.Content = s;
                ListChat.Items.Add(chk);
                searchM = true;
            }
        }

        List<String> gruppo;

        private void aggiuntaGruppo(object sender, RoutedEventArgs e)
        {
            bttGruppoConfirm.IsEnabled = false;
            bttGruppo.IsEnabled = true;
            gruppo = new List<string>();
            CheckBox chk = e.Source as CheckBox;
            if (chk.IsChecked == true)
            {
                gruppo.Add(chk.Content.ToString());
            }
            else
            {
                gruppo.Remove(chk.Content.ToString());
            }

        }

        int step = 0;

        private void bttGruppoConfirm_Click(object sender, RoutedEventArgs e)
        {
            connessioneTCP inst = connessioneTCP.getInstance();

            String str = "";
            if (step == 0)
            {
                foreach (String s in gruppo)
                    str += s + ",";

                labelGruppo.Visibility = Visibility.Visible;
                txtNomeGruppo.Visibility = Visibility.Hidden;
                ListChat.Visibility = Visibility.Hidden;
            }
            else if (step == 1)
            {
                step = 0;

                labelGruppo.Visibility = Visibility.Hidden;
                txtNomeGruppo.Visibility = Visibility.Hidden;
                ListChat.Visibility = Visibility.Visible;

                bttGruppoConfirm.Visibility = Visibility.Visible;
                bttGruppo.Visibility = Visibility.Hidden;

                inst.send("newGruppo;" + txtNomeGruppo.Text + ";" + str);
            }

        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            MessageBox.Show("Closing called");

            connessioneTCP inst = connessioneTCP.getInstance();
            inst.send("Close");
        }

        void getUtenti()
        {
            if (tuttiUtenti == null)
            {
                connessioneTCP inst = connessioneTCP.getInstance();
                inst.send("getUtenti");
                String s = inst.recive();
                String[] ut = s.Split(';');
                foreach (String s2 in ut)
                    tuttiUtenti.Add(s2);
            }
        }
    }
}
