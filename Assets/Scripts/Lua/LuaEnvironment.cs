
using System.IO;
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

        public void InitializeLuaEnvironment()
        {
            Script = new Script();
        }

        public DynValue LoadLuaFile( string file )
        {
            if( Path.GetExtension( file ) != ".lua" || !File.Exists( file ) )
            {
                Console.ThrowWarning( "Failed to include Lua file: " + file );
                return DynValue.Nil;
            }

            try
            {
                DynValue output = Script.DoFile( file );
                return output;
            } catch( InterpreterException ex )
            {
                Console.ThrowLuaExceptionToConsole( ex );
                return DynValue.Nil;
            }
        }

        // Unity Bindings
        private void OnEnable() => Instance = this;
        private void OnDisable() => Instance = null;

        private void Start()
        {
            Console = Console.Instance;
            Script.DefaultOptions.DebugPrint = print => Console.PrintToConsole( print );
            InitializeLuaEnvironment();
            LoadLuaFile( "balls.lua" );
        }
    }
}