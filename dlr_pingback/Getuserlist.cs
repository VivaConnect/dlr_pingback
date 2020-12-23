using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Configuration;
using logconfiguration;

namespace dlr_pingback
{
    class Getuserlist
    {

        #region Adding users into cache
        public static Dictionary<string, List<string>> GetDetails()
        {

            if (HttpRuntime.Cache["key"] != null)
                return (Dictionary<string, List<string>>)HttpRuntime.Cache["key"];
            else
            {
                Dictionary<string, List<string>> dict = loaduserdetails();
                HttpRuntime.Cache.Add("key", dict, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, int.Parse(ConfigurationManager.AppSettings["cacheexpiration"].ToString()), 0), System.Web.Caching.CacheItemPriority.High, null);
                return dict;
            }
        }
        #endregion


        #region Get data of user
        public static Dictionary<string, List<string>> loaduserdetails()
        {
            try
            {

                Dictionary<string, List<string>> dict_userdetails = new Dictionary<string, List<string>>();
                string str_getuserdetails = "select * from callback_url_dlr_details where url_active=1;";
                DataSet ds_udetails = DL.DL_ExecuteSimpleQuery(str_getuserdetails);
                if (ds_udetails != null && ds_udetails.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_udetails.Tables[0].Rows)
                    {
                        List<string> lst_udetails = new List<string>();

                        if (!dict_userdetails.ContainsKey(dr["username"].ToString()))
                        {
                            #region Add details in list
                            lst_udetails.Add(dr["call_back_url"].ToString());
                            lst_udetails.Add(dr["messageid_p"].ToString());
                            lst_udetails.Add(dr["messageid_p_active"].ToString());
                            lst_udetails.Add(dr["delivery_date_p"].ToString());
                            lst_udetails.Add(dr["delivery_date_p_active"].ToString());
                            lst_udetails.Add(dr["report_p"].ToString());
                            lst_udetails.Add(dr["report_p_active"].ToString());
                            lst_udetails.Add(dr["destination_p"].ToString());
                            lst_udetails.Add(dr["destination_p_active"].ToString());
                            lst_udetails.Add(dr["error_code_p"].ToString());
                            lst_udetails.Add(dr["error_code_p_active"].ToString());
                            lst_udetails.Add(dr["split_count_p"].ToString());
                            lst_udetails.Add(dr["split_count_p_active"].ToString());
                            lst_udetails.Add(dr["send_date_p"].ToString());
                            lst_udetails.Add(dr["send_date_p_active"].ToString());
                            lst_udetails.Add(dr["username_p"].ToString());
                            lst_udetails.Add(dr["username_p_active"].ToString());

                            #endregion

                            dict_userdetails.Add(dr["username"].ToString(), lst_udetails);
                        }

                    }

                }
                else
                {

                    dict_userdetails = null;

                }
                return dict_userdetails;
            }
            catch (Exception ex)
            {
                ____logconfig.Error_Write(____logconfig.LogLevel.EXC, 55, ex, "dlrcallbackerr");
                Console.WriteLine(ex);
                return null;

            }

        }
        #endregion
    }
}
