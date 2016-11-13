using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WispGroup : MonoBehaviour {

    [SerializeField] public float moveSpd = 1.0f;
    [SerializeField] public float radius = 1.0f;
    [SerializeField] public GameObject wispModel = null;

    public List<GameObject> wispGroup = new List<GameObject>();

    void Awake () {
        _pos = transform.position;
        StartCoroutine("CircularCoroutine");

        for ( int i = 0; i < 3; i++)
        {
            CreateWisp();
        }

    }
	
	void Update () {
        int i = 0;

        foreach (GameObject wisp in wispGroup) {
            i++;
            wisp.transform.position = DirToAngle( (360 / wispCount) * i + angle);
            wisp.SetActive(true);
        }

        if ( wispCount < 5 ) {
            if ( delay > 5 ) {
                delay = 0;
                wispCount++;

                CreateWisp();
            }
            else
                delay += Time.deltaTime;
        }

        if ( wispCount == 0) {
            wispGroup.Clear();
            Destroy(gameObject);
        }
            
	}

    private void CreateWisp () {
        GameObject _wisp = (GameObject)Instantiate(wispModel, transform.position, transform.rotation);
        _wisp.transform.parent = this.gameObject.transform;
        //_wisp.SetActive(false);

        wispGroup.Add(_wisp);
    }

    IEnumerator CircularCoroutine() {
        while (!isDead) {
            if (angle < 360)
                angle += moveSpd;
            else if (angle >= 360)
                angle = 0;
            yield return new WaitForEndOfFrame();
        }
    }

    public Vector3 DirToAngle(float degree) {
        return new Vector3(_pos.x + Mathf.Cos(degree * Mathf.Deg2Rad) * radius, _pos.y + Mathf.Sin(degree * Mathf.Deg2Rad) * radius, 0);
    }

    [SerializeField] private float angle = 0.0f;
    [SerializeField] public int wispCount = 0;
    [SerializeField] public float delay = 0;
    private Vector3 _pos = Vector3.zero;
    private bool isDead = false;
}
