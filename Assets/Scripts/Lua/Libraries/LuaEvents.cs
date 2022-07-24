
using System.Collections.Generic;
using MoonSharp.Interpreter;

namespace Protomod.Lua
{
    public class LuaEvents
    {
        public static Dictionary<string, List<Closure>> RegisteredEvents;

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

        public static void Call( string eventId, DynValue[] args )
        {
            if( !RegisteredEvents.ContainsKey( eventId ) ) return;

            foreach( Closure closure in RegisteredEvents[eventId] )
                closure.Call( args );
        }
    }
}