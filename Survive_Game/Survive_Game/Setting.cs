iPopup popSetting = null;
iStrTex stSetting;
iImage[] imgSettingBtn;
string windowString;

void loadSetting()
{
	iPopup pop = new iPopup();

	iImage img = new iImage();
	iStrTex st = new iStrTex(methodStSetting, 500, MainCamera.devHeight - 100);
	st.setString("0");
	img.add(st.tex);
	pop.add(img);
	stSetting = st;

	imgSettingBtn = new iImage[9];
	string[] strBtn = new string[]{ "X", "<", ">" };
	for (int i = 0; i < 3; i++)
	{
		img = new iImage();

		st = new iStrTex(methodStSettingBtn, 50, 50);
		st.setString(i + "\n" + strBtn[i] + "\n");
		if (i == 0)
			img.position = new iPoint(440, 10);
		else if (i == 1)
			img.position = new iPoint(50, 100);
		else// if (i == 2)
			img.position = new iPoint(400, 100);
		img.add(st.tex);
		pop.add(img);
		imgSettingBtn[i] = img;
	}
	for (int i = 0; i < 2; i++)
	{
		img = imgSettingBtn[1 + i].clone();
		img.position.y += 150;
		pop.add(img);
		imgSettingBtn[3 + i] = img;
	}

	img = new iImage();
	string[] strMode = new string[]{ "Full Screen", "Window" };
	for (int i = 0; i < 2; i++)
	{
		st = new iStrTex(methodStSettingBtn, 250, 50);
		st.setString((i + 6) + "\n" + strMode[i] + "\n");
		img.add(st.tex);
	}
	img.position = new iPoint(125, 350);
	pop.add(img);
	imgSettingBtn[5] = img;

	for (int i = 0; i < 2; i++)
	{
		img = new iImage();
		img.add(imgSettingBtn[5].listTex[i]);
		img.position = new iPoint(125, 400 + 50 * i);
		pop.add(img);
		img.alpha = 0f;
		imgSettingBtn[6 + i] = img;
	}

	pop.style = iPopupStyle.zoom;
	pop.openPoint = new iPoint(800, 500);
	pop.closePoint = new iPoint((MainCamera.devWidth - 500) / 2, 50);
	pop.methodDrawBefore = drawBeforeSetting;
	pop._aniDt = 0.5f;
	popSetting = pop;
}

void drawBeforeSetting(float dt, iPopup pop, iPoint zero)
{
	if (popSetting.selected != -1)
	{//
		string s = SoundManager.instance().printVolume(iSound.BGM) + "" + SoundManager.instance().printVolume(iSound.ButtonClick);
		stSetting.setString(s + " " + popSetting.selected);
	}
}

void methodStSetting(iStrTex st)
{
	setStringName("BMJUA_ttf");
	setRGBA(0.8f, 0.8f, 0.8f, 1);
	fillRect(0, 0, 500, MainCamera.devHeight - 100);
	setRGBA(1, 1, 1, 1);
	setStringRGBA(0, 0, 0, 1);
	setStringSize(30);
	drawString("BGM Volume", new iPoint(st.tex.tex.width / 2, 50), TOP | HCENTER);
	drawVolume(iSound.BGM, new iPoint(135, 100));

	drawString("Effect Volume", new iPoint(st.tex.tex.width / 2, 200), TOP | HCENTER);
	drawVolume(iSound.ButtonClick, new iPoint(135, 250));
}

void drawVolume(iSound st, iPoint p)
{
	int vol = SoundManager.instance().intVolume(st);

	for (int i = 0; i < 4; i++)
	{
		setRGBA(0.5f, 0.5f, 0.5f, 1);
		if (i < vol)
			setRGBA(0, 1, 0, 1);

		fillRect(p.x, p.y, 50, 50);
		p.x += 60;
	}
}

void methodStSettingBtn(iStrTex st)
{
	string[] strs = st.str.Split("\n");
	int index = int.Parse(strs[0]);
	string s = strs[1];

	setStringName("BMJUA_ttf");
	iPoint pos = new iPoint(0, 0);

	if (index == 0)
		setRGBA(0.8f, 0, 0, 1);
	else
		setRGBA(1, 1, 1, 1);
	setStringRGBA(0, 0, 0, 1);

	int w = st.tex.tex.width;
	int h = st.tex.tex.height;
	fillRect(0, 0, w, h);
	setStringRGBA(0, 0, 0, 1);
	drawString(s, w / 2, h / 2, VCENTER | HCENTER);
}

void drawSetting(float dt)
{
	popSetting.paint(dt);
}

bool keySetting(iKeystate stat, iPoint point)
{
	if (!popSetting.bShow)
		return false;

	iPoint p;
	p = popSetting.closePoint;
	iSize s = new iSize(0, 0);

	if (stat == iKeystate.Began)
	{
		for (int i = 0; i < 8; i++)
		{
			if (imgSettingBtn[i].touchRect(p, s).containPoint(point))
			{
				SoundManager.instance().play(iSound.ButtonClick);
				if (i > 5)
				{
					if (imgSettingBtn[6].alpha > 0)
						popSetting.selected = i;
				}
				else
					popSetting.selected = i;

				if (popSetting.selected == 1)
					SoundManager.instance().volume(iSound.BGM, true);
				else if (popSetting.selected == 2)
					SoundManager.instance().volume(iSound.BGM, false);
				else if (popSetting.selected == 3)
					SoundManager.instance().volume(iSound.ButtonClick, true);
				else if (popSetting.selected == 4)
					SoundManager.instance().volume(iSound.ButtonClick, false);
				break;
			}
		}
		//popSetting.show(false);
	}
	else if (stat == iKeystate.Drag)
	{
		Debug.Log(popSetting.selected);
	}
	else if (stat == iKeystate.Ended)
	{
		int i = popSetting.selected;
		if (i == -1)
			return true;
		popSetting.selected = -1;

		if (i == 0)
			popSetting.show(false);
		else if (i < 5)
		{
			// nothing...
		}
		else if (i == 5)
		{
			for (i = 0; i < 2; i++)
				imgSettingBtn[6 + i].alpha = 1f;
		}
		else// if( i==6, i==7, i==8 )//전체 창모드는 지우기 / 전체/창 두개만
		{
			if (i == 6)
			{
				Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
				iGUI.setResolutionFull(MainCamera.devWidth, MainCamera.devHeight);
				Screen.fullScreen = true;
			}
			else// if( i==7 )
			{
				Screen.fullScreen = false;
				iGUI.setResolution(MainCamera.devWidth, MainCamera.devHeight);
				Screen.fullScreenMode = FullScreenMode.Windowed;
			}

			imgSettingBtn[5].frame = (i - 6);
			for (i = 0; i < 2; i++)
				imgSettingBtn[6 + i].alpha = 0;
		}
	}
	return true;
}