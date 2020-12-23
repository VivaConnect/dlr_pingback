using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Threading;
using System.Configuration;
using logconfiguration;

namespace dlr_pingback
{
    class utility
    {

        public static void Make_async_webreq(string uri)
        {
            int id = 0;
            Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["webreqsleep"].ToString()));
            WebRequest web_req = WebRequest.Create(uri);
            RequestState req = new RequestState();
            req.id = id;
            req.req = web_req;
            web_req.BeginGetResponse(OnComplete, req);

        }


        public static void OnComplete(IAsyncResult ar)
        {
            RequestState web_req = (RequestState)ar.AsyncState;
            try
            {
                WebResponse web_resp = web_req.req.EndGetResponse(ar);
                string response = "";
                using (System.IO.Stream strm = web_resp.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(strm))
                    {
                        response = sr.ReadToEnd();
                    }
                    strm.Close();
                }

                ____logconfig.Log_Write(____logconfig.LogLevel.DEBUG, 41, "Request => " + web_req.req.RequestUri.AbsoluteUri + " Response =>" + response, "dlrcallback_RequestResponse");
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                //  this.Update(web_req.id, 2, "Exception Occurred : " + ex.Message);
                Console.WriteLine(ex.ToString());
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 48, ex, "dlrcallbackerr");
                //   FileWrite file = new FileWrite();
                //   file.Asyncwrite(ex.ToString(), "exception_" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt", "ExceptionLog/DIDSend/WebExceptions");
            }

        }

        public static void update(List<string> lst_id, int up_status)
        {
            string str_update = "update " + ConfigurationManager.AppSettings["pickupTable"].ToString() + " set `status`=" + up_status + " where id in (" + string.Join(",", lst_id) + ")";
            ____logconfig.Log_Write(____logconfig.LogLevel.DEBUG, 64, "Query for ID's Updated==>" + str_update, "dlrcallback");
            int rows_updt = DL.DL_ExecuteSimpleNonQuery(str_update);
            ____logconfig.Log_Write(____logconfig.LogLevel.DEBUG, 64, "ID's Updated==>" + rows_updt, "dlrcallback");

        }
        public static void delete_rows(List<string> lst_id)
        {
            string str_delete = "delete from " + ConfigurationManager.AppSettings["pickupTable"].ToString() + "  where id in (" + string.Join(",", lst_id) + ")";
            ____logconfig.Log_Write(____logconfig.LogLevel.DEBUG, 64, "Query for ID's deleted==>" + str_delete, "dlrcallback");
            int rows_updt = DL.DL_ExecuteSimpleNonQuery(str_delete);
            ____logconfig.Log_Write(____logconfig.LogLevel.DEBUG, 64, "ID's deleted==>" + rows_updt, "dlrcallback");
        }
    }

    public class RequestState
    {
        public WebRequest req;
        public int id;
    }
}
