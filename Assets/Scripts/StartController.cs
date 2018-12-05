using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartController : MonoBehaviour {

	public Image startImage;
	private Color gameOverColor;
	private Color gameOverTextColor;
	private bool isGameover = true;

	public GameObject cont;

	// Use this for initialization
	void Start () {
		gameOverColor = startImage.color;

		cont.SetActive (true);
	}
}