//people move

void loadPeople()
{
	people = playerEvent.storage.people;
	for (int i = 0; i < people; i++)
	{
		playerEvent.pState[i].pos = new iPoint(MainCamera.devWidth - 250, MainCamera.devHeight - 150);
		playerEvent.pState[i].curPos = playerEvent.pState[i].pos;
	}

	_moveDt = 3f;
	peopleInOut = new iPoint[]
	{
			new iPoint(MainCamera.devWidth - 250, MainCamera.devHeight - 130),//home
			new iPoint(MainCamera.devWidth / 2, MainCamera.devHeight - 130),//street
			new iPoint(MainCamera.devWidth / 2, -50),//up
			new iPoint(200, MainCamera.devHeight - 150),//field
			new iPoint(MainCamera.devWidth - 200, MainCamera.devHeight/2 - 100),//lab
	};
	float len = 0f, l = 0f;
	for (int i = 0; i < 2; i++)
	{
		iPoint p = peopleInOut[i] - peopleInOut[1 + i];
		float n = Mathf.Sqrt(p.x * p.x + p.y * p.y);
		len += n;
		if (i == 0)
			l = n;
	}
	moveRate = l / len;
	//setPeople(1, cbPeopleGo);
}

void setPeople(int behave, MethodPeople method)
{
	for (int i = 0; i < people; i++)
	{
		PeopleState ps = playerEvent.pState[i];
		if ((behave == 1 && ps.behave == 0) || (behave == 2 && ps.behave != 0))
		{
			ps.behave = behave;// 1 or 2 go or back
			ps.moveDt = -0.2f * i;
			ps.curPos = ps.pos;
		}
	}
	methodPeople = method;
}

void drawPeople(float dt)
{
	people = playerEvent.storage.people;
	if (people == 0)
		return;

	bool endOfGo = true;
	bool endOfBack = true;

	for (int i = 0; i < people; i++)
	{
		PeopleState ps = playerEvent.pState[i];
		
		setRGBA(1, 1, 1, 1);
		string[] texname = new string[] { "jobless", "explorer", "worker", "farmer", "researcher" };
		Texture peopleTex = Util.createTexture(texname[ps.job]);
		drawImage(peopleTex, ps.pos, psize.width / peopleTex.width, psize.height / peopleTex.height, VCENTER | HCENTER);

		drawString(ps.name, ps.pos + new iPoint(-18, -50), TOP | LEFT);

		Texture building = Util.createTexture("house");
		drawImage(building, new iPoint(MainCamera.devWidth - 300, MainCamera.devHeight - 250), 150.0f / building.width, 150.0f / building.height, LEFT | HCENTER);
		drawImage(Util.createTexture("research"), new iPoint(MainCamera.devWidth - 280, MainCamera.devHeight / 2 - 180), 150.0f / building.width, 150.0f / building.height, LEFT | HCENTER);

		int at = ps.job != 4 ? 2 : 3;
		switch (ps.job)
		{
			case 0:
				at = 1;
				break;
			case 1:
			case 2:
				at = 2;
				break;
			case 3:
				at = 3;
				break;
			case 4:
				at = 4;
				break;
		}

		if (ps.moveDt < 0f)
		{
			ps.moveDt += dt;
			continue;
		}
		if (ps.behave == 1)
		{
			// go
			float r = ps.moveDt / _moveDt;
			if (r < moveRate)
				ps.pos = Math.linear(r / moveRate, peopleInOut[0], peopleInOut[1]);
			else
			{
				ps.pos = Math.linear((r - moveRate) / (1f - moveRate), peopleInOut[1], peopleInOut[at]);
				ps.curPos = peopleInOut[at];
			}

			ps.moveDt += dt;
			if (ps.moveDt > _moveDt)
				ps.behave = 3;
			else
			{
				endOfGo = false;
			}
		}
		else if (ps.behave == 2)
		{
			// back

			float r = ps.moveDt / _moveDt;
			if (r < moveRate)
				ps.pos = Math.linear(r / moveRate, ps.curPos, peopleInOut[1]);
			else
			{
				ps.pos = Math.linear((r - moveRate) / (1f - moveRate), peopleInOut[1], peopleInOut[0]);
				ps.curPos = peopleInOut[0];
			}

			ps.moveDt += dt;
			if (ps.moveDt > _moveDt)
				ps.behave = 0;
			else
			{
				endOfBack = false;
			}
		}
	}

	if (methodPeople != null)
	{
		if (endOfGo && endOfBack)
		{
			methodPeople();
			methodPeople = null;
		}
	}
}