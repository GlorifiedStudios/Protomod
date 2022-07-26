
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protomod.Lua
{
    // TODO: Make use of the require function for manual including. See https://www.moonsharp.org/scriptloaders.html
    public class LuaFileLoader : MonoBehaviour
    {
        public LuaEnvironment Environment;
        public Console Console;
        public static string ModFolderName = "Mods";

        public string GetModsPath()
        {
            return Path.Combine( Path.GetFullPath( "." ), ModFolderName );
        }

        public void LoadAllMods()
        {
            foreach( string folder in Directory.GetDirectories( GetModsPath(), "*.*", SearchOption.AllDirectories ) )
                foreach( string file in Directory.GetFiles( folder, "*.lua", SearchOption.AllDirectories ) )
                    Environment.LoadLuaFile( file );

            LuaEvents.Call( "ModsLoaded" );
        }

        private void OnEnable() => LuaEnvironment.OnLuaInitialized += OnLuaInitialized;
        private void OnDisable() => LuaEnvironment.OnLuaInitialized -= OnLuaInitialized;

        private void LuaRefreshCalled( string[] args )
        {
            LoadAllMods();
        }

        private void OnLuaInitialized()
        {
            LoadAllMods();
            Console.RegisterConsoleCommand( "lua_refresh", LuaRefreshCalled );
        }
    }
}