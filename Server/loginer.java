import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;

public class loginer {

    public static String verifica(boolean tipo, String nomeU, String pass) throws IOException {
        String rispo = "0";
        if (tipo == true) {
            FileReader fr = new FileReader("file/credenziali.csv");
            BufferedReader reader = new BufferedReader(fr);
            String line;
            while ((line = reader.readLine()) != null) {
                if ((line.split(";")[0]).equals(nomeU) && (line.split(";")[1]).equals(pass))
                    rispo = "1";
            }
            fr.close();
        }
        if (tipo == false) {
            // File file = new File("file/credenziali.csv");
            // vverificare se credenziali già presenti e m andare utente già registrato
            FileWriter fw = new FileWriter("file/credenziali.csv", true);
            fw.write(nomeU + ";" + pass + "\n");
            fw.flush();
            fw.close();
            rispo = "ok";
        }
        return rispo;
    }

    public static String getChatNames(String string) {
        return null;
    }

}
