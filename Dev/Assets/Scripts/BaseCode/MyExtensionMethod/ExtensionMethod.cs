using System;
using System.Collections;
using UnityEngine;

namespace MyExtensionMethod
{
    public static class ExtensionMethod
    {
        /// <summary>
        /// iTweens MoveTo 封装
        /// </summary>
        /// <param name="go"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static MyHashTable MoveTo(this GameObject go, Vector3 pos)
        {
            var table = new MyHashTable(go, "MoveTo", "position", pos);
            return table;
        }

        /// <summary>
        /// iTweens MoveFrom 封装
        /// </summary>
        /// <param name="go"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static MyHashTable MoveFrom(this GameObject go, Vector3 pos)
        {
            var table = new MyHashTable(go, "MoveFrom", "position", pos);
            return table;
        }

        /// <summary>
        /// iTweens ScaleFrom 封装
        /// </summary>
        /// <param name="go"></param>
        /// <param name="amout"></param>
        /// <returns></returns>
        public static MyHashTable ScaleFrom(this GameObject go, Vector3 amout)
        {
            var table = new MyHashTable(go, "ScaleFrom", "scale", amout);
            return table;
        }

        /// <summary>
        /// 泛型查找 GameObject, Component, Transform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Find<T>(this Transform r, string path) where T : class
        {
            var type = typeof(T);

            if (r.Find(path) == null) return null;

            if (type == typeof(GameObject)) {
                return (T)(object)r.Find(path).gameObject;
            }

            if (type.IsSubclassOf(typeof(Component))) {
                return (T)(object)r.Find(path).GetComponent(type);
            }

            if (type == typeof(Transform)) {
                return (T)(object)r.Find(path);
            }

            throw new Exception(string.Format("{0} is not a Component or GameObject", type.ToString()));
        }

        /// <summary>
        /// 获取 Hashtable 中 key 对应的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T>(this Hashtable table, string key)
        {
            return table.ContainsKey(key) ? (T)table[key] : default(T);
        }

        /// <summary>
        /// 获取 Hashtable 中 key 对应的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetValue<T>(this Hashtable table, string key, out T value)
        {
            if (table.ContainsKey(key)) {
                value = (T)table[key];
                return true;
            }

            value = default(T);
            return false;
        }
    }

    /// <summary>
    /// iTweens 参数封装
    /// </summary>
    public class MyHashTable
    {
        private readonly GameObject _go;
        private readonly Hashtable _table;
        private readonly string _type;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="go"></param>
        /// <param name="type"></param>
        /// <param name="parmName"></param>
        /// <param name="parm"></param>
        public MyHashTable(GameObject go, string type, string parmName, object parm)
        {
            _table = new Hashtable();
            _table[parmName] = parm;
            _type = type;
            _go = go;
        }

        public MyHashTable Time(float t)
        {
            _table["time"] = t;
            return this;
        }

        public MyHashTable IsLocal(bool islocal)
        {
            _table["islocal"] = islocal;
            return this;
        }

        public MyHashTable Delay(float delay)
        {
            _table["delay"] = delay;
            return this;
        }

        public MyHashTable EaseType(iTween.EaseType easeType)
        {
            _table["easetype"] = easeType;
            return this;
        }

        public void Play()
        {
            switch (_type) {
                case "MoveTo":
                    iTween.MoveTo(_go, _table);
                    break;
                case "MoveFrom":
                    iTween.MoveTo(_go, _table);
                    break;
                case "ScaleFrom":
                    iTween.ScaleFrom(_go, _table);
                    break;
            }
        }

        public MyHashTable OnComplete(Action onComplete)
        {
            if (onComplete == null)
                return this;
            _table["oncomplete"] = onComplete.Method.Name;
            var target = onComplete.Target as MonoBehaviour;
            if (target == null) {
                Utils.LogSys.LogError("onComplete target cannot be null");
                return this;
            }

            _table["oncompletetarget"] = target.gameObject;
            return this;
        }

        public MyHashTable OnComplete(string onComplete, GameObject target, object param = null)
        {
            _table["oncomplete"] = onComplete;
            _table["oncompletetarget"] = target;
            if (param != null) {
                _table["oncompleteparams"] = param;
            }
            return this;
        }

        public MyHashTable IgnoreTimeScale(bool ignoreTimeScale)
        {
            _table["ignoretimescale"] = ignoreTimeScale;
            return this;
        }
    }
}
