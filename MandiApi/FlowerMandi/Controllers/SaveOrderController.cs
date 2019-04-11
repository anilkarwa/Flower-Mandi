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
    public class SaveOrderController : ApiController
    {

        public OrderFields  Post([FromBody] OrderFields order)
        {
            String text = System.IO.File.ReadAllText(@"C:\MandiDB\mandidbpath.txt");

            // Display the file contents to the console. Variable text is a string.

            string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;"
                   + "Data Source=" + text;
            var orderData = new OrderFields();
            var itemLength = order.items.Length;
            OleDbConnection cn = new OleDbConnection(connectString);
            cn.Open();
            string selectString = "insert into TrnHdrSLP(SLPNO,Prefix,PrintSLPNO,SLPDate,Cancelled,Ammendment,AmmDetails,AmmNo,AmmDate,Authorised,AuthDate,AuthorisedBy,InsertDocID,InsertDocNo,TOTQTY,TOTAMT,COMMPER,COMMAMT,GRANDTOT,FARMERID,Misc,RECORDADDEDBY)" +
                " values('-','','-',Format (#"+order.SLPDate+ "#, 'mm/dd/yyyy') ,'N','N','','0',Format (#" + order.SLPDate + "#, 'mm/dd/yyyy') ,'Y',Format (#" + order.SLPDate + "#, 'mm/dd/yyyy') ,1,0,''," + order.TOTQTY+ ","+order.TOTAMT+ ",0,0,"+order.GRANDTOT+ ","+order.FarmerId+ ",'','"+order.RecordAddedBy+"') ";
            string query2 = "Select @@Identity";
            OleDbCommand cmd = new OleDbCommand(selectString, cn);
            try
            {

                cmd.ExecuteNonQuery();
                cmd.CommandText = query2;
                orderData.SLPID =cmd.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                cn.Close();
            }

            for( int i=0; i< itemLength; i++)
            {
                cn.Open();
                string selectString2 = "insert into TrnDtlSLP values("+orderData.SLPID+ ",'-',"+order.items[i].SlNo+ ","+order.items[i].ItemSlNo+ ","+order.items[i].QTY+ ","+order.items[i].CustID+ ","+order.items[i].ItemRate+ ","+order.items[i].ItemAmt+ ","+order.items[i].BQTY+ ",0,0,"+order.items[i].CustInAmt+","+order.items[i].ItemID+","+order.items[i].ItemRate+","+order.items[i].FarmerID+")";
                OleDbCommand cmd2 = new OleDbCommand(selectString2, cn);
                try
                {

                    cmd2.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    cn.Close();
                }
            }

            return orderData;
        }

        public IHttpActionResult Put([FromBody] OrderFields order)
        {
            String text = System.IO.File.ReadAllText(@"C:\MandiDB\mandidbpath.txt");

            // Display the file contents to the console. Variable text is a string.

            string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;"
                   + "Data Source=" + text;

            Boolean valid = false;
            var itemLength = order.items.Length;
            OleDbConnection cn = new OleDbConnection(connectString);
            cn.Open();
            string selectString = "update TrnHdrSLP set TOTQTY="+order.TOTQTY+", TOTAMT= "+order.TOTAMT+", GRANDTOT="+order.GRANDTOT+" where SLPID="+order.SLPID+" ";
            string query2 = "delete from TrnDtlSLP where SLPID ="+order.SLPID+" ";
            OleDbCommand cmd = new OleDbCommand(selectString, cn);
            try
            {
                cmd.ExecuteNonQuery();
                cmd.CommandText = query2;
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
            for (int i = 0; i < itemLength; i++)
            {
                cn.Open();
                string selectString2 = "insert into TrnDtlSLP values(" + order.SLPID + ",'-'," + order.items[i].SlNo + "," + order.items[i].ItemSlNo + "," + order.items[i].QTY + "," + order.items[i].CustID + "," + order.items[i].ItemRate + "," + order.items[i].ItemAmt + "," + order.items[i].BQTY + ",0,0," + order.items[i].CustInAmt + "," + order.items[i].ItemID + "," + order.items[i].ItemRate + ","+order.items[i].FarmerID+")";
                OleDbCommand cmd2 = new OleDbCommand(selectString2, cn);
                try
                {

                    cmd2.ExecuteNonQuery();
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

            }
            if (valid)
            {
                return Ok("Saved Items sucessfully");
            }
            else
            {
                return Content(HttpStatusCode.NotFound, "Error Updating Item, Try Again");
            }

        }
    }
}