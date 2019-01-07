using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace game
{
    public partial class PlayAnimEA : TElementAgent<Animator>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            sd.strValue = "";
        }

        public override void Set(Element element, int index)
        {
            if (element.target == null)
                return;
            var anim = element.GetTarget<Animator>();
            anim.Play(element.stateData[index].strValue);
        }

#if UNITY_EDITOR
        public override void ShowElementTarget(Element element)
        {
            var target = EditorGUILayout.ObjectField(element.Name, element.target, typeof(Animator), true) as Animator;
            if (element.target != target)
                RegisterUndo(() => { element.target = target; });
        }

        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            var old = sc.intValue;
            var str = EditorGUILayout.TextField("Name", sc.strValue);

            if (GUI.changed)
                RegisterUndo(() => { sc.strValue = str; });

            return sc.intValue != old;
        }
#endif
    }
}