using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Continue : MonoBehaviour {

	public Image blackImage;
	private Color blackImageColor;

	private float pushedTime = 0f;
	private Vector3 origPos;

	void Start () {
		origPos = transform.position;
		blackImageColor = blackImage.color;
	}

	void Update () {

		if (pushedTime > 0f) {
			float t = Time.time - pushedTime;
			if (t < 0.5f) {
				blackImageColor.a = 0f;
			} else if (t >= 0.5f && t <= 1.5f) {
				blackImageColor.a = t - 0.5f;
			} else if (t > 1.5f) {
				blackImageColor.a = 1f;
				pushedTime = 0f;
				SceneManager.LoadScene ("Main");
			}
			blackImage.color = blackImageColor;
		}
	}

	public void Push () {
		if (pushedTime == 0f) {
			pushedTime = Time.time;
			Vector3 newPos = new Vector3 (origPos.x, origPos.y - 0.1f, origPos.z);
			iTween.MoveTo (gameObject, iTween.Hash ("position", newPos, "oncomplete", "PushAnimCompleted", "time", 0.5f));
		}
	}

	public void PushAnimCompleted () {
		iTween.MoveTo (gameObject, iTween.Hash ("position", origPos, "time", 0.5f));
	}
}