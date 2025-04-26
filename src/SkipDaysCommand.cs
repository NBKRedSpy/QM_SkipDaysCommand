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
            return "Skips game time X number of days. Example: 'skip-days 30'.  alias: 'sd'";
        }

        private static GameObject _SkipDayProcess = null;

        public string Execute(string[] tokens)
        {

            if (tokens.Length == 0)
            {
                return "Please provide a number of days to skip.";
            }

            if (!int.TryParse(tokens[0], out int days))
            {
                return "Invalid number of days value.";
            }

            if (_SkipDayProcess != null)
            {
                return "This command is already running.";
            }

            _SkipDayProcess = new GameObject();
            SkipDaysCommand command = _SkipDayProcess.AddComponent<SkipDaysCommand>();
            command.StartCoroutine(SkipDays(days));

            return "The process is now running in the background.";
        }

        public static IEnumerator SkipDays(int days)
        {

            try
            {
                int i = 0;
                while (i < days)
                {
                    //Keeps the UI's date simple as the game always has 4 weeks in a month.
                    int skipDays = 7;

                    if (i + skipDays > days)
                    {
                        skipDays = days - i;
                    }

                    i += skipDays;

                    //This is the same code as MGSC.SandboxDebugWindow.SkipWeekButtonOnClick(MGSC.CommonButton, int)
                    SpaceGameMode instance = SingletonMonoBehaviour<SpaceGameMode>.Instance;
                    DateTime time = instance.SpaceTime.Time;
                    instance.SpaceTime.Time = instance.SpaceTime.Time.AddDays(skipDays);
                    instance.SpaceTime.DeltaTime = (float)(instance.SpaceTime.Time - time).TotalSeconds;

                    Plugin.Logger.Log($"Skip Day {i}");

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
