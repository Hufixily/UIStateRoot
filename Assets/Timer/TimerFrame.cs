using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace game
{

    public delegate bool TimerFrameUpdate(System.Object callbackData);

    public class TimerFrame
    {
        public class Frame
        {
            public TimerFrameUpdate callback;
            public System.Object callbackData;
            public bool cancel;

            public void Set(TimerFrameUpdate callback, System.Object callbackData)
            {
                this.callback = callback;
                this.callbackData = callbackData;
                cancel = false;
            }
            public void Release()
            {
                callback = null;
                callbackData = null;
                cancel = false;
            }

        }

        List<Frame> mCachedFrameList = new List<Frame>();
        List<Frame> mExecuteFrameList = new List<Frame>();
        Frame mCurrentFrame = null;

        static void Release(Frame frame)
        {
            frame.Release();
            BufferPool<Frame>.Instance.Release(frame);
        }

        static Frame DefaultFrame(TimerFrameUpdate callback, System.Object callbackData)
        {
            Frame frame = BufferPool<Frame>.Instance.Allocate();
            frame.Set(callback, callbackData);
            return frame;
        }

        public static int frameCount
        {
            get;
            protected set;
        }

        static void SwapFrameList(ref List<Frame> leftList, ref List<Frame> rightList)
        {
            List<Frame> tempList = leftList;
            leftList = rightList;
            rightList = tempList;
        }

        public Frame AddFrame(TimerFrameUpdate callback, System.Object callbackData)
        {
            Frame frame = DefaultFrame(callback, callbackData);
            mCachedFrameList.Add(frame);
            return frame;
        }

        public void AddFrameCallback(TimerFrameUpdate callback, System.Object callbackData)
        {
            Frame frame = DefaultFrame(callback, callbackData);
            mCachedFrameList.Add(frame);
        }

        public void UpdateFrame()
        {
            if (mCachedFrameList.Count == 0)
                return;

            SwapFrameList(ref mExecuteFrameList, ref mCachedFrameList);
            for (int i = 0, imax = mExecuteFrameList.Count; i < imax; i++)
            {
                mCurrentFrame = mExecuteFrameList[i];
                if (mCurrentFrame.cancel == false)
                {
                    try
                    {
                        if (mCurrentFrame.callback(mCurrentFrame.callbackData) == true)
                        {
                            mCachedFrameList.Add(mCurrentFrame);
                        }
                        else
                        {
                            Release(mCurrentFrame);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.Log(this.GetType().Name + ", UpdateFrame Error " + ex);
                    }
                }
                else
                {
                    mCurrentFrame.callback = null;
                    mCurrentFrame.callbackData = null;
                    Release(mCurrentFrame);
                }
            }//end for
            mExecuteFrameList.Clear();
        }

    }
}
