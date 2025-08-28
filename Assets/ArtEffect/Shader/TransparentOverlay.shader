Shader "Custom/TransparentOverlay"   

{    

    Properties    

    {    

        _MainTex ("MainTex(RGBA)",2D) = "white" {}    

    }  

    SubShader    

    {    

        LOD 100  

        Cull Off Lighting Off ZWrite Off ZTest Always  

        Blend SrcAlpha OneMinusSrcAlpha  

        Tags  

        {  

            "Queue" ="Overlay"  

            "IgnoreProjector" ="True"  

            "RenderType" ="Transparent+2"  

        }  

        Pass  

        {  

            SetTexture[_MainTex]  

        }  

    }  

}  