using UnityEngine;
#if UNITY_EDITOR
#endif

namespace game
{
    public class ScaleEA : TElementAgentSmooth<Transform>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
                sd.vector3 = element.GetTarget<Transform>().localScale;
        }

        public override void InitBySmooth(Transform target, SmoothData sd)
        {
            sd.vector3 = target.localScale;
        }

        public override void SetBySmooth(Transform target, SmoothData sd, ElementStateData esd, float progress)
        {
            if (sd == null)
            {
                target.localScale = esd.vector3;
            }
            else
            {
                target.localScale = sd.Get(esd.vector3, progress);
            }
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            return ShowVector3(ref sc.vector3, "缩放");
        }
#endif
    }
    
}