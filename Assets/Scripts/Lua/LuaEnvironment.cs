
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

namespace Protomod
{
    public class LuaEnvironment : MonoBehaviour
    {
        public static LuaEnvironment Instance;
        public Script Script;
        public Console Console;

        // Unity Bindings
        private void OnEnable() => Instance = this;
        private void OnDisable() => Instance = null;

        private void Start()
        {
            Console = Console.Instance;
            Script.DefaultOptions.DebugPrint = print => Console.PrintToConsole( print );
        }
    }
}