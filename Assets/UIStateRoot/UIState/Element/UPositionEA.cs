namespace game
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class UPositionEA : TElementAgentSmooth<RectTransform>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
                sd.vector3 = element.GetTarget<RectTransform>().anchoredPosition;
        }

        public override void InitBySmooth(RectTransform target, SmoothData sd)
        {
            sd.vector3 = target.anchoredPosition;
        }
        public override void SetBySmooth(RectTransform target, SmoothData sd, ElementStateData esd, float progress)
        {
            if (sd == null)
            {
                target.anchoredPosition = esd.vector3;
            }
            else
            {
                target.anchoredPosition = sd.Get(esd.vector3, progress);
            }
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            return ShowVector3(ref sc.vector3, "U位置");
        }
#endif
    }
}
