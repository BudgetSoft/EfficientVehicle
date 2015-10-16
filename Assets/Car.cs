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
		endRoad = gameManager.StartRoad ();
		endMarker = endRoad.WaypointPos ();
		endRoad.filled = true;
		StartCoroutine(RUNLerp(startMarker,endMarker));
	}
	void Update() {

	}

	IEnumerator RUNLerp (Vector3 start,Vector3 end) {
		startTime = Time.time;
		journeyLength = Vector3.Distance(start, end);
		while (Vector3.Distance (gameObject.transform.position, end) >= 0.01f) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (start, end, fracJourney);
			yield return null;
		}
		endRoad.filled = false;
		startMarker = endMarker;
		endMarker = endRoad.adjRoads [0, 1].WaypointPos();
		endRoad = endRoad.adjRoads [0, 1];
		if (endRoad.adjRoads [0, 1] == null) {
			Destroy (gameObject);
		}
		while (endRoad.filled) {
			yield return null;
		}
		StartCoroutine (RUNLerp (startMarker, endMarker));
	}
}