import java.io.*;
import java.nio.charset.Charset;
import java.sql.SQLException;
import java.util.*;
import java.net.*;
import java.util.Arrays;

public class s_rFile extends Thread {
    private InputStream in = null;
    Scanner scn;
    private String daFare, nomeFile, chi, chat;
    MySocket s;
    Charset utf8 = Charset.forName("UTF-8");

    public s_rFile(MySocket s, String daFare, Scanner scn, String nomeFile, String chi, String chat)
            throws IOException {
        this.scn = scn;
        this.daFare = daFare;
        this.s = s;
        this.nomeFile = nomeFile;
        this.chi = chi;
        this.chat = chat;
        in = new DataInputStream(s.socket.getInputStream());
    }

    @Override
    public void run() {

        if (daFare.equals("sendFile")) {
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            byte buffer[] = new byte[2147483647];
            try {
                baos.write(buffer, 0, in.read(buffer));
            } catch (IOException e1) {
                // TODO Auto-generated catch block
                e1.printStackTrace();
            }

            byte result[] = baos.toByteArray();

            try {
                writeFile(result);
            } catch (IOException | ClassNotFoundException | SQLException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }

            /*
             * try {
             * gestoreDB.sendFileReg(chi, chat, nomeFile);
             * } catch (ClassNotFoundException | SQLException e) {
             * // TODO Auto-generated catch block
             * e.printStackTrace();
             * };
             */
        }
    }

    public void writeFile(byte[] result)
            throws FileNotFoundException, IOException, ClassNotFoundException, SQLException {
        try (FileOutputStream fos = new FileOutputStream("file/" + nomeFile)) {
            try {
                fos.write(result);
            } catch (IOException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
            // fos.close(); There is no more need for this line since you had created the
            // instance of "fos" inside the try. And this will automatically close the
            // OutputStream
        }
        gestoreDB.sendMex(chi, chat, nomeFile, "1");
    }

    public void provaSend(/* forse nome file */) {
        BufferedOutputStream outToClient = null;
        try {

            outToClient = new BufferedOutputStream(s.socket.getOutputStream());
        } catch (

        IOException ex) {
            // Do exception handling
        }

        if (outToClient != null) {
            File myFile = new File(/* file da mandare */);
            byte[] mybytearray = new byte[(int) myFile.length()];

            FileInputStream fis = null;

            try {
                fis = new FileInputStream(myFile);
            } catch (FileNotFoundException ex) {
                // Do exception handling
            }
            BufferedInputStream bis = new BufferedInputStream(fis);

            try {
                bis.read(mybytearray, 0, mybytearray.length);
                outToClient.write(mybytearray, 0, mybytearray.length);
                outToClient.flush();
                outToClient.close();

                // File sent, exit the main method
            } catch (IOException ex) {
                // Do exception handling
            }
        }
    }
}