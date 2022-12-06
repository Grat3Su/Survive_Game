fixed4 frag(v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.uv) * inColor;
	float2 fragCoord = i.uv * _MainTex_TexelSize.zw;
	float len = length(fragCoord.xy - circle.xy);

	col = mix(col, shaderColor, clamp(len - circle.z, 0, 1.));

	return col;
}