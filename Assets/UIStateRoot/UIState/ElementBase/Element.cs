using UnityEngine;
using System.Collections;

namespace game
{
    [System.Serializable]
    public class Element
    {
        [SerializeField]
        string type_key;

        public Type type
        {
            get { return Str2Enum.To<Type>(type_key, default(Type)); }
            set { type_key = value.ToString(); }
        }
        
        public Object target; // 目标对象
        public ElementStateData[] stateData; // 状态数据

        public T GetTarget<T>() where T : Object
        {
            return target as T;
        }

        public ElementAgent Agent { get { return Factory.GetAgent(this); } }

#if UNITY_EDITOR
        public void AddState()
        {
            int lenght = stateData == null ? 0 : stateData.Length;
            System.Array.Resize<ElementStateData>(ref stateData, lenght + 1);
            stateData[lenght] = new ElementStateData();

            Agent.Init(this, stateData[lenght]);
        }

        public void RemoveState(int index)
        {
            StateElementDraw.ArrayRemove(ref stateData, index);
        }

        public string Name
        {
            get { return Factory.Names[(int)type];}
        }
#endif
    }
}