using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace game
{
    public enum Type
    {
        Go,

        UImage,
        UText,
        UTextFont,
        UAlpha,
        UWidth,
        UHeight,
        UColor,
        UMaterial,
        UGradient,

        CanvasGroup, // 透明组件

        Pos,
        Rotate,
        Scale,
        StateRoot,
        PlayAnim,

        //
        Max,
    }
}
