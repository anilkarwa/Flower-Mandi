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
    public class FarmerListController : ApiController
    {

        public List<Farmers> Get(String term)
        {
            String text = System.IO.File.ReadAllText(@"C:\MandiDB\mandidbpath.txt");

            // Display the file contents to the console. Variable text is a string.

            string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;"
                   + "Data Source=" + text;


            var farmerList = new List<Farmers>();
            //Create an OleDbConnection object, 
            //and then pass in the ConnectionString to the constructor.
            OleDbConnection cn = new OleDbConnection(connectString);
            cn.Open();
            string selectString = "SELECT FarmerID,FarmerName From MstFarmer where InActive='N' and (FarmerID like '%" + term + "%' or FarmerName like '%" + term + "%') order by FarmerName asc ";
            //Create an OleDbCommand object.
            //Notice that this line passes in the SQL statement and the OleDbConnection object
            OleDbCommand cmd = new OleDbCommand(selectString, cn);

            //Send the CommandText to the connection, and then build an OleDbDataReader.
            //Note: The OleDbDataReader is forward-only.
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var farmerData = new Farmers();
                farmerData.FarmerId=int.Parse(reader["FarmerID"].ToString());
                farmerData.FarmerName = reader["FarmerName"].ToString();

                farmerList.Add(farmerData);
            }
            //Close the reader and the related connection.
            reader.Close();
            cn.Close();

            return farmerList;
        }
    }
}