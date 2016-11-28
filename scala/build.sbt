name := "spark"

version := "1.0"

scalaVersion := "2.12.0"

libraryDependencies ++= Seq(
  "org.apache.spark" % "spark-core_2.11" % "1.6.3" exclude ("org.apache.hadoop","hadoop-yarn-server-web-proxy"),
  "org.apache.spark" % "spark-sql_2.11" % "1.6.3" exclude ("org.apache.hadoop","hadoop-yarn-server-web-proxy"),
  "org.apache.hadoop" % "hadoop-common" % "2.7.0" exclude ("org.apache.hadoop","hadoop-yarn-server-web-proxy"),
  "org.apache.spark" % "spark-sql_2.11" % "1.6.3" exclude ("org.apache.hadoop","hadoop-yarn-server-web-proxy"),
  "org.apache.spark" % "spark-hive_2.11" % "1.6.3" exclude ("org.apache.hadoop","hadoop-yarn-server-web-proxy"),
  "org.apache.spark" % "spark-yarn_2.11" % "1.6.3" exclude ("org.apache.hadoop","hadoop-yarn-server-web-proxy"),
  "mysql" % "mysql-connector-java" % "l5.1.40"
)