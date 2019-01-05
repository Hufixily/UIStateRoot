using UnityEngine;
#if UNITY_EDITOR
#endif

namespace game
{
    public partial class UHeightEA : TElementAgentSmooth<RectTransform>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
                sd.vector3.x = element.GetTarget<RectTransform>().rect.height;
        }

        public override void InitBySmooth(RectTransform target, SmoothData sd)
        {
            sd.vector3.x = target.rect.height;
        }

        public override void SetBySmooth(RectTransform target, SmoothData sd, ElementStateData esd, float progress)
        {
            if (sd == null)
            {
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, esd.vector3.x);
            }
            else
            {
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sd.Get(esd.vector3.x, progress));
            }
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            return ShowFloat(ref sc.vector3, "高度");
        }
#endif
    }
}