using UnityEngine;
using System.Collections;
using System;

public class Wisp : MonoBehaviour {

    [SerializeField] public int hp = 0;
    [SerializeField] public float spd = 1f;

    void Awake() {
        _animator = GetComponent<Animator>();
        _pos = transform.position;
    }

    void Update() {
        transform.position = _pos + EightCurve();
    }

    IEnumerator AngleProcess() {
        while (!onDead) {
            if (angle < 360)
                angle += spd * Time.deltaTime;
            else
                angle = 0f;

            yield return new WaitForEndOfFrame();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Bullet") {

            /*
            Animator p_Animator = other.GetComponent<Animator>();
            Rigidbody2D p_rigidBody = other.GetComponent<Rigidbody2D>();
            other.gameObject.tag = "Untagged";

            p_rigidBody.gravityScale = 0;
            p_rigidBody.velocity = new Vector3(0, 0, 0);
            */

            Destroy(other.gameObject);

            StopCoroutine("AngleProcess");

            if (hp > 1) {
                hp--;
                _animator.Play("Damage");
            }
            else if (hp == 1) {
                hp = 0;
                _animator.SetBool("isDead", true);
            }
        }
    }

    private void Gen() {
        StartCoroutine("AngleProcess");
    }

    private void Dead() {
        Destroy(gameObject);
    }

    Vector3 EightCurve() {

        float _x = 1 * Mathf.Cos(angle * Mathf.Deg2Rad);
        float _y = 1 * Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Cos(angle * Mathf.Deg2Rad);

        return new Vector3(_x, _y, 0);
    }

    private Animator _animator = null;
    private Vector3 _pos;

    private bool onDead = false;
    [SerializeField] private float angle = 1f;
}
