
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protomod.Lua
{
    public class LuaFileLoader : MonoBehaviour
    {
        public LuaEnvironment Environment;
        public string ModFolderName = "Mods";

        public string GetModsPath()
        {
            return Path.Combine( Path.GetFullPath( "." ), ModFolderName );
        }

        public void LoadAllMods()
        {
            foreach( string folder in Directory.GetDirectories( GetModsPath(), "*.*", SearchOption.AllDirectories ) )
                foreach( string file in Directory.GetFiles( folder, "*.lua", SearchOption.AllDirectories ) )
                    Environment.LoadLuaFile( file );
        }

        private void OnEnable() => LuaEnvironment.OnLuaInitialized += OnLuaInitialized;
        private void OnDisable() => LuaEnvironment.OnLuaInitialized -= OnLuaInitialized;

        private void OnLuaInitialized()
        {
            LoadAllMods();
        }
    }
}