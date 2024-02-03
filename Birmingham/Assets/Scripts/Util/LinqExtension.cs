using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Util
{
    public static class LinqExtension
    {
        /// <summary>
        /// 将迭代器内元素随机打乱
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            // 将源集合复制到一个可编辑的列表
            List<T> list = source.ToList();
            int n = list.Count;

            // 使用Fisher-Yates算法进行随机置换
            Random random = new Random();
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                // 交换元素
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }

            return list;
        }
    }
}