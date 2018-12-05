using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour {

	public PlayerController player;
	public Gate gate;
	public float speed;

	public int HP;
	private bool damagingInvalid = false;
	private bool damagedPlayer = false;
	public ParticleSystem particle;

	private Vector3 nextDirection = Vector3.zero;
	private float nextDistance;
	private Vector3 startPoint;
	private Animator anim;

	void Start () {
		anim = GetComponent<Animator> ();
		ChangeState ("Appear");
	}

	void Update () {
		if (player.isGameover) {
			gate.Remove (gameObject);
		}
		// If animation is changing, do nothing.
		if (IsStateChanging ())
			return;

		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);

		if (info.IsName ("Appear")) {
			float t = info.normalizedTime;
			if (t >= 1.0f) {
				ChangeState ("Idle");
			} else {
				transform.position = new Vector3 (transform.position.x, -1.4f + t * 1.6f, transform.position.z);
			}
		} else if (info.IsName ("Idle")) {
			damagingInvalid = false;
			damagedPlayer = false;

			if (player != null) {
				if (nextDirection == Vector3.zero) {
					float dist = Vector3.Distance (player.transform.position, transform.position);

					// Get next direction with random.
					float randAngle = dist < 3.0f ? 0f : (Random.value - 0.5f) * dist * 8;
					nextDirection = Quaternion.LookRotation (player.transform.position - transform.position).eulerAngles;
					nextDirection.y += randAngle;
					if (nextDirection.y < 0) {
						nextDirection.y += 360;
					}
					nextDirection.x = 0;
					nextDirection.z = 0;

					// How much move and startPoint
					nextDistance = dist < 3.0f ? dist : dist / 4 + Random.value * (dist / 4);
					startPoint = transform.position;

				} else {
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (nextDirection), Time.deltaTime);
					if (Mathf.Abs (Mathf.DeltaAngle (transform.rotation.eulerAngles.y, nextDirection.y)) <= 1.0f) {
						ChangeState ("Walk");
					}
				}
			}
		} else if (info.IsName ("Walk")) {
			float playerDist = Vector3.Distance (player.transform.position, transform.position);

			if (playerDist <= 2) {
				ChangeState ("Attack");
			} else {
				// Walk to the direction he faces
				transform.position += transform.TransformDirection (Vector3.forward) * Time.deltaTime * speed;
				float dist = Vector3.Distance (startPoint, transform.position);
				if (dist >= nextDistance) {
					nextDirection = Vector3.zero;
					ChangeState ("Idle");
				}
			}
		} else if (info.IsName ("Attack")) {
			// Calculate time from attack animation start
			float t = info.normalizedTime;
			if (t > 0.35f && !damagedPlayer) {
				damagedPlayer = true;
				player.Damage ();
			}
		} else if (info.IsName ("Damage")) {
			float t = info.normalizedTime;
			if (HP <= 0f && t > 0.5f) {
				ChangeState ("Dead");
			} else {
				transform.position += transform.TransformDirection (Vector3.back) * Time.deltaTime;
			}
		} else if (info.IsName ("Dead")) {
			float t = info.normalizedTime;
			if (t >= 1f) {
				if (gate != null) {
					gate.Remove (gameObject);
				}
				GameObject.Destroy (gameObject);
			}
		}
	}

	private string changingStateName = null;
	private void ChangeState (string name) {
		changingStateName = name;
		anim.SetTrigger (name);
	}

	private bool IsStateChanging () {
		AnimatorStateInfo next = anim.GetNextAnimatorStateInfo (0);
		bool ret = changingStateName != null && next.IsName (changingStateName);
		if (changingStateName != null && !ret) {
			changingStateName = null;
		}
		return ret;
	}

	public void Damage () {
		if (!damagingInvalid) {
			HP -= 1;
			damagingInvalid = true;
			ChangeState ("Damage");
			particle.Play ();
		}
	}
}