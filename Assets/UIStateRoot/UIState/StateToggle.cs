using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace game
{
    public class StateToggle : MonoBehaviour
    {
        public bool includeChilder = false;
        public int startSelect = 0;

        public List<StateRoot> list = new List<StateRoot>();

        public System.Action<StateRoot, int> OnSelectChange;
        public System.Func<StateRoot, int, bool> OnPreChange;
        
        public int Select
        {
            get
            {
                for (int i = 0; i < list.Count; ++i)
                    if (list[i].currentState == 1)
                        return i;
                return -1;
            }
            set
            {
                StateRoot s = null;
                int index = -1;

                for (int i = 0; i < list.Count; ++i)
                {
                    if (i == value)
                    {
                        s = list[i];
                        index = i;
                    }
                    else
                    {
                        list[i].currentState = 0;
                    }
                }

                if (s != null && s.currentState != 1)
                {
                    s.currentState = 1;
                    OnSelectChange?.Invoke(s, index);
                }
            }
        }

        public StateRoot SelectObj
        {
            get
            {
                int s = Select;
                if (s == -1)
                    return null;
                return list[s];
            }
            set
            {
                Select = list.IndexOf(value);
            }
        }

        void Start()
        {
            if (includeChilder)
                GetComponentsInChildren(true, list);

            Select = startSelect;
            for (int i = 0; i < list.Count; ++i)
            {
                var x = new Pair<StateRoot, int>(list[i], i);
                list[i].onClick.AddListener(()=> 
                {
                    if (x.first.currentState == 1)
                        return;

                    if (OnPreChange != null && !OnPreChange(x.first, x.second))
                    {
                        return;
                    }

                    Select = x.second;
                });
            }
        }

        public void OnSelect(int selIndex)
        {
            StateRoot s = null;
            int index = -1;

            for (int i = 0; i < list.Count; ++i)
            {
                if (i == selIndex)
                {
                    s = list[i];
                    index = i;
                }
                else
                {
                    list[i].currentState = 0;
                }
            }

            if (s != null && s.currentState != 1)
                s.currentState = 1;
            OnSelectChange?.Invoke(s, index);
        }
    }
}