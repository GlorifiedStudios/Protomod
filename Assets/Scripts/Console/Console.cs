﻿
// TODO: Create a [ConsoleCommand] attribute that can be hooked to methods for easier concommand registration.

using ImGuiNET;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using MoonSharp.Interpreter;

namespace Protomod
{
    public struct ConsoleEntry
    {
        public string text;
        public Color color;
        public DateTime datetime;

        public ConsoleEntry( string text )
        {
            this.text = text;
            this.color = Color.white;
            this.datetime = DateTime.Now;
        }

        public ConsoleEntry( string text, Color color )
        {
            this.text = text;
            this.color = color;
            this.datetime = DateTime.Now;
        }
    }

    public struct ConsoleCommand
    {
        public string command;
        public Action<string[]> method;

        public void Execute( string[] args )
        {
            if( method == null ) return;
            method.Invoke( args );
        }

        public ConsoleCommand( string command )
        {
            this.command = command;
            this.method = null;
        }

        public ConsoleCommand( string command, Action<string[]> method )
        {
            this.command = command;
            this.method = method;
        }
    }

    public class Console : MonoBehaviour
    {
        public List<ConsoleEntry> ConsoleEntries = new List<ConsoleEntry>();
        public List<ConsoleCommand> ConsoleCommands = new List<ConsoleCommand>();

        public int maxConsoleEntries = 1000;
        public Vector2 defaultWindowSize = new Vector2( 620, 420 );
        public Color timestampColor = new Color( 1f, 1f, 1f, 0.62f );
        public Color commandSentColor = new Color( 1f, 0.8f, 0.6f );
        public bool autoScroll = true;
        public bool timestamps = true;

        public static bool consoleActive = false;

        private Vector2 logScrollingRegionSize = Vector2.zero;
        private bool shouldCopy = false;
        private bool shouldScrollToBottom = false;
        private bool shouldFocusCommandLine = true;

        public void AddLineToConsole( string newText, Color color )
        {
            ConsoleEntry consoleEntry = new ConsoleEntry( newText, color );
            ConsoleEntries.Add( consoleEntry );
            if( autoScroll ) shouldScrollToBottom = true;
            if( ConsoleEntries.Count >= maxConsoleEntries )
                ConsoleEntries.RemoveRange( 0, ( ConsoleEntries.Count - maxConsoleEntries ) );
        }

        public void ThrowError( string errorText ) => AddLineToConsole( "[error] " + errorText, Color.red );
        public void ThrowWarning( string warningText ) => AddLineToConsole( "[warning] " + warningText, Color.yellow );
        public void PrintToConsole( string printText ) => AddLineToConsole( printText, Color.white );

        public void ThrowLuaExceptionToConsole( InterpreterException ex )
        {
            string niceMessage = ex.DecoratedMessage;
            niceMessage = niceMessage.Replace( @"\", "/" );
            ThrowError( "Lua error:\n" + niceMessage );
        }

        public void ToggleConsole()
        {
            consoleActive = !consoleActive;

            if( consoleActive )
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                shouldFocusCommandLine = true;
            }
        }

        public string[] SplitAndTrimBySpaces( string input )
        {
            return input.Split( ' ' ).Select( p => p.Trim() ).Where( p => !string.IsNullOrWhiteSpace( p ) ).ToArray();
        }
        
        // TODO: This can be simplified by a regular expression.
        public bool MatchesFilter( string input, string filter )
        {
            input = input.ToLower();
            filter = filter.ToLower();

            foreach( string filterRaw in SplitAndTrimBySpaces( filter ) )
            {
                string filterPiece = filterRaw;
                bool exclusive = false;
                if( filterRaw.StartsWith( "-" ) )
                {
                    exclusive = true;
                    filterPiece = filterRaw.Remove( 0, 1 );
                }

                if( !exclusive && !input.Contains( filterPiece ) ) return false;
                if( exclusive && input.Contains( filterPiece ) ) return false;
            }

            return true;
        }

        public bool ConsoleCommandExists( string command )
        {
            foreach( ConsoleCommand consoleCommand in ConsoleCommands )
                if( consoleCommand.command == command.ToLower() ) return true;

            return false;
        }

        public void RegisterConsoleCommand( string command, Action<string[]> callback )
        {
            if( ConsoleCommandExists( command ) ) return;
            ConsoleCommands.Add( new ConsoleCommand( command, callback ) );
        }

        public bool ExecuteCommandLineString( string cmdLineRaw )
        {
            AddLineToConsole( "# " + cmdLineRaw, commandSentColor );

            string[] cmdSplit = SplitAndTrimBySpaces( cmdLineRaw );
            string command = cmdSplit[0];
            List<string> args = new List<string>( cmdSplit );
            args.RemoveAt( 0 );

            foreach( ConsoleCommand consoleCommand in ConsoleCommands )
            {
                if( consoleCommand.command == command )
                {
                    consoleCommand.Execute( args.ToArray() );
                    return true;
                }
            }

            PrintToConsole( "Undefined command '" + command + "'" );
            return false;
        }

        // Unity Bindings
        private void Update()
        {
            if( Input.GetKeyDown( KeyCode.F1 ) )
                ToggleConsole();

            /* // While the below code is useful, Unity automatically makes the crosshair visible when you press Escape, so this should be used in the future/in 2D games.
            if( !consoleActive && Input.GetKeyDown( KeyCode.BackQuote ) )
                ToggleConsole();

            if( consoleActive && Input.GetKeyDown( KeyCode.Escape ) )
                ToggleConsole();
            */
        }


        private void OnEnable() => ImGuiUn.Layout += OnLayout;
        private void OnDisable() => ImGuiUn.Layout -= OnLayout;

        // Drawing
        private ImGuiInputTextFlags commandLineFlags = ImGuiInputTextFlags.EnterReturnsTrue;
        private ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoCollapse;
        private string filterText = "";
        private string commandLineText = "";

        private void OnLayout()
        {
            if( !consoleActive ) return;

            // Window Configuration Start //
            ImGui.SetNextWindowSize( defaultWindowSize, ImGuiCond.Once );

            if( !ImGui.Begin( "Developer Console", windowFlags ) )
            {
                ImGui.End();
                return;
            }
            // Window Configuration End //

            // Options Start //
            if( ImGui.Button( "Copy" ) )
                shouldCopy = true;

            ImGui.SameLine();

            if( ImGui.Button( "Clear" ) )
                ConsoleEntries.Clear();

            ImGui.SameLine();

            if( ImGui.BeginPopup( "Options" ) )
            {
                ImGui.Checkbox( "Auto-scroll", ref autoScroll );
                ImGui.Checkbox( "Timestamps", ref timestamps );
                ImGui.EndPopup();
            }

            if( ImGui.Button( "Options" ) )
                ImGui.OpenPopup( "Options" );

            ImGui.SameLine();

            ImGui.Text( "Filter (\"incl,-excl\")" );

            ImGui.SameLine();

            ImGui.PushItemWidth( -1 );
            ImGui.PushID( "console_filter" );
            ImGui.InputText( "", ref filterText, 128 );
            ImGui.PopID();
            ImGui.PopItemWidth();
            // Options End

            // Debug //
            if( ImGui.SmallButton( "Test Print" ) )
                PrintToConsole( "this is a test print" );

            ImGui.SameLine();

            if( ImGui.SmallButton( "Test Error" ) )
                ThrowError( "this is a test error" );

            ImGui.SameLine();

            if( ImGui.SmallButton( "Test Warning" ) )
                ThrowWarning( "this is a test warning" );
            // Debug End //

            ImGui.Separator();

            // Log Start //
            logScrollingRegionSize.y = -( ImGui.GetStyle().ItemSpacing.y + ImGui.GetFrameHeightWithSpacing() );
            ImGui.BeginChild( "ScrollingRegion", logScrollingRegionSize, false, ImGuiWindowFlags.HorizontalScrollbar ); ;

            if( shouldCopy )
                ImGui.LogToClipboard();

            foreach( ConsoleEntry line in ConsoleEntries )
            {
                if( !string.IsNullOrWhiteSpace( filterText ) )
                    if( !MatchesFilter( line.text, filterText ) ) continue;

                if( timestamps )
                {
                    ImGui.PushStyleColor( ImGuiCol.Text, timestampColor );
                    ImGui.TextUnformatted( "[" + line.datetime.ToString( "HH:mm:ss" ) + "]" );
                    ImGui.PopStyleColor();
                    ImGui.SameLine();
                }

                ImGui.PushStyleColor( ImGuiCol.Text, line.color );
                ImGui.TextUnformatted( line.text );
                ImGui.PopStyleColor();
            }

            if( shouldScrollToBottom || autoScroll && ImGui.GetScrollY() >= ImGui.GetScrollMaxY() )
                ImGui.SetScrollHereY( 1.0f );

            shouldScrollToBottom = false;

            if( shouldCopy )
            {
                ImGui.LogFinish();
                shouldCopy = false;
            }

            ImGui.EndChild();
            // Log End //

            ImGui.Separator();

            // Command Line Start //
            ImGui.PushItemWidth( -1 );
            ImGui.PushID( "console_commandline" );
            if( ImGui.InputText( "", ref commandLineText, 128, commandLineFlags ) )
            {
                shouldFocusCommandLine = true;
                if( !string.IsNullOrWhiteSpace( commandLineText ) )
                {
                    ExecuteCommandLineString( commandLineText );
                    commandLineText = "";
                }
            }
            ImGui.PopID();
            ImGui.PopItemWidth();
            // Command Line End //

            ImGui.SetItemDefaultFocus();

            if( shouldFocusCommandLine )
            {
                ImGui.SetKeyboardFocusHere( -1 );
                shouldFocusCommandLine = false;
            }

            ImGui.End();
        }

        // Help Command
        private void HelpCommandCalled( string[] args )
        {
            string consoleCommandsFormatted = "";
            foreach( ConsoleCommand consoleCommand in ConsoleCommands )
                consoleCommandsFormatted = consoleCommandsFormatted + consoleCommand.command + "\n";
            AddLineToConsole( consoleCommandsFormatted, Color.grey );
        }

        private void Start() => RegisterConsoleCommand( "help", HelpCommandCalled );
    }
}