using System;
using System.Collections.Generic;

namespace MyExtensionMethod
{
    public static class ListExtensionMethod
    {
        /// <summary>
        /// 将数据添加或替换到列表中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static bool AddOrPeplace<T>(this List<T> list, T data, Predicate<T> match)
        {
            if (match == null) return false;
            var node = list.Find(match);
            if (node != null) { list.Remove(node); }
            list.Add(data);
            return true;
        }
    }
}