using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Xml;
using System.Threading;

namespace FakeBTBIDBDeploy
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverName = "APC-SERVER-SS10";
            string dbName = "BTBIDB_20090228";
            if (args.Length == 0) Console.WriteLine("Use default value: FakeBTBIDBDeploy APC-SERVER-SS10 BTBIDB_20090228");
            if (args.Length > 0) serverName = args[0];
            if (args.Length > 1) dbName = args[1];

            string ConnectionString = String.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;", serverName,dbName);
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                try
                {
                    for (; ; )
                    {
                        cmd.CommandTimeout = 60;
                        cmd.CommandText = String.Format("UPDATE dbo.SegmentChangeRequest SET STATUSID=2 WHERE STATUSID=5");
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0) Console.WriteLine(String.Format("{0} Deploy plans succeed.", rows));
                        Thread.Sleep(2000);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    cmd.Dispose();
                }
                Console.ReadKey();
            }
        }


    }
}


