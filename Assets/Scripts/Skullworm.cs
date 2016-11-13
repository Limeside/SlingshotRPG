using UnityEngine;
using System.Collections;

public class Skullworm : MonoBehaviour
{
    [SerializeField] public GameObject _head = null;

    void isDead() {
        Vector3 headTransform = transform.position + new Vector3(-0.68f, 0, 0);
        GameObject Head = (GameObject)Instantiate(_head, headTransform, transform.rotation);
        Destroy(this.gameObject);
    }

}
