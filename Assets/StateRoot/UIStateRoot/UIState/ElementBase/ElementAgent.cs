using UnityEngine;
using System.Collections.Generic;

namespace game
{
    public abstract class ElementAgent
    {
        public abstract void Init(Element element, ElementStateData sd);

        public abstract void Set(Element element, int index);

        public virtual bool isSmooth { get { return false; } } // 是否允许渐变

#if UNITY_EDITOR
        public abstract bool ShowState(Element element, ElementStateData sc, int index);

        public abstract void ShowElementTarget(Element element);

        public abstract void ShowElementState(Element element, int stateid, bool iscanset);

        public string Name { get; set; }

        public static System.Func<StateRoot> getCurrentStateRoot = null;

        protected static StateRoot GetStateRoot()
        {
            if (getCurrentStateRoot == null)
                return null;

            return getCurrentStateRoot();
        }

        static public void RegisterUndo(System.Action action, params Object[] objs)
        {
            object o = null;
            RegisterUndo<object>((ref object vv) =>
            {
                action();
            }, ref o, objs);
        }

        public delegate void ActionRef<T>(ref T obj);

        static public void RegisterUndo<T>(ActionRef<T> action, ref T t, params Object[] objs)
        {
            var rs = new List<Object>();
            var sr = GetStateRoot();
            if (sr != null)
            {
                rs.Add(sr);
            }

            if (objs != null)
            {
                rs.AddRange(objs);
            }

            UnityEditor.Undo.RecordObjects(rs.ToArray(), "State Root Change");
            for (int i = 0; i < rs.Count; ++i)
                UnityEditor.EditorUtility.SetDirty(rs[i]);

            action(ref t);

            for (int i = 0; i < rs.Count; ++i)
                UnityEditor.EditorUtility.SetDirty(rs[i]);
        }

        static public bool ShowVector3(ref Vector3 v3, string name)
        {
            var v = UnityEditor.EditorGUILayout.Vector3Field(name, v3);
            if (v3 == v)
                return false;

            RegisterUndo<Vector3>((ref Vector3 vv) =>
            {
                vv = v;
            }, ref v3);

            return true;
        }

        public static bool Color32Equal(ref Color32 x, ref Color32 y)
        {
            return x.a == y.a && x.b == y.b && x.g == y.g && x.r == y.r;
        }

        static public bool ShowColor32(ref Color32 c32, string name)
        {
            Color32 v = UnityEditor.EditorGUILayout.ColorField(name, c32);
            if (Color32Equal(ref v, ref c32))
            {
                return false;
            }

            RegisterUndo<Color32>((ref Color32 vv) =>
            {
                vv = v;
            }, ref c32);

            return true;
        }

        static public bool ShowString(ref string str)
        {
            var v = UnityEditor.EditorGUILayout.TextField(str);
            if (v == str)
            {
                return false;
            }

            RegisterUndo<string>((ref string vv) =>
            {
                vv = v;
            }, ref str);

            return true;
        }

        static public bool ShowSliderFloat(ref Vector3 v3, string name, float min, float max)
        {
            var x = UnityEditor.EditorGUILayout.Slider(name, v3.x, min, max);
            if (x == v3.x)
                return false;

            RegisterUndo<Vector3>((ref Vector3 vv) =>
            {
                vv.x = x;
            }, ref v3);

            return true;
        }

        static public bool ShowFloat(ref Vector3 v3, string name)
        {
            var x = UnityEditor.EditorGUILayout.FloatField(v3.x);
            if (x == v3.x)
                return false;

            RegisterUndo<Vector3>((ref Vector3 vv) =>
            {
                vv.x = x;
            }, ref v3);

            return true;
        }

        static public bool ShowIntField(ref int intv, string name)
        {
            int x = UnityEditor.EditorGUILayout.IntField(name, intv);
            if (x == intv)
                return false;

            RegisterUndo<int>((ref int vv) =>
            {
                vv = x;
            }, ref intv);

            return true;
        }

        static public bool ShowToggle(ref bool bv, string name)
        {
            var x = UnityEditor.EditorGUILayout.Toggle(name, bv);
            if (x == bv)
                return false;

            RegisterUndo<bool>((ref bool vv) =>
            {
                vv = x;
            }, ref bv);

            return true;
        }
#endif
    }
}