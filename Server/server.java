import java.io.IOException;

public class server {

    public static void main(String[] args) throws IOException {
        serverUDP.avviaServerUDP();
        serverTCP.avvia();
    }
}