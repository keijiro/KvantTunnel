//
// GPGPU kernels for Tunnel
//
Shader "Hidden/Kvant/Tunnel/Kernels"
{
    Properties
    {
        _MainTex ("-", 2D) = ""{}
    }

    CGINCLUDE

    #pragma multi_compile DEPTH1 DEPTH2 DEPTH3 DEPTH4 DEPTH5
    #pragma multi_compile _ ENABLE_WARP

    #include "UnityCG.cginc"
    #include "ClassicNoise2D.cginc"

    sampler2D _MainTex;
    float2 _MainTex_TexelSize;

    float2 _Extent;
    float2 _Offset;
    float2 _Frequency;
    float3 _Amplitude;
    float2 _ClampRange;

    // Base shape (cylinder).
    float3 cylinder(float2 uv)
    {
        float x = cos(uv.x * UNITY_PI * 2);
        float y = sin(uv.x * UNITY_PI * 2);
        float z = uv.y - 0.5;
        return float3(x, y, z) * _Extent.xxy;
    }

    // Pass 0: Calculates vertex positions
    float4 frag_position(v2f_img i) : SV_Target 
    {
        float3 vp = cylinder(i.uv);

        float2 nc1 = (float2(i.uv.x, vp.z) + _Offset) * _Frequency;
    #if ENABLE_WARP
        float2 nc2 = nc1 + float2(124.343, 311.591);
        float2 nc3 = nc1 + float2(273.534, 178.392);
    #endif

        float2 np = float2(_Frequency.x, 100000);

        float n1 = pnoise(nc1, np);
    #if ENABLE_WARP
        float n2 = pnoise(nc2, np);
        float n3 = pnoise(nc3, np);
    #endif

    #if DEPTH2 || DEPTH3 || DEPTH4 || DEPTH5
        n1 += pnoise(nc1 * 2, np * 2) * 0.5;
    #if ENABLE_WARP
        n2 += pnoise(nc2 * 2, np * 2) * 0.5;
        n3 += pnoise(nc3 * 2, np * 2) * 0.5;
    #endif
    #endif

    #if DEPTH3 || DEPTH4 || DEPTH5
        n1 += pnoise(nc1 * 4, np * 4) * 0.25;
    #if ENABLE_WARP
        n2 += pnoise(nc2 * 4, np * 4) * 0.25;
        n3 += pnoise(nc3 * 4, np * 4) * 0.25;
    #endif
    #endif

    #if DEPTH4 || DEPTH5
        n1 += pnoise(nc1 * 8, np * 8) * 0.125;
    #if ENABLE_WARP
        n2 += pnoise(nc1 * 8, np * 8) * 0.125;
        n3 += pnoise(nc1 * 8, np * 8) * 0.125;
    #endif
    #endif

    #if DEPTH5
        n1 += pnoise(nc1 * 16, np * 16) * 0.0625;
    #if ENABLE_WARP
        n2 += pnoise(nc1 * 16, np * 16) * 0.0625;
        n3 += pnoise(nc1 * 16, np * 16) * 0.0625;
    #endif
    #endif

    #if ENABLE_WARP
        float3 d = float3(n1, n2, n3);
    #else
        float3 d = float3(n1, 0, 0);
    #endif

        d = clamp(d, _ClampRange.x, _ClampRange.y) * _Amplitude;

        float3 v1 = normalize(vp * float3(-1, -1, 0));
        float3 v2 = float3(0, 0, 1);
        float3 v3 = cross(v1, v2);

        d = v1 * d.x + v2 * d.y + v3 * d.z;

        return float4(vp + d, 1);
    }

    // Pass 1: Calculates normal vectors for the 1st submesh
    float4 frag_normal1(v2f_img i) : SV_Target 
    {
        float2 duv = _MainTex_TexelSize;

        float3 v1 = tex2D(_MainTex, i.uv + float2(0, 0) * duv).xyz;
        float3 v2 = tex2D(_MainTex, i.uv + float2(1, 1) * duv).xyz;
        float3 v3 = tex2D(_MainTex, i.uv + float2(2, 0) * duv).xyz;

        float3 n = normalize(cross(v2 - v1, v3 - v1));

        return float4(n, 0);
    }

    // Pass 2: Calculates normal vectors for the 2nd submesh
    float4 frag_normal2(v2f_img i) : SV_Target 
    {
        float2 duv = _MainTex_TexelSize;

        float3 v1 = tex2D(_MainTex, i.uv + float2( 0, 0) * duv).xyz;
        float3 v2 = tex2D(_MainTex, i.uv + float2(-1, 1) * duv).xyz;
        float3 v3 = tex2D(_MainTex, i.uv + float2( 1, 1) * duv).xyz;

        float3 n = normalize(cross(v2 - v1, v3 - v1));

        return float4(n, 0);
    }

    ENDCG

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert_img
            #pragma fragment frag_position
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert_img
            #pragma fragment frag_normal1
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert_img
            #pragma fragment frag_normal2
            ENDCG
        }
    }
}
