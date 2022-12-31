import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

public class serverTCP extends Thread {
    public final static int port = 8080;

    public static void avvia() throws IOException {
        // server is listening on port 8080
        ServerSocket ss = new ServerSocket(port);
        Socket s;
        int i = 0;

        shared inst = shared.getInstance();
        // running infinite loop for getting
        // client request
        while (true) {
            // Accept the incoming request
            try {
                s = ss.accept();
                System.out.println("New client request received : " + s);
                // obtain input and output streams

                MySocket ms = new MySocket(s);

                if (inst.addSocket(ms)) // solo se le ho aggiunte ( ovvero c'era posto )
                {
                    clientThread ct = new clientThread(ms, "client " + i);
                    ct.start();
                }

            } catch (IOException e) {
                // errore ... fa niente, ritorno ad aspettare
            }

            /*
             * System.out.println("Creating a new handler for this client...");
             * // Create a new handler object for handling this request.
             * ClientHandler mtch = new ClientHandler(ms, "client " + i, dis, dos);
             * // Create a new Thread with this object.
             * Thread t = new Thread(mtch);
             * System.out.println("Adding this client to active client list");
             * // add this client to active clients list
             * ar.add(mtch);
             * // start the thread.
             * t.start();
             * // increment i for new client.
             * // i is used for naming only, and can be replaced
             * // by any naming scheme
             */
            i++;
        }
    }
}