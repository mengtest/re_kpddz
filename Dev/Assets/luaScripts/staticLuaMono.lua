-- 静态绑定举例
-- WP.Chu 2016.10.15

local M = GENERATE_MODULE("staticLuaMono")

function M.Awake(gameObj)
    -- print("[STATIC] Awake is invoked #### " .. gameObj.name .. " ####")
end

function M.Start(gameObj)
    -- print("[STATIC] Start is invoked #### " .. gameObj.name .. " ####")
end

function M.Update()
    --print("[STATIC] Update is invoked")
end

function M.OnEnable(gameObj)
    -- print("[STATIC] OnEnable is invoked #### " .. gameObj.name .. " ####")
end


function M.OnDisable(gameObj)
    -- print("[STATIC] OnDisable is invoked #### " .. gameObj.name .. " ####")
end

function M.OnDestroy(gameObj)
    -- print("[STATIC] OnDestroy is invoked #### " .. gameObj.name .. " ####")
end


return M