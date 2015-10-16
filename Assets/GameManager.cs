using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public GameObject roadPrefab,carPrefab;
	GameObject lastL,lastC,lastR;
	public Road startRoadL,startRoadC,startRoadR;
	// Use this for initialization
	void Start () {
		lastL = (GameObject)Instantiate (roadPrefab, new Vector3 (0, 0, -1), Quaternion.identity);
		lastC = (GameObject)Instantiate (roadPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
		lastR = (GameObject)Instantiate (roadPrefab, new Vector3 (0, 0, 1), Quaternion.identity);

		lastL.GetComponent<Road>().Init (lastL,lastC);
		lastC.GetComponent<Road>().Init (lastL,lastR);
		lastR.GetComponent<Road>().Init (lastC,lastR);

		startRoadL = lastL.GetComponent<Road>();
		startRoadC = lastC.GetComponent<Road>();
		startRoadR = lastR.GetComponent<Road>();

		for (int i = 0; i < 100; i++)
		{
			CreateRoad();
		}
		InvokeRepeating ("SpawnCar", 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void CreateRoad () {
		GameObject roadL = (GameObject)Instantiate (roadPrefab, lastL.transform.position + Vector3.right, Quaternion.identity);
		GameObject roadC = (GameObject)Instantiate (roadPrefab, lastC.transform.position + Vector3.right, Quaternion.identity);
		GameObject roadR = (GameObject)Instantiate (roadPrefab, lastR.transform.position + Vector3.right, Quaternion.identity);

		roadL.GetComponent<Road> ().Init (roadL, roadC);
		roadC.GetComponent<Road> ().Init (roadL, roadR);
		roadR.GetComponent<Road> ().Init (roadC, roadR);

		lastL.GetComponent<Road> ().UpdateLinks (lastL, roadL, roadC);
		lastC.GetComponent<Road> ().UpdateLinks (roadL, roadC, roadR);
		lastR.GetComponent<Road> ().UpdateLinks (roadC, roadR, lastR);

		lastL = roadL;
		lastC = roadC;
		lastR = roadR;
	}

	public Road StartRoad () {
		int num = Random.Range (0, 3);
		if (num == 0)
			return startRoadL;
		if (num == 1)
			return startRoadC;
		else
			return startRoadR;
	}

	public void SpawnCar () {
		GameObject car = (GameObject) Instantiate (carPrefab, Vector3.up, Quaternion.identity);
		car.GetComponent<Car> ().gameManager = this;
	}
}
