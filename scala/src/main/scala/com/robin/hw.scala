import java.sql.{Connection, DriverManager}

import com.robin.HelloWorld

object Hi {
	def main(args: Array[String]) : Unit = {
//		println("Hello World!")

    new HelloWorld().say()
// Change to Your Database Config
    val conn_str = "jdbc:mysql://192.168.1.204:3306/dp?user=root&password=root123"
    val url = "jdbc:mysql://192.168.1.204:3306/dp"
    val driver = "com.mysql.jdbc.Driver"
    val username = "root"
    val password = "root123"
//    var connection:Connection =

    try {
      Class.forName(driver)
      var connection = DriverManager.getConnection(url, username, password)
      val statement = connection.createStatement
      val rs = statement.executeQuery("SELECT * FROM t_bigtable_schema")
      while (rs.next) {
        val colName = rs.getString("name")
        val colType = rs.getString("type")
        println("name = %s, type = %s".format(colName,colType))
      }
    } catch {
      case e: Exception => e.printStackTrace
    }
//    connection.close

  }
}
