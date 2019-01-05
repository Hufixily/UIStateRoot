using UnityEngine;
#if UNITY_EDITOR
#endif

namespace game
{
    public partial class RotateEA : TElementAgentSmooth<Transform>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
                sd.vector3 = element.GetTarget<Transform>().localEulerAngles;
        }

        public override void InitBySmooth(Transform target, SmoothData sd)
        {
            sd.vector3 = target.localEulerAngles;
        }

        public override void SetBySmooth(Transform target, SmoothData sd, ElementStateData esd, float progress)
        {
            if (sd == null)
            {
                target.localEulerAngles = esd.vector3;
            }
            else
            {
                target.localEulerAngles = sd.Get(esd.vector3, progress);
            }
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            return ShowVector3(ref sc.vector3, "旋转");
        }
#endif
    }
    
}