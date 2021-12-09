
function table.Print( tbl, tblCount )
    if not tblCount then tblCount = 0 end
    local concat = ""
    for i = 1, tblCount do
        concat = concat .. "  "
    end
    for k, v in pairs( tbl ) do
        if( istable( v ) ) then
            print( concat .. k, tostring( v ) .. ":" )
            table.Print( v, tblCount + 1 )
        else
            print( concat .. k, v )
        end
    end
end

function table.GetKeys( tbl )
    local keys = {} local id = 1
    for k, v in pairs( tbl ) do keys[id] = k id = id + 1 end
    return keys
end