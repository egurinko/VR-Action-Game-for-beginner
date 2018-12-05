using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

	public int max = 3;
	public float duration = 30f;
	public float initial = 0f;
	public float durationDelta = 3f;
	public float durationMin = 10f;

	public PlayerController player;
	public GameObject enemyPrefab;
	public ParticleSystem particleSystem;

	private float lastCreated;
	private List<GameObject> enemies = new List<GameObject> ();

	void Start () {
		lastCreated = initial - duration + Time.deltaTime;
	}

	void Update () {
		if (player.isGameover) {
			return;
		}
		if ((Time.time - lastCreated > duration) && enemies.Count < max) {
			lastCreated = Time.time;
			duration -= durationDelta;
			if (duration < durationMin) {
				duration = durationMin;
			}
			GameObject obj = GameObject.Instantiate (enemyPrefab);
			obj.transform.parent = transform;
			obj.transform.position = transform.position;
			SkeletonController skeleton = obj.GetComponent<SkeletonController> ();
			skeleton.gate = this;
			skeleton.player = player;

			particleSystem.Play ();
		}
	}

	public void Remove (GameObject obj) {
		enemies.Remove (obj);
		GameObject.Destroy (obj);
	}
}