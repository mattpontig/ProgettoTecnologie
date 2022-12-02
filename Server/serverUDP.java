import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;

public class serverUDP {
    static DatagramSocket socket;
    static byte[] buffer;
    static DatagramPacket packet;

    public static void avviaServerUDP() throws IOException {
        boolean flag = true;
        socket = new DatagramSocket(12345);
        while (flag) {
            buffer = new byte[1500];
            packet = new DatagramPacket(buffer, buffer.length);
            socket.receive(packet);
            String messaggio = new String(packet.getData());
            String[] mexSplit = messaggio.split(";");
            String risposta = "";
            if (mexSplit[0].equals("Login")) {
                risposta = loginer.verifica(true, mexSplit[1], mexSplit[2]);
                invia(risposta);
                if (risposta.equals("1")) {
                    // risposta = loginer.getChatNames(mexSplit[1]);
                    // invia(risposta);
                    flag = false;
                }
            }
            if (mexSplit[0].equals("Register")) {
                risposta = loginer.verifica(false, mexSplit[1], mexSplit[2]);
                invia(risposta);
            }
            if (flag == false)
                socket.close();
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
