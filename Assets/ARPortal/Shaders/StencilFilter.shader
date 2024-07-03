Shader "ARPortal/StencilFilter"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        [Enum(Less,2,Equal,3,NotEqual,6)] _StencilComp ("Stencil Test", float) = 6
        _Stencil ("Stencil", float) = 0
    }
    SubShader
    {
        Color [_Color]
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilTest]
        }

        Pass
        {

        }
    }
}
