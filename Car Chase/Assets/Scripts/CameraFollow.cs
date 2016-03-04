using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public GameObject target;
	public float xOffset = 0;
	public float yOffset = 0;
	public float zOffset = 0;
	
	void LateUpdate() {
	this.transform.position = new Vector3(
		Mathf.Clamp(target.transform.position.x + xOffset, -114, 113),
		target.transform.position.y + yOffset,
		Mathf.Clamp(target.transform.position.z + zOffset, -198, 197));
	}
}
