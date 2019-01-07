using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace game
{
    public enum Type
    {
        Go,
        Pos,
        Rotate,
        Scale,

        UPos,
        UAnchors,
        UImage,
        UText,
        UTextFont,
        UAlpha,
        UWidth,
        UHeight,
        UColor,
        //UMaterial,
        UGradient,

        CanvasGroup, // 透明组件

        StateRoot,
        PlayAnim,

        //
        Max,
    }
}
