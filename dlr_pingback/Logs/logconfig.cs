using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Runtime.CompilerServices;

namespace logconfiguration
{
    public class ____logconfig
    {
        public enum LogLevel
        {
            INFO = 0,
            DEBUG = 1,
            DB = 2,
            EXC = 3,
            CRIT = 4
        }

        #region Log_Write(LogLevel GetLog_level, int TR_Num, string Log_String)

        //declares a delegate 
        public delegate void Log_Write_Delgt1(LogLevel GetLog_level, int TR_Num, string Log_String);

        //we call this method
        public static void Log_Write(LogLevel GetLog_level, int TR_Num, string Log_String)
        {
            Log_Write_Delgt1 obj = new Log_Write_Delgt1(Log_Write_Call);
            obj.BeginInvoke(GetLog_level, TR_Num, Log_String, null, null);
        }

        //delegate calls this method 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Log_Write_Call(LogLevel GetLog_level, int TR_Num, string Log_String)
        {
            string Log_Write_String = "";
            try
            {
                Log_String = Log_String.Replace("\r\n", "\n").Replace("\n", "*_*");
                string Log_level = ConfigurationManager.AppSettings["level" + (int)GetLog_level].ToString();
                if (Log_level == "1")
                {
                    Log_String = "[" + GetLog_level.ToString() + "]::" + Log_String;
                    Log_Write_String = string.Format("{0}{1,-8}{2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), "[" + TR_Num + "]", Log_String);

                    string filepath = ConfigurationManager.AppSettings["strlogpath"].ToString() + DateTime.Now.ToString("yyyy\\\\MM\\\\dd") + "\\";
                    if (!Directory.Exists(filepath))
                        Directory.CreateDirectory(filepath);


                    string strLogFileName = ConfigurationManager.AppSettings["strLogFileName"] + "_" + DateTime.Now.Hour.ToString("D2");
                    string finalpath = renameFiles(filepath, strLogFileName);
                    using (FileStream fs = new FileStream(finalpath, FileMode.Append, FileAccess.Write, FileShare.Write, 8, true))
                    {
                        byte[] bt = System.Text.ASCIIEncoding.ASCII.GetBytes(Log_Write_String);
                        fs.BeginWrite(bt, 0, bt.Length, ascallback, fs);
                    }
                }
            }
            catch (Exception ex)
            {
                logger_for_exception.Append(ex.ToString(), 117, Log_Write_String);
            }
        }

        #endregion

        #region Log_Write_Call(LogLevel GetLog_level, StackTrace objStkTrc, string Log_String)

        //declares a delegate
        public delegate void Log_Write_Delgt2(LogLevel GetLog_level, StackTrace objStkTrc, string Log_String);

        //we call this method
        public static void Log_Write(LogLevel GetLog_level, StackTrace objStkTrc, string Log_String)
        {
            Log_Write_Delgt2 obj = new Log_Write_Delgt2(Log_Write_Call);
            obj.BeginInvoke(GetLog_level, objStkTrc, Log_String, null, null);
        }

        //delegate calls this method 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Log_Write_Call(LogLevel GetLog_level, StackTrace objStkTrc, string Log_String)
        {
            string Log_Write_String = "";
            try
            {
                Log_String = Log_String.Replace("\r\n", "\n").Replace("\n", "*_*");
                string Log_level = ConfigurationManager.AppSettings["level" + (int)GetLog_level].ToString();
                if (Log_level == "1")
                {
                    //int TR_Num = objStkTrc.GetFrame(0).GetFileLineNumber();
                    //Log_String = "[" + GetLog_level.ToString() + "]::" + Path.GetFileName(objStkTrc.GetFrame(0).GetFileName()) + "::" + objStkTrc.GetFrame(0).GetMethod().Name + "::" + Log_String;
                    string logstring = "[" + GetLog_level.ToString() + "]::";
                    StackFrame sf = objStkTrc.GetFrame(objStkTrc.FrameCount - 1);
                    StringBuilder stbuild = new StringBuilder();
                    for (int i = 0; i < objStkTrc.FrameCount; i++)
                    {
                        sf = objStkTrc.GetFrame(i);
                        stbuild = new StringBuilder();
                        stbuild.Append(Path.GetFileName(sf.GetFileName()));
                        stbuild.Append("::");
                        stbuild.Append(sf.GetMethod().Name + "()");
                        stbuild.Append("::");
                        stbuild.Append("[" + sf.GetFileLineNumber() + "]");
                        stbuild.Append("::");
                        logstring += stbuild.ToString();
                    }
                    Log_String = logstring + Log_String;
                    int TR_Num = sf.GetFileLineNumber();

                    Log_Write_String = string.Format("{0}{1,-8}{2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), "[" + TR_Num + "]", Log_String);
                    string filepath = ConfigurationManager.AppSettings["strlogpath"].ToString() + DateTime.Now.ToString("yyyy\\\\MM\\\\dd") + "\\";
                    if (!Directory.Exists(filepath))
                        Directory.CreateDirectory(filepath);

                    string strLogFileName = ConfigurationManager.AppSettings["strLogFileName"] + "_" + DateTime.Now.Hour.ToString("D2");
                    string finalpath = renameFiles(filepath, strLogFileName);
                    using (FileStream fs = new FileStream(finalpath, FileMode.Append, FileAccess.Write, FileShare.None, 8, true))
                    {
                        byte[] bt = System.Text.ASCIIEncoding.ASCII.GetBytes(Log_Write_String);
                        fs.BeginWrite(bt, 0, bt.Length, ascallback, fs);
                    }
                }
            }
            catch (Exception ex)
            {
                logger_for_exception.Append(ex.ToString(), 52, Log_Write_String);
            }
        }

        #endregion

        #region Log_Write(LogLevel GetLog_level, StackTrace objStkTrc, string Log_String, string strLogFileName)

        //declares a delegate
        public delegate void Log_Write_Delgt3(LogLevel GetLog_level, StackTrace objStkTrc, string Log_String, string strLogFileName);

        //we call this method
        public static void Log_Write(LogLevel GetLog_level, StackTrace objStkTrc, string Log_String, string strLogFileName)
        {
            Log_Write_Delgt3 obj = new Log_Write_Delgt3(Log_Write_Call);
            obj.BeginInvoke(GetLog_level, objStkTrc, Log_String, strLogFileName, null, null);
        }

        //delegate calls this method 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Log_Write_Call(LogLevel GetLog_level, StackTrace objStkTrc, string Log_String, string strLogFileName)
        {
            string Log_Write_String = "";
            try
            {
                Log_String = Log_String.Replace("\r\n", "\n").Replace("\n", "*_*");
                string Log_level = ConfigurationManager.AppSettings["level" + (int)GetLog_level].ToString();
                if (Log_level == "1")
                {
                    //int TR_Num = objStkTrc.GetFrame(0).GetFileLineNumber();
                    //Log_String = "[" + GetLog_level.ToString() + "]::" + Path.GetFileName(objStkTrc.GetFrame(0).GetFileName()) + "::" + objStkTrc.GetFrame(0).GetMethod().Name + "::" + Log_String;
                    string logstring = "[" + GetLog_level.ToString() + "]::";
                    StackFrame sf = objStkTrc.GetFrame(objStkTrc.FrameCount - 1);
                    StringBuilder stbuild = new StringBuilder();
                    for (int i = 0; i < objStkTrc.FrameCount; i++)
                    {
                        sf = objStkTrc.GetFrame(i);
                        stbuild = new StringBuilder();
                        stbuild.Append(Path.GetFileName(sf.GetFileName()));
                        stbuild.Append("::");
                        stbuild.Append(sf.GetMethod().Name + "()");
                        stbuild.Append("::");
                        stbuild.Append("[" + sf.GetFileLineNumber() + "]");
                        stbuild.Append("::");
                        logstring += stbuild.ToString();
                    }
                    Log_String = logstring + Log_String;
                    int TR_Num = sf.GetFileLineNumber();

                    Log_Write_String = string.Format("{0}{1,-8}{2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), "[" + TR_Num + "]", Log_String);

                    string filepath = ConfigurationManager.AppSettings["strlogpath"].ToString() + DateTime.Now.ToString("yyyy\\\\MM\\\\dd") + "\\";
                    if (!Directory.Exists(filepath))
                        Directory.CreateDirectory(filepath);

                    strLogFileName = strLogFileName + "_" + DateTime.Now.Hour.ToString("D2");
                    string finalpath = renameFiles(filepath, strLogFileName);
                    using (FileStream fs = new FileStream(finalpath, FileMode.Append, FileAccess.Write, FileShare.None, 8, true))
                    {
                        byte[] bt = System.Text.ASCIIEncoding.ASCII.GetBytes(Log_Write_String);
                        fs.BeginWrite(bt, 0, bt.Length, ascallback, fs);
                    }
                }
            }
            catch (Exception ex)
            {
                logger_for_exception.Append(ex.ToString(), 84, Log_Write_String);
            }
        }

        #endregion

        #region Log_Write(LogLevel GetLog_level, int TR_Num, string Log_String, string strLogFileName)

        //declares a delegate
        public delegate void Log_Write_Delgt4(LogLevel GetLog_level, int TR_Num, string Log_String, string strLogFileName);

        //we call this method
        public static void Log_Write(LogLevel GetLog_level, int TR_Num, string Log_String, string strLogFileName)
        {
            Log_Write_Delgt4 obj = new Log_Write_Delgt4(Log_Write_Call);
            obj.BeginInvoke(GetLog_level, TR_Num, Log_String, strLogFileName, null, null);
        }

        //delegate calls this method 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Log_Write_Call(LogLevel GetLog_level, int TR_Num, string Log_String, string strLogFileName)
        {
            string Log_Write_String = "";
            try
            {
                Log_String = Log_String.Replace("\r\n", "\n").Replace("\n", "*_*");
                string Log_level = ConfigurationManager.AppSettings["level" + (int)GetLog_level].ToString();
                if (Log_level == "1")
                {

                    Log_String = "[" + GetLog_level.ToString() + "]::" + Log_String;
                    Log_Write_String = string.Format("{0}{1,-8}{2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), "[" + TR_Num + "]", Log_String);

                    string filepath = ConfigurationManager.AppSettings["strlogpath"].ToString() + DateTime.Now.ToString("yyyy\\\\MM\\\\dd") + "\\";
                    if (!Directory.Exists(filepath))
                        Directory.CreateDirectory(filepath);

                    strLogFileName = strLogFileName + "_" + DateTime.Now.Hour.ToString("D2");
                    string finalpath = renameFiles(filepath, strLogFileName);
                    using (FileStream fs = new FileStream(finalpath, FileMode.Append, FileAccess.Write, FileShare.None, 8, true))
                    {
                        byte[] bt = System.Text.ASCIIEncoding.ASCII.GetBytes(Log_Write_String);
                        fs.BeginWrite(bt, 0, bt.Length, ascallback, fs);
                    }
                }
            }
            catch (Exception ex)
            {
                logger_for_exception.Append(ex.ToString(), 149, Log_Write_String);
            }
        }

        #endregion

        #region Error_Write(LogLevel GetLog_level, int ER_Num, Exception exString)

        //declares a delegate
        public delegate void Error_Write_Delgt1(LogLevel GetLog_level, int ER_Num, Exception exString);

        //we call this method
        public static void Error_Write(LogLevel GetLog_level, int ER_Num, Exception exString)
        {
            Error_Write_Delgt1 obj = new Error_Write_Delgt1(Error_Write_Call);
            obj.BeginInvoke(GetLog_level, ER_Num, exString, null, null);
        }

        //delegate calls this method 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Error_Write_Call(LogLevel GetLog_level, int ER_Num, Exception exString)
        {
            string Log_Write_String = "";
            try
            {

                //string Log_Level = ConfigurationManager.AppSettings[GetLoglevel].ToString();
                string Log_level = ConfigurationManager.AppSettings["level" + (int)GetLog_level].ToString();
                if (Log_level == "1")
                {
                    StackTrace st = new StackTrace(exString, true);
                    string Err_String = "[" + GetLog_level.ToString() + "]::";
                    StackFrame sf = st.GetFrame(st.FrameCount - 1);
                    StringBuilder stbuild = new StringBuilder();
                    for (int i = 0; i < st.FrameCount; i++)
                    {
                        sf = st.GetFrame(i);
                        stbuild = new StringBuilder();
                        stbuild.Append(Path.GetFileName(sf.GetFileName()));
                        stbuild.Append("::");
                        stbuild.Append(sf.GetMethod().Name + "()");
                        stbuild.Append("::");
                        stbuild.Append("[" + sf.GetFileLineNumber() + "]");
                        stbuild.Append("::");
                        Err_String += stbuild.ToString();
                    }

                    ER_Num = sf.GetFileLineNumber();
                    Err_String += exString.ToString().Replace("\r\n", "\n").Replace("\n", "*_*");
                    Log_Write_String = string.Format("{0}{1,-8}{2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), "[" + ER_Num + "]", Err_String);

                    string Errorfilepath = ConfigurationManager.AppSettings["strlogpath"].ToString() + DateTime.Now.ToString("yyyy\\\\MM\\\\dd") + "\\";
                    if (!Directory.Exists(Errorfilepath))
                        Directory.CreateDirectory(Errorfilepath);

                    string strLogFileName = ConfigurationManager.AppSettings["ErrorLogfileName"].ToString() + "_" + DateTime.Now.Hour.ToString("D2");
                    string finalpath = renameFiles(Errorfilepath, strLogFileName);

                    using (FileStream fs = new FileStream(finalpath, FileMode.Append, FileAccess.Write, FileShare.None, 8, true))
                    {
                        byte[] bt = System.Text.ASCIIEncoding.ASCII.GetBytes(Log_Write_String);
                        fs.BeginWrite(bt, 0, bt.Length, ascallback, fs);
                    }
                }
            }
            catch (Exception ex)
            {
                logger_for_exception.Append(ex.ToString(), 205, Log_Write_String);
            }
        }

        #endregion

        #region Error_Write(LogLevel GetLog_level, int ER_Num, Exception exString, string ErrorLogfileName)

        //declares a delegate
        public delegate void Error_Write_Delgt2(LogLevel GetLog_level, int ER_Num, Exception exString, string ErrorLogfileName);

        //we call this method
        public static void Error_Write(LogLevel GetLog_level, int ER_Num, Exception exString, string ErrorLogfileName)
        {
            Error_Write_Delgt2 obj = new Error_Write_Delgt2(Error_Write_Call);
            obj.BeginInvoke(GetLog_level, ER_Num, exString, ErrorLogfileName, null, null);
        }

        //delegate calls this method 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Error_Write_Call(LogLevel GetLog_level, int ER_Num, Exception exString, string ErrorLogfileName)
        {
            string Log_Write_String = "";
            try
            {
                string Log_level = ConfigurationManager.AppSettings["level" + (int)GetLog_level].ToString();
                if (Log_level == "1")
                {
                    StackTrace st = new StackTrace(exString, true);
                    string Err_String = "[" + GetLog_level.ToString() + "]::";
                    StackFrame sf = st.GetFrame(st.FrameCount - 1);
                    StringBuilder stbuild = new StringBuilder();
                    for (int i = 0; i < st.FrameCount; i++)
                    {
                        sf = st.GetFrame(i);
                        stbuild = new StringBuilder();
                        stbuild.Append(Path.GetFileName(sf.GetFileName()));
                        stbuild.Append("::");
                        stbuild.Append(sf.GetMethod().Name + "()");
                        stbuild.Append("::");
                        stbuild.Append("[" + sf.GetFileLineNumber() + "]");
                        stbuild.Append("::");
                        Err_String += stbuild.ToString();
                    }

                    ER_Num = sf.GetFileLineNumber();
                    Err_String += exString.ToString().Replace("\r\n", "\n").Replace("\n", "*_*");
                    Log_Write_String = string.Format("{0}{1,-8}{2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), "[" + ER_Num + "]", Err_String);

                    string Errorfilepath = ConfigurationManager.AppSettings["strlogpath"].ToString() + DateTime.Now.ToString("yyyy\\\\MM\\\\dd") + "\\";
                    if (!Directory.Exists(Errorfilepath))
                        Directory.CreateDirectory(Errorfilepath);

                    ErrorLogfileName = ErrorLogfileName + "_" + DateTime.Now.Hour.ToString("D2");
                    string finalpath = renameFiles(Errorfilepath, ErrorLogfileName);
                    using (FileStream fs = new FileStream(finalpath, FileMode.Append, FileAccess.Write, FileShare.None, 8, true))
                    {
                        byte[] bt = System.Text.ASCIIEncoding.ASCII.GetBytes(Log_Write_String);
                        fs.BeginWrite(bt, 0, bt.Length, ascallback, fs);
                    }
                }
            }
            catch (Exception ex)
            {
                logger_for_exception.Append(ex.ToString(), 258, Log_Write_String);
            }
        }

        #endregion

        #region Error_Write(LogLevel GetLog_level, int ER_Num, string exString)

        //declares a delegate
        public delegate void Error_Write_Delgt3(LogLevel GetLog_level, int ER_Num, string exString);

        //we call this method
        public static void Error_Write(LogLevel GetLog_level, int ER_Num, string exString)
        {
            Error_Write_Delgt3 obj = new Error_Write_Delgt3(Error_Write_Call);
            obj.BeginInvoke(GetLog_level, ER_Num, exString, null, null);
        }

        //delegate calls this method 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Error_Write_Call(LogLevel GetLog_level, int ER_Num, string exString)
        {
            string Log_Write_String = "";
            try
            {
                string Log_level = ConfigurationManager.AppSettings["level" + (int)GetLog_level].ToString();
                if (Log_level == "1")
                {

                    string Err_String = "[" + GetLog_level.ToString() + "]::";
                    Err_String += exString.ToString().Replace("\r\n", "\n").Replace("\n", "*_*");
                    Log_Write_String = string.Format("{0}{1,-8}{2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), "[" + ER_Num + "]", Err_String);

                    string Errorfilepath = ConfigurationManager.AppSettings["strlogpath"].ToString() + DateTime.Now.ToString("yyyy\\\\MM\\\\dd") + "\\";
                    if (!Directory.Exists(Errorfilepath))
                        Directory.CreateDirectory(Errorfilepath);
                    string strLogFileName = ConfigurationManager.AppSettings["ErrorLogfileName"].ToString() + "_" + DateTime.Now.Hour.ToString("D2");
                    string finalpath = renameFiles(Errorfilepath, strLogFileName);

                    using (FileStream fs = new FileStream(finalpath, FileMode.Append, FileAccess.Write, FileShare.None, 8, true))
                    {
                        byte[] bt = System.Text.ASCIIEncoding.ASCII.GetBytes(Log_Write_String);
                        fs.BeginWrite(bt, 0, bt.Length, ascallback, fs);
                    }
                }
            }
            catch (Exception ex)
            {
                logger_for_exception.Append(ex.ToString(), 298, Log_Write_String);
            }
        }

        #endregion

        #region Error_Write(LogLevel GetLog_level, int ER_Num, string exString, string ErrorLogfileName)

        //declares a delegate
        public delegate void Error_Write_Delgt4(LogLevel GetLog_level, int ER_Num, string exString, string ErrorLogfileName);

        //we call this method
        public static void Error_Write(LogLevel GetLog_level, int ER_Num, string exString, string ErrorLogfileName)
        {
            Error_Write_Delgt4 obj = new Error_Write_Delgt4(Error_Write_Call);
            obj.BeginInvoke(GetLog_level, ER_Num, exString, ErrorLogfileName, null, null);
        }

        //delegate calls this method 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Error_Write_Call(LogLevel GetLog_level, int ER_Num, string exString, string ErrorLogfileName)
        {
            string Log_Write_String = "";
            try
            {
                string Log_level = ConfigurationManager.AppSettings["level" + (int)GetLog_level].ToString();
                if (Log_level == "1")
                {
                    string Err_String = "[" + GetLog_level.ToString() + "]::";
                    Err_String += exString.ToString().Replace("\r\n", "\n").Replace("\n", "*_*");
                    Log_Write_String = string.Format("{0}{1,-8}{2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), "[" + ER_Num + "]", Err_String);

                    string Errorfilepath = ConfigurationManager.AppSettings["strlogpath"].ToString() + DateTime.Now.ToString("yyyy\\\\MM\\\\dd") + "\\";
                    if (!Directory.Exists(Errorfilepath))
                        Directory.CreateDirectory(Errorfilepath);
                    ErrorLogfileName = ErrorLogfileName + "_" + DateTime.Now.Hour.ToString("D2");
                    string finalpath = renameFiles(Errorfilepath, ErrorLogfileName);

                    using (FileStream fs = new FileStream(finalpath, FileMode.Append, FileAccess.Write, FileShare.None, 8, true))
                    {
                        byte[] bt = System.Text.ASCIIEncoding.ASCII.GetBytes(Log_Write_String);
                        fs.BeginWrite(bt, 0, bt.Length, ascallback, fs);
                    }
                }
            }
            catch (Exception ex)
            {
                logger_for_exception.Append(ex.ToString(), 335, Log_Write_String);
            }
        }

        #endregion

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void ascallback(IAsyncResult ar)
        {
            try
            {
                using (FileStream fs1 = (FileStream)ar.AsyncState)
                {
                    fs1.EndWrite(ar);
                    fs1.Close();
                }
            }
            catch (Exception ex)
            {
                logger_for_exception.Append(ex.ToString(), 349, ex.ToString());
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static string renameFiles(string filepath, string filename)
        {
            DirectoryInfo logdirinfo = new DirectoryInfo(filepath);
            FileInfo[] logfileinfo = logdirinfo.GetFiles(filename + ".*");
            FileInfo fi;
            if (logfileinfo.Length > 0)
            {
                fi = new FileInfo(filepath + filename);
                long filesize = fi.Length;
                long getfilesizefromconfig = long.Parse(ConfigurationManager.AppSettings["filesize"].ToString());
                long chkfilesize = checkfilesizefunction(filesize);
                if (chkfilesize >= getfilesizefromconfig)
                {

                    for (int i = logfileinfo.Length - 1; i >= 0; i--)
                    {
                        string exten = Path.GetExtension(logfileinfo[i].FullName);
                        string appendNumber = (Convert.ToInt32(exten == "" ? "0" : exten.Replace(".", "")) + 1).ToString("D2");
                        try
                        {

                            File.Move(logfileinfo[i].FullName, filepath + filename + "." + appendNumber);
                            new FileInfo(filepath + filename + "." + appendNumber).Attributes = FileAttributes.ReadOnly;


                        }
                        catch (Exception ex)
                        {
                            logger_for_exception.Append(ex.ToString(), 371, ex.ToString());
                        }
                    }

                }
            }
            return filepath + filename;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static long checkfilesizefunction(long filesizetocheck)
        {
            try
            {
                long filesize = filesizetocheck;
                long getfilesizefromconfig = long.Parse(ConfigurationManager.AppSettings["filesize"].ToString());
                string getfilesizeextension = ConfigurationManager.AppSettings["FileSizeFormat"].ToString();
                if (getfilesizeextension.ToUpper() == "KB")
                {
                    long chkfilesize = filesize / (1024);
                    return chkfilesize;
                }
                else if (getfilesizeextension.ToUpper() == "MB")
                {
                    long chkfilesize = filesize / (1024 * 1024);
                    return chkfilesize;
                }
                else if (getfilesizeextension.ToUpper() == "GB")
                {
                    long chkfilesize = filesize / (1024 * 1024 * 1024);
                    return chkfilesize;
                }
                return filesize;
            }
            catch (Exception ex)
            {
                logger_for_exception.Append(ex.ToString(), 542, ex.ToString());
                return 0;
            }
        }

    }

    public class logger_for_exception
    {
        private static StreamWriter sw;
        private static String logDirectory;
        static logger_for_exception()
        {
            try
            {
                logDirectory = ConfigurationManager.AppSettings["strlogpath"].ToString() + "logconfig.err";
                if (!File.Exists(logDirectory))
                {
                    FileStream fs = File.Create(logDirectory);
                    fs.Close();
                }
                sw = File.AppendText(logDirectory);
            }
            catch (Exception exc)
            {
            }
        }
        public static void Append(string Log_String, int TR_Num, string actualLogString)
        {
            try
            {
                lock (sw)
                {
                    Log_String = "[CRIT]::" + Log_String.Replace("\r\n", "\n").Replace("\n", "*_*"); ;
                    string Log_Write_String = string.Format("{0}{1,-8}{2}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), "[" + TR_Num + "]", actualLogString.Replace("\r\n", "\n").Replace("\n", "*_*") + ":::" + Log_String);
                    sw.Write(Log_Write_String);
                    sw.Flush();
                }
            }
            catch
            {
                // do nothing
            }
        }

    }

}

//____logconfig.Log_Write(____logconfig.LogLevel.INFO, new System.Diagnostics.StackTrace(true), "All threads executed successfully");
//____logconfig.Log_Write(____logconfig.LogLevel.INFO, new System.Diagnostics.StackTrace(true), "All threads executed successfully","FSSocket");
//____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc, "FSSocket");
//____logconfig.Error_Write(____logconfig.LogLevel.EXC, 0, exc);
//____logconfig.Log_Write(____logconfig.LogLevel.DB, new System.Diagnostics.StackTrace(true), "rows affected: " + count + " for query :" + strQuery);
//____logconfig.Log_Write(____logconfig.LogLevel.DB, new System.Diagnostics.StackTrace(true), "rows returned: " + count + " for query :" + strQuery);

//____logconfig.Log_Write(____logconfig.LogLevel.DB, 348, "page:-frm_Import" + "method:cmb_DB_Name_SelectedIndexChanged()" + "Query to get database details:" + strQry);
//____logconfig.Log_Write(____logconfig.LogLevel.INFO, 127, "page:-frm_Import" + "method:GetTabledetails()" + "Username=" + strUser + ",Pwd:" + strPwd);
//____logconfig.Log_Write(____logconfig.LogLevel.INFO, new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber(), Path.GetFileName(Request.Path) + "::" + (new System.Diagnostics.StackFrame()).GetMethod().Name + ":" + errorwrite);


/*
 <!--*********************logconfig parameters*********************-->
    <add key="filesize" value="2"/>
    <add key="FileSizeFormat" value="mb"/>
    <add key="strLogFileName" value="Log"/>
    <add key="strlogpath" value="D:\Voice\logfolder\"/>
    <add key="ErrorLogfileName" value="Error"/>
    <!--INFO = 0,    DEBUG = 1,    DB = 2,    EXC = 3,    PANIC = 4-->
    <add key="level0" value="1"/>
    <add key="level1" value="1"/>
    <add key="level2" value="1"/>
    <add key="level3" value="1"/>
    <add key="level4" value="1"/>
<!--*********************logconfig parameters*********************-->

*/