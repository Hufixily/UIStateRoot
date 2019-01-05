using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace game
{
    public partial class UTextFontEA : TElementAgent<Text>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
                sd.obj = element.GetTarget<Text>().font;
        }

        public override void Set(Element element, int index)
        {
            if (element.target != null)
            {
                Text label = element.GetTarget<Text>();
                label.font = element.stateData[index].obj as Font;

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(label);
#endif
            }
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            sc.obj = EditorGUILayout.ObjectField(sc.obj, typeof(Font), false);
            return false;
        }
#endif
    }
}