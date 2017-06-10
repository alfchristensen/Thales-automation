using System;
using System.Threading;
using System.Diagnostics;

namespace RestartWhenIdle
{
    class Program
    {
        static void Main(string[] args)
        {
            while ((IdleTimeFinder.GetIdleTime() / 1000) < 180)
            {
                Console.WriteLine(IdleTimeFinder.GetIdleTime() / 1000);
                Thread.Sleep(1000);
            };

            Console.WriteLine("Countdown completed - Killing processes");

            bool processesKilled = false;

            while (processesKilled == false)
            {
                foreach (Process proc in Process.GetProcessesByName("TVM_MMI"))
                {
                    proc.Kill();
                }
                foreach (Process proc in Process.GetProcessesByName("TVM"))
                {
                    proc.Kill();
                }
                foreach (Process proc in Process.GetProcessesByName("VCF"))
                {
                    proc.Kill();
                }

                Thread.Sleep(5000);

                if ((Process.GetProcessesByName("TVM_MMI").Length == 0) && (Process.GetProcessesByName("TVM").Length == 0) && (Process.GetProcessesByName("VCF").Length == 0))
                {
                    processesKilled = true;
                }
                else processesKilled = false;
            }

            Console.WriteLine("Killing processes completed - restarting TVM");

            System.Diagnostics.Process.Start("shutdown.exe", "-r -f");
        }
    }
}
