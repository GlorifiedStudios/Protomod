
using RoslynCSharp;
using UnityEngine;

namespace Protomod.Modding
{
    public class ModHotloader : MonoBehaviour
    {
        public ScriptDomain ScriptDomain;

        void Start()
        {
            ScriptDomain = ScriptDomain.CreateDomain( "Mods" );
            /*ScriptType testScript = ScriptDomain.CompileAndLoadMainSource( @"
                using UnityEngine;
                using Protomod.Player;
                
                public class Test : MonoBehaviour {
                    void TestDebug() {
                        Debug.Log( PlayerReferences.Instance );
                    }
                }
            " );

            ScriptProxy testProxy = testScript.CreateInstance( gameObject );
            testProxy.Call( "TestDebug" );*/
        }
    }
}