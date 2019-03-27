using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyExtensionMethod
{
    public static class DictionaryExtensionMethod
    {
        /// <summary>
        /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
        /// </summary>
        public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, value);
            return dict;
        }

        /// <summary>
        /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static Dictionary<TKey, TValue> AddOrPeplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }

        /// <summary>
        /// 获取与指定的键相关联的值，如果没有则返回输入的默认值
        /// </summary>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            TValue value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }

        /// <summary>
        /// 向字典中批量添加键值对
        /// </summary>
        /// <param name="values"></param>
        /// <param name="replaceExisted">如果已存在，是否替换</param>
        /// <param name="dict"></param>
        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted)
        {
            var enumerator = values.GetEnumerator();
            while (enumerator.MoveNext()) {
                var node = enumerator.Current;
                if (!dict.ContainsKey(node.Key) || replaceExisted)
                    dict[node.Key] = node.Value;
            }
            return dict;
        }
    }
}