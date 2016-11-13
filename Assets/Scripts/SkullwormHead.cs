using UnityEngine;
using System.Collections;

public class SkullwormHead : MonoBehaviour {

	void Start () {
        Invoke("OnDestroy", 5.0f);
	}
	
	void OnDestroy () {
        Destroy(this.gameObject);
	}

}
