using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour {

    void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();

        // _animator = GetComponent<Animator>();
        // _controller = GetComponent<PlayerController>();
    }

    void Update() {
        if (!isDropped){
            Vector3 vel = GetComponent<Rigidbody2D>().velocity;

            float angleZ = (Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg);
            float angleY = (Mathf.Atan2(vel.z, vel.x) * Mathf.Rad2Deg);

            transform.eulerAngles = new Vector3(0, -angleY, angleZ);
        }
    }

    void OnTriggerEnter2D(Collider2D other) { // isDropped
        if ( other.gameObject.tag == "Ground" && isDropped == false ) {
            isDropped = true;

            Destroy(_rigidBody);
            Destroy(_collider);

            Invoke("DeleteProjectile", 5.0f);
        }
    }

    void DeleteProjectile() {
        Destroy(this.gameObject);
    }

    private Rigidbody2D _rigidBody;
    private Collider2D _collider;
    // private Animator _animator;
    // private PlayerController _controller;
    private bool isDropped = false;


}
