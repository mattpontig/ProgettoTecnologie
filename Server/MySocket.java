import java.io.BufferedWriter;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.Socket;

public class MySocket {
    static long idCounter = 0;
    long id;
    Socket socket;
    PrintWriter out;

    public MySocket(Socket socket) throws IOException {
        id = idCounter;
        idCounter++;
        this.socket = socket;
        out = new PrintWriter(new BufferedWriter(new OutputStreamWriter(socket.getOutputStream())), true);
    }

    @Override
    public boolean equals(Object obj) {
        if (obj == null)
            return false;

        if (!(obj instanceof MySocket))
            return false;

        MySocket tmp = (MySocket) obj;
        return tmp.id == id;
    }
}
