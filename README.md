# Protomod
Protomod was created to prototype Unity's modding potential for future [Glorified Studios](https://glorifiedstudios.com) projects. I have decided to make the project public so other people may be able to learn from it.

It features a fully working console made with ImGui, as well as a publisher-subscriber event system made in C# and hooked into Lua for an easy way to communicate events.

### How does it work?
Protomod implements [MoonSharp](https://www.moonsharp.org) into Unity to enable Lua scripting support. We then auto-include all Lua files under the `{ApplicationPath}/Mods` folder.

You can find most of the meat under the `Assets/Scripts/Lua` folder.

Press `F1` to toggle the ingame console.

![Screenshot of the in-game console](https://i.imgur.com/YYHVN4r.png)
