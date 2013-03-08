/***************************************************************************
 *    
 *      Copyright (c) 2009 Trading Technologies International, Inc.
 *                     All Rights Reserved Worldwide
 *
 *        * * *   S T R I C T L Y   P R O P R I E T A R Y   * * *
 *
 * WARNING:  This file is the confidential property of Trading Technologies
 * International, Inc. and is to be maintained in strict confidence.  For
 * use only by those with the express written permission and license from
 * Trading Technologies International, Inc.  Unauthorized reproduction,
 * distribution, use or disclosure of this file or any program (or document)
 * derived from it is prohibited by State and Federal law, and by local law
 * outside of the U.S. 
 *
 ***************************************************************************
 * $Date: 2009/12/21 23:34:00EST $
 * $Revision: 1.0 $
 ***************************************************************************/

using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace LOG
{
    /// <summary>
    /// version 2009.02.19
    /// </summary>
     public class LogFiles
    {
        private string logFileName = "LOG";
        private string logFileExt = "log";
        private string logFile = null;
        private ListBox lb = null;
        private TextWriter logTextWriter = null;
        private ushort ctr = 0;



        internal void CreateLog(string fileName, ListBox box)
        {
            if (box != null) { this.lb = box; }

            //if (fileName.Contains("."))
            //{
            //    this.logFileName = fileName.Substring(0, fileName.IndexOf("."));
            //    this.logFileExt = fileName.Substring(fileName.IndexOf(".") + 1, fileName.Length - (fileName.IndexOf(".") + 1));
            //}
            //else
            { this.logFileName = fileName; }

            logFile = Application.StartupPath.ToString() + @"\appLOG";

            // if logfile directory does not exists, create it
            bool blnDirectoryExists = Directory.Exists(logFile);
            if (!blnDirectoryExists)
            {
                try
                { Directory.CreateDirectory(logFile); }
                catch (Exception e)
                { MessageBox.Show("CREATELOG: " + e.ToString()); }
            }

            logFile += @"\" + logFileName + "_";
            logFile += DateTime.Now.ToString("yyyyMMMdd_HH_mm_ss");
            logFile += "." + logFileExt;

            bool blnFileExists = File.Exists(logFile);
            if (!blnFileExists)
            {
                try
                { 
                    TextWriter tw = new StreamWriter(logFile);
                    logTextWriter = TextWriter.Synchronized(tw);
                }
                catch (Exception e)
                { MessageBox.Show("CREATELOG: " + e.ToString()); }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        internal void CloseLog()
        {
            try
            {
                if (logTextWriter != null)
                { logTextWriter.Close(); }
            }
            catch (Exception e)
            { MessageBox.Show("WRITELOG:" + e.ToString()); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        internal void WriteLog(string msg)
        {
            Console.WriteLine(msg);

            try
            {
                if (ctr > 65000)
                {
                    logTextWriter.WriteLine("CREATELOG triggered by line count");
                    logTextWriter.Flush();
                    logTextWriter.Close();

                    CreateLog(this.logFileName + "." + this.logFileExt, lb);
                    ctr = 0;
                }

                logTextWriter.WriteLine(DateTime.Now.ToString("yyyyMMdd\tHH:mm:ss.fff\t") + " " + msg);
                logTextWriter.Flush();
                ctr++;
            }
            catch (Exception e)
            { MessageBox.Show("WRITELOG:" + e.ToString()); }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        internal void WriteList(string msg)
        {
            if (msg == null) { msg = "NULL MESSAGE"; }

            if (lb != null)
            {
                int maxLines = (lb.Size.Height / lb.ItemHeight);


                if (lb.Items.Count >= maxLines)
                {
                    lb.Items.RemoveAt(0);
                    lb.Items.Add(msg);
                }
                else
                {
                    lb.Items.Add(msg);
                }
            }

            WriteLog(msg);
        }

        /// <summary>
        /// delete all log files older than daysSaved
        /// </summary>
        internal static void CleanLog(int daysSaved)
        {
            string logpath = Application.StartupPath.ToString() + @"\LOG";

            if (System.IO.Directory.Exists(logpath))
            {
                string[] files = System.IO.Directory.GetFiles(logpath);

                foreach (string s in files)
                {
                    FileInfo fi = new FileInfo(s);

                    if (DateTime.Compare(fi.LastWriteTime, DateTime.Today.Subtract(TimeSpan.FromDays(daysSaved))) < 0)
                    {
                        try
                        {
                            fi.Delete();
                        }
                        catch (System.IO.IOException e)
                        {
                           // WriteLog("CLEAN: " + e.ToString());
                            MessageBox.Show("CLEAN: " + e.ToString());
                        }
                    }
                }
            }
        }



    }
}
