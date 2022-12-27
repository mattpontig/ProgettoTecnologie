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
}
