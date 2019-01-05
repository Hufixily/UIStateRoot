using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
#if MEMORY_CHECK
using MonoBehaviour = MemoryMonoBehaviour;
#endif

namespace game
{
    public class StateRoot : MonoBehaviour
    {
        private static StateConfig[] Empty = new StateConfig[0];//状态列表

        public List<Element> elements; // 元素列表

        public StateConfig[] states = Empty; // 状态名

        public bool isClickSwitchState = false; // 点击是否切换状态

        [SerializeField]
        Button m_Button;

        [SerializeField]
        int m_StateIndex; // 当前状态
        
        public int currentState
        {
            get { return m_StateIndex; }
            set
            {
                if (m_StateIndex == value)
                    return;

                SetState(value);
            }
        }

        public Button currentButton
        {
            get { return m_Button; }
            set
            {
                if (m_Button == value)
                    return;

                if (m_Button != null)
                    m_Button.onClick.RemoveListener(OnButtonClick);

                m_Button = value;

#if UNITY_EDITOR
                if (!Application.isPlaying)
                    return;
#endif
                BindButtonEvent();
            }
        }

        void BindButtonEvent()
        {
            if (m_Button != null)
            {
                m_Button.onClick.AddListener(OnButtonClick);
            }
        }

        void OnButtonClick()
        {
            if (isClickSwitchState)
            {
                if (!NextState())
                {
                    SetState(0);
                }
            }
            onClick.Invoke();
        }

        public bool NextState()
        {
            return SetState(currentState + 1);
        }

        public bool FrontState()
        {
            return SetState(currentState - 1);
        }

        public void SetNextStateWithLoop(bool isNotify = false)
        {
            if (!SetCurrentState(currentState + 1, isNotify))
            {
                SetCurrentState(0, isNotify);
            }
        }

        public void SetFrontStateWithLoop(bool isNotify = false)
        {
            if (!SetCurrentState(currentState - 1, isNotify))
            {
                SetCurrentState(states.Length - 1, isNotify);
            }
        }

        public UnityEvent onStateChange = new UnityEvent();
        public UnityEvent onClick = new UnityEvent();

#if UNITY_EDITOR
        public string[] StateNames
        {
            get
            {
                var s = new string[states.Length];
                for (int i = 0; i < s.Length; ++i)
                    s[i] = states[i].Name;
                return s;
            }
        }
#endif

        void Awake()
        {
            BindButtonEvent();
        }

        private void Start()
        {
            SetState(currentState);
        }

        public string CurrentStateName
        {
            get
            {
                return states[m_StateIndex].Name;
            }
        }

        public bool SetCurrentState(int value, bool isnotify)
        {
            if (value < 0 || value >= states.Length)
                return false;

            m_StateIndex = value;
            if (elements != null)
            {
                for (int i = 0; i < elements.Count; ++i)
                {
                    elements[i].Agent.Set(elements[i], value);
                }
            }

            if (isnotify)
                onStateChange.Invoke();
            return true;
        }

        public bool SetCurrentState(string stateName, bool isnotify = false)
        {
            for (int i = 0; i < states.Length; ++i)
            {
                if (stateName == states[i].Name)
                {
                    m_StateIndex = i;
                    if (elements != null)
                        for (int j = 0; j < elements.Count; ++j)
                            elements[j].Agent.Set(elements[j], i);
                    if (isnotify)
                        onStateChange.Invoke();
                    return true;
                }
            }
            return false;
        }

        bool SetState(int value)
        {
            return SetCurrentState(value, true);
        }

#if UNITY_EDITOR
        public void AddElement(Type type)
        {
            int lenght = states.Length;

            Element element = new Element();
            element.type = type;
            element.stateData = new ElementStateData[lenght];
            for (int i = 0; i < lenght; ++i)
            {
                element.stateData[i] = new ElementStateData();
                element.Agent.Init(element, element.stateData[i]);
            }

            if (elements == null)
                elements = new List<Element>();

            elements.Add(element);
        }

        public bool RemoveElement(Element element)
        {
            if (elements == null)
                return false;

            return elements.Remove(element);
        }

        
        public void AddState()
        {
            var len = states.Length;
            System.Array.Resize<StateConfig>(ref states, len + 1);

            states[len] = new StateConfig();
            states[len].Name = len.ToString();

            if (elements != null)
            {
                for (int i = 0; i < elements.Count; ++i)
                    elements[i].AddState();
            }
        }

        public void RemoveState(int index)
        {
            StateElementDraw.ArrayRemove(ref states, index);

            if (elements == null)
                return;

            for (int i = elements.Count - 1; i >= 0; i --)
            {
                elements[i].RemoveState(index);
            }
        }
#endif
    }
}