
require( "types" )
require( "tableutil" )
require( "hooks" )

function bruh()
    print('yoyoyooyoyoy')
end

-- below is a bunch of debugging stuff
table.Print({
    ["Bruh"] = {
        ["Boy"] = 5,
        ["Boy2"] = 4,
        ["Boy3"] = {
            "Bruh", "Bruh2"
        }
    },
    ["Bruv"] = 6
})

Hook.Attach( "TestHook", "UniqueHookID", function()
    print( "UniqueHookID Called (TestHook)" )
end )

Hook.Attach( "TestHook", "UniqueHookID2", function()
    print( "UniqueHookID2 Called (TestHook)" )
end )

Hook.Attach( "ModulesLoaded", "AutorunFilesLoadedUniqueID", function()
    print( "Module Files Loaded" )
end )

Timer.Begin( 5, function()
    Hook.Remove( "TestHook", "UniqueHookID" )
    Hook.Call( "TestHook" )
end )