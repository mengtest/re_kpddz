/***************************************************************


 *
 *
 * Filename:  	luaMonoBehaviour.cs	
 * Summary: 	lua行为组件，负责和绑定到对象的lua脚本交互
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/14 18:04
 ***************************************************************/

using UnityEngine;
using System.Collections;
using SLua;
using System.Collections.Generic;

namespace sluaAux
{
    /// <summary>
    /// 脚本函数类型
    /// </summary>
    public enum ELuaMonoFunc
    {
        Awake,
        Start,
        Update,
        OnEnable,
        OnDisable,
        OnDestroy,
    }

    /// <summary>
    /// lua行为组件，负责和绑定到对象的lua脚本交互
    /// </summary>
    [CustomLuaClass]
    public class luaMonoBehaviour : MonoBehaviour
    {
        //绑定的脚本
        [SerializeField]
        public string bindScript = null;

        //lua
        LuaSvr _luaSvr = null;

        //lua函数部分
        Dictionary<ELuaMonoFunc, LuaFunction> _luaFuncs = new Dictionary<ELuaMonoFunc, LuaFunction>();
        LuaFunction _luaUpdate = null;

        ////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 加载Lua脚本
        /// </summary>
        void safeCallLuaScript()
        {
            if (string.IsNullOrEmpty(bindScript))
            {
#if UNITY_EDITOR
                string s = string.Format("The lua script of game object ** {0} ** is missing", gameObject.name.ToString());
                Debug.LogWarning(s);
#endif
                return;
            }

            if(_luaSvr == null || _luaSvr.luaState == null)
                return;

            //加载脚本
            LuaState ls = _luaSvr.luaState;
            LuaTable monoTbl = (LuaTable)ls.doFile(bindScript);
            if (monoTbl == null)
                return;

            addLuaFunctionBind(ELuaMonoFunc.Awake, (LuaFunction)monoTbl[ELuaMonoFunc.Awake.ToString()]);
            addLuaFunctionBind(ELuaMonoFunc.Start, (LuaFunction)monoTbl[ELuaMonoFunc.Start.ToString()]);
            addLuaFunctionBind(ELuaMonoFunc.Update, (LuaFunction)monoTbl[ELuaMonoFunc.Update.ToString()]);
            addLuaFunctionBind(ELuaMonoFunc.OnEnable, (LuaFunction)monoTbl[ELuaMonoFunc.OnEnable.ToString()]);
            addLuaFunctionBind(ELuaMonoFunc.OnDisable, (LuaFunction)monoTbl[ELuaMonoFunc.OnDisable.ToString()]);
            addLuaFunctionBind(ELuaMonoFunc.OnDestroy, (LuaFunction)monoTbl[ELuaMonoFunc.OnDestroy.ToString()]);
            _luaUpdate = _luaFuncs.ContainsKey(ELuaMonoFunc.Update) ? _luaFuncs[ELuaMonoFunc.Update] : null;

        }

        ////////////////////////////////////////////////////////////////////////////////////

        private void addLuaFunctionBind(ELuaMonoFunc eluaFunc, LuaFunction luafunc)
        {
            if (_luaFuncs.ContainsKey(eluaFunc) || luafunc == null) return;

            _luaFuncs.Add(eluaFunc, luafunc);
        }

        /// <summary>
        /// 调用脚本函数
        /// </summary>
        /// <param name="eFunc"></param>
        protected object callLuaFunction(ELuaMonoFunc eFunc)
        {
            if (bindScript == null)
                return null;

            LuaFunction func = null;
            if (_luaFuncs.TryGetValue(eFunc, out func))
                return func.call(gameObject);

#if UNITY_EDITOR
            string s = string.Format("Function [{0}] in the lua script ** {1}.{2} ** is missing", eFunc.ToString(), gameObject.name, bindScript);
            Debug.LogWarning(s);
#endif

            return null;
        }

        ////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 重载脚本
        /// </summary>
        public void loadLuaScript()
        {
            _luaFuncs.Clear();

            bindScript = string.IsNullOrEmpty(bindScript)
                ? luaSvrManager.getInstance().getDynLuaMono(gameObject.name)
                : bindScript;

            _luaSvr = luaSvrManager.getInstance().luaSvr;
            safeCallLuaScript();
        }

        ////////////////////////////////////////////////////////////////////////////////////

        #region MONO

        void Awake()
        {
            double t = UtilTools.GetCurrentTime();
            loadLuaScript();
            callLuaFunction(ELuaMonoFunc.Awake);
            UnityEngine.Debug.Log(bindScript + ":" + (UtilTools.GetCurrentTime() - t));
        }

        // Use this for initialization
        void Start()
        {
            callLuaFunction(ELuaMonoFunc.Start);
        }

        // Update is called once per frame
        void Update()
        {
            if (_luaUpdate != null)
                _luaUpdate.call();
        }

        void OnEnable()
        {
            callLuaFunction(ELuaMonoFunc.OnEnable);
        }

        void OnDisable()
        {
            callLuaFunction(ELuaMonoFunc.OnDisable);
        }

        void OnDestroy()
        {
            callLuaFunction(ELuaMonoFunc.OnDestroy);
        }

        #endregion //MONO
    }
}


