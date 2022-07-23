
using ImGuiNET;
using UnityEngine;

namespace Protomod
{
    public class ConsoleController : MonoBehaviour
    {
        public Vector2 defaultWindowSize = new Vector2( 620, 420 );
        public bool autoScrollDefault = true;
        public bool timestampsDefault = true;

        public static bool consoleActive = false;

        public static void AddLineToConsole( string newText )
        {
        }

        public static void ThrowError( string errorText ) { }
        public static void ThrowWarning( string warningText ) { }
        public static void PrintToConsole( string printText ) { }

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


            // Options Start //
            if( ImGui.BeginPopup( "Options" ) )
            {
                ImGui.Checkbox( "Auto-scroll", ref autoScrollDefault );
                ImGui.Checkbox( "Timestamps", ref timestampsDefault );
                ImGui.EndPopup();
            }

            if( ImGui.Button( "Options" ) )
                ImGui.OpenPopup( "Options" );

            ImGui.SameLine();

            byte[] filterBuffer = new byte[128];
            ImGui.InputText( "Filter", filterBuffer, 128 );
            // Options End

            ImGui.Separator();

            // Log Start //
            logScrollingRegionSize.y = -( ImGui.GetStyle().ItemSpacing.y + ImGui.GetFrameHeightWithSpacing() );
            ImGui.BeginChild( "ScrollingRegion", logScrollingRegionSize, false, ImGuiWindowFlags.HorizontalScrollbar );

            ImGui.LogToClipboard();

            ImGui.TextUnformatted( "Test 1" );
            ImGui.TextUnformatted( "Test 2" );

            ImGui.LogFinish();

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