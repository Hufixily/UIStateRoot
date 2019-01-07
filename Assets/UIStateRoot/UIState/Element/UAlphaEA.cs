using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
#endif

namespace game
{
    public partial class UAlphaEA : TElementAgentSmooth<MaskableGraphic>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
                sd.vector3.x = element.GetTarget<MaskableGraphic>().color.a;
        }

        public override void InitBySmooth(MaskableGraphic target, SmoothData sd)
        {
            sd.vector3.x = target.color.a;
        }

        public override void SetBySmooth(MaskableGraphic target, SmoothData sd, ElementStateData esd, float progress)
        {
            if (sd == null)
            {
                Color color = target.color;
                color.a = esd.vector3.x;
                target.color = color;
            }
            else
            {
                Color color = target.color;
                color.a = sd.Get(esd.vector3.x, progress);
                target.color = color;
            }
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            return ShowSliderFloat(ref sc.vector3, "透明", 0, 1);
        }
#endif
    }
}