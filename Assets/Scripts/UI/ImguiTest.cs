using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;

public class ImguiTest : MonoBehaviour
{
    void OnEnable()
    {
        ImGuiUn.Layout += OnLayout;
    }

    void OnDisable()
    {
        ImGuiUn.Layout -= OnLayout;
    }

    void OnLayout()
    {
        ImGui.ShowDemoWindow();
    }
}
