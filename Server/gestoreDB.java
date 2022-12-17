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
}
