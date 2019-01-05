using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace game
{
    [InitializeOnLoad()]
    class InitStateRoot
    {
        static InitStateRoot()
        {
            if (ElementAgent.getCurrentStateRoot == null)
                ElementAgent.getCurrentStateRoot = () => { return StateRootEditor.instance == null ? null : StateRootEditor.instance.Get(); };

            if (StateElementDraw.CurrentDrawStateInfo == null)
                StateElementDraw.CurrentDrawStateInfo = StateRootEditor.DrawStateInfo;
        }
    }

    [CustomEditor(typeof(StateRoot), true)]
    public class StateRootEditor : Editor
    {
        bool m_ShowElement = true;

        bool m_IsShowRecord = false; // 记录当前数据

        public static StateRootEditor instance = null;

        public StateRoot Get()
        {
            if (target == null)
                return null;

            return target as StateRoot;
        }

        SerializedProperty onStateChangeProperty;
        SerializedProperty onClickProperty;

        void OnEnable()
        {
            instance = this;
            onStateChangeProperty = serializedObject.FindProperty("onStateChange");
            onClickProperty = serializedObject.FindProperty("onClick");
        }

        public static bool DrawStateInfo(Element element, int stateid, bool isShowRecord, bool iscanset)
        {
            var target = (instance == null ? null : instance.Get());

            EditorGUILayout.BeginHorizontal();
            element.Agent.ShowElementState(element, stateid, iscanset);
            GUILayout.Space(-5);

            var preWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 40;

            var esd = element.stateData[stateid];
            if (element.Agent.ShowState(element, esd, stateid))
            {
                if (target == null || target.currentState == stateid)
                {
                    element.Agent.Set(element, stateid);
                }
            }

            if (element.Agent.isSmooth)
            {
                GUI.changed = false;
                bool issmooth = GUILayout.Toggle(esd.isSmooth, "渐变", GUILayout.ExpandWidth(false));
                float smoothTime = esd.smoothTime;
                if (issmooth)
                {
                    smoothTime = EditorGUILayout.FloatField("时长", smoothTime);
                }

                if (GUI.changed)
                {
                    ElementAgent.RegisterUndo(() =>
                    {
                        esd.isSmooth = issmooth;
                        esd.smoothTime = smoothTime;
                    });
                }
            }

            if (isShowRecord)
            {
                if (GUILayout.Button("记录"))
                {
                    ElementAgent.RegisterUndo(() =>
                    {
                        element.Agent.Init(element, element.stateData[stateid]);
                    });
                }
            }

            if (iscanset)
                if (GUILayout.Button("移除"))
                    return true;

            EditorGUIUtility.labelWidth = preWidth;
            EditorGUILayout.EndHorizontal();

            return false;
        }

        static public void RegisterUndo(UnityEngine.Object obj, string name)
        {
#if UNITY_EDITOR
            Undo.RecordObject(obj, name);
            SetDirty(obj);
#endif
        }


        static public void SetDirty(UnityEngine.Object obj)
        {
#if UNITY_EDITOR
            if (obj)
            {
                EditorUtility.SetDirty(obj);
            }
#endif
        }

        void RegisterUndo()
        {
#if UNITY_EDITOR
            RegisterUndo(target, "State Root Change");
#endif
        }

        public override void OnInspectorGUI()
        {
            var target = this.target as StateRoot;

            EditorGUILayout.Space();
            EditorGUIUtility.labelWidth = 80;
            GUI.changed = false;
            //功能区
            OnDrawInfo(target);

            //元素区
            EditorGUILayout.BeginVertical("box");
            OnDrawElement(target);

            //当前状态
            EditorGUILayout.BeginVertical("box");
            var currentState = EditorGUILayout.Popup("初始状态", target.currentState, target.StateNames);
            if (GUI.changed)
            {
                RegisterUndo();
                target.currentState = currentState;
            }
            //状态按钮
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("增加一个状态", GUILayout.Height(40)))
            {
                RegisterUndo();
                target.AddState();
                target.currentState = target.states.Length - 1;
            }
            if (GUILayout.Button("删除当前状态", GUILayout.Height(40)))
            {
                RegisterUndo();
                target.RemoveState(target.currentState);
                target.currentState = target.states.Length > 0 ? target.states.Length - 1 : 0;
            }
            EditorGUILayout.EndHorizontal();
            
            var index = target.currentState;
            if(target.states.Length != 0)
            {
                if (target.elements != null)
                {
                    EditorGUILayout.BeginVertical("window");
                 
                    var config = target.states[index];
                    config.Name = EditorGUILayout.TextField("状态名", config.Name);

                    foreach (Element element in target.elements)
                    {
                        DrawStateInfo(element, index, m_IsShowRecord, false);
                    }
                    EditorGUILayout.EndVertical();
                }
            }


            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            //事件区
            OnDrawEventInfo(target);

            serializedObject.ApplyModifiedProperties();
        }

        private void OnDrawInfo(StateRoot target)
        {
            GUI.changed = false;

            EditorGUILayout.BeginHorizontal("box");
            //m_IsShowRecord = EditorGUILayout.Toggle("记录按钮", m_IsShowRecord);
            var isClickSwitchState = EditorGUILayout.Toggle("点击切换状态", target.isClickSwitchState);
            EditorGUILayout.EndHorizontal();

            if(GUI.changed)
            {
                RegisterUndo();
                target.isClickSwitchState = isClickSwitchState;
            }
        }

        private void OnDrawEventInfo(StateRoot target)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            var btn = EditorGUILayout.ObjectField("按钮", target.currentButton, typeof(Button), true) as Button;
            if (btn != target.currentButton)
            {
                RegisterUndo();
                target.currentButton = btn;
            }
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(onStateChangeProperty);
            EditorGUILayout.PropertyField(onClickProperty);

            EditorGUILayout.EndVertical();
        }

        private void OnDrawElement(StateRoot target)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical("box");
            m_ShowElement = EditorGUILayout.Foldout(m_ShowElement, "元素");
            if (m_ShowElement)
            {
                EditorGUI.indentLevel++;
                if (target.elements != null)
                {
                    for (int i = 0; i < target.elements.Count;)
                    {
                        if (StateElementDraw.DrawElement(target.elements[i]))
                        {
                            RegisterUndo();
                            target.RemoveElement(target.elements[i]);
                        }
                        else
                        {
                            ++i;
                        }
                    }
                }

                //增加按钮
                int idx = StateElementDraw.DrawType();
                if (idx != -1)
                {
                    RegisterUndo();
                    target.AddElement((Type)idx);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
    }
}