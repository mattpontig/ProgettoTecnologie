import java.io.IOException;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;

public class shared {

    private static shared _instance = null;

    public static shared getInstance() {
        if (_instance == null)
            _instance = new shared();

        return _instance;
    }

    private shared() {
    }

    List<MySocket> sockets = new ArrayList<MySocket>();

    public boolean addSocket(MySocket s) throws IOException {
        sockets.add(s);
        return true;

    }

    public MySocket findSocketById(long id) {
        for (MySocket mySocket : sockets) {
            if (mySocket.id == id)
                return mySocket;
        }
        return null;
    }

    /**
     * ritorna la prima socket che non è uguale all'id specificato
     * 
     * @param id
     * @return
     */
    public MySocket findDifferentSocketById(long id) {
        for (MySocket mySocket : sockets) {
            if (mySocket.id != id)
                return mySocket;
        }
        return null;
    }

    public void removeSocket(Socket socket) {
        long id = getId(socket);
        if (id != -1)
            removeSocket(id);
    }

    public void removeSocket(long id) {
        MySocket toRemove = null;
        for (MySocket mySocket : sockets) {
            if (mySocket.id == id) {
                toRemove = mySocket;
            }
        }

        if (toRemove != null) {
            removeSocket(toRemove);
        }

    }

    public void removeSocket(MySocket mysocket) {
        try {
            mysocket.socket.close(); // la provo a chiudere
        } catch (Exception e) {
        }
        sockets.remove(mysocket);
    }

    public long getId(Socket socket) {
        for (MySocket mySocket : sockets) {
            if (mySocket.socket == socket) {
                return mySocket.id;
            }
        }
        return -1;
    }

    public long[] getAllIds() {
        int size = sockets.size();
        long[] ids = new long[size];
        for (int i = 0; i < size; i++) {
            ids[i] = sockets.get(i).id;
        }
        return ids;
    }

}
