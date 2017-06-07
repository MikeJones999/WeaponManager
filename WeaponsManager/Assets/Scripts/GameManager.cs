using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public Dictionary<int, Vector3> FlightWayPoints { get; set; }
	private List<Transform> AllGameObjects;

	 void Awake()
	{
		if(instance == null)
		{
			instance = new GameManager();
			AllGameObjects = new List<Transform>();
		}
		else
		{
			Destroy(this);
		}
	}

	public void AddGameTransform(Transform gameObj)
	{
		if(AllGameObjects == null)
		{
			AllGameObjects = new List<Transform>();
		}

		AllGameObjects.Add(gameObj);
	}


	public GameObject ReturnGameObject(string name)
	{
		if(!string.IsNullOrEmpty(name))
		{
			if(AllGameObjects != null)
			{

				Transform temp = AllGameObjects.Where(x => x.gameObject.name == name).FirstOrDefault();
				if (temp != null)
				{
					return temp.gameObject;
				}
			}
		}
		else
		{
			Debug.Log("No name provided to look up game object in game manager");
		}
		return null;

	}

	
}
