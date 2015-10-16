using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {
	public Vector3 startMarker;
	public Vector3 endMarker;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;
	public GameManager gameManager;
	Road endRoad;
	void Start() {
		startMarker = gameObject.transform.position;
		endMarker = gameManager.startRoad.WaypointPos();
		endRoad = gameManager.startRoad;
		StartCoroutine(RUNLerp(startMarker,endMarker));
	}
	void Update() {

	}

	IEnumerator RUNLerp (Vector3 start,Vector3 end) {
		startTime = Time.time;
		journeyLength = Vector3.Distance(start, end);
		while (Vector3.Distance (gameObject.transform.position, end) >= 0.1f) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (start, end, fracJourney);
			yield return null;
		}
			StartLerp ();
	}

	void StartLerp () {
		startMarker = endMarker;
		endMarker = endRoad.adjRoads [0, 1].WaypointPos();
		Debug.Log (startMarker + " to " + endMarker);
		StartCoroutine (RUNLerp (startMarker, endMarker));
	}
}