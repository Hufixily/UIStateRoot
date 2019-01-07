#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;

namespace game
{
    public class StateElementDraw
    {
        public static bool ArrayRemove<T>(ref T[] array, int index)
        {
            if (index < 0 || index >= array.Length)
                return false;

            T[] newv = new T[array.Length - 1];
            if (index == 0)
            {
                System.Array.Copy(array, 1, newv, 0, newv.Length);
            }
            else if (index == newv.Length)
            {
                System.Array.Copy(array, newv, newv.Length);
            }
            else
            {
                System.Array.Copy(array, newv, index);
                System.Array.Copy(array, index + 1, newv, index, array.Length - index - 1);
            }

            array = newv;
            return true;
        }

        public static int DrawType()
        {
            //增加按钮
            return EditorGUILayout.Popup("增加", -1, Factory.Names);
        }

        static public void DrawElements(ref Element[] elements)
        {
            for (int i = 0; i < elements.Length; )
            {
                if (StateElementDraw.DrawElement(elements[i]))
                {
                    ArrayRemove<Element>(ref elements, i);
                }
                else
                {
                    ++i;
                }
            }
        }

        public static System.Func<Element, int, bool, bool, bool> CurrentDrawStateInfo { get; set; }

        static public void DrawStateInfos(ref Element[] elements, int stateid, bool isShowRecord, bool iscanset)
        {
            for (int i = 0; i < elements.Length;)
            {
                if (CurrentDrawStateInfo(elements[i], stateid, isShowRecord, iscanset))
                {
                    ArrayRemove(ref elements, i);
                }
                else
                {
                    ++i;
                }
            }
        }        

        static public bool DrawElement(Element element)
        {
            bool isdel = false;
            EditorGUILayout.BeginHorizontal();

            element.Agent.ShowElementTarget(element);
            if (GUILayout.Button("删除", GUILayout.Width(50)))
            {
                isdel = true;
            }

            EditorGUILayout.EndHorizontal();
            return isdel;
        }
    }
}
#endif