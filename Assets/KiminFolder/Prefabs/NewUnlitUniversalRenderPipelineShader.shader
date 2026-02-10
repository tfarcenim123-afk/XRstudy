Shader "Custom/BubbleDistortion"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,0.5)
        _DistortionStrength ("Distortion Strength", Range(0, 0.1)) = 0.02
        _FresnelPower ("Fresnel Power", Range(0.1, 5.0)) = 2.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 screenPos : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _DistortionStrength;
                float _FresnelPower;
            CBUFFER_END

            Varyings vert (Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.screenPos = ComputeScreenPos(output.positionCS);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.viewDirWS = GetWorldSpaceViewDir(TransformObjectToWorld(input.positionOS.xyz));
                return output;
            }

            // 카메라 뒤 배경을 가져오기 위한 텍스처 선언
            TEXTURE2D(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            half4 frag (Varyings input) : SV_Target
            {
                // 1. 프레넬 효과 (가장자리 빛남)
                float3 normal = normalize(input.normalWS);
                float3 viewDir = normalize(input.viewDirWS);
                float fresnel = pow(1.0 - saturate(dot(normal, viewDir)), _FresnelPower);

                // 2. 배경 왜곡 (Refraction)
                float2 uv = input.screenPos.xy / input.screenPos.w;
                float2 distortion = normal.xy * _DistortionStrength;
                half3 background = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv + distortion).rgb;

                // 3. 최종 색상 조합
                half3 finalColor = lerp(background, _Color.rgb, 0.2) + (fresnel * 0.5);
                
                return half4(finalColor, _Color.a);
            }
            ENDHLSL
        }
    }
}