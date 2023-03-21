using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace JSContainer.Interop
{
    [InteropModule("console", true)]
    public class ConsoleModule
    {
        public static void log(string text)
        {
            Debug.Log(text);
        }
    }
}