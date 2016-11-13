using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    [SerializeField] float chargeMax = 9.0f;
    [SerializeField] public float charged = 0.0f;
    [SerializeField] float chargeSpd = 0.005f;

    [SerializeField] public Transform Arm;

    [SerializeField] GameObject dot;
    [SerializeField] public int points = 12;

    [SerializeField] GameObject[] projectile;
    [SerializeField] AudioSource[] sounds;

    void Awake () {
        _Anim = GetComponent<Animator>();

        _RendererArm = GameObject.Find("Aim").GetComponent<SpriteRenderer>();
        _RendererHead = GameObject.Find("Head").GetComponent<SpriteRenderer>();

        _Arm = _RendererArm.sprite; // save "Player_Arm.png" to _Arm
        _RendererArm.sprite = null;
        _Head = _RendererHead.sprite; // save "Player_Head.png" to _Arm
        _RendererHead.sprite = null;

        pointList = new List<GameObject>();

        for (int i = 0; i < points; i++)
        {
            Transform _child = GetComponentInChildren<Transform>();
            GameObject _dot = (GameObject)Instantiate(dot, _child.position, _child.rotation);
            _dot.GetComponent<Renderer>().enabled = false;
            pointList.Insert(i, _dot);
        }

    }

    void TakeAim()
    {
        Vector3 vel = GetForceFrom(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;

        Arm.transform.rotation = Quaternion.Euler( new Vector3(0, 0, Mathf.Clamp(angle, -5, 30)) );

        DrawTrajectoryPoint(transform.position, vel);
    }

    void Update()
    {
        if ( Input.GetMouseButtonDown(0) && _onMouse == false ){
            _onMouse = true;

            _Anim.SetBool("Aim", true);

            sounds[0].Play();

            StartCoroutine("AimCharging");

        }
        else if ( Input.GetMouseButtonUp(0) && _onMouse == true && charged > 0.5f ) {
            _onMouse = false;

            _Anim.SetBool("Aim", false);

            sounds[0].Stop();

            sounds[1].pitch = (charged * 0.35f) + 0.8f;
            sounds[1].Play();


            StopCoroutine("AimCharging");

            //----------------------------//

            GameObject Bullet = (GameObject)Instantiate(projectile[0], transform.position, transform.rotation);
            Bullet.GetComponent<Rigidbody2D>().AddForce( GetForceFrom(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)), ForceMode2D.Impulse );

            charged = 0;

            for (int i = 0; i < points; i++)
            {
                pointList[i].GetComponent<Renderer>().enabled = false;
            }

            //----------------------------//
        }

        if (_onMouse)
            TakeAim();
    }

    public IEnumerator AimCharging() {
        charged = 0.2f;
        while (charged < chargeMax) {
            charged += chargeSpd;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    void DrawTrajectoryPoint(Vector3 _pos, Vector3 _vel)
    {
        float velocity = Mathf.Sqrt((_vel.x * _vel.x) + (_vel.y * _vel.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(_vel.y, _vel.x));
        float fTime = 0.05f;

        for (int i = 0; i < points; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);

            Vector3 pos = new Vector3(_pos.x + dx, _pos.y + dy, 2);

            pointList[i].transform.position = pos;
            pointList[i].GetComponent<Renderer>().enabled = true;

            pointList[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(_vel.y - (Physics.gravity.magnitude) * fTime, _vel.x) * Mathf.Rad2Deg);
            pointList[i].transform.localScale = new Vector3(1.5f - (0.15f * i), 1.5f - (0.15f * i), 1);

            fTime += 0.05f;
        }
    }

    Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * charged; // * ball.rigidbody.mass;
    }

    public void OnDrawArm() {
        _RendererArm.sprite = _Arm;
        _RendererHead.sprite = _Head;
    }

    public void OffDrawArm() {
        _RendererArm.sprite = null;
        _RendererHead.sprite = null;
    }

    private bool _onMouse;

    private Animator _Anim;
    private Sprite _Arm;
    private Sprite _Head;
    private SpriteRenderer _RendererArm;
    private SpriteRenderer _RendererHead;

    private AudioSource _AudioBowDraw;

    private List<GameObject> pointList;



}
