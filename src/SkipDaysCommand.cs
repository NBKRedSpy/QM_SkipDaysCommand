using JetBrains.Annotations;
using MGSC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SkipDaysCommand
{
    [ConsoleCommand(new string[] { "skip-days", "sd" })]
    public class SkipDaysCommand : MonoBehaviour
    {
        public static string Help(string command, bool verbose)
        {
            return "Skips game time X number of days or hours.  Use a 'h' suffix for hours.  Example: 'skip-days 30' or 'skip-days 8h.  alias: 'sd'";
        }

        private static GameObject _SkipDayProcess = null;

        public string Execute(string[] tokens)
        {

            if (_SkipDayProcess != null)
            {
                return "This command is currently running.";
            }

            if (tokens.Length == 0)
            {
                return "Please provide a number of days to skip.";
            }

            if (tokens.Length > 1)
            {
                return "Too many arguments. Please provide only one number of days to skip.";
            }

            string timeParameter = tokens[0].Trim();

            int hours = 0;
            int days = 0;


            //Check if there is a h suffix for hours
            if (timeParameter.EndsWith("h", StringComparison.OrdinalIgnoreCase))
            {
                if (!int.TryParse(timeParameter.Substring(0, timeParameter.Length - 1), out hours))
                {
                    return "Invalid number of hours value.";
                }
            }
            else if (!int.TryParse(timeParameter, out days))
            {
                return "Invalid number of days value.";
            }

            _SkipDayProcess = new GameObject();
            SkipDaysCommand command = _SkipDayProcess.AddComponent<SkipDaysCommand>();
            command.StartCoroutine(SkipDays(days * 24 + hours));

            return "The process is now running in the background.  Status is logged in the Unity Player.log";
        }

        public static IEnumerator SkipDays(int hours)
        {

            try
            {
                int i = 0;
                while (i < hours)
                {
                    //7 day blocks seem to be about right for processing.
                    int skipHours = 7 * 24;  

                    if (i + skipHours > hours)
                    {
                        skipHours = hours - i;
                    }

                    i += skipHours;

                    //This is the same code as MGSC.SandboxDebugWindow.SkipWeekButtonOnClick(MGSC.CommonButton, int)
                    SpaceGameMode instance = SingletonMonoBehaviour<SpaceGameMode>.Instance;
                    DateTime time = instance.SpaceTime.Time;
                    instance.SpaceTime.Time = instance.SpaceTime.Time.AddHours(skipHours);
                    instance.SpaceTime.DeltaTime = (float)(instance.SpaceTime.Time - time).TotalSeconds;

                    Plugin.Logger.Log($"Skip Day {i / 24}");

                    yield return null;
                }
            }
            finally
            {
                GameObject gameObject = _SkipDayProcess;
                _SkipDayProcess = null;
                GameObject.Destroy(gameObject);
            }
        }

        public static List<string> FetchAutocompleteOptions(string command, string[] tokens)
        {
            return null;
        }

        public static bool IsAvailable()
        {
            return SpaceGameMode.Instance != null;
        }

        public static bool ShowInHelpAndAutocomplete()
        {
            return true;
        }
    }
}
