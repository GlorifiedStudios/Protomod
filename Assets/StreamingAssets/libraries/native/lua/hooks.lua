
Hook = {}
local cachedHooks = {}

function Hook.Attach( hookID, uniqueID, func )
    if not cachedHooks[hookID] then
        cachedHooks[hookID] = {}
    end
    cachedHooks[hookID][uniqueID] = func
end

function Hook.Call( hookID, ... )
    if( cachedHooks[hookID] ) then
        for k, v in pairs( cachedHooks[hookID] ) do
            v( ... )
        end
    end
end

function Hook.Remove( hookID, uniqueID )
    if cachedHooks and cachedHooks[hookID] and cachedHooks[hookID][uniqueID] then
        cachedHooks[hookID][uniqueID] = nil
    end
end

function Hook.GetAll()
    return cachedHooks
end