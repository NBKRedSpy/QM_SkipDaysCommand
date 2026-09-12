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

            SpaceTime spaceTime = Bootstrap._state.Get<SpaceTime>();
            TimeScale originalTime = spaceTime.TimeScale;
            State state = Bootstrap._state;

            try
            {
                SpaceGameMode instance = SingletonMonoBehaviour<SpaceGameMode>.Instance;
                DateTime targetTime = instance.SpaceTime.Time.AddHours(hours);

                instance.SpaceTime.TimeScale = TimeScale.X100;
                Time.timeScale = 150;

                //Hide, or the time will not change.
                UI.Hide<DevConsole>();

                yield return null;

                int lastDay = 0;

                while (targetTime > instance.SpaceTime.Time)
                {
                    int currentDay = ((int)(targetTime - instance.SpaceTime.Time).TotalDays);

                    if (currentDay != lastDay)
                    {
                        lastDay = currentDay;
                        Plugin.Logger.Log($"Skip Day {currentDay}");
                    }

                    //Check if the user toggled the console.
                    if(UI.IsShowing<DevConsole>())
                    {
                        string message = $"User toggled the console.  Stopping the skip days process.  Days unprocessed {currentDay}";
                        Plugin.Logger.Log(message);
                        UI.Get<DevConsole>()?.PrintText(message);

                        break;
                    }   

                    yield return null;
                }
            }
            finally
            {
                Time.timeScale = 1;
                spaceTime.TimeScale = originalTime;

                GameObject gameObject = _SkipDayProcess;
                _SkipDayProcess = null;
                GameObject.Destroy(gameObject);

                UI.Chain<DevConsole>().Show();

                Plugin.Logger.Log("Skip days process completed.");
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
