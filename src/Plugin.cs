﻿using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SkipDaysCommand
{
    public static class Plugin
    {

        public static ConfigDirectories ConfigDirectories = new ConfigDirectories();

        public static Logger Logger = new Logger();
    }
}
