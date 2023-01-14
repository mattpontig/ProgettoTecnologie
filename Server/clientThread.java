import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.sql.SQLException;
import java.util.Scanner;

import com.mysql.cj.x.protobuf.MysqlxCrud.DataModel;

public class clientThread extends Thread {

    BufferedReader in;
    Scanner scn = new Scanner(System.in);
    private String name;
    MySocket s;
    boolean isloggedin;

    public clientThread(MySocket s, String name) throws IOException {
        in = new BufferedReader(new InputStreamReader(s.socket.getInputStream()));
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
                received = in.readLine();
                if (received != null) {
                    System.out.println(received);
                    if (received.equals("Close")) {
                        this.isloggedin = false;
                        this.s.Close();

                        cicla = false;
                        break;
                    } else if (received.equals("END")) {
                    } else if (received.equals("start")) {
                        this.s.out.println("start");
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
                            //daMandare = gestoreDB.newChat(st[1], st[2]);
                            String[] daMandare2 = gestoreDB.newChat(st[1], st[2]).split(";");
                            String utenti = daMandare2[1];
                            noticaCreazioneChat(utenti);
                        } else if (st[0].equals("nuovoGruppo")) {
                            //daMandare = gestoreDB.newGroup(st[1], st[2], st[3]);
                            String[] daMandare2 = gestoreDB.newGroup(st[1], st[2], st[3]).split(";");
                            daMandare = daMandare2[0];
                            String utenti = daMandare2[1];
                            noticaCreazioneChat(utenti);
                        } else if (st[0].equals("send")) {
                            daMandare = gestoreDB.sendMex(st[1], st[2], st[3]);
                            String utenti = gestoreDB.chatToId(st[1], st[2]);
                            gestoreDB.aggiungiNonLetti(st[2], st[1]);
                            notificaUtenti(utenti, st[2]);
                            /*
                             * // read the filename from the client
                             * String filename = inFromClient.readUTF();
                             * // create a new filename by prefixing "renamed_" to the original filename
                             * String newFilename = "renamed_" + filename;
                             * 
                             * // specify the folder path
                             * String folderPath = "/path/to/folder/";
                             * // check if the folder exists, if not create it
                             * File folder = new File(folderPath);
                             * if (!folder.exists()) {
                             * folder.mkdir();
                             * }
                             * 
                             * // create a FileOutputStream to write the file to the specified folder
                             * FileOutputStream fos = new FileOutputStream(folderPath + newFilename);
                             * // buffer to read the data from the input stream
                             * byte[] buffer = new byte[4096];
                             * // read the data from the input stream
                             * int read = 0;
                             * while((read = inFromClient.read(buffer)) > 0) {
                             * // write the data to the file
                             * fos.write(buffer, 0, read);
                             * }
                             * // close the FileOutputStream
                             * fos.close();
                             */
                        }
                        this.s.out.println(daMandare);
                        System.out.println(daMandare);
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
                if (shared.getInstance().sockets.get(j).id == Integer.parseInt(idSocket[i]))
                    shared.getInstance().sockets.get(j).out
                            .println("RichiedoChats;" + gestoreDB.idToName(shared.getInstance().sockets
                                    .get(j).id));
            }
        }
    }
}