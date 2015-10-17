using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {
	public Vector3 startMarker;
	public Vector3 endMarker;
	float normalSpeed;
	public float speed = 1f;
	private float startTime;
	private float journeyLength;
	public GameManager gameManager;

	public Texture redTexture,brownTexture, orangeTexture;
	Renderer rend;

	Road endRoad,previousRoad;
	void Start() {
		rend = GetComponentInChildren<Renderer> ();
		speed = Random.Range (2f, 6f) * speed;
		normalSpeed = speed;
		startMarker = gameObject.transform.position;
		endRoad = gameManager.StartRoad ();
		endMarker = endRoad.WaypointPos ();
		StartCoroutine(RUNLerp(startMarker,endMarker,true));

		if (speed <= 3) {
			rend.material.mainTexture = brownTexture;
		} if (speed >= 4) {
			rend.material.mainTexture = redTexture;
		}
	}

	void Update() {

	}

	IEnumerator RUNLerp (Vector3 start,Vector3 end, bool firstRoad) {
		endRoad.SetFilled (speed);
		startTime = Time.time;
		journeyLength = Vector3.Distance(start, end);
		while (Vector3.Distance (gameObject.transform.position, end) >= 0.01f) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (start, end, fracJourney);
			yield return null;
		}
		if (!firstRoad) {
			previousRoad.SetNotFilled();
		}
		startMarker = endMarker;
		endMarker = endRoad.adjRoads [0, 1].WaypointPos();
		previousRoad = endRoad;
		endRoad = endRoad.adjRoads [0, 1];
		if (endRoad.adjRoads [0, 1] == null) {
			endRoad.SetNotFilled();
			previousRoad.SetNotFilled();
			Destroy (gameObject);
		}
		else if (endRoad.CheckFilled () < 0) {
			speed = 0;
			previousRoad.SetFilled (-1);
			yield return null;
		}
		else if (endRoad.CheckFilled () != 0) {
			speed = endRoad.CheckFilled ();
			yield return StartCoroutine (ChangeLanes ());
		} else {
			StartCoroutine (RUNLerp (startMarker, endMarker, false));
		}
	}

	IEnumerator ChangeLanes(){
		if (endRoad.adjRoads [0, 0] != null && endRoad.adjRoads [0, 0].CheckFilled () == 0 && endRoad.adjRoads [1, 0].CheckFilled () == 0) {
			previousRoad.SetNotFilled ();
			endMarker = endRoad.adjRoads [0, 0].WaypointPos ();
			previousRoad = endRoad;
			endRoad = endRoad.adjRoads [0, 0];
			speed = normalSpeed;
			yield return StartCoroutine (RUNLerp (startMarker, endMarker, false));
		} else if (endRoad.adjRoads [0, 2] != null && endRoad.adjRoads [0, 2].CheckFilled () == 0 && endRoad.adjRoads [1, 2].CheckFilled () == 0) {
			previousRoad.SetNotFilled ();
			endMarker = endRoad.adjRoads [0, 2].WaypointPos ();
			previousRoad = endRoad;
			endRoad = endRoad.adjRoads [0, 2];
			speed = normalSpeed;
			yield return StartCoroutine (RUNLerp (startMarker, endMarker, false));
		} else {
			StartCoroutine (RUNLerp (startMarker, endMarker, false));
		}
	}
}