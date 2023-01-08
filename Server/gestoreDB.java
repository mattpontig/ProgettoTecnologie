import java.sql.*;

public class gestoreDB {

    // public static void connetti() {
    // try {
    // Class.forName("com.mysql.cj.jdbc.Driver");
    // Connection con =
    // DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram",
    // "root", "");
    // // Statement stmt = con.createStatement();
    // // ResultSet rs = stmt.executeQuery("select * from login");
    // // while (rs.next())
    // // System.out.println(rs.getInt(1) + " " + rs.getString(2) + " " +
    // // rs.getString(3));
    // // con.close();
    // } catch (Exception e) {
    // e.printStackTrace();
    // }
    // }

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

        String chat = chatUtente(string);
        String[] chats = chat.split(";");
        String idMex = "";
        for (int i = 0; i < chats.length; i++) {
            idMex += maxMexId(chats[i]) + ";";
        }
        String[] idsMes = idMex.split(";");
        String ultimoMex = "";
        for (int i = 0; i < idsMes.length; i++) {
            ultimoMex += lastMex(chats[i], idsMes[i]) + "\n";
        }
        String[] ultimiMessaggi = ultimoMex.split("\n");
        /*
         * query per avere tutti i nomi dei gruppi che sono in contatto con string che
         * sarebbe l'user inserito nel login
         */
        Statement stmt = con.createStatement(
                ResultSet.TYPE_SCROLL_INSENSITIVE,
                ResultSet.CONCUR_READ_ONLY);
        ResultSet rs = stmt
                .executeQuery(
                        "select distinct c.titolo, c.idChat, lo.user from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat where not c.titolo='' and not lo.user='"
                                + string
                                + "' and uc.idChat in (select uc.idChat from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat where user='"
                                + string + "')");

        int id = 0;
        while (rs.next()) {
            if (rs.getInt(2) != id) {
                id = rs.getInt(2);
                for (int i = 0; i < ultimiMessaggi.length; i++) {
                    String[] lastMex = ultimiMessaggi[i].split(",");
                    if (Integer.parseInt(lastMex[0]) == id)
                        ris += ";" + rs.getInt(2) + ",g," + rs.getString(1) + "," + rs.getString(3) + "-" + lastMex[2];
                    //;2,g,primoGruppo,prova,prova2-5,prova,triplo;
                    //1,pippo, non letto&29
                }
            }
        }
            /*if (ris.contains(Integer.toString(rs.getInt(2)))) { // da sistemare questo if
                ris += "," + rs.getString(3);
                rs.previous();
                if (ris.contains(Integer.toString(rs.getInt(2)))) {
                    for (int i = 0; i < ultimiMessaggi.length; i++) {
                        String[] last = ultimiMessaggi[i].split(",");
                        if (last[0].equals(Integer.toString(rs.getInt(2))))
                            ris += "-" + last[1] + "," + last[2] + "," + last[3];
                    }
                }
                rs.next();
            } else
                ris += ";" + rs.getInt(2) + ",g," + rs.getString(1) + "," + rs.getString(3) + "-" +ultimiMessaggi[j];*/
        //ris += ";";
        /* query per avere tutti i nomi dei singoli che sono in contatto con pippo */

        stmt = con.createStatement(
                ResultSet.TYPE_SCROLL_INSENSITIVE,
                ResultSet.CONCUR_READ_ONLY);
        rs = stmt
                .executeQuery(
                        "select user,c.idChat from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat where c.titolo='' and not lo.user='"
                                + string
                                + "' and uc.idChat in (select uc.idChat from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat where user='"
                                + string + "')");

        id = 0;
        while (rs.next()) {
            if (rs.getInt(2) != id) {
                id = rs.getInt(2);
                for (int i = 0; i < ultimiMessaggi.length; i++) {
                    String[] lastMex = ultimiMessaggi[i].split(",");
                    if (Integer.parseInt(lastMex[0]) == id)
                        ris += ";" + rs.getInt(2) + ",s," + rs.getString(1) + "-" + lastMex[2];
                    //;2,g,primoGruppo,prova,prova2-5,prova,triplo;
                    //1,pippo, non letto&29
                }
            }
        }

        /*stmt = con.createStatement(
                ResultSet.TYPE_SCROLL_INSENSITIVE,
                ResultSet.CONCUR_READ_ONLY);
        rs = stmt
                .executeQuery(
                        "select user,c.idChat from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat where not lo.user='"
                                + string
                                + "' and uc.idChat in (select uc.idChat from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat where user='"
                                + string + "' and c.titolo='')");
        while (rs.next()) {
            ris += rs.getInt(2) + ",s,," + rs.getString(1);
            for (int i = 0; i < ultimiMessaggi.length; i++) {
                String[] last = ultimiMessaggi[i].split(",");
                if (last[0].equals(Integer.toString(rs.getInt(2))))
                    ris += "-" + last[1] + "," + last[2] + "," + last[3] + ";";
            }
        }*/
        return ";" + ris;
    }

    public static String lastMex(String chat, String maxMexId) throws ClassNotFoundException, SQLException {
        String mes = "";
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram",
                "root", "");

        Statement stmt = con.createStatement();
        ResultSet rs;
        rs = stmt.executeQuery(
                "select login.id,login.user,messaggio from messaggichat join login on messaggichat.idMittente=login.id where idchat="
                        + Integer.parseInt(chat) + " and idmex=" + Integer.parseInt(maxMexId) + "");
        while (rs.next())
            mes = chat + "," /*+ Integer.toString(rs.getInt(1)) + ","*/ + rs.getString(2) + "," + rs.getString(3);
        return mes+"&"+maxMexId;
    }

    public static String getNames(String string) throws SQLException, ClassNotFoundException {
        String ris = "";
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram", "root", "");

        /*
         * query per avere tutti i nomi di chi si è registrato
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
         * query per avere tutti i messaggi delle chat selezionata
         */
        Statement stmt = con.createStatement();
        ResultSet rs = stmt
                .executeQuery(
                        "select mc.idMex,lo.user,mc.messaggio from messaggichat as mc join login as lo on lo.id=mc.idMittente where mc.idChat="
                                + Integer.parseInt(string));
        while (rs.next()) {
            ris += rs.getInt(1) + "," + rs.getString(2) + "," + rs.getString(3) + ";";
        }
        return ris;
    }

    public static String newChat(String utente1, String utente2) throws SQLException, ClassNotFoundException {
        // non va per le restrizioni
        String utente = "";
        String chat = "";
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram", "root", "");
        /*
         * prende tutte le chat singole con utente1 per vedere se può creare una nuova
         * chat o no
         */
        String ris = "";
        Statement stmt = con.createStatement();
        ResultSet rs;
        rs = stmt
                .executeQuery(
                        "select user,c.idChat from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat where not lo.user='"
                                + utente1
                                + "' and uc.idChat in (select uc.idChat from (utentichat as uc join login as lo on uc.idUtente=lo.id) join chat as c on uc.idChat=c.idChat where user='"
                                + utente1 + "' and c.titolo='')");
        while (rs.next()) {
            ris += rs.getInt(2) + ",s,," + rs.getString(1) + ";";
        }
        /*
         * se non c'è giaà una chat singola con l'altro utente la crea
         */
        if (!ris.contains("utente2")) {
            /*
             * crea una chat non di gruppo vuota
             */
            stmt = con.createStatement();
            stmt.executeUpdate("insert into chat (gruppo)" + "  values (0)");

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

            stmt.executeUpdate(
                    "insert into utentichat (idUtente,idChat)" + " values (" + Integer.parseInt(utente2) + ","
                            + Integer.parseInt(chat) + ")");

            return "ok";
        }
        return "chat già esistente";
    }

    public static String sendMex(String chi, String chat, String mex) throws ClassNotFoundException, SQLException {
        String idAltro = "";
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram",
                "root", "");

        Statement stmt = con.createStatement();
        ResultSet rs;
        rs = stmt.executeQuery(
                "select id from login where user='" + chi + "'");
        while (rs.next())
            idAltro += rs.getString(1);

        stmt.executeUpdate("insert into messaggichat (messaggio,idChat,IdMittente) values('" + mex + "',"
                + Integer.parseInt(chat) + "," + Integer.parseInt(idAltro) + ")");

        return "ok";
    }

    public static String chatToId(String chi, String chat) throws ClassNotFoundException, SQLException {
        String idAltro = "";
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram",
                "root", "");

        Statement stmt = con.createStatement();
        ResultSet rs;
        rs = stmt.executeQuery(
                "select id from login join utentichat on id = idUtente where not user='" + chi + "' and idChat="
                        + Integer.parseInt(chat) + "");
        while (rs.next())
            idAltro += Integer.toString(rs.getInt(1)) + ";";

        return idAltro;
    }

    public static long getIdFromName(String string) throws ClassNotFoundException, SQLException {
        long id = 0;
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram",
                "root", "");

        Statement stmt = con.createStatement();
        ResultSet rs;
        rs = stmt.executeQuery(
                "select id from login where user='" + string + "'");
        while (rs.next())
            id = rs.getInt(1);

        return id;
    }

    public static String newGroup(String creatore, String titoloG, String utentiG)
            throws ClassNotFoundException, SQLException {
        String[] utenti = utentiG.split("-");
        int idCreatore = 0;
        int idChat = 0;
        String idUtenti = "";
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram",
                "root", "");

        Statement stmt = con.createStatement();
        ResultSet rs;
        stmt = con.createStatement();

        stmt.executeUpdate("insert into chat (gruppo,titolo)" + "  values (1,'" + titoloG + "')");

        rs = stmt.executeQuery("select id from login where user='" + creatore + "'");
        while (rs.next())
            idCreatore = rs.getInt(1);

        rs = stmt.executeQuery("select MAX(idChat) from chat where titolo='" + titoloG + "'");
        while (rs.next())
            idChat = rs.getInt(1);

        stmt.executeUpdate("insert into utentichat (idUtente,idChat)" + " values (" + idCreatore + ","
                + idChat + ")");

        for (int i = 0; i < utenti.length; i++) {
            rs = stmt.executeQuery("select id from login where user='" + utenti[i] + "'");
            while (rs.next())
                idUtenti += rs.getString(1) + ";";
        }

        String[] utentiSplit = idUtenti.split(";");
        for (int i = 0; i < utentiSplit.length; i++) {
            stmt.executeUpdate(
                    "insert into utentichat (idUtente,idChat)" + " values (" + Integer.parseInt(utentiSplit[i])
                            + "," + idChat + ")");
        }
        return "ok";
    }

    public static String chatUtente(String utente) throws ClassNotFoundException, SQLException {
        String chat = "";
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram",
                "root", "");

        Statement stmt = con.createStatement();
        ResultSet rs;
        rs = stmt.executeQuery(
                "select idChat from utentichat join login on utentichat.idUtente=login.id where login.user='" + utente
                        + "'");
        while (rs.next())
            chat += Integer.toString(rs.getInt(1)) + ";";
        return chat;
    }

    public static String maxMexId(String chat) throws ClassNotFoundException, SQLException {
        String id = "";
        Class.forName("com.mysql.cj.jdbc.Driver");
        Connection con = DriverManager.getConnection("jdbc:mysql://localhost:3306/db_telegram",
                "root", "");

        Statement stmt = con.createStatement();
        ResultSet rs;
        rs = stmt.executeQuery(
                "select max(idmex) from messaggichat where idchat='" + Integer.parseInt(chat) + "'");
        while (rs.next())
            id = Integer.toString(rs.getInt(1));
        return id;
    }
}
