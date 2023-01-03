using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Security;
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
        List<Utente> tuttiUtenti = null;
        Window1 w;
        bool searchM;
        ClientSocket s;
        OpenFileDialog openFileDialog1;
        Thread t;

        public MainWindow()
        {
            InitializeComponent();
            tuttiUtenti = new List<Utente>();
            Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
            enableChat();
            w = new Window1();
            w.ShowDialog();
            if (w.txtUtente.Text == "")
            {
                this.Close();
            }
            else
            {
                nome = w.txtUtente.Text;
                index = -1;
                searchM = false;
            }
            s = new ClientSocket(8080);
            t = new Thread(new ThreadStart(s.run));
            t.Start();
            refresh();
        }


        void enableChat()
        {
            bttGruppoConfirm.Visibility = Visibility.Hidden;
            labelGruppo.Visibility = Visibility.Hidden;
            txtNomeGruppo.Visibility = Visibility.Hidden;
        }


        private void refresh()
        {
            connessioneTCP inst = connessioneTCP.getInstance();
            ListChat.Items.Clear();
            try
            {
                inst.send("RichiedoChats;" + nome + ";");
                String chats = "";
                do
                {
                    if (s.nuovoMess)
                    {
                        chats = s.m;
                        s.nuovoMess = false;
                    }
                } while (chats == "" || chats == null);
                chatList = parseClass.toList(nome, chats);
                foreach (Chat c in chatList)
                    ListChat.Items.Add(c.toString());
            }
            catch (Exception ex) { ListChat.SelectedIndex = -1; }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (ListChat.SelectedIndex != -1)
                index = ListChat.SelectedIndex;
            if (searchM == false)
            {
                reloadChat();
            }
            //reloadChat();
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

                int i = ListChat.SelectedIndex;
                inst.send("send;" + chatList[index].id + txtMess.Text);

                Utente u = (Utente)ListChat.SelectedItem;

                ListChat.Items.RemoveAt(i);
                ListChat.Items.Add(u);
            }
            if (inst.recive() == "ok")
            {
                Messaggio m = new Messaggio(nome, txtMess.Text);
                chatList[index].messaggi.Add(m);
                reloadChat();
            }


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
                    inst.send("richiedoChat;" + chatList[index].id);
                    String chat = "";
                    do
                    {
                        if (s.nuovoMess)
                        {
                            chat = s.m;
                            s.nuovoMess = false;
                        }
                    } while (chat == "" || chat == null);
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

        private void bttGruppo_Click(object sender, RoutedEventArgs e)
        {
            getUtenti();

            ListChat.Items.Clear();

            CheckBox chk;
            foreach (Utente s in tuttiUtenti)
            {
                chk = new CheckBox();
                chk.Click += aggiuntaChat;
                chk.Content = s.toString();
                chk.Name = "Id" + s.getId();
                ListChat.Items.Add(chk);
                searchM = true;
            }
        }

        List<Utente> chatGruppo;
        Boolean selezionato = false;

        private void aggiuntaChat(object sender, RoutedEventArgs e)
        {
            chatGruppo= new List<Utente>();
            CheckBox chk = e.Source as CheckBox;
            int id = int.Parse(chk.Name.ToString().Substring(2));
            chatGruppo.Add(new Utente(id,chk.Content.ToString()));
            selezionato=true;
            bttGruppoConfirm.Visibility = Visibility.Visible;
            /*bttGruppoConfirm.IsEnabled = false;
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
            }*/

        }

        int step = 0;

        private void bttGruppoConfirm_Click(object sender, RoutedEventArgs e)
        {
            connessioneTCP inst = connessioneTCP.getInstance();

            inst.send("nuovaChat" + ";" + nome + ";" + chatGruppo[0].getId());

            /*String str = "";
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
            }*/

            enableChat();
            refresh();
        }

        void getUtenti()
        {
            connessioneTCP inst = connessioneTCP.getInstance();
            inst.send("getUtenti;" + nome + ";");
            String utenti = "";
            do
            {
                if (s.nuovoMess)
                {
                    utenti = s.m;
                    s.nuovoMess = false;
                }
            } while (utenti == "" || utenti == null);
            tuttiUtenti = parseClass.toUser(utenti);
        }

        private void bttSendFile_Click(object sender, RoutedEventArgs e)
        {
            /*connessioneTCP inst = connessioneTCP.getInstance();
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    //var sr = new StreamReader(openFileDialog1.FileName);
                    inst.sendImg(openFileDialog1.FileName);
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }*/
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*getUtenti();

            ListChat.Items.Clear();
            foreach (Utente u in tuttiUtenti)
            {
                ListChat.Items.Add(u.toString());
                searchM = true;
            }*/
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("ARE YOU WANT TO CLOSE?", "CLOSING", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MessageBox.Show("Closing called");

                connessioneTCP inst = connessioneTCP.getInstance();
                inst.send("Close");
            }
        }
    }
}
