//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.


using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

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
            //{ 
                this.logFileName = fileName; 
            //}

            logFile = Application.StartupPath.ToString() + @"\appLOG";

            // if logfile directory does not exists, create it
            bool blnDirectoryExists = Directory.Exists(logFile);
            if (!blnDirectoryExists)
            {
                try
                { Directory.CreateDirectory(logFile); }
                catch (Exception e)
                { Debug.WriteLine("CREATELOG: " + e.ToString()); }
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
                    //this line below added to correct warning:
                    //Warning	395	CA2000 : Microsoft.Reliability : 
                    //In method 'LogFiles.CreateLog(string, ListBox)', call System.IDisposable.Dispose on object 'tw' before all references to it are out of scope.	C:\Users\ahadjigeorgalis\Documents\GitHub\TT-FIX-AutoFlatten\FIX_AutoFlatten\LogFiles.cs	70	FIX_AutoFlatten
                    //warning still shows after edit
                    //tw.Dispose();
                }
                catch (Exception e)
                { Debug.WriteLine("CREATELOG: " + e.ToString()); }
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
            { Debug.WriteLine("CloseLog:" + e.ToString()); }
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
            { Debug.WriteLine(DateTime.Now.ToString() + ": WRITELOG:" + System.Environment.NewLine + e.ToString()); }

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
                            Debug.WriteLine("CleanLog: " + e.ToString());
                        }
                    }
                }
            }
        }



    }
}
