using System;
using System.Diagnostics;
using System.Reflection;
using SLUM.lib.Client;
using StormLib.Module;
using StormLib.Exceptions;

namespace SLUM
{
    class Program
    {
        static void Main(string[] args)
        {
            StormLib.Console.Init();
            
            Trace.Listeners.Add(new ConsoleTraceListener());
            Trace.Listeners.Add(new StormLib.Diagnostics.ConsoleTraceListiner());

            Trace.WriteLine("Hello World!");

            //Load up the configuration file
            try
            {
                SLUM.lib.Config.ServerConfig.LoadConfig();
            }
            catch (MissingResourceExecption ex)
            {
                Trace.WriteLine(string.Format("The server could not load the embedded resource. Please check following embedded resource: {0}", ex.Data["ResourceName"]));
                return;
            }
            catch (FileException ex)
            {
                Trace.WriteLine(string.Format("The server could not load or create the configuration file. More information: {0}", ex.InnerException.ToString()));
                return;
            }
            catch (InvalidOperationException ex)
            {
                Trace.WriteLine(string.Format("An unknown error occurred during deserialization. More information: {0}", ex.InnerException.ToString()));
                return;
            }
            if (SLUM.lib.Config.ServerConfig.Instance.MysqlUser == "{EDIT ME}" ||
               SLUM.lib.Config.ServerConfig.Instance.MysqlPass == "{EDIT ME}")
            {
                Trace.WriteLine("Edit the LoginConfig.xml file");
                // return;
            }
            if (SLUM.lib.Config.ServerConfig.Instance.AcceptAnyAddress)
            {
                Trace.WriteLine("Warning: This server is set to accept server connections from any IP address. ANY server with the secrets can connect.");
            }

            ModuleInfo.LoadModules(Assembly.GetExecutingAssembly(), true);

            TCPListener a = TCPListener.Instance;

            while (true)
            {
                Console.Write("$$>");
                Console.WriteLine(StormLib.Console.ProcessLine(Console.ReadLine()).Value);
            }
        }
    }
}
