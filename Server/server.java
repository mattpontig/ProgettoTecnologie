import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;

public class server {
    static DatagramSocket socket;
    static byte[] buffer = new byte[1500];
    static DatagramPacket packet = new DatagramPacket(buffer, buffer.length);

    public static void main(String[] args) throws IOException {
        boolean flag = true;
        while (flag) {
            socket = new DatagramSocket(12345);
            socket.receive(packet);
            String messaggio = new String(packet.getData());
            String[] mexSplit = messaggio.split(";");
            String risposta = "";
            if (mexSplit[0].equals("Login")) {
                risposta = loginer.verifica(true, mexSplit[1], mexSplit[2]);
                invia(risposta);
                if (risposta.equals("1")) {
                    // risposta=loginer.getChatNames(mexSplit[1]);
                    // invia(risposta);
                }
            }
            if (mexSplit[0].equals("Register")) {
                risposta = loginer.verifica(false, mexSplit[1], mexSplit[2]);
                invia(risposta);
            }
            flag = false;
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