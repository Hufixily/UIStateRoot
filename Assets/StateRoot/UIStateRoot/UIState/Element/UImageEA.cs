using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace game
{
    public partial class UImageEA : TElementAgent<Image>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
                sd.obj = element.GetTarget<Image>().sprite;
        }

        public override void Set(Element element, int index)
        {
            if (element.target != null)
            {
                Image image = element.GetTarget<Image>();
                image.sprite = element.stateData[index].obj as Sprite;

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(image);
#endif
            }
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            sc.obj = EditorGUILayout.ObjectField(sc.obj, typeof(Sprite), true);

            return false;
        }
#endif
    }
}