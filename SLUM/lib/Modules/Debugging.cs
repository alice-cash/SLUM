using System;
using System.Collections.Generic;
using System.Text;
using SLUM.lib.Client;
using StormLib;
using StormLib.Module;

namespace SLUM.lib.Modules
{
    class Debugging : IModuleLoader
    {
        public static void SyncError(RemoteClient Sender, Dictionary<String, Object> ErrorLog0, Dictionary<String, Object> ErrorLog1, Dictionary<String, Object> ErrorLog2)
        {
            string level = StormLib.Console.GetValue("Debugging_Level").Value;
            switch (level)
            {
                default:
                case "0":
                   // Sender.SyncError(ErrorLog0);
                    break;
                case "1":
                  //  Sender.SyncError(ErrorLog1);
                    break;
                case "2":
                  //  Sender.SyncError(ErrorLog2);
                    break;
            }
        }

        public static void LogError(RemoteClient Sender, Dictionary<String, Object> ErrorLog0, Dictionary<String, Object> ErrorLog1, Dictionary<String, Object> ErrorLog2)
        {

        }

        public Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }

        public string Name
        {
            get { return "Debugging"; }
        }

        public void Load()
        {
            StormLib.Console.SetValue("Debugging_Level", new ConsoleVarable()
            {
                ValidCheck = CheckConsoleInput,
                Value = "0",
                HelpInfo = StormLib.Localization.DefaultLanguage.Strings.GetString("DEBUGGING_LEVEL_HELP")
            });

            StormLib.Console.SetFunc("debug_db_grabusr", new ConsoleFunction()
            {
                Function = new Func<string[], ConsoleResponse>((string[] debug_args) =>
                {
                    if (debug_args.Length == 0 || debug_args.Length > 1)
                        return ConsoleResponse.Failed(StormLib.Localization.DefaultLanguage.Strings.GetString("DEBUG_DB_GRABUSR_HELP"));
                    if (debug_args.Length == 1)
                    {
                       // var v = Data.Tables.Account.GetAccountByUsername(debug_args[0]);
                       // if (v.Sucess == false)
                        //    return ConsoleResponse.Failed(StormLib.Localization.DefaultLanguage.Strings.GetFormatedString("DEBUG_DB_GRABUSR_NOFIND", debug_args[0]));

                       // return ConsoleResponse.Failed(StormLib.Localization.DefaultLanguage.Strings.GetFormatedString("DEBUG_DB_GRABUSR_FIND", v.Result.ID, v.Result.Username, v.Result.Password));
                    }
                    else
                    {

                    }

                    return ConsoleResponse.Succeeded("");
                }),
                HelpInfo = StormLib.Localization.DefaultLanguage.Strings.GetString("DEBUG_DB_GRABUSR_HELP")
            });

        }

        ExecutionState CheckConsoleInput(string input)
        {
            input = input.Trim();
            switch (input)
            {
                case "0":
                case "1":
                case "2":
                    return ExecutionState.Succeeded();
                default:
                    return ExecutionState.Failed("Input must be 0, 1, or 2");
            }
        }
    }
}
