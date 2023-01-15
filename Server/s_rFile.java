import java.io.*;
import java.nio.charset.Charset;
import java.util.*;

public class s_rFile extends Thread {
    BufferedReader in;
    Scanner scn;
    private String daFare;
    MySocket s;
    Charset utf8 = Charset.forName("UTF-8");

    public s_rFile(MySocket s, String daFare, BufferedReader in, Scanner scn) {
        this.in = in;
        this.scn = scn;
        this.daFare = daFare;
        this.s = s;
    }

    @Override
    public void run() {

        if (daFare.equals("sendFile")) {
            while (true) {
                while (true) {
                    //we read header first

                    // no need foe a while statement here:
                    bytesRead = in.read(input, 0, headerSize);
                    Byte.parseByte()
                    // if you are going to use a while statement, then in each loop
                    // you should be processing the input but because it will get overwritten on the next read.
                    String resString = new String(input, utf8);
                    System.out.println(resString);

                    if (resString.equals("!$$$")) {
                        break;
                    }
                }
            }
        }
    }
}