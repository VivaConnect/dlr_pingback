using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using System.Globalization;
using logconfiguration;

namespace dlr_pingback
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                try
                {




                    Console.WriteLine("Started");
                    Console.WriteLine("Get the data from sms table" + Environment.NewLine);
                    #region Get data for sending dlr call back
                    string str_getdlrcallbackdata = "select *,DATE_FORMAT(date_current,'%Y-%m-%d %H:%i:%s') as 'sent_date' from " + ConfigurationManager.AppSettings["pickupTable"].ToString() + " where `status`=0 limit " + ConfigurationManager.AppSettings["limittopick"].ToString() + ";";
                    DataSet ds_getdlrcallbackdata = DL.DL_ExecuteSimpleQuery(str_getdlrcallbackdata);
                    #endregion



                    System.Net.ServicePointManager.DefaultConnectionLimit = int.Parse(ConfigurationManager.AppSettings["servicepointconnlimit"].ToString());

                    if (ds_getdlrcallbackdata != null && ds_getdlrcallbackdata.Tables[0].Rows.Count > 0)
                    {
                        ____logconfig.Log_Write(____logconfig.LogLevel.DEBUG, 29, "Data found for dlr call back", "dlrcallback");
                        List<string> lst_success = new List<string>();
                        List<string> lst_failed = new List<string>();

                        Console.WriteLine("Getting user details data");
                        ____logconfig.Log_Write(____logconfig.LogLevel.DEBUG, 37, "Getting of users", "dlrcallback");

                        #region Get list of all the users
                        Dictionary<string, List<string>> dict_udet = Getuserlist.GetDetails();
                        #endregion


                        //var x = new ParallelOptions();
                        //x.MaxDegreeOfParallelism = Math.Max(Environment.ProcessorCount / 2, 1);
                        StringBuilder str_domainname = new StringBuilder();
                        StringBuilder uri_send = new StringBuilder();
                        for (int i = 0; i < ds_getdlrcallbackdata.Tables[0].Rows.Count; i++)
                        {
                            if (dict_udet.ContainsKey(ds_getdlrcallbackdata.Tables[0].Rows[i]["username"].ToString()))
                            {
                                List<string> lst_data = null;
                                lst_data = dict_udet[ds_getdlrcallbackdata.Tables[0].Rows[i]["username"].ToString()];
                                if (lst_data != null)
                                {
                                    string domainname = lst_data[0];
                                    string msg_id_p = lst_data[1];
                                    string msg_id_p_isactive = lst_data[2];
                                    string deliverydate_p = lst_data[3];
                                    string deliverydate_p_active = lst_data[4];
                                    string report_p = lst_data[5];
                                    string report_p_isactive = lst_data[6];
                                    string destination_p = lst_data[7];
                                    string destination_p_isactive = lst_data[8];
                                    string error_code_p = lst_data[9];
                                    string error_code_p_isactive = lst_data[10];
                                    string split_count_p = lst_data[11];
                                    string split_count_p_isactive = lst_data[12];
                                    string send_date_p = lst_data[13];
                                    string send_date_p_isactive = lst_data[14];
                                    string username_p = lst_data[15];
                                    string username_p_isactive = lst_data[16];

                                    str_domainname.Append(domainname);

                                    if (msg_id_p_isactive == "1")
                                    {
                                        uri_send.Append(((uri_send.Length != 0) ? "&" : "") + msg_id_p + "=" + ds_getdlrcallbackdata.Tables[0].Rows[i]["messageid"].ToString());
                                    }
                                    if (deliverydate_p_active == "1")
                                    {
                                        uri_send.Append(((uri_send.Length != 0) ? "&" : "") + deliverydate_p + "=" + ds_getdlrcallbackdata.Tables[0].Rows[i]["delivery_date"].ToString());

                                    }
                                    if (report_p_isactive == "1")
                                    {
                                        uri_send.Append(((uri_send.Length != 0) ? "&" : "") + report_p + "=" + ds_getdlrcallbackdata.Tables[0].Rows[i]["report"].ToString());
                                    }
                                    if (destination_p_isactive == "1")
                                    {

                                        uri_send.Append(((uri_send.Length != 0) ? "&" : "") + destination_p + "=" + ds_getdlrcallbackdata.Tables[0].Rows[i]["destination"].ToString());
                                    }
                                    if (error_code_p_isactive == "1")
                                    {
                                        uri_send.Append(((uri_send.Length != 0) ? "&" : "") + error_code_p + "=" + ds_getdlrcallbackdata.Tables[0].Rows[i]["error_code"].ToString());
                                    }
                                    if (split_count_p_isactive == "1")
                                    {
                                        uri_send.Append(((uri_send.Length != 0) ? "&" : "") + split_count_p + "=" + ds_getdlrcallbackdata.Tables[0].Rows[i]["split_count"].ToString());
                                    }
                                    if (send_date_p_isactive == "1")
                                    {


                                        uri_send.Append(((uri_send.Length != 0) ? "&" : "") + send_date_p + "=" + ds_getdlrcallbackdata.Tables[0].Rows[i]["sent_date"].ToString());
                                    }
                                    if (username_p_isactive == "1")
                                    {
                                        uri_send.Append(((uri_send.Length != 0) ? "&" : "") + username_p + "=" + ds_getdlrcallbackdata.Tables[0].Rows[i]["username"].ToString());
                                    }

                                    Console.WriteLine("Uri==>" + str_domainname + uri_send + Environment.NewLine);
                                    ____logconfig.Log_Write(____logconfig.LogLevel.DEBUG, 81, "URI generate==>" + str_domainname + uri_send, "dlrcallback_request");
                                    utility.Make_async_webreq(str_domainname.Append(uri_send.ToString()).ToString());
                                    uri_send.Clear();
                                    str_domainname.Clear();
                                    lst_success.Add(ds_getdlrcallbackdata.Tables[0].Rows[i]["id"].ToString());

                                }
                                else
                                {
                                    ____logconfig.Log_Write(____logconfig.LogLevel.DEBUG, 87, "Users not found for dlr callback", "dlrcallback");
                                    Console.WriteLine("Users not found for dlr forwarding");
                                }
                            }
                            else
                            {

                                lst_failed.Add(ds_getdlrcallbackdata.Tables[0].Rows[i]["id"].ToString());
                            }


                        }

                        #region Update the rows on the basis of success and failed
                        if (lst_success.Count > 0)
                        {
                            Console.WriteLine("Updating success status count==>" + lst_success.Count);
                            utility.delete_rows(lst_success);
                        }

                        if (lst_failed.Count > 0)
                        {
                            Console.WriteLine("updating failed status count==>" + lst_failed.Count);
                            utility.update(lst_failed, 1);
                        }
                        #endregion
                    }
                    else
                    {
                        Console.WriteLine("Data not found for dlr callback" + Environment.NewLine);
                    }

                    Console.WriteLine("Sleep");
                    Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["sleepcontroller"].ToString()));
                }
                catch (Exception ex)
                {
                    ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 118, ex, "dlrcallbackerr");
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
