/***************************************************************


 *
 *
 * Filename:  	Math.cs	
 * Summary: 	自定义的一些数学函数
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/07/23 4:57
 ***************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using unityMathf = UnityEngine.Mathf;

namespace Utils
{
    public class LocalMathf
    {
        /// <summary>
        /// 判断两个浮点数数是否相等
        /// </summary>
        /// <returns></returns>
        public static bool Approximately(float lfs, float rhs)
        {
            //return unityMathf.Abs(lfs - rhs) <= unityMathf.Epsilon;
            if (lfs >= rhs - 0.0001 && lfs <= rhs + 0.0001)
                return true;
            else
                return false;
        }
    }

    public class PermutationAndCombination<T>
    {
        /// <summary>
        /// 交换两个变量
        /// </summary>
        /// <param name="a">变量1</param>
        /// <param name="b">变量2</param>
        public static void Swap(List<T> arr, int a, int b)
        {
            T temp = arr[a];
            arr[a] = arr[b];
            arr[b] = temp;
        }

        /// <summary>
        /// 递归算法求数组的组合(私有成员)
        /// </summary>
        /// <param name="list">返回的范型</param>
        /// <param name="t">所求数组</param>
        /// <param name="n">辅助变量</param>
        /// <param name="m">辅助变量</param>
        /// <param name="b">辅助数组</param>
        /// <param name="M">辅助变量M</param>
        private static void GetCombination(ref List<List<T>> list, List<T> t, int n, int m, int[] b, int M)
        {
            for (int i = n; i >= m; i--)
            {
                b[m - 1] = i - 1;
                if (m > 1)
                {
                    GetCombination(ref list, t, i - 1, m - 1, b, M);
                }
                else
                {
                    if (list == null)
                    {
                        list = new List<List<T>>();
                    }
                    List<T> temp = new List<T>();
                    for (int j = 0; j < b.Length; j++)
                    {
                        temp.Add(t[b[j]]);
                    }
                    list.Add(temp);
                }
            }
        }
        
        /// <summary>
        /// 递归算法求排列(私有成员)
        /// </summary>
        /// <param name="list">返回的列表</param>
        /// <param name="t">所求数组</param>
        /// <param name="startIndex">起始标号</param>
        /// <param name="endIndex">结束标号</param>
        private static void GetPermutation(ref List<List<T>> list, List<T> t, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
            {
                if (list == null)
                {
                    list = new List<List<T>>();
                }
                T[] temp = new T[t.Count];
                t.CopyTo(temp, 0);
                list.Add(temp.ToList<T>());
            }
            else
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    Swap(t, startIndex, i);
                    GetPermutation(ref list, t, startIndex + 1, endIndex);
                    Swap(t, startIndex, i);
                }
            }
        }

        /// <summary>
        /// 求从起始标号到结束标号的排列，其余元素不变
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <param name="startIndex">起始标号</param>
        /// <param name="endIndex">结束标号</param>
        /// <returns>从起始标号到结束标号排列的范型</returns>
        public static List<List<T>> GetPermutation(List<T> t, int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex > t.Count - 1)
            {
                return null;
            }
            List<List<T>> list = new List<List<T>>();
            GetPermutation(ref list, t, startIndex, endIndex);
            return list;
        }

        /// <summary>
        /// 返回数组所有元素的全排列
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <returns>全排列的范型</returns>
        public static List<List<T>> GetPermutation(List<T> t)
        {
            return GetPermutation(t, 0, t.Count - 1);
        }

        /// <summary>
        /// 求数组中n个元素的排列
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <param name="n">元素个数</param>
        /// <returns>数组中n个元素的排列</returns>
        public static List<List<T>> GetPermutation(List<T> t, int n)
        {
            if (n > t.Count)
            {
                return null;
            }
            List<List<T>> list = new List<List<T>>();
            List<List<T>> c = GetCombination(t, n);
            for (int i = 0; i < c.Count; i++)
            {
                List<List<T>> l = new List<List<T>>();
                GetPermutation(ref l, c[i], 0, n - 1);
                list.AddRange(l);
            }
            return list;
        }


        /// <summary>
        /// 求数组中n个元素的组合
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <param name="n">元素个数</param>
        /// <returns>数组中n个元素的组合的范型</returns>
        public static List<List<T>> GetCombination(List<T> t, int n)
        {
            if (t.Count < n)
            {
                return null;
            }
            int[] temp = new int[n];
            List<List<T>> list = new List<List<T>>();
            GetCombination(ref list, t, t.Count, n, temp, n);
            return list;
        }
    }
}


