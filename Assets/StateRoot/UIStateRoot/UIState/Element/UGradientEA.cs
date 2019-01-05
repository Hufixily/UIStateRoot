using UnityEngine;
#if UNITY_EDITOR
#endif

namespace game
{
    public partial class UGradientEA : TElementAgent<UnityEngine.UI.Gradient>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
            {
                sd.topColor = element.GetTarget<UnityEngine.UI.Gradient>().gradientTop;
                sd.bottomColor = element.GetTarget<UnityEngine.UI.Gradient>().gradientBottom;
            }
        }

        public override void Set(Element element, int index)
        {
            if (element.target != null)
            {
                UnityEngine.UI.Gradient gradient = element.GetTarget<UnityEngine.UI.Gradient>();
                gradient.gradientTop = element.stateData[index].topColor;
                gradient.gradientBottom = element.stateData[index].bottomColor;

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(gradient);
#endif

                var target = gradient.target;
                if (target != null)
                    target.SetVerticesDirty();
            }
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            Color32 v = UnityEditor.EditorGUILayout.ColorField("顶颜色", sc.topColor);
            Color32 v2 = UnityEditor.EditorGUILayout.ColorField("底颜色", sc.bottomColor);
            if (ElementAgent.Color32Equal(ref v, ref sc.topColor) && ElementAgent.Color32Equal(ref v2, ref sc.bottomColor))
            {
                return false;
            }

            RegisterUndo<Color32>((ref Color32 vv) =>
            {
                vv = v;
            },
            ref sc.topColor);

            RegisterUndo<Color32>((ref Color32 vv) =>
            {
                vv = v2;
            },
            ref sc.bottomColor);

            return true;
        }
#endif
    }
}