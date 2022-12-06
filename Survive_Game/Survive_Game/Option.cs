//Setting

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
	string[] strBtn = new string[] { "X", "<", ">" };
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
	string[] strMode = new string[] { "Full Screen", "Window" };
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
		else// if( i==6, i==7, i==8 )//��ü â���� ����� / ��ü/â �ΰ���
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