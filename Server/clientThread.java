import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.nio.charset.Charset;
import java.sql.SQLException;
import java.util.Scanner;

import com.mysql.cj.x.protobuf.MysqlxCrud.DataModel;

public class clientThread extends Thread {

    BufferedReader in;
    Scanner scn = new Scanner(System.in);
    private String name;
    MySocket s;
    boolean isloggedin;
    Charset utf8 = Charset.forName("UTF-8");
    s_rFile f;

    public clientThread(MySocket s, String name) throws IOException {
        in = new BufferedReader(new InputStreamReader(s.socket.getInputStream(), utf8));
        this.name = name;
        this.s = s;
        this.isloggedin = true;
    }

    @Override
    public void run() {
        shared inst = shared.getInstance();
        String received = "";
        boolean cicla = true;
        String daMandare = "";
        while (cicla) {
            try {
                // receive the string
                try {
                    received = in.readLine();
                } catch (Exception e) {
                }
                if (received != null) {
                    if (!received.equals("END")) {
                        System.out.println(received);
                        if (received.equals("Close")) {
                            this.isloggedin = false;
                            this.s.Close();

                            cicla = false;
                            break;
                        } else if (received.equals("start")) {
                            this.s.out.println("start");
                        } else if (received.startsWith("sendFile")) {
                            // break the string into message
                            String[] st = received.split(";");
                            daMandare = "ok";
                            this.s.out.println(daMandare);
                            System.out.println(daMandare);
                            f = new s_rFile(s, st[0], st[3], st[1], st[2]);
                            f.start();
                            try {
                                f.join();
                                this.s.out.println("ok");
                            } catch (InterruptedException e) {
                                // TODO Auto-generated catch block
                                e.printStackTrace();
                            }

                        } else if (received.startsWith("reciveFile")) {
                            // break the string into message
                            String[] st = received.split(";");
                            daMandare = "ok";
                            this.s.out.println(daMandare);
                            System.out.println(daMandare);
                            f = new s_rFile(s, st[0], st[1]);
                            f.start();
                            try {
                                f.join();
                            } catch (InterruptedException e) {
                                // TODO Auto-generated catch block
                                e.printStackTrace();
                            }
                        } else {
                            // break the string into message
                            String[] st = received.split(";");
                            if (st[0].equals("RichiedoChats")) {
                                daMandare = gestoreDB.getChatNames(st[1]);
                                this.s.id = gestoreDB.getIdFromName(st[1]);
                            } else if (st[0].equals("getUtenti")) {
                                daMandare = gestoreDB.getNames(st[1]);
                            } else if (st[0].equals("richiedoChat")) {
                                daMandare = gestoreDB.getChatMex(st[1], this.s.id);
                                // fare metodo per mettere che leggi la chat quando la richiedi
                            } else if (st[0].equals("nuovaChat")) {
                                daMandare = gestoreDB.newChat(st[1], st[2]);
                                if (daMandare.startsWith("ok")) {
                                    String[] daMandare2 = daMandare.split(";");
                                    String utenti = daMandare2[1];
                                    noticaCreazioneChat(utenti);
                                }
                            } else if (st[0].equals("nuovoGruppo")) {
                                daMandare = gestoreDB.newGroup(st[1], st[2], st[3]);
                                String[] daMandare2 = daMandare.split(";");
                                daMandare = daMandare2[0];
                                String utenti = daMandare2[1];
                                noticaCreazioneChat(utenti);
                            } else if (st[0].equals("send")) {
                                daMandare = gestoreDB.sendMex(st[1], st[2], st[3], "0");
                                String utenti = gestoreDB.chatToId(st[1], st[2]);
                                gestoreDB.aggiungiNonLetti(st[2], st[1]);
                                notificaUtenti(utenti, st[2]);
                            }
                            this.s.out.println(daMandare);
                            System.out.println(daMandare);
                        }
                    }
                }
            } catch (IOException e) {
                e.printStackTrace();
                cicla = false;
                this.s.Close();
                shared.getInstance().removeSocket(s); // rimuove la socket dalla lista di socket attive
            } catch (ClassNotFoundException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            } catch (SQLException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            } catch (Exception e) {
                e.printStackTrace();
                cicla = false;
                this.s.Close();
                shared.getInstance().removeSocket(s); // rimuove la socket dalla lista di socket attive
            }
        }
        try {
            // closing resources
            in.close();
            cicla = false;
            this.s.Close();
            shared.getInstance().removeSocket(s); // rimuove la socket dalla lista di socket attive
        } catch (IOException e) {
            e.printStackTrace();
        } catch (Exception e) {
            e.printStackTrace();
        }
        // inst.removeSocket(_socket);
    }

    public void notificaUtenti(String utenti, String chat) {
        String[] idSocket = utenti.split(";");
        for (int i = 0; i < idSocket.length; i++) {
            for (int j = 0; j < shared.getInstance().sockets.size(); j++) {
                if (shared.getInstance().sockets.get(j).id == Integer.parseInt(idSocket[i]))
                    shared.getInstance().sockets.get(j).out.println("messInArr;" + chat + ";");
            }
        }
    }

    public void noticaCreazioneChat(String utenti) throws ClassNotFoundException, SQLException {
        String[] idSocket = utenti.split(";");
        for (int i = 0; i < idSocket.length; i++) {
            for (int j = 0; j < shared.getInstance().sockets.size(); j++) {
                if (shared.getInstance().sockets.get(j).id == Integer.parseInt(idSocket[i])) {
                    shared.getInstance().sockets.get(j).out
                            .println("RichiedoChats;")/*
                                                       * + gestoreDB.idToName(shared.getInstance().sockets
                                                       * .get(j).id)
                                                       */;
                    System.out.println("RichiedoChats;");
                }
            }
        }
    }
}