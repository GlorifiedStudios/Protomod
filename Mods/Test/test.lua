
local function OnModsLoaded()
    print( "All of our mods have been loaded!" )
end

Event.Register( "ModsLoaded", OnModsLoaded )