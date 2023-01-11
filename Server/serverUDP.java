import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.SocketException;
import java.sql.SQLException;

public class serverUDP extends Thread {
    static DatagramSocket socket;
    static byte[] buffer;
    static DatagramPacket packet;

    public void run() {
        boolean flag = true;
        try {
            socket = new DatagramSocket(12345);
        } catch (SocketException e1) {
            // TODO Auto-generated catch block
            e1.printStackTrace();
        }
        while (flag) {
            buffer = new byte[1500];
            packet = new DatagramPacket(buffer, buffer.length);
            try {
                socket.receive(packet);
            } catch (IOException e1) {
                // TODO Auto-generated catch block
                e1.printStackTrace();
            }
            String messaggio = new String(packet.getData());
            String[] mexSplit = messaggio.split(";");
            String risposta = "";
            if (mexSplit[0].equals("Login")) {
                try {
                    risposta = loginer.verifica(true, mexSplit[1], mexSplit[2]);
                } catch (IOException e1) {
                    // TODO Auto-generated catch block
                    e1.printStackTrace();
                }
                try {
                    invia(risposta);
                } catch (IOException e1) {
                    // TODO Auto-generated catch block
                    e1.printStackTrace();
                }
                // if (risposta.equals("1")) {
                // try {
                // risposta = gestoreDB.getChatNames(mexSplit[1]);
                // } catch (ClassNotFoundException | SQLException e) {
                // // TODO Auto-generated catch block
                // e.printStackTrace();
                // }
                // try {
                // invia(risposta);
                // } catch (IOException e) {
                // // TODO Auto-generated catch block
                // e.printStackTrace();
                // }
                // }
            }
            if (mexSplit[0].equals("Register")) {
                try {
                    risposta = loginer.verifica(false, mexSplit[1], mexSplit[2]);
                } catch (IOException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                }
                try {
                    invia(risposta);
                } catch (IOException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                }
            }
        }
    }

    public static void invia(String risp) throws IOException {
        // ottenere Porta dal messaggio ricevuto
        int portaMittente = packet.getPort();
        // invio
        buffer = risp.getBytes();
        packet = new DatagramPacket(buffer, buffer.length);
        packet.setAddress(InetAddress.getByName("127.0.0.1")); // IP/nome del destinatario
        packet.setPort(portaMittente); // porta del destinatario
        socket.send(packet);
    }
}
