using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace game
{
    public abstract class TElementAgent<T> : ElementAgent where T : Object
    {
#if UNITY_EDITOR
        public override void ShowElementTarget(Element element)
        {
            Object target = EditorGUILayout.ObjectField(element.Name, element.target, typeof(T), true);
            if (element.target == null && target != null)
            {
                RegisterUndo(() => 
                {
                    element.target = target;
                    for (int i = 0; i < element.stateData.Length; ++i)
                        element.Agent.Init(element, element.stateData[i]);
                });
            }
            else if (element.target != null && target != element.target)
            {
                RegisterUndo(() =>
                {
                    element.target = target;
                });
            }
        }

        public override void ShowElementState(Element element, int stateid, bool iscanset)
        {
            if (!iscanset)
                EditorGUILayout.ObjectField(element.Name, element.target, typeof(T), true);
            else
            {
                ShowElementTarget(element);
            }
        }
#endif
    }
}