using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public Image damageImage;
	private Color damageImageColor;
	private float damagedTime = 0f;

	public Image gameOverImage;
	public TextMesh gameOverText;
	private Color gameOverColor;
	private Color gameOverTextColor;
	private float gameoverTime = 0f;
	public bool isGameover = false;

	public GameObject cont;

	void Start () {
		damageImageColor = damageImage.color;
		ShowHP ();

		gameOverColor = gameOverImage.color;
		gameOverTextColor = gameOverText.color;

		cont.SetActive (false);
	}

	void Update () {

		if (damagedTime > 0f) {
			float t = Time.time - damagedTime;
			if (t <= 1f) {
				damageImageColor.a = 1f - t;
			} else {
				damagedTime = 0f;
				damageImageColor.a = 0f;
			}
			damageImage.color = damageImageColor;
		}

		if (gameoverTime > 0f) {
			float t = Time.time - gameoverTime;
			if (t <= 2f) {
				gameOverColor.a = t / 2f;
			} else if (t > 2f && t <= 3) {
				isGameover = true;
				cont.SetActive (true);
				gameOverColor.a = 3f - t;
				gameOverTextColor.a = (t - 2f);
			} else {
				gameOverColor.a = 0f;
				gameOverTextColor.a = 1f;
			}
			gameOverImage.color = gameOverColor;
			gameOverText.color = gameOverTextColor;
		}
	}

	public void Damage () {
		if (damagedTime == 0f) {
			damagedTime = Time.time;
			HP -= 0.5f;
			if (HP <= 0 && gameoverTime == 0f) {
				gameoverTime = Time.time;
			}
			ShowHP ();
		}
	}

	public GameObject lifePartsPrefab;
	public GameObject life;
	public float HP = 3;
	private void ShowHP () {

		if (life.transform.childCount == 0) {
			for (int i = 0; i < HP; i++) {
				GameObject lifeParts = Instantiate (lifePartsPrefab);
				lifeParts.transform.parent = life.transform;
				lifeParts.transform.localPosition = new Vector3 (i * 0.15f, 0, 0);
				lifeParts.transform.name = (i + 1).ToString ();
			}
		}

		for (int i = 0; i < life.transform.childCount; i++) {
			GameObject obj = life.transform.GetChild (i).gameObject;
			float l = float.Parse (obj.name);
			SpriteRenderer sprite = obj.GetComponent<SpriteRenderer> ();
			if (l <= HP) {
				sprite.color = Color.red;
			} else {
				float col = HP - (l - 1);
				if (col < 0)
					col = 0;
				sprite.color = new Color (col, 0, 0);
			}
		}
	}
}