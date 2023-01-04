import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.sql.SQLException;
import java.util.Scanner;

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
        shared inst=shared.getInstance();
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
                        // break the string into message and recipient part
                        String[] st = received.split(";");
                        if (st[0].equals("RichiedoChats")) {
                            daMandare = gestoreDB.getChatNames(st[1]);
                        } else if (st[0].equals("getUtenti")) {
                            daMandare = gestoreDB.getNames(st[1]);
                        } else if (st[0].equals("richiedoChat")) {
                            daMandare = gestoreDB.getChatMex(st[1]);
                        } else if (st[0].equals("nuovaChat")) {
                            daMandare = gestoreDB.newChat(st[1], st[2]);
                        } else if (st[0].equals("send")) {
                            daMandare = gestoreDB.sendMex(st[1], st[2], st[3]);
                            /*Long idDest = Long.parseLong(gestoreDB.chatToId(st[1], st[2]));
                            MySocket tmp = inst.findDifferentSocketById(idDest);
                            if(tmp!=null)
                                tmp.out.println("messInArr;" + st[2] + ";");*/
                        }
                        this.s.out.println(daMandare);
                        System.out.println(daMandare);
                    }
                }
            } catch (IOException e) {
                e.printStackTrace();
                cicla = false;
                this.s.Close();
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
        } catch (IOException e) {
            e.printStackTrace();
        }
        // inst.removeSocket(_socket);
    }
}