using UnityEngine;
#if UNITY_EDITOR
#endif

namespace game
{
    public partial class CanvasGroupEA : TElementAgentSmooth<CanvasGroup>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
                sd.vector3.x = element.GetTarget<CanvasGroup>().alpha;
        }

        public override void InitBySmooth(CanvasGroup target, SmoothData sd)
        {
            sd.vector3.x = target.alpha;
        }

        public override void SetBySmooth(CanvasGroup target, SmoothData sd, ElementStateData esd, float progress)
        {
            if (sd == null)
            {
                target.alpha = esd.vector3.x;
            }
            else
            {
                target.alpha = sd.Get(esd.vector3.x, progress);
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