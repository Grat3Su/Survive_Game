//outline shader

#define mix(x, y, a)  ((x) * (1-(a)) + (y) * (a))
fixed4 frag(v2f i) : SV_Target
{
	float2 fragCoord = i.uv * _MainTex_TexelSize.zw;

	fixed4 col = tex2D(_MainTex, i.uv) * inColor;

	float2 size = outlineWidth * _MainTex_TexelSize.xy;
	float outline =
		tex2D(_MainTex, i.uv + float2(-size.x, -size.y)).a +
		tex2D(_MainTex, i.uv + float2(0, -size.y)).a +
		tex2D(_MainTex, i.uv + float2(+size.x, -size.y)).a +
		tex2D(_MainTex, i.uv + float2(-size.x, 0)).a +
		tex2D(_MainTex, i.uv + float2(+size.x, 0)).a +
		tex2D(_MainTex, i.uv + float2(-size.x, +size.y)).a +
		tex2D(_MainTex, i.uv + float2(0, +size.y)).a +
		tex2D(_MainTex, i.uv + float2(+size.x, +size.y)).a;
	outline = min(outline, 1);

	col = mix(col, shaderColor, outline - col.a);// mix(x, y, a) = x * (1-a) + y * a

	return col;
}
ENDCG