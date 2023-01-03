import java.sql.*;

public class gestoreDB {

    public static void connetti() {
        try {
            Class.forName("com.mysql.cj.jdbc.Driver");
            Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram", "root", "");
            // Statement stmt = con.createStatement();
            // ResultSet rs = stmt.executeQuery("select * from login");
            // while (rs.next())
            // System.out.println(rs.getInt(1) + " " + rs.getString(2) + " " +
            // rs.getString(3));
            // con.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public static String verificaLogin(String nomeU, String pass)
            throws ClassNotFoundException, SQLException {
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram", "root", "");

        Statement stmt = con.createStatement();
        ResultSet rs = stmt
                .executeQuery("select * from login where user='" + nomeU + "' and pass='" + MD5.getMd5(pass) + "'");
        if (rs.next()) {
            con.close();
            return "1";
        } else {
            con.close();
            return "0";
        }
    }

    public static String registrazione(String nomeU, String pass) throws ClassNotFoundException, SQLException {
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram", "root", "");

        Statement stmt = con.createStatement();
        ResultSet rs = stmt
                .executeQuery("select * from login where user='" + nomeU + "' and pass='" + MD5.getMd5(pass) + "'");
        if (!rs.next()) {
            stmt = con.createStatement();
            stmt.executeUpdate("insert into login (user,pass) values('" + nomeU + "','" + MD5.getMd5(pass) + "')");
            con.close();
            return "ok";
        } else {
            con.close();
            return "Utente già registrato!";
        }
    }

    public static String getChatNames(String string) throws SQLException, ClassNotFoundException {
        String ris = "";
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram", "root", "");

        /*
         * query per avere tutti i nomi dei gruppi che sono in contatto con string che
         * sarebbe l'user inserito nel login
         */
        Statement stmt = con.createStatement();
        ResultSet rs = stmt
                .executeQuery(
                        "select distinct c.titolo, c.idChat, lo.user from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat where not c.titolo='' and not lo.user='"
                                + string
                                + "' and uc.idChat in (select uc.idChat from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat where user='"
                                + string + "')");
        while (rs.next()) {
            if (ris.contains(Integer.toString(rs.getInt(2))))
                ris += "," + rs.getString(3);
            else
                ris += ";" + rs.getInt(2) + ",g," + rs.getString(1) + "," + rs.getString(3);
        }

        /* query per avere tutti i nomi dei singoli che sono in contatto con pippo */
        stmt = con.createStatement();
        rs = stmt
                .executeQuery(
                        "select user,c.idChat from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat where not lo.user='"
                                + string
                                + "' and uc.idChat in (select uc.idChat from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat where user='"
                                + string + "' and c.titolo='')");
        while (rs.next()) {
            ris += ";" + rs.getInt(2) + ",s,," + rs.getString(1) + ";";
        }
        return ris;
    }

    public static String getNames(String string) throws SQLException, ClassNotFoundException {
        String ris = "";
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram", "root", "");

        /*
         * query per avere tutti i nomi
         */
        Statement stmt = con.createStatement();
        ResultSet rs = stmt
                .executeQuery(
                        "select id,user from login where not user='" + string + "'");
        while (rs.next()) {
            ris += rs.getInt(1) + "," + rs.getString(2) + ";";
        }
        return ris;
    }

    public static String getChatMex(String string) throws ClassNotFoundException, SQLException {
        String ris = "";
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram", "root", "");

        /*
         * query per avere tutti i nomi dei gruppi che sono in contatto con string che
         * sarebbe l'user inserito nel login
         */
        Statement stmt = con.createStatement();
        ResultSet rs = stmt
                .executeQuery(
                        "select lo.user,mc.messaggio from messaggichat as mc join login as lo on lo.id=mc.idMittente where mc.idChat="
                                + Integer.parseInt(string));
        while (rs.next()) {
            ris += rs.getString(1) + "," + rs.getString(2) + ";";
        }
        return ris;
    }

    public static String newChat(String utente1, String utente2) throws SQLException, ClassNotFoundException {
        //non va per le restrizioni
        String utente = "";
        String chat = "";
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram", "root", "");

        /*
         * query per avere tutti i nomi dei gruppi che sono in contatto con string che
         * sarebbe l'user inserito nel login
         */
        Statement stmt = con.createStatement();
        stmt.executeUpdate("insert into chat (gruppo)" + "  values (0)");

        ResultSet rs;
        rs = stmt.executeQuery(
                        "select id from login where user='" + utente1 + "'");
        while (rs.next())
            utente += rs.getString(1);

        rs = stmt.executeQuery(
                        "select MAX(idChat) from chat");
        while (rs.next())
            chat += rs.getString(1);

        stmt.executeUpdate("insert into utentichat (idUtente,idChat)" + " values (" + Integer.parseInt(utente) + ","
                + Integer.parseInt(chat) + ")");

        stmt.executeUpdate("insert into utentichat (idUtente,idChat)" + " values (" + Integer.parseInt(utente2) + ","
                + Integer.parseInt(chat) + ")");
        
        return "ok";
    }
}
