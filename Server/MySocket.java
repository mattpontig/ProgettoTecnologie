import java.io.BufferedWriter;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.Socket;

public class MySocket {
    static long idCounter = 0;
    public long id;
    public Socket socket;
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

    public void Close() {
        try {
            socket.close();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }

}
