Shader "Custom/AlphCancel" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}
SubShader { 
 LOD 100
 Pass {
  Tags { "LIGHTMODE"="Vertex" }
  Lighting On
  Material {
   Diffuse [_Color]
  }
  Blend SrcAlpha OneMinusSrcAlpha
  AlphaTest Less 0.9
  SetTexture [_MainTex] { combine texture * primary double, texture alpha * primary alpha }
 }
}
}