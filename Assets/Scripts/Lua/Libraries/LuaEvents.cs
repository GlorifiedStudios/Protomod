
using System.Collections.Generic;
using MoonSharp.Interpreter;

namespace Protomod.Lua
{
    [MoonSharpUserData]
    public class LuaEvents
    {
        public static Dictionary<string, List<Closure>> RegisteredEvents = new Dictionary<string, List<Closure>>();

        public static Closure Register( string eventId, Closure closure )
        {
            if( RegisteredEvents.ContainsKey( eventId ) )
                RegisteredEvents[eventId].Add( closure );
            else
            {
                List<Closure> closures = new List<Closure>();
                closures.Add( closure );
                RegisteredEvents[eventId] = closures;
            }
            return closure;
        }

        public static void Unregister( string eventId, Closure closure )
        {
            if( RegisteredEvents.ContainsKey( eventId ) )
                RegisteredEvents[eventId].Remove( closure );
        }

        public static void Call( string eventId )
        {
            if( !RegisteredEvents.ContainsKey( eventId ) ) return;

            foreach( Closure closure in RegisteredEvents[eventId] )
                closure.Call();
        }

        public static void Call( string eventId, CallbackArguments args )
        {
            // Since we want to support varargs with this function, we have to remove the 1st value from the argument list.
            List<DynValue> argValues = new List<DynValue>( args.GetArray() );
            argValues.RemoveAt( 0 );
            
            if( !RegisteredEvents.ContainsKey( eventId ) ) return;

            foreach( Closure closure in RegisteredEvents[eventId] )
                closure.Call( argValues.ToArray() );
        }
    }
}