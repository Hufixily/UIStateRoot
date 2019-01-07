using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace game
{
    public partial class UMaterialEA : TElementAgent<Graphic>
    {
        static Material GetGraphicMaterial(Graphic graphic)
        {
            if (graphic.material == Graphic.defaultGraphicMaterial)
                return null;
            return graphic.material;
        }

        public override void Init(Element element, ElementStateData sd)
        {
            if (element.target != null)
            {
                Graphic graphic = element.GetTarget<Graphic>();
                sd.obj = GetGraphicMaterial(graphic);
            }
        }

        public override void Set(Element element, int index)
        {
            if (element.target != null)
            {
                Graphic graphic = element.GetTarget<Graphic>();
                graphic.material = element.stateData[index].obj as Material;

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(graphic);
#endif
            }
        }

#if UNITY_EDITOR
        public override bool ShowState(Element element, ElementStateData sc, int index)
        {
            Material old = sc.obj as Material;
            Material material = (Material)EditorGUILayout.ObjectField(old, typeof(Material), false);
            if (GUILayout.Button("灰"))
                material = Resources.Load<Material>("UIGray");

            if (GUILayout.Button("空"))
                material = null;

            if (material != old)
            {
                RegisterUndo(() =>
                {
                    sc.obj = material;
                    if (sc.obj == Graphic.defaultGraphicMaterial)
                        sc.obj = null;
                });
                return true;
            }

            return false;
        }
#endif
    }
}