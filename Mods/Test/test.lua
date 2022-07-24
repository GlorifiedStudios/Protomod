
local function OnModsLoaded()
    print( "All of our mods have been loaded!" )
end

Event.Register( "ModsLoaded", OnModsLoaded ) -- If we want to unregister this event, we can assign the expression to a variable and then call Event.Unregister()