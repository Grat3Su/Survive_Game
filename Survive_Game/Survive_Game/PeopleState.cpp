//������ ����

void jobAction()//-> 
{// 0 : ��� / 1 : Ž�谡 / 2 : �υ� / 3 : ��� / 4 : ������
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
		//�� ��ȸ
		//behave = 0;
	}
	else if (job == 1)
	{
		bonus += (int)(h.storage.getStorage(4) * 0.1f + 0.5f);
		//Ž��. ���� ������ �߰�
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
		//�ķ� ��������
		bonus += (int)(h.storage.getStorage(4) * 0.1f + 0.5f);
		int mount = Math.random(1, bonus + 1);
		h.plusItem.food += mount;
		h.storage.addStorage(1, mount);
	}
	else if (job == 3)
	{
		//�ķ� �߰�
		int mount = Math.random(1, bonus + 1);
		h.storage.addStorage(1, mount);
		h.plusItem.food += mount;
	}
	else if (job == 4)
	{
		//���� ����Ʈ 2 �߰�
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

//���� ����
public void jobUpdate(int newjob)
{
	if (newjob == job)//�� ������ �ٸ� ��
		return;

	job = newjob;
	jobExp = 0;//����ġ �ʱ�ȭ
}