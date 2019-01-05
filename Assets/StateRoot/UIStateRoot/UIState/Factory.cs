namespace game
{
    public class Factory
    {
#if UNITY_EDITOR
        public static string[] Names = new string[(int)Type.Max];
#endif
        static ElementAgent[] factorys = new ElementAgent[(int)Type.Max];

        static T Create<T>(string name) where T : new()
        {
            T t = new T();
#if UNITY_EDITOR
            ElementAgent tt = t as ElementAgent;
            tt.Name = name;
#endif
            return t;
        }

        static Factory()
        {
            factorys[(int)Type.Go] = Create<GoEA>("对象");
            
            {
                factorys[(int)Type.UImage] = Create<UImageEA>("U精灵");
                factorys[(int)Type.UText] = Create<UTextEA>("U文本");
                factorys[(int)Type.UAlpha] = Create<UAlphaEA>("U透明");
                factorys[(int)Type.UWidth] = Create<UWidthEA>("U宽度");
                factorys[(int)Type.UHeight] = Create<UHeightEA>("U高度");
                factorys[(int)Type.UColor] = Create<UColorEA>("U颜色");
                factorys[(int)Type.UMaterial] = Create<UMaterialEA>("U材质");
                factorys[(int)Type.UGradient] = Create<UGradientEA>("U颜色渐变");
                factorys[(int)Type.CanvasGroup] = Create<CanvasGroupEA>("透明");
                factorys[(int)Type.UTextFont] = Create<UTextFontEA>("U字体");
            }

            factorys[(int)Type.Pos] = Create<PositionEA>("位置");
            
            factorys[(int)Type.Rotate] = Create<RotateEA>("旋转");
            factorys[(int)Type.Scale] = Create<ScaleEA>("缩放");
            factorys[(int)Type.StateRoot] = Create<StateRootEA>("状态");
            factorys[(int)Type.PlayAnim] = Create<PlayAnimEA>("动画");

#if UNITY_EDITOR
            for (int i = 0; i < (int)Type.Max; ++i)
            {
                Names[i] = (factorys[i] == null ? ((Type)i).ToString() : factorys[i].Name);
            }
#endif
        }

        static public ElementAgent GetAgent(Element element)
        {
            return factorys[(int)element.type];
        }
    }
}