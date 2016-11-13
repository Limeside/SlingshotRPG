using UnityEngine;
using System.Collections;
using System;

public class Wisp : MonoBehaviour {

    [SerializeField] public int hp = 0;

    void Awake() {
        _animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Bullet") {
            Animator p_Animator = other.GetComponent<Animator>();
            Rigidbody2D p_rigidBody = other.GetComponent<Rigidbody2D>();
            other.gameObject.tag = "Untagged";

            p_Animator.Play("Projectile_Hit");

            p_rigidBody.gravityScale = 0;
            p_rigidBody.velocity = new Vector3(0, 0, 0);

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

    private void Dead() {
        transform.parent.GetComponent<WispGroup>().wispCount--;
        transform.parent.GetComponent<WispGroup>().wispGroup.Remove(this.gameObject);
        transform.parent.GetComponent<WispGroup>().delay = 0;


        Destroy(gameObject);
    }

    private Animator _animator = null;

}
