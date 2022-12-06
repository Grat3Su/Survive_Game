
//Spawn
public void spawnPeople()
{
	int people = storage.getStorage(0);
	for (int i = people - 1; i > -1; i--)
	{
		if (pState[i].name == "null")
		{
			int newjob = Math.random(0, 5);
			pState[i].jobUpdate(newjob);
			pState[i].behave = 0;
			curp++;
			string[] n = { "가", "나", "다", "라", "마", "바", "사", "아", "자", "차", "카", "타", "파", "하", "야", "샤", "수", "경", "재", "문" };
			string name = n[Math.random(0, n.Length)] + n[Math.random(0, n.Length)];
			pState[i].name = name;
			pState[i].pos = new iPoint(MainCamera.devWidth - 250, MainCamera.devHeight - 130);
		}
		else
			break;
	}
}

//Delete
void deletePeople()
{
	int people = storage.getStorage(0);

	if (people < 0)
		people = 0;

	for (int i = people; i < 100; i++)
	{
		if (pState[i].name == "null")
			return;
		pState[i].behave = 0;
		pState[i].name = "null";
		pState[i].pos = new iPoint(MainCamera.devWidth - 250, MainCamera.devHeight - 130);
	}
}

//Event
public void doEvent(DoEvent type)
{
	//for (int i = 0; i < storage.people; i++)
	//    pState[i].behave = 1;
	string[] etype = new string[] { "탐험", "사냥", "연구" };
	AddItem item = new AddItem(0);
	if (type == DoEvent.Adventure)
	{
		//item.food = Math.random(0, 2);
		item.people = Math.random(0, 100) > 50 ? Math.random(1, 2) : 0;
		item.takeTime = 4;

		item.mapExp += 4;
	}
	else if (type == DoEvent.Hunt)
	{
		item.food = Math.random(1, 2);
		//item.people = Math.random(0, 100) > 50 ? Math.random(1, 2) : 0;
		item.takeTime = 4;
	}
	else if (type == DoEvent.Research)
	{
		int labLevel = storage.getStorage(4);
		item.labExp = labLevel < 5 ? Math.random(1, 3) : Math.random(2, 5);
		item.takeTime = 4;
	}
	else if (type == DoEvent.SkipDay)
	{
		skipDay();
		Debug.Log("skipday");
		return;
	}
	Debug.Log(etype[(int)type]);

	updateEvent(item);
}

public struct AddItem
{
	public int people;
	public int food;
	public int takeTime;
	public int labExp;
	public int mapExp;

	public AddItem(int init)
	{
		people = init;
		food = init;
		takeTime = init;
		labExp = init;
		mapExp = init;
	}
	public void init()
	{
		people = 0;
		food = 0;
		takeTime = 0;
		labExp = 0;
		mapExp = 0;
	}
}