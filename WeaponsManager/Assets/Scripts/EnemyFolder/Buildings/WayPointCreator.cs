﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointCreator : MonoBehaviour
{

	public Transform[] waypoints;
	protected Transform centerObject;
	private float height;
	protected Dictionary<int, Vector3> WaypointDict;
	protected Vector3 centrePos;


	// Use this for initialization
	void Start () {

		centerObject = transform;
		centrePos = transform.position;
		height = centerObject.lossyScale.y  + 10;
		WaypointDict = new Dictionary<int, Vector3>();
		PopulateWayPoints();
		createVisualWayPoints();
		//Pass game manager the flight waypoints for the bird to follow
		GameManager.instance.FlightWayPoints = WaypointDict;
		//inform camera manager where center point is
		CameraManager.instance.setCentrePointLoc(centrePos);
	}
	
	public Dictionary<int,Vector3> ReturnWaypointsDictionary()
	{
		if(waypoints != null)
		{
			return WaypointDict;
		}
		return new Dictionary<int, Vector3>();
	}

	//Create the necessary waypoints around the centre point - this should change to  standard height rather that increment on teh objetc itself
	void PopulateWayPoints()
	{
		WaypointDict.Add(1, new Vector3(centrePos.x - 10, height, centrePos.z - 20));
		WaypointDict.Add(2, new Vector3(centrePos.x +10, height, centrePos.z - 20));
		WaypointDict.Add(3, new Vector3(centrePos.x +20, height, centrePos.z - 10));
		WaypointDict.Add(4, new Vector3(centrePos.x +20, height, centrePos.z +10));
		WaypointDict.Add(5, new Vector3(centrePos.x  +10, height, centrePos.z +20));
		WaypointDict.Add(6, new Vector3(centrePos.x - 10, height, centrePos.z +20));
	}

	public void createVisualWayPoints()
	{
		foreach(KeyValuePair<int, Vector3> wayPoint in WaypointDict)
		{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.transform.position = wayPoint.Value;
		}
	}
}
