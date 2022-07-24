
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

namespace Protomod.Lua
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

        public DynValue RunString( string lua )
        {
            try
            {
                DynValue output = Script.DoString( lua );
                return output;
            } catch( InterpreterException ex )
            {
                Console.ThrowLuaExceptionToConsole( ex );
                return DynValue.Nil;
            }
        }

        // External Bindings
        public void LuaRunCalled( string[] args ) => RunString( string.Join( " ", args ) );

        private void OnEnable() => Instance = this;
        private void OnDisable() => Instance = null;

        private void Start()
        {
            Console = Console.Instance;
            Script.DefaultOptions.DebugPrint = print => Console.PrintToConsole( print );
            InitializeLuaEnvironment();

            Console.RegisterConsoleCommand( "luarun", LuaRunCalled );
        }
    }
}