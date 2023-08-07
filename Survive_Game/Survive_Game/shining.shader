//Shining Shader


fixed4 frag(v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.uv) * inColor;

	if (col.a > 0)
	{
		float2 fragCoord = i.uv * _MainTex_TexelSize.zw;
		float x = _MainTex_TexelSize.z * shiningTime * 0.3 - i.uv.y * 500;

		float a = clamp(abs(fragCoord.x - x) / shiningWidth, 0, 1);
		a = 1 - a;
		a = a * a * a;

		col = mix(col, shaderColor, a);
	}

	return col;
}