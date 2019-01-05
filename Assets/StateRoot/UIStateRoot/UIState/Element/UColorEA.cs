using UnityEngine.UI;
#if UNITY_EDITOR
#endif

namespace game
{
    public partial class UColorEA : TElementAgentSmooth<MaskableGraphic>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
                sd.color32 = element.GetTarget<MaskableGraphic>().color;
        }

        public override void InitBySmooth(MaskableGraphic target, SmoothData sd)
        {
            sd.color32 = target.color;
        }

        public override void SetBySmooth(MaskableGraphic target, SmoothData sd, ElementStateData esd, float progress)
        {
            if (sd == null)
            {
                target.color = esd.color32;
            }
            else
            {
                target.color = sd.Get(esd.color32, progress);
            }
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            return ShowColor32(ref sc.color32, "颜色");
        }
#endif
    }
}