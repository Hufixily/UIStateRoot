using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace game
{
    [System.SerializableAttribute]
    public class ElementStateData
    {
        public bool isSmooth = false;
        public float smoothTime = 1f;

        public bool isEnable = false;
        public int intValue = 0;
        public Vector3 vector3 = Vector3.zero;
        public Vector2 vector2Min = Vector2.zero;
        public Vector2 vector2Max = Vector2.zero;

        public Color32 color32;
        public string strValue = string.Empty;
        public Object obj = null;

        public Color32 topColor;
        public Color32 bottomColor;

        // 渐变数据
        public TimerFrame.Frame frameupdate = null;

        public void AddFrameUpdate(TimerFrameUpdate fun, object p)
        {
            if (frameupdate != null)
                frameupdate.cancel = true;

#if UNITY_EDITOR
            TimerMarshal.CreateInstance();
#endif
            frameupdate = TimerMarshal.Instance.AddUpateTimerFrame(
                (object pf) => 
                {
                    if (fun == null)
                    {
                        frameupdate = null;
                        return false;
                    }

                    bool v = fun(pf);
                    if (v == false)
                        frameupdate = null;

                    return v;
                }, p);
        }
    }

    public class SmoothData
    {
        public int intValue = 0;
        public Vector3 vector3 = Vector3.zero;
        public Color32 color32;

        public float esc_timer = 0f; // 经历的时长

        public int Get(int dst, float progress)
        {
            return (int)Mathf.Lerp(intValue, dst, progress);
        }

        public Color32 Get(Color32 dst, float progress)
        {
            return Color32.Lerp(color32, dst, progress);
        }

        public float Get(float dst, float progress)
        {
            return Mathf.Lerp(vector3.x, dst, progress);
        }

        public Vector3 Get(Vector3 dst, float progress)
        {
            return Vector3.Lerp(vector3, dst, progress);
        }
    }

    [System.SerializableAttribute]
    public class StateConfig
    {
        public string Name; // 状态名

#if UNITY_EDITOR
        public bool isFoldouts { get; set; }
#endif
    }
}