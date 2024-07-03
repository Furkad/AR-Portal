Shader "ARPortal/PortalWindow"
{
    Properties
    {
        _Stencil ("Stencil", float) = 0
        [Enum(UnityEngine.Rendering.CullMode)] _Culling ("Culling", float) = 2
    }

    SubShader
    {
        Tags { "Queue" = "Geometry-1"}

        ZWrite off
        Cull [_Culling]
        ColorMask 0

        Stencil
        {
            Ref [_Stencil]
            Pass Replace
        }

        Pass
        {
            
        }
    }
}
