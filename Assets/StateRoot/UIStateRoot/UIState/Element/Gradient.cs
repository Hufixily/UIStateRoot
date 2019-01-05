using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Effects/Gradient")]
    public class Gradient : BaseMeshEffect
    {
        public Color32 gradientTop = Color.white;
        public Color32 gradientBottom = Color.black;

        public Graphic target { get { return graphic; } }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            int count = vh.currentVertCount;
            if (count > 0)
            {
                UIVertex vertex = new UIVertex();
                vh.PopulateUIVertex(ref vertex, 0);

                float bottomY = vertex.position.y;
                float topY = vertex.position.y;

                for (int i = 1; i < count; i++)
                {
                    vh.PopulateUIVertex(ref vertex, i);

                    float y = vertex.position.y;
                    if (y > topY)
                    {
                        topY = y;
                    }
                    else if (y < bottomY)
                    {
                        bottomY = y;
                    }
                }

                float uiElementHeight = topY - bottomY;
                for (int i = 0; i < count; i++)
                {
                    vh.PopulateUIVertex(ref vertex, i);
                    vertex.color = Color32.Lerp(gradientBottom, gradientTop, (vertex.position.y - bottomY) / uiElementHeight);
                    vh.SetUIVertex(vertex, i);
                }
            }
        }
    }
}