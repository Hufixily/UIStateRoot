using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace game
{
    public abstract class TElementAgentSmooth<T> : TElementAgent<T> where T : Object
    {
        public override bool isSmooth { get { return true; } } // 是否允许渐变

        public abstract void InitBySmooth(T target, SmoothData sd);

        public abstract void SetBySmooth(T target, SmoothData sd, ElementStateData esd, float progress);

        public override void Set(Element element, int index)
        {
            if (element.target != null)
            {
                T target = element.GetTarget<T>();
                ElementStateData esd = element.stateData[index];
                if (esd.isSmooth)
                {
                    SmoothData sd = new SmoothData();
                    InitBySmooth(target, sd);
                    esd.AddFrameUpdate((object p) =>
                    {
                        if (target == null)
                            return false;

#if UNITY_EDITOR
                        if (!Application.isPlaying)
                        {
                            sd.esc_timer += 0.011f;
                        }
                        else
#endif
                        {
                            float deltaTime = Time.deltaTime;
                            sd.esc_timer += deltaTime;
                        }

                        bool isend = false;
                        float progress = 1f;
                        if (sd.esc_timer >= esd.smoothTime)
                        {
                            isend = true;
                        }
                        else
                        {
                            progress = sd.esc_timer / esd.smoothTime;
                        }

                        SetBySmooth(target, sd, esd, progress);

#if UNITY_EDITOR
                        EditorUtility.SetDirty(target);
#endif
                        return !isend;
                    }, null);
                }
                else
                {
                    SetBySmooth(target, null, esd, -1f);
                }

#if UNITY_EDITOR
                EditorUtility.SetDirty(target);
#endif
            }
        }
    }
}