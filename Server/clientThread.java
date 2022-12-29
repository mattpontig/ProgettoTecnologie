import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.util.Scanner;

public class clientThread extends Thread{
    
    BufferedReader in;
    Scanner scn = new Scanner(System.in);
    private String name;
    MySocket s;
    boolean isloggedin;

    public clientThread(MySocket s, String name) throws IOException{
        in =new BufferedReader(new InputStreamReader(s.socket.getInputStream()));
        this.name = name;
        this.s = s;
        this.isloggedin = true;
    }

    @Override
    public void run() {

        String received;
        boolean cicla = true;
        while (cicla) {
            try {
                // receive the string
                received = in.readLine();
                System.out.println(received);
                if (received.equals("Close")) {
                    this.isloggedin = false;
                    this.s.Close();;
                    cicla = false;
                    break;
                }

                if (received.equals("start")) {
                    this.s.out.println("start");
                } else {

                    // break the string into message and recipient part
                    String[] st = received.split(";");
                }
                // toDo: metodi a seconda del messaggio

                // search for the client in the connected devices list.
                // ar is the vector storing client of active users
                // for (ClientHandler mc : serverTCP.ar) {
                // // // if the client is found, write on its
                // // // output stream
                // if (mc.name.equals(/*nome destinatario messaggio */)) {
                // mc.dos.println(this.name + " : ciao" /*+ MsgToSend*/);
                // break;
                // }
                // }
            } catch (IOException e) {
                e.printStackTrace();
                cicla = false;
                this.s.Close();
            }
        }
        try {
            // closing resources
            in.close();
        } catch (IOException e) {
            e.printStackTrace();
        }

        //inst.removeSocket(_socket);

    }

    
}