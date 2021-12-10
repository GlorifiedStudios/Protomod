﻿using UnityEngine;

namespace RoslynCSharp.Example
{
    /// <summary>
    /// An example script that shows how to use the compiler service to compile and load a C# source code string and then call a static method.
    /// </summary>
    public class Ex10_StaticMethods : MonoBehaviour
    {
        private ScriptDomain domain = null;
        private const string sourceCode = @"
        using UnityEngine;
        class Example
        {
            static void ExampleMethod(string input)
            {
                Debug.Log(""Hello "" + input);
            }
        }";

        /// <summary>
        /// Called by Unity.
        /// </summary>
        public void Start()
        {
            // Create domain
            domain = ScriptDomain.CreateDomain("Example Domain");


            // Compile and load code - Note that we use 'CompileAndLoadMainSource' which is the same as 'CompileAndLoadSource' but returns the main type in the compiled assembly
            ScriptType type = domain.CompileAndLoadMainSource(sourceCode, ScriptSecurityMode.UseSettings);



            // Call the static method called 'ExampleMethod' and pass the string argument 'World'
            // Note that any exceptions thrown by the target method will not be caught
            type.CallStatic("ExampleMethod", "World");


            // Call the static method called 'ExampleMethod' and pass the string argument 'Safe World'
            // Note that any exceptions thrown by the target method will handled as indicated by the 'Safe' name
            type.SafeCallStatic("ExampleMethod", "Safe World");
        }
    }
}