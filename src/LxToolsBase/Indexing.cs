using System;
using System.Collections.Generic;
using System.Linq;

namespace LxTools
{
    public struct Indexing<T>
    {
        public Indexing(int index, T obj)
        {
            this.Object = obj;
            this.Index = index;
        }

        public T Object;
        public int Index;
    }

    public static class Indexing
    {
        public static IEnumerable<Indexing<T>> Index<T>(this IEnumerable<T> source, Func<T, T, bool> similarity)
        {
            int index = 0;
            int itemcount = 0;
            T last = default(T);
            foreach (T o in source)
            {
                itemcount++;
                if (itemcount == 1 || !similarity(last, o)) index = itemcount;
                last = o;
                yield return new Indexing<T>(index, o);
            }
        }

        public static IEnumerable<Indexing<T>> TakeTop<T>(this IEnumerable<Indexing<T>> source, int top)
        {
            return source.TakeWhile((idx) => idx.Index <= top);
        }
    }
}
