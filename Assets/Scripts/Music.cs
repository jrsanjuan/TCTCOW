using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	void Start() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
