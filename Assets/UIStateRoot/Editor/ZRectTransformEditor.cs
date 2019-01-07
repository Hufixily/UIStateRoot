using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using UnityEngine.Events;

[CustomEditor(typeof(RectTransform))]
public class ZRectTransformEditor : Editor
{
    private Editor instance;

    protected Editor Instance
    {
        get
        {
            if (instance == null && targets != null && targets.Length != 0)
            {
                instance = Editor.CreateEditor(targets, refEditorType);
                m_Object = new SerializedObject(targets[0]);
                m_Property = m_Object.FindProperty("m_LocalPosition");
            }

            if (instance == null)
            {
                Debug.LogError("Could not create editor");
            }
            return instance;
        }
    }

    SerializedObject m_Object;
    SerializedProperty m_Property;

    private System.Type refEditorType;
    private System.Type refEditorObjType;

    //空参数
    private static readonly object[] EMPTY = new object[0];
    //editor Assembly
    private static Assembly m_EditorAssembly = Assembly.GetAssembly(typeof(Editor));
    //editor 使用过的方法记录
    private static Dictionary<string, MethodInfo> m_MethodInfos = new Dictionary<string, MethodInfo>();

    public ZRectTransformEditor()
    {
        Init("RectTransformEditor");
    }

    private void  Init(string typeName)
    {
        var flag = BindingFlags.NonPublic | BindingFlags.Instance;
        //获得类型
        refEditorType = m_EditorAssembly.GetTypes().Where(t => t.Name == typeName).FirstOrDefault();
        //初始化
        refEditorObjType = GetCustomEditorType(GetType(), flag);
        //check
        var originalEditorType = GetCustomEditorType(refEditorType, flag);

        if(originalEditorType != refEditorObjType)
            throw new System.ArgumentException(string.Format("Type {0} does not match the editor {1} type {2}", refEditorObjType, refEditorType, originalEditorType));
    }

    private void OnDisable()
    {
        if (instance != null)
            DestroyImmediate(instance);
    }

    protected System.Type GetCustomEditorType(System.Type type,BindingFlags flag)
    {
        var attributes = type.GetCustomAttributes(typeof(CustomEditor), true) as CustomEditor[];
        var field = attributes.Select(editor => editor.GetType().GetField("m_InspectedType", flag)).First();
        return field.GetValue(attributes[0]) as System.Type;
    }

    protected void InvokeMethod(string methodName)
    {
        MethodInfo method = null;
        if(!m_MethodInfos.TryGetValue(methodName,out method))
        {
            var flag = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            method = refEditorType.GetMethod(methodName, flag);
            if (method != null)
                m_MethodInfos.Add(methodName, method);
            else
                Debug.LogError(string.Format("Could not Find method {0}",methodName));
        }
        if (method != null)
            method.Invoke(Instance, EMPTY);
    }

    public override void OnInspectorGUI()
    {
        Instance.OnInspectorGUI();
        GUILayout.Space(10);
        if (GUILayout.Button("Reset"))
        {
            var tar = target as RectTransform;
            tar.anchoredPosition = Vector3.zero;
            tar.sizeDelta = new Vector2(100, 100);
            tar.anchorMin = new Vector2(0.5f, 0.5f);
            tar.anchorMax = new Vector2(0.5f, 0.5f);
            tar.pivot = new Vector2(.5f, .5f);
            tar.localScale = Vector3.one;
            tar.eulerAngles = Vector3.zero;
        }
        GUILayout.Space(10);
// 
//         GUI.skin.window.name = "WindowTest";
//         EditorGUILayout.BeginVertical(GUI.skin.window);
//         EditorGUILayout.PropertyField(m_Property);
//         EditorGUILayout.EndVertical();
    }
}
