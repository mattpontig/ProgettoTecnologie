import java.io.*;
import java.nio.charset.*;
import java.nio.file.*;
import java.sql.*;
import java.util.*;
import java.net.*;
import java.util.Arrays;

import com.mysql.cj.xdevapi.DocFilterParams;

public class s_rFile extends Thread {
    private InputStream in = null;
    private OutputStream out = null;
    private String daFare, nomeFile, chi, chat;
    MySocket s;
    Charset utf8 = Charset.forName("UTF-8");

    public s_rFile(MySocket s, String daFare, String nomeFile, String chi, String chat)
            throws IOException {
        this.daFare = daFare;
        this.s = s;
        this.nomeFile = nomeFile;
        this.chi = chi;
        this.chat = chat;
        in = new DataInputStream(s.socket.getInputStream());
    }

    public s_rFile(MySocket s, String daFare, String nomeFile)
            throws IOException {
        this.daFare = daFare;
        this.s = s;
        this.nomeFile = nomeFile;
    }

    @Override
    public void run() {

            if (daFare.equals("sendFile")) {
                ByteArrayOutputStream baos = new ByteArrayOutputStream();
                byte buffer[] = new byte[1024];
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
            } else if (daFare.equals("reciveFile")) {
                try {
                    provaSend(nomeFile);
                } catch (IOException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                }
            }
    }

    public void writeFile(byte[] result)
            throws FileNotFoundException, IOException, ClassNotFoundException, SQLException {
        try (FileOutputStream fos = new FileOutputStream("file/" + nomeFile)) {
            try {
                fos.write(result);
                fos.flush();
                fos.close();
            } catch (Exception e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
            // fos.close(); There is no more need for this line since you had created the
            // instance of "fos" inside the try. And this will automatically close the
            // OutputStream
        }
        gestoreDB.sendMex(chi, chat, nomeFile, "1");
    }

    public void provaSend(String nomeFile) throws IOException {
        // Create a byte array of the same length as the file 
        //File f = new File("file/" + nomeFile);

        //byte[] byteArray = new byte[(int)f.length()];

        FileInputStream fis = new FileInputStream("file/" + nomeFile); 
        // Get the output stream of the socket 
        OutputStream os = s.socket.getOutputStream(); 

        // Create a buffer 
        //byte[] buffer = new byte[1024]; 
        // Create a byte array 
        byte[] array = new byte[(int)fis.available()]; 

        // Read bytes from file into buffer 
        int bytesRead = fis.read(array); ; 

        fis.close();

        System.out.println(array);
        // Write the bytes to the output stream
        os.write(array, 0, bytesRead); 


        // Flush the output stream and close the socket
        os.flush();
        //s.socket.close();

        /*OutputStream  outToClient = null;
        
        try {
        
            outToClient = s.socket.getOutputStream();
        } catch (
        
        IOException ex) {
            // Do exception handling
        }
        
        if (outToClient != null) {
        
            Path path = Paths.get("file/" + nomeFile);
        
            byte[] bytes = Files.readAllBytes(path);
        
            FileInputStream fis = null;
        
            try {
                fis = new FileInputStream("/file/" + nomeFile);
            } catch (FileNotFoundException ex) {
                // Do exception handling
            }
            BufferedInputStream bis = new BufferedInputStream(fis);
        
            try {
                //bis.read(bytes, 0, bytes.length);
                outToClient.write(bytes, 0, bytes.length);
                outToClient.flush();
                outToClient.close();
                
                //outToClient.close();
        
                // File sent, exit the main method
            } catch (Exception ex) {
                // Do exception handling
            }
        }*/

    }
}