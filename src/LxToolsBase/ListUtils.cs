using System;
using System.Collections.Generic;

namespace LxTools
{
    public static class ListUtils
    {
        public static void AddUnique<T>(this List<T> list, T newitem)
        {
            if (!list.Contains(newitem))
                list.Add(newitem);
        }
    }
}
