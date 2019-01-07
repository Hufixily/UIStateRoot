using UnityEngine;
#if UNITY_EDITOR
#endif

namespace game
{
    public partial class UWidthEA : TElementAgentSmooth<RectTransform>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
                sd.vector3.x = element.GetTarget<RectTransform>().rect.width;
        }

        public override void InitBySmooth(RectTransform target, SmoothData sd)
        {
            sd.vector3.x = target.rect.width;
        }

        public override void SetBySmooth(RectTransform target, SmoothData sd, ElementStateData esd, float progress)
        {
            if (sd == null)
            {
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, esd.vector3.x);
            }
            else
            {
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sd.Get(esd.vector3.x, progress));
            }
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            return ShowFloat(ref sc.vector3, "宽度");
        }
#endif
    }
}