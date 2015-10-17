using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour {

	public Road[,] adjRoads = new Road[3,3];
	public GameObject waypoint;
	public bool filled;
	public float speedOfCurrentCar;
	Renderer mRend;
	// Use this for initialization
	void Start () {
		mRend = GetComponentInChildren<Renderer> ();
		waypoint = transform.FindChild ("Waypoint").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (speedOfCurrentCar != 0) {
			mRend.material.color = Color.red;
		} else if (speedOfCurrentCar == 0) {
			mRend.material.color = Color.grey;
		}
		//if (adjRoads [0, 1] != null) {
			//Debug.DrawLine (WaypointPos (), adjRoads [0, 1].WaypointPos (),Color.red);
		//}
	}

	public void Init (GameObject midL,GameObject midR) {
		
		adjRoads [1, 0] = midL.GetComponent<Road>();
		adjRoads [1, 1] = this;
		adjRoads [1, 2] = midR.GetComponent<Road>();
	}
	public void UpdateLinks (GameObject topL,GameObject topC,GameObject topR){
		adjRoads [0, 0] = topL.GetComponent<Road>();
		adjRoads [0, 1] = topC.GetComponent<Road>();
		adjRoads [0, 2] = topR.GetComponent<Road>();
	}
	public Vector3 WaypointPos (){
		return waypoint.transform.position;
	}
	public void SetFilled (float speed){
		filled = true;
		speedOfCurrentCar = speed;
	}
	public float CheckFilled () {
		return speedOfCurrentCar;
	}
	public void SetNotFilled () {
		filled = false;
		speedOfCurrentCar = 0;
	}
}
