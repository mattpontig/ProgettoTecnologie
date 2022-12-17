import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.sql.SQLException;

public class loginer {

    public static String verifica(boolean tipo, String nomeU, String pass) throws IOException {
        String rispo = "0";
        if (tipo == true) {
            try {
                rispo = gestoreDB.verificaLogin(nomeU, pass);
            } catch (ClassNotFoundException | SQLException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
        }
        if (tipo == false) {
            try {
                rispo = gestoreDB.registrazione(nomeU, pass);
            } catch (ClassNotFoundException | SQLException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
        }
        return rispo;
    }

    public static String getChatNames(String string) throws IOException {
        FileReader fr = new FileReader("file/chatUtenti.csv");
        BufferedReader reader = new BufferedReader(fr);
        String line, stringaChat = "";
        while ((line = reader.readLine()) != null) {
            stringaChat += line;
        }
        return stringaChat;
    }

}
