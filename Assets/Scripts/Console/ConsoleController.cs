
using ImGuiNET;
using System.Collections.Generic;
using UnityEngine;

namespace Protomod
{
    public struct ConsoleEntry
    {
        public string text;
        public Color color;
        public float timestamp;

        public ConsoleEntry( string text )
        {
            this.text = text;
            this.color = Color.white;
            this.timestamp = Time.realtimeSinceStartup;
        }

        public ConsoleEntry( string text, Color color )
        {
            this.text = text;
            this.color = color;
            this.timestamp = Time.realtimeSinceStartup;
        }
    }

    public class ConsoleController : MonoBehaviour
    {
        public List<ConsoleEntry> consoleLines = new List<ConsoleEntry>();
        public Vector2 defaultWindowSize = new Vector2( 620, 420 );
        public bool autoScroll = true;
        public bool timestamps = true;

        public static bool consoleActive = false;

        private bool shouldScrollToBottom = false;

        public void AddLineToConsole( string newText, Color color )
        {
            ConsoleEntry consoleEntry = new ConsoleEntry( newText, color );
            consoleLines.Add( consoleEntry );
            shouldScrollToBottom = true;
        }

        public void ThrowError( string errorText ) => AddLineToConsole( "[error] " + errorText, Color.red );
        public void ThrowWarning( string warningText ) => AddLineToConsole( "[warning] " + warningText, Color.yellow );
        public void PrintToConsole( string printText ) => AddLineToConsole( printText, Color.white );

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
            }
        }

        void Update()
        {
            if( Input.GetKeyDown( KeyCode.F1 ) )
                ToggleConsole();
        }

        // UI
        private void OnEnable() => ImGuiUn.Layout += OnLayout;
        private void OnDisable() => ImGuiUn.Layout -= OnLayout;

        private Vector2 logScrollingRegionSize = Vector2.zero;
        private void OnLayout()
        {
            if( !consoleActive ) return;

            ImGui.ShowDemoWindow();

            // Window Configuration Start //
            ImGuiWindowFlags windowFlags = 0;
            windowFlags |= ImGuiWindowFlags.NoCollapse;

            ImGui.SetNextWindowSize( defaultWindowSize, ImGuiCond.Once );

            if( !ImGui.Begin( "Developer Console", windowFlags ) )
            {
                ImGui.End();
                return;
            }
            // Window Configuration End //

            // Debug //
            if( ImGui.Button( "Test Print" ) ) PrintToConsole( "this is a test print" );
            ImGui.SameLine();
            if( ImGui.Button( "Test Error" ) ) ThrowError( "this is a test error" );
            ImGui.SameLine();
            if( ImGui.Button( "Test Warning" ) ) ThrowWarning( "this is a test warning" );
            // Debug End //

            // Options Start //
            ImGui.Button( "Copy" );

            ImGui.SameLine();

            ImGui.Button( "Clear" );

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

            ImGui.Text( "Filter" );
            ImGui.SameLine();
            byte[] filterBuffer = new byte[128];
            ImGui.InputText( "", filterBuffer, 128 );
            // Options End

            ImGui.Separator();

            // Log Start //
            logScrollingRegionSize.y = -( ImGui.GetStyle().ItemSpacing.y + ImGui.GetFrameHeightWithSpacing() );
            ImGui.BeginChild( "ScrollingRegion", logScrollingRegionSize, false, ImGuiWindowFlags.HorizontalScrollbar ); ;

            foreach( ConsoleEntry line in consoleLines )
            {
                ImGui.PushStyleColor( ImGuiCol.Text, line.color );

                ImGui.TextUnformatted( line.text );

                ImGui.PopStyleColor();
            }

            if( shouldScrollToBottom || autoScroll && ImGui.GetScrollY() >= ImGui.GetScrollMaxY() )
                ImGui.SetScrollHereY( 1.0f );

            shouldScrollToBottom = false;

            ImGui.EndChild();
            // Log End //

            ImGui.Separator();

            // Command Line Start //
            byte[] commandLineBuffer = new byte[128];
            ImGui.InputText( "Command Line", commandLineBuffer, 128 );
            // Command Line End //

            ImGui.End();
        }
    }
}