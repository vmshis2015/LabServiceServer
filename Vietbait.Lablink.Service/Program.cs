using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Service
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            //Thread.Sleep(10000);
            try
            {
                while (!Common.CheckConnection(Common.GetDefaultConnectionString()))
                {
                    Thread.Sleep(5000);
                    LablinkServiceConfig.RefreshConfig();
                }
                //Get Directory of exe file
                var servicesToRun = new ServiceBase[] {new MainService()};
                ServiceBase.Run(servicesToRun);
            }
            catch (Exception ex)
            {
            }
        }
    }
}