# MT-Monitor
A tool to investigate whether MetaTrader is running or not.

Development language: Visual Studio 2019 C#  
Platform: Windows OS

This tool is intended to be run on a daily, scheduled basis by the Windows Task Scheduler or similar functionality.

In this source code, a message box is displayed when it is determined that MetaTrader is not running, but it is recommended that a message be sent to email or Slack, as appropriate, to continue the process.

To determine if MetaTrader is running, we check the log output from MetaTrader.
If there are no logs for the previous day, we assume that MetaTrader is not running.

However, during holidays and Christmas, we do not use this method to determine if it is running or not because the markets are closed.

Also, out of concern that the number of logs can become very large due to MetaTrader constantly running, the tool retains the ability to delete logs that are a week old.

However, this feature is commented out.
To use this function, uncomment the RemoveMTLog() method in the Start() method of "main.cs".


    // Delete logs older than 1 week
    //RemoveMTLog(mqlPathList);


If you have any questions or requests for modification, please contact us from the (MetaTrader Developer Tools)[https://metatrader25.wixsite.com/metatrader-developer] website.
