using UnityEngine;
using System.Collections;

namespace game
{

    public class TuplePair<TKey, TValue>
    {
        public readonly TKey m1;
        public readonly TValue m2;

        public TuplePair(TKey key, TValue value)
        {
            m1 = key;
            m2 = value;
        }
    }

    public class TuplePair<T1, T2, T3>
    {
        public readonly T1 m1;
        public readonly T2 m2;
        public readonly T3 m3;

        public TuplePair(T1 m1, T2 m2, T3 m3)
        {
            this.m1 = m1;
            this.m2 = m2;
            this.m3 = m3;
        }
    }
}
