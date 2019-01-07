namespace game
{
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class UAnchorsEA : TElementAgent<RectTransform>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target == null)
                return;
            sd.vector2Min = element.GetTarget<RectTransform>().anchorMin;
            sd.vector2Max = element.GetTarget<RectTransform>().anchorMax;
        }

        public override void Set(Element element, int index)
        {
            if (element == null)
                return;
            var rtf = element.GetTarget<RectTransform>();
            rtf.anchorMin = element.stateData[index].vector2Min;
            rtf.anchorMax = element.stateData[index].vector2Max;
#if UNITY_EDITOR
            EditorUtility.SetDirty(rtf);
#endif
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            GUILayout.BeginVertical("box");
            bool change = ShowVector2(ref sc.vector2Min, "Min") || ShowVector2(ref sc.vector2Max, "Max");
            GUILayout.EndVertical();
            return change;
        }
#endif
    }
}
