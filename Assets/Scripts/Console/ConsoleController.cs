
using ImGuiNET;
using UnityEngine;
using System.Collections.Generic;

namespace Protomod
{
    public class ConsoleController : MonoBehaviour
    {
        public List<string> consoleLines = new List<string>();
        public Vector2 defaultWindowSize = new Vector2( 620, 420 );
        public bool autoScroll = true;
        public bool timestamps = true;

        public static bool consoleActive = false;

        private bool shouldScrollToBottom = false;

        public void AddLineToConsole( string newText )
        {
            consoleLines.Add( newText );
            shouldScrollToBottom = true;
        }

        public void ThrowError( string errorText ) => AddLineToConsole( "[error] " + errorText );
        public void ThrowWarning( string warningText ) => AddLineToConsole( "[warning] " + warningText);
        public void PrintToConsole( string printText ) => AddLineToConsole( printText );

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
            ImGui.SameLine(); if( ImGui.Button( "Test Error" ) ) ThrowError( "this is a test print" );
            ImGui.SameLine(); if( ImGui.Button( "Test Warning" ) ) ThrowWarning( "this is a test print" );
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
            ImGui.BeginChild( "ScrollingRegion", logScrollingRegionSize, false, ImGuiWindowFlags.HorizontalScrollbar );;

            foreach( string line in consoleLines )
            {
                bool hasColor = false;

                if( line.Contains( "[error]" ) )
                {
                    ImGui.PushStyleColor( ImGuiCol.Text, Color.red );
                    hasColor = true;
                } else if( line.Contains( "[warning]" ) )
                {
                    ImGui.PushStyleColor( ImGuiCol.Text, Color.yellow );
                    hasColor = true;
                }

                ImGui.TextUnformatted( line );

                if( hasColor )
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