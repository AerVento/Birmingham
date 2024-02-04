using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Util
{
    public static class LinqExtension
    {
        /// <summary>
        /// ����������Ԫ���������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            // ��Դ���ϸ��Ƶ�һ���ɱ༭���б�
            List<T> list = source.ToList();
            int n = list.Count;

            // ʹ��Fisher-Yates�㷨��������û�
            Random random = new Random();
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                // ����Ԫ��
                (list[j], list[i]) = (list[i], list[j]);
            }

            return list;
        }

        /// <summary>
        /// ��һ������ת���ɿ�ö�ٶ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToEnumerable<T>(this T obj)
        {
            return new T[] { obj };
        }
    }
}