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
    public class ItemsController : ApiController
    {

        public List<Items> Get(String term)
        {
            String text = System.IO.File.ReadAllText(@"C:\MandiDB\mandidbpath.txt");

            // Display the file contents to the console. Variable text is a string.

            string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;"
                   + "Data Source=" + text;

            var itemList = new List<Items>();
            //Create an OleDbConnection object, 
            //and then pass in the ConnectionString to the constructor.
            OleDbConnection cn = new OleDbConnection(connectString);
            cn.Open();
            string selectString = "SELECT ItemID,ItemCode,ITEMTABCODE,ItemName From MstItem where InActive='N' and (ItemID like '%"+term+ "%' or ItemCode like '%"+term+ "%' or ItemName like '%" + term + "%' or ITEMTABCODE like '%" + term + "%') order by ITEMTABCODE asc";
            //Create an OleDbCommand object.
            //Notice that this line passes in the SQL statement and the OleDbConnection object
            OleDbCommand cmd = new OleDbCommand(selectString, cn);

            //Send the CommandText to the connection, and then build an OleDbDataReader.
            //Note: The OleDbDataReader is forward-only.
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var itemData = new Items();
                itemData.ItemId = int.Parse(reader["ItemID"].ToString());
                itemData.ItemCode = reader["ItemCode"].ToString();
                itemData.ItemTabCode = reader["ITEMTABCODE"].ToString();
                itemData.ItemName = reader["ItemName"].ToString();
                itemList.Add(itemData);
            }
            //Close the reader and the related connection.
            reader.Close();
            cn.Close();
            return itemList;
        }
    }
}