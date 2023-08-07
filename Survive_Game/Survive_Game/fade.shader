//
// Fade Shader
//
// outline, shining와 다르게 모든 텍스쳐를 참고해야 하므로
// 모든 텍스쳐를 관리하는 draw 함수에서 텍스쳐를 통합한 뒤 

public override void draw(float dt)
{
	if (texRt == null)
	{
		texRt = new RenderTexture(MainCamera.devWidth,
			MainCamera.devHeight, 32,
			RenderTextureFormat.ARGB32);
	}

	RenderTexture bk = RenderTexture.active;
	RenderTexture.active = texRt;
	GL.Clear(true, true, Color.clear);

	drawBG();
	drawTitle(dt);
	drawMenu(dt);
	drawSetting(dt);
	drawH2P(dt);
	drawExit(dt);

	RenderTexture.active = bk;

	if (startClick)
	{
		float _shaderDt = 2.5f;
		shaderDt += dt;
		if (shaderDt > _shaderDt)
			Main.me.reset("Proc");
		setShader(1);
		float r = shaderDt / _shaderDt;
		if (r < 0.3f)
			r = r / 0.3f * 80;
		else if (r < 0.7f)
			r = 80;
		else// if( r < 1.0f )
		{
			r = (r - 0.7f) / 0.3f * 12 + 80;
			if (r > 90)
				r = 90;
		}
		float radius = 800 * Mathf.Abs(Mathf.Cos(r * Mathf.Deg2Rad));
		setShaderFade(300, 200, radius, Color.black);
	}
	drawImage(texRt, 0, 0, TOP | LEFT);
	setShader(0);
	setRGBA(1, 1, 1, 1);
}


//실제 쉐이더 구현 부분
fixed4 frag(v2f i) : SV_Target
{
	fixed4 col = tex2D(_MainTex, i.uv) * inColor;
	float2 fragCoord = i.uv * _MainTex_TexelSize.zw;
	float len = length(fragCoord.xy - circle.xy);

	col = mix(col, shaderColor, clamp(len - circle.z, 0, 1.));

	return col;
}