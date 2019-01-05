using UnityEngine;
using System.Collections;

namespace game
{

    public class TimerMarshal : SingletonMonoBehaviour<TimerMarshal>
    {

        TimerFrame mUpdateTimerFrame;

        TimerFrame mLateUpdateTimerFrame;

        public TimerFrame.Frame AddUpateTimerFrame(TimerFrameUpdate callback, System.Object callbackData)
        {
            return mUpdateTimerFrame.AddFrame(callback, callbackData);
        }
        public TimerFrame.Frame AddLateUpdateTimerFrame(TimerFrameUpdate callback, System.Object callbackData)
        {
            return mLateUpdateTimerFrame.AddFrame(callback, callbackData);
        }
        public static int frameCount { get; protected set; }

        void Update()
        {
            ++frameCount;
            mUpdateTimerFrame.UpdateFrame();
        }
        void LateUpdate()
        {
            mLateUpdateTimerFrame.UpdateFrame();
        }

        protected override void Init()
        {
            base.Init();
            mUpdateTimerFrame = new TimerFrame();
            mLateUpdateTimerFrame = new TimerFrame();
        }

    }
}