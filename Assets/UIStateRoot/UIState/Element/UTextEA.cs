using UnityEngine.UI;
#if UNITY_EDITOR
#endif

namespace game
{
    public partial class UTextEA : TElementAgent<Text>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
                sd.strValue = element.GetTarget<Text>().text;
        }

        public override void Set(Element element, int index)
        {
            if (element.target != null)
            {
                Text label = element.GetTarget<Text>();
                label.text = element.stateData[index].strValue;

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(label);
#endif
            }
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            return ShowString(ref sc.strValue);
        }
#endif
    }
}