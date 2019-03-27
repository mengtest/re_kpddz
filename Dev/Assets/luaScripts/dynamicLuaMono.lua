-- 动态绑定举例
-- WP.Chu 2016.10.15

local M = GENERATE_MODULE("dynamicLuaMono")

function M.Awake(gameObj)
    -- print("[DYNAMIC] Awake is invoked #### " .. gameObj.name .. " ####")
end

function M.Start(gameObj)
    -- print("[DYNAMIC] Start is invoked #### " .. gameObj.name .. " ####")
end

function M.Update()
    -- print("[DYNAMIC] Update is invoked")
end

function M.OnEnable(gameObj)
    -- print("[DYNAMIC] OnEnable is invoked #### " .. gameObj.name .. " ####")
end


function M.OnDisable(gameObj)
    -- print("[DYNAMIC] OnDisable is invoked #### " .. gameObj.name .. " ####")
end

function M.OnDestroy(gameObj)
    -- print("[DYNAMIC] OnDestroy is invoked #### " .. gameObj.name .. " ####")
end

return M