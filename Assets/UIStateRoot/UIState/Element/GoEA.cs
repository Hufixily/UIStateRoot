using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace game
{
    public class GoEA : TElementAgent<GameObject>
    {
        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
                sd.isEnable = element.GetTarget<GameObject>().activeSelf;
        }

        public override void Set(Element element, int index)
        {
            if (element.target != null)
            {
                element.GetTarget<GameObject>().SetActive(element.stateData[index].isEnable);
            }
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            return ShowToggle(ref sc.isEnable, "显示");
        }
#endif
    }
}