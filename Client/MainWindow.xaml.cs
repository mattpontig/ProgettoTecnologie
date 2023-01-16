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
        List<Chat> chatsFiltro;
        List<Utente> tuttiUtenti = null;
        Window1 w;
        bool searchM;
        ClientSocket s;
        OpenFileDialog openFileDialog1;
        Thread clientSocket;
        Thread nuovoMess;
        Thread nuovaChat;
        TextBox txt;
        Button b;

        public MainWindow()
        {
            InitializeComponent();
            txtSearch.Text = "";
            tuttiUtenti = new List<Utente>();
            Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
            enableChat();
            bttSend.Visibility = Visibility.Hidden;
            txtMess.Visibility = Visibility.Hidden;
            bttSendFile.Visibility = Visibility.Hidden;
            txt = new TextBox();

            w = new Window1();
            w.ShowDialog();
            if (w.nomeUtente == "")
            {
                this.Close();
            }
            else
            {
                nome = w.nomeUtente;
                index = -1;
                searchM = false;
            }
            s = new ClientSocket(8080);
            clientSocket = new Thread(new ThreadStart(s.run));
            clientSocket.Start();

            nuovoMess = new Thread(new ThreadStart(controlloMess));
            nuovoMess.Start();

            nuovaChat = new Thread(new ThreadStart(controlloNewChat));
            nuovaChat.Start();
            refresh();
        }

        private void scaricaFile(object sender, RoutedEventArgs e)
        {
            connessioneTCP inst = connessioneTCP.getInstance();
            Button b = e.Source as Button;
            inst.reciveFile(b.Content.ToString());
        }

        void enableChat()
        {
            bttGruppoConfirm.Visibility = Visibility.Hidden;
            labelGruppo.Visibility = Visibility.Hidden;
            txtNomeGruppo.Visibility = Visibility.Hidden;
            bttIndietro.Visibility = Visibility.Hidden;

            bttSendFile.Visibility = Visibility.Visible;
            ListChat.Visibility = Visibility.Visible;
        }

        void controlloMess()
        {
            String str = "";
            while (true)
            {
                if (s.messaggioCoda)
                {
                    str = s.m;
                    notificaNuovoMess(str);
                    s.messaggioCoda = false;
                }
            }
        }

        void controlloNewChat()
        {
            String str = "";
            while (true)
            {
                if (s.nuovaChat)
                {
                    //str = s.m;
                    chatList = null;
                    this.Dispatcher.Invoke(() => { refresh(); });
                    s.nuovaChat = false;
                }
            }
        }

        void notificaNuovoMess(String str)
        {
            String[] chat = str.Split(';');
            int idChat = int.Parse(chat[1]);
            Chat c = new Chat();
            int i = 0, idChatPrelevare = 0;
            foreach (Chat cht in chatList)
            {
                if (cht.id == idChat)
                {
                    c = cht;
                    idChatPrelevare = i;
                }
                i++;
            }

            this.Dispatcher.Invoke(() => { addMessNonLetti(idChatPrelevare); });

            Chat u = chatList[idChatPrelevare];

            chatList.RemoveAt(idChatPrelevare);
            chatList.Insert(0, u);
            this.Dispatcher.Invoke(() => { refresh(); });
            if (index != -1)
            {
                if (ListChat.Items[index].ToString() == chatList[idChatPrelevare].getName())
                    this.Dispatcher.Invoke(() => { reloadChat(); });
            }
            //refresh();


        }

        public void addMessNonLetti(int id)
        {
            bool nonSelezionato = false;
            if (index == -1)
                nonSelezionato = true;
            else if (ListChat.Items[index].ToString() != chatList[id].getName())
                nonSelezionato = true;
            if (nonSelezionato)
                chatList[id].messNonLetti++;
        }

        public void refresh()
        {
            connessioneTCP inst = connessioneTCP.getInstance();
            ListChat.Items.Clear();
            if (chatList == null)
            {
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
                    } while (chats == "" || chats == null || chats.StartsWith("ok") || chats.StartsWith("chat"));
                    chatList = parseClass.toList(nome, chats);
                    chatsFiltro = chatList;
                }
                catch (Exception ex) { /*ListChat.SelectedIndex = -1; */}
            }
            foreach (Chat c in chatsFiltro)
                ListChat.Items.Add(c.toString());
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            chatsFiltro = new List<Chat>();
            if (txtSearch.Text == "")
                chatsFiltro = chatList;
            else if (txtSearch.Text != "")
            {
                foreach (Chat c in chatList)
                {
                    if (c.titolo != "")
                    {
                        if (c.titolo.ToLower().StartsWith(txtSearch.Text))
                            chatsFiltro.Add(c);
                    }
                    else if (c.titolo == "")
                    {
                        if (c.utenti[0].ToLower().StartsWith(txtSearch.Text))
                            chatsFiltro.Add(c);
                    }
                }
            }

            refresh();
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListChat.SelectedIndex != -1)
            {
                index = ListChat.SelectedIndex;
                bttSend.Visibility = Visibility.Hidden;
                txtMess.Visibility = Visibility.Hidden;
            }
            if (searchM == false)
            {
                bttSend.Visibility = Visibility.Visible;
                txtMess.Visibility = Visibility.Visible;
                bttSendFile.Visibility = Visibility.Visible;

                reloadChat();
            }
        }

        public static String getNome()
        {
            return nome;
        }

        private void bttSend_Click(object sender, RoutedEventArgs e)
        {
            connessioneTCP inst = connessioneTCP.getInstance();
            inst.send("send;" + nome + ";" + chatList[index].id + ";" + txtMess.Text);
            Chat u = chatList[index];
            chatList.RemoveAt(index);
            chatList.Insert(0, u);
            index = 0;
            txtMess.Text = "";
            refresh();
            this.Dispatcher.Invoke(() => { reloadChat(); });
            //ListChat.SelectedIndex = -1;
        }

        public void reloadChat()
        {
            connessioneTCP inst = connessioneTCP.getInstance();
            if (index != -1)
            {
                if (chatList[index].messNonLetti != 0)
                {
                    chatList[index].messNonLetti = 0;
                    refresh();
                }
                try
                {
                    //if (stessaChat == false || chatList[index].messaggi == null)
                    {
                        inst.send("richiedoChat;" + chatsFiltro[index].id);
                        String chat = "";
                        do
                        {
                            if (s.nuovoMess)
                            {
                                chat = s.m;
                                s.nuovoMess = false;
                            }
                        } while (chat == "" || chat == null || chat.StartsWith("ok"));
                        chatList[index].messaggi = parseClass.toChat(chat);
                    }


                    SingleChat.Items.Clear();
                    b = new Button();
                    b.MouseDoubleClick += scaricaFile;

                    List<Messaggio> chatMess = chatList[index].messaggi;
                    for (int i = 0; i < chatMess.Count; i++)
                    {
                        if (chatMess[i].messaggio != " ")
                        {
                            if (nome == chatMess[i].nome)
                            {
                                if (chatMess[i].file == 0)
                                {
                                    txt.Text = chatMess[i].toMessHost();
                                    SingleChat.Items.Add(txt.Text);
                                }
                                else if (chatMess[i].file == 1)
                                {
                                    b.Content = chatMess[i].toMessHost();
                                    SingleChat.Items.Add(b.Content);
                                }
                            }
                            else
                            {
                                if (chatMess[i].file == 0)
                                {
                                    txt.Text = chatMess[i].toMessGuest();
                                    SingleChat.Items.Add(txt.Text);
                                }
                                else if (chatMess[i].file == 1)
                                {
                                    b.Content = chatMess[i].toMessGuest();
                                    SingleChat.Items.Add(b.Content);
                                }
                            }
                        }
                    }

                    SingleChat.Items.Add("");
                    //ListChat.SelectedIndex = -1;

                    SingleChat.SelectedIndex = SingleChat.Items.Count - 1;
                    SingleChat.ScrollIntoView(SingleChat.SelectedItem);
                    SingleChat.SelectedIndex = -1;
                }

                catch (Exception e)
                {
                    this.Dispatcher.Invoke(() => { reloadChat(); });
                }
            }
        }

        private void bttGruppo_Click(object sender, RoutedEventArgs e)
        {
            getUtenti();

            bttIndietro.Visibility = Visibility.Visible;

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

        List<Utente> chatGruppo = new List<Utente>();

        private void aggiuntaChat(object sender, RoutedEventArgs e)
        {
            CheckBox chk = e.Source as CheckBox;
            int id = int.Parse(chk.Name.ToString().Substring(2));
            if (chk.IsChecked == true)
            {
                chatGruppo.Add(new Utente(id, chk.Content.ToString()));
            }
            else if (chk.IsChecked == false)
            {
                int i = -1;
                int j = 0;
                foreach (Utente s in chatGruppo)
                {
                    if (s.getId() == id)
                        i = j;
                    j++;
                }
                chatGruppo.RemoveAt(i);
            }
            if (chatGruppo.Count > 0)
                bttGruppoConfirm.Visibility = Visibility.Visible;
            else if (chatGruppo.Count == 0)
                bttGruppoConfirm.Visibility = Visibility.Hidden;
        }

        int step = 0;

        private void bttGruppoConfirm_Click(object sender, RoutedEventArgs e)
        {
            connessioneTCP inst = connessioneTCP.getInstance();

            if (chatGruppo.Count() == 1)
            {
                inst.send("nuovaChat" + ";" + nome + ";" + chatGruppo[0].getId());

                searchM = false;
                chatList = null;
                refresh();
                chatGruppo = new List<Utente>();
                enableChat();
                index = -1;

            }
            else
            {
                String str = "";
                if (step == 0)
                {
                    labelGruppo.Visibility = Visibility.Visible;
                    txtNomeGruppo.Visibility = Visibility.Visible;
                    ListChat.Visibility = Visibility.Hidden;
                    step++;
                }
                else if (step == 1)
                {
                    step = 0;

                    labelGruppo.Visibility = Visibility.Hidden;
                    txtNomeGruppo.Visibility = Visibility.Hidden;
                    ListChat.Visibility = Visibility.Visible;

                    bttGruppoConfirm.Visibility = Visibility.Visible;
                    bttGruppo.Visibility = Visibility.Visible;

                    String ss = "";
                    foreach (Utente te in chatGruppo)
                        ss += te.toString() + "-";
                    inst.send("nuovoGruppo" + ";" + nome + ";" + txtNomeGruppo.Text + ";" + ss + ";");

                    searchM = false;
                    chatList = null;
                    refresh();
                    chatGruppo = new List<Utente>();
                    enableChat();
                    index = -1;
                }
            }
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
            connessioneTCP inst = connessioneTCP.getInstance();
            String risp = "";
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == true)
            {
                String nameFile = System.IO.Path.GetFileName(openFileDialog1.FileName);
                inst.send("sendFile;" + nome + ";" + chatList[index].id + ";" + nameFile);
                do
                {
                    risp = s.m;
                } while (risp == "" || risp == null || risp.StartsWith("ok") == false);

                if (risp.StartsWith("ok"))
                {
                    try
                    {
                        inst.sendFile(openFileDialog1.FileName);
                    }
                    catch (SecurityException ex)
                    {
                        MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                        $"Details:\n\n{ex.StackTrace}");
                    }
                }
            }
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("ARE YOU WANT TO CLOSE?", "CLOSING", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                connessioneTCP inst = connessioneTCP.getInstance();
                inst.send("Close");
                s.socket.Close();
                clientSocket.Abort();
                nuovoMess.Abort();
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void bttIndietro_Click(object sender, RoutedEventArgs e)
        {
            enableChat();
            refresh();
            chatGruppo = new List<Utente>();
            searchM = false;
        }
    }
}
