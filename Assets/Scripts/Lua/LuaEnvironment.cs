
using System.IO;
using UnityEngine;
using MoonSharp.Interpreter;
using System;
using MoonSharp.Interpreter.Loaders;

namespace Protomod.Lua
{
    public class LuaEnvironment : MonoBehaviour
    {
        public bool LuaInitialized = false;
        public static LuaEnvironment Instance;
        public Script Script;
        public Console Console;

        public static event Action OnLuaInitialized;

        private Color fileIncludedColor = new Color( 0.5f, 0.62f, 0.9f );

        public void InitializeLuaEnvironment()
        {
            Script = new Script();
            Script.Options.ScriptLoader = new FileSystemScriptLoader();
            Script.Options.DebugPrint = print => Console.PrintToConsole( print );

            // Hook our libraries to Lua globals.
            UserData.RegisterAssembly();
            Script.Globals["Event"] = new LuaEvents();

            LuaInitialized = true;

            OnLuaInitialized.Invoke();
        }

        public DynValue LoadLuaFile( string file )
        {
            int modsIndex = file.IndexOf( "Mods" );
            string fileNiceName = file.Substring( modsIndex, file.Length - modsIndex );

            if( Path.GetExtension( file ) != ".lua" || !File.Exists( file ) )
            {
                Console.ThrowWarning( "Failed to include Lua file '" + fileNiceName + "'" );
                return DynValue.Nil;
            }

            try
            {
                Console.AddLineToConsole( "Including Lua file '" + fileNiceName + "'", fileIncludedColor );
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

        private void Awake()
        {
            InitializeLuaEnvironment();

            Console.RegisterConsoleCommand( "luarun", LuaRunCalled );
        }
    }
}