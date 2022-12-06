//scroll
iRect checkScrollbar(int barW, int barH)
{
	people = playerEvent.storage.people;
	// 가로 크기 / 총 크기
	int miniWidth = 200;
	int miniHeight = 500;

	int mapWidth = 200;
	int mapHeight = 60 * people;

	// 칸수
	float numW = 1.0f * mapWidth / miniWidth;
	float numH = 1.0f * mapHeight / miniHeight;

	//int bW = barW / bNumW;
	//int bH = barH / bNumH;
	int bW = barW * miniWidth / mapWidth;
	int bH = barH * miniHeight / mapHeight;

	int bX = (int)Math.linear(offPerson.x / offMin.x, 0, bW * (numW - 1));
	int bY = (int)Math.linear(offPerson.y / offMin.y, 0, bH * (numH - 1));

	return new iRect(bX, bY, bW, bH);
}

// ======================================================
// popPerson
// ======================================================
void loadPopPerson()
{
	iPopup pop = new iPopup();

	iImage img = new iImage();
	iStrTex st = new iStrTex(methodStPerson, 200, 500);
	st.setString("0");
	img.add(st.tex);
	pop.add(img);
	stPerson = st;
	people = playerEvent.storage.people;
	imgPersonBtn = new iImage[100];
	stPersons = new iStrTex[100][];

	for (int i = 0; i < 100; i++)
	{
		img = new iImage();
		stPersons[i] = new iStrTex[2];
		for (int j = 0; j < 2; j++)
		{
			st = new iStrTex(methodStPersonBtn, 150, 50);
			string s = "null";
			if (i < people) s = playerEvent.pState[i].name;
			st.setString(j + "\n" + s + "\n" + i);
			img.add(st.tex);
			stPersons[i][j] = st;
		}
		img.position = new iPoint(20, 10 + 60 * i);
		imgPersonBtn[i] = img;
	}

	pop.style = iPopupStyle.move;
	pop.methodClose = closePopPerson;
	pop.methodOpen = closePopPerson;
	pop.methodDrawBefore = drawPopPersonBefor;
	pop.openPoint = new iPoint(MainCamera.devWidth, (MainCamera.devHeight - 500) / 2);
	pop.closePoint = new iPoint(MainCamera.devWidth - 210, (MainCamera.devHeight - 500) / 2);
	pop._aniDt = 0.2f;
	popPerson = pop;

	offPerson = new iPoint(0, 0);
	offMin = new iPoint(0, 490 - 60 * people);
	offMax = new iPoint(0, 0);
}

public void methodStPerson(iStrTex st)
{
	setStringName("BMJUA_ttf");

	setRGBA(0.3f, 0.3f, 0.3f, 0.5f);
	fillRect(0, 0, 300, 600);
	people = playerEvent.storage.people;
	setRGBA(1, 1, 1, 1);


	for (int i = 0; i < people; i++)
	{	
		imgPersonBtn[i].frame = (popPerson.selected == i ? 1 : 0);
		imgPersonBtn[i].paint(0.0f, offPerson);
	}

	if (playerEvent.storage.getStorage(0) > 9)
	{
		iRect rt = checkScrollbar(200 - 20,
								500 - 40);
		// 상하 스크롤바
		float x = 200 - 20;
		float y = 10;
		float w = 10;
		float h = 500 - 20;
		setRGBA(0, 0, 0, 1f);
		fillRect(x + w / 2 - 2, y, 4, h);

		// 손잡이
		y += 10 + rt.origin.y;
		h = rt.size.height;
		fillRect(x, y, w, h);
	}
}

bool scroll;
iPoint prevPoint, firstPoint, mp;

bool keyPopPerson(iKeystate stat, iPoint point)
{
	if (popPerson.bShow == false || popPerson.state == iPopupState.close)
		return false;

	iPoint p;
	p = popPerson.closePoint;
	p.y += offPerson.y;

	int i;
	iSize s = new iSize(0, 0);

	switch (stat)
	{
		case iKeystate.Began:
			scroll = false;
			firstPoint = point;
			prevPoint = point;
			people = playerEvent.storage.people;
			for (i = 0; i < people; i++)
			{
				if (imgPersonBtn[i].touchRect(p, s).containPoint(point))//클릭
				{
					SoundManager.instance().play(iSound.ButtonClick);
					popPerson.selected = i;
					break;
				}
			}
			break;
		case iKeystate.Drag:
			if (scroll == false)
			{
				mp = point - firstPoint;
				if (Mathf.Sqrt(mp.x * mp.x + mp.y * mp.y) > 5)
				{
					if (point.x > popPerson.closePoint.x && point.x < popPerson.closePoint.x + 200 &&
						point.y > popPerson.closePoint.y && point.y < popPerson.closePoint.y + 500)
						scroll = true;
					prevPoint = point;

					popPerson.selected = -1;
				}
			}

			if (scroll)
			{
				people = playerEvent.storage.people;
				if (people > 8)
				{
					mp = point - prevPoint;
					prevPoint = point;

					offPerson.y += mp.y;
					if (offPerson.y < offMin.y)
						offPerson.y = offMin.y;
					else if (offPerson.y > offMax.y)
						offPerson.y = offMax.y;
				}
			}
			break;

		case iKeystate.Ended:
			if (scroll == false)
			{
				if (popPersonInfo.bShow == false)
				{
					if (popPerson.selected != -1)
					{
						if (!pe.bShowNewDay())
						{
							select = popPerson.selected;
							newJob = playerEvent.pState[select].job;

							SoundManager.instance().play(iSound.PopUp);
							popPersonInfo.show(true);
						}

						popPersonInfo.openPoint = imgPersonBtn[popPerson.selected].center(p);
					}
				}
			}
			break;
	}
	return false;
}
