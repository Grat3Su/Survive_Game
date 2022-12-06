//생존자 정보

void jobAction()//-> 
{// 0 : 백수 / 1 : 탐험가 / 2 : 일뀬 / 3 : 농부 / 4 : 연구원
 //taketime ==4
	if (h == null)
	{
		h = MainCamera.mainCamera.GetComponent<Event>();
		Debug.Log(h.gameObject.name);
	}
	takeTime -= 4;
	if (takeTime < 0)
		takeTime = 0;
	//Debug.Log("work");
	int bonus = (int)(jobLevel[job] * 0.5f);

	if (job == 0)
	{
		//맵 배회
		//behave = 0;
	}
	else if (job == 1)
	{
		bonus += (int)(h.storage.getStorage(4) * 0.1f + 0.5f);
		//탐험. 가끔 생존자 발견
		if (Random.Range(0, 100) > (80 - bonus))
		{
			h.storage.addStorage(0, 1);
			h.spawnPeople();
			h.plusItem.people += 1;
		}
		h.storage.addStorage(4, 1 + bonus);
		h.plusItem.mapExp += 2;
	}
	else if (job == 2)
	{
		//식량 가저오기
		bonus += (int)(h.storage.getStorage(4) * 0.1f + 0.5f);
		int mount = Math.random(1, bonus + 1);
		h.plusItem.food += mount;
		h.storage.addStorage(1, mount);
	}
	else if (job == 3)
	{
		//식량 추가
		int mount = Math.random(1, bonus + 1);
		h.storage.addStorage(1, mount);
		h.plusItem.food += mount;
	}
	else if (job == 4)
	{
		//연구 포인트 2 추가
		int mount = Math.random(bonus, bonus + 2);
		h.storage.addStorage(3, mount);
		h.plusItem.labExp += mount;
	}

	jobExp += 2;
	if (jobExp > jobLevel[job] * 5)
	{
		jobExp -= (jobLevel[job] - 1) * 5;
		jobLevel[job]++;
	}
}

//직업 변경
public void jobUpdate(int newjob)
{
	if (newjob == job)//새 직업과 다를 때
		return;

	job = newjob;
	jobExp = 0;//경험치 초기화
}