import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

public class serverTCP {
    public final static int port = 8080;
    // Vector to store active clients
    static List<ClientHandler> ar = new ArrayList<ClientHandler>();
    // counter for clients
    static int i = 0;

    public static void avvia() throws IOException {
        // server is listening on port 8080
        ServerSocket ss = new ServerSocket(port);
        Socket s;
        // running infinite loop for getting
        // client request
        while (true) {
            // Accept the incoming request
            s = ss.accept();
            System.out.println("New client request received : " + s);
            // obtain input and output streams
            BufferedReader dis = new BufferedReader(new InputStreamReader(s.getInputStream()));
            PrintWriter dos = new PrintWriter(new BufferedWriter(new OutputStreamWriter(s.getOutputStream())),
                    true);
            System.out.println("Creating a new handler for this client...");
            // Create a new handler object for handling this request.
            ClientHandler mtch = new ClientHandler(s, "client " + i, dis, dos);
            // Create a new Thread with this object.
            Thread t = new Thread(mtch);
            System.out.println("Adding this client to active client list");
            // add this client to active clients list
            ar.add(mtch);
            // start the thread.
            t.start();
            // increment i for new client.
            // i is used for naming only, and can be replaced
            // by any naming scheme
            i++;
        }
    }
}

// ClientHandler class
class ClientHandler implements Runnable {
    Scanner scn = new Scanner(System.in);
    private String name;
    final BufferedReader dis;
    final PrintWriter dos;
    Socket s;
    boolean isloggedin;

    // constructor
    public ClientHandler(Socket s, String name,
            BufferedReader dis2, PrintWriter dos2) {
        this.dis = dis2;
        this.dos = dos2;
        this.name = name;
        this.s = s;
        this.isloggedin = true;
    }

    @Override
    public void run() {
        String received;
        while (true) {
            try {
                // receive the string
                received = dis.readLine();
                System.out.println(received);
                if (received.equals("Close")) {
                    this.isloggedin = false;
                    this.s.close();
                    break;
                }
                // break the string into message and recipient part
                String[] st = received.split(";");
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
            }
        }
        try {
            // closing resources
            this.dis.close();
            this.dos.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}