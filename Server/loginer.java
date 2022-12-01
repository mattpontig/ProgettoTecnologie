import java.io.BufferedReader;
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
            // verificare se credenziali già presenti e mandare utente già registrato
            String ver = verGiaPresente(nomeU, pass);
            if (ver.equals("")) {
                FileWriter fw = new FileWriter("file/credenziali.csv", true);
                fw.write(nomeU + ";" + pass + "\n");
                fw.flush();
                fw.close();
                rispo = "ok";
            } else
                rispo = ver;

        }
        return rispo;
    }

    public static String verGiaPresente(String Uten, String pass) throws IOException {
        FileReader fr = new FileReader("file/credenziali.csv");
        BufferedReader reader = new BufferedReader(fr);
        String line, rit = "";
        while ((line = reader.readLine()) != null) {
            if (line.split(";")[0].equals(Uten) && line.split(";")[1].equals(pass))
                rit = "Utente già registrato!";
            else if (line.split(";")[0].equals(Uten))
                rit = "Nome Utente non disponibile!";
        }
        return rit;
    }

    public static String getChatNames(String string) {
        return null;
    }

}
