Shader "Vaporwave"
{
	Properties
	{
		[HideInInspector]
		_MainTex( "Texture", 2D ) = "white" {}

		[Header(Chromatic Aberation)]
		_CaSplitDist( "Split Distance",  Range(0, 0.01) ) = 0.001
		_CaDirection( "Split Direction", Range( 0, 360) ) = 0

		[Header(Noise)]
		_Noisiness( "Noisiness", Range(0,1) ) = 0.1

		[Header(Glitchy Stuff)]
		[Toggle]_SplitToggle( "Enabled?", Float      ) = 0
		_SplitHeight( "Split Height",   Range( 0, 1) ) = 0.5
		_SplitDist(   "Split Distance", Range(-0.1, 0.1) ) = 0.01

		[Header(Wiggles)]
		_wSpeed(     "Speed",     Float          ) = 20
		_wSpread(    "Spread",    Float          ) = 100
		_wIntensity( "Intensity", Range(0,0.001) ) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			float _Noisiness;

			float _wSpeed;
			float _wSpread;
			float _wIntensity;

			float _SplitToggle;
			float _SplitHeight;
			float _SplitDist;

			float _CaSplitDist;
			float _CaDirection;

			// Rotate float2 by degrees
			float2 Rotate( float2 v, float deg) 
			{
				v.x = ( cos( radians(deg) ) * v.x ) - ( sin( radians(deg) ) * v.y );
				v.y = ( sin( radians(deg) ) * v.x ) + ( cos( radians(deg) ) * v.y );
				return v;
			}

			// Generate random 0-1 value based on uv pos
			float GenNoise( float2 uvPos )
			{
				float3 input = float3( uvPos.x, uvPos.y, _Time.z );
				return frac( sin( dot( input.xyz ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
			}

		/* Frag */
			fixed4 frag (v2f i) : SV_Target
			{
			/* Wiggle effects */
				i.uv.x += sin( i.uv.y * _wSpread + (_Time.y * _wSpeed) ) * _wIntensity;

			/* Glitchy Stuff */
				i.uv.x += step( 1 - _SplitHeight, i.uv.y ) * _SplitToggle * _SplitDist;

			/* Grab original pixel color */
				fixed4 col = tex2D(_MainTex, i.uv);

			/* Chromatic Aberration */
				// Calculate offsets
				float2 redOffset  = Rotate( float2( _CaSplitDist,        0 ), _CaDirection );
				float2 blueOffset = Rotate( float2( -_CaSplitDist * 0.6, 0 ), _CaDirection );

				// Set red and blue based on offsets
				col.r = tex2D(_MainTex, i.uv + redOffset).r;
				col.b = tex2D(_MainTex, i.uv + blueOffset).b;

			/* Noise */
				col *= clamp( GenNoise( i.uv ), 1 - _Noisiness, 1 );

				return col;
			}
			ENDCG
		}
	}
}
