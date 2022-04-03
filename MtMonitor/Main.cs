using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MetaTraderDeveloperTools
{
    class Main
    {
        public void Start()
        {
            string[] mqlPathList = GetMqlPathList();
            if (mqlPathList.Count() < 1)
            {
                return;
            }

            // Delete logs older than 1 week
            //RemoveMTLog(mqlPathList);

            CheckMTLog(mqlPathList);
        }

        private string[] GetMqlPathList()
        {
            string[] mqlPath = { };
            int idx = 0;

            string[] userList = Directory.GetDirectories(@"C:\Users");

            for (int i = 0; i <= userList.Length - 1; ++i)
            {
                string tmPath = userList[i] + @"\AppData\Roaming\MetaQuotes\Terminal";
                if (Directory.Exists(tmPath) == false)
                {
                    continue;
                }
                foreach (string d in Directory.GetDirectories(tmPath))
                {
                    if (Path.GetFileName(d).Length == 32)
                    {
                        if (File.Exists(d + @"\origin.txt") == false)
                        {
                            continue;
                        }
                        Array.Resize(ref mqlPath, idx + 1);
                        mqlPath.SetValue(d, idx);
                        ++idx;
                    }
                }
            }

            return mqlPath;
        }

        private void RemoveMTLog(string[] mqlPathList)
        {
            int targetYmd = int.Parse(DateTime.Now.AddDays(-7).ToString("yyyyMMdd"));

            for (int i = 0; i <= mqlPathList.Length - 1; ++i)
            {
                RemoveMTLog_MTOriginal(targetYmd, mqlPathList[i]);

                string mqlDir;
                string dirPath_MQL4 = mqlPathList[i] + @"\MQL4";
                string dirPath_MQL5 = mqlPathList[i] + @"\MQL5";

                if (Directory.Exists(dirPath_MQL4))
                {
                    mqlDir = dirPath_MQL4;
                }
                else if (Directory.Exists(dirPath_MQL5))
                {
                    mqlDir = dirPath_MQL5;
                }
                else
                {
                    continue;
                }

                RemoveMTLog_MTOriginal(targetYmd, mqlDir);
            }
        }

        private void RemoveMTLog_MTOriginal(int targetYmd, string mqlDir)
        {
            string logDir = mqlDir + @"\Logs";
            if (Directory.Exists(logDir) == false)
            {
                return;
            }

            foreach (string filePath in Directory.GetFiles(logDir))
            {
                string baseName = Path.GetFileNameWithoutExtension(filePath);

                if (baseName.Length != 8)
                {
                    continue;
                }

                int fileYmd;
                if (int.TryParse(baseName, out fileYmd) == false)
                {
                    continue;
                }

                if (fileYmd < targetYmd)
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch
                    {
                        //
                    }
                }
            }
        }

        private void CheckMTLog(string[] mqlPathList)
        {
            DateTime targetDate = DateTime.Now.AddDays(-1);
            while (true)
            {
                if (IsDayOff(targetDate) == false)
                {
                    break;
                }

                targetDate = targetDate.AddDays(-1);
            }

            for (int i = 0; i <= mqlPathList.Length - 1; ++i)
            {
                string logPath_MQL4 = mqlPathList[i] + @"\MQL4\Logs\" + targetDate.ToString("yyyyMMdd") + ".log";
                string logPath_MQL5 = mqlPathList[i] + @"\MQL5\Logs\" + targetDate.ToString("yyyyMMdd") + ".log";

                bool b = false;
                if (File.Exists(logPath_MQL4))
                {
                    b = true;
                }
                else if (File.Exists(logPath_MQL5))
                {
                    b = true;
                }

                if (b == false)
                {
                    MessageBox.Show("[MetaTrader may not be working.]");
                }
            }
        }

        private bool IsDayOff(DateTime dt)
        {
            if ((dt.DayOfWeek == DayOfWeek.Saturday) || (dt.DayOfWeek == DayOfWeek.Sunday))
            {
                return true;
            }

            if ((dt.Month == 1) && (dt.Day == 1))
            {
                return true;
            }

            return false;
        }
    }
}
