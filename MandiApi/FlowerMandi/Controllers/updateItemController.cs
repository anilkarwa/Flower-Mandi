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
    public class updateItemController : ApiController
    {
       
        public IHttpActionResult Put([FromBody] OrderItem item)
        {
            String text = System.IO.File.ReadAllText(@"C:\MandiDB\mandidbpath.txt");

            // Display the file contents to the console. Variable text is a string.

            string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;"
                   + "Data Source=" + text;
            Boolean valid = false;
            
            OleDbConnection cn = new OleDbConnection(connectString);
            cn.Open();
            string selectString = "update TrnDtlSLP set QTY="+item.QTY+",ITEMRATE="+item.ItemRate+",ITEMAMT="+item.ItemAmt+", BQTY="+item.BQTY+",CUSTID="+item.CustID+", ITEMID="+item.ItemID+" where SLPID="+item.SLPID+" and SlNo="+item.SlNo+" ";
            OleDbCommand cmd = new OleDbCommand(selectString, cn);
            try
            {
                cmd.ExecuteNonQuery();
                valid = true;
            }
            catch (Exception ex)
            {
                valid = false;
            }
            finally
            {
                cn.Close();
            }
            if (valid)
            {
                return Ok("Updated Item Details");
            }
            else
            {
                return Content(HttpStatusCode.NotFound, "Error Updating Item, Try Again");
            }
        }

        public IHttpActionResult Delete([FromBody]OrderItem item)
        {
            String text = System.IO.File.ReadAllText(@"C:\MandiDB\mandidbpath.txt");

            // Display the file contents to the console. Variable text is a string.

            string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;"
                   + "Data Source=" + text;
            Boolean valid = false;

            OleDbConnection cn = new OleDbConnection(connectString);
            cn.Open();
            string selectString = "delete from TrnDtlSLP where SLPID=" + item.SLPID + " and SlNo=" + item.SlNo + " ";
            OleDbCommand cmd = new OleDbCommand(selectString, cn);
            try
            {
                cmd.ExecuteNonQuery();
                valid = true;
            }
            catch (Exception ex)
            {
                valid = false;
            }
            finally
            {
                cn.Close();
            }
            if (valid)
            {
                return Ok("Item Deleted");
            }
            else
            {
                return Content(HttpStatusCode.NotFound, "Error Deleting Item, Try Again");
            }
        }
    }
}