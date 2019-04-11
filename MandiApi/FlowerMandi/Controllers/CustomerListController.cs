using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FlowerMandi.BusinessEntities;
using System.Data.OleDb;

namespace FlowerMandi.Controllers
{
    public class CustomerListController : ApiController
    {

        public List<Customers> Get(String term)
        {

            String text = System.IO.File.ReadAllText(@"C:\MandiDB\mandidbpath.txt");

            // Display the file contents to the console. Variable text is a string.

            string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;"
                   + "Data Source=" + text;

            var customerList = new List<Customers>();
            //Create an OleDbConnection object, 
            //and then pass in the ConnectionString to the constructor.
            OleDbConnection cn = new OleDbConnection(connectString);
            cn.Open();
            string selectString = "SELECT CustID,CustCode, CUSTTABCODE,CustName From MstCust where InActive='N' and (CustID like '%" + term + "%' or CustCode like '%" + term + "%' or CustName like '%" + term + "%' or CUSTTABCODE like '%" + term + "%') order by CUSTTABCODE asc ";
            //Create an OleDbCommand object.
            //Notice that this line passes in the SQL statement and the OleDbConnection object
            OleDbCommand cmd = new OleDbCommand(selectString, cn);

            //Send the CommandText to the connection, and then build an OleDbDataReader.
            //Note: The OleDbDataReader is forward-only.
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var customerData = new Customers();
                customerData.CustId =int.Parse(reader["CustID"].ToString());
                customerData.CustCode = reader["CustCode"].ToString();
                customerData.CustTabCode = reader["CUSTTABCODE"].ToString();
                customerData.CustomerName = reader["CustName"].ToString();

                customerList.Add(customerData);

            }
            //Close the reader and the related connection.
            reader.Close();
            cn.Close();

            return customerList;
        }
    }
}