using UnityEngine;
using System.Collections;

public class PlayerDrawTrajectory : MonoBehaviour
{
    [SerializeField] public float timeResolution = 0.02f;
    [SerializeField] public float maxTime = 1.0f;
    [SerializeField] public LayerMask layerMask = -1;

    [SerializeField] public Transform _Arm = null;

    void Start() {
        _renderer = GetComponentInChildren<LineRenderer>();
    }

    public void DrawTrajectory(float velocity) {
        int index = 0;
        Vector2 velocityVector = -_Arm.transform.right * velocity;
        Vector2 currentPos = _Arm.transform.position;

        _renderer.SetVertexCount( (int)(maxTime / timeResolution) );

        for (float t = 0.0f; t < maxTime; t += timeResolution) {
            _renderer.SetPosition(index, currentPos);
            RaycastHit2D hit = Physics2D.Raycast(currentPos, velocityVector, velocityVector.magnitude * timeResolution, layerMask);

            if (hit.collider) {
                _renderer.SetVertexCount(index + 2);
                _renderer.SetPosition(index + 1, hit.point);

                break;
            }

            currentPos += velocityVector * timeResolution;
            velocityVector += Physics2D.gravity * 1.85f * timeResolution;

            index++;
        }
    }
    /*
    public void setTrajectoryPoints(Vector3 _pos, Vector3 vel)
    {
        float velocity = Mathf.Sqrt((vel.x * vel.x) + (vel.y * vel.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(vel.y, vel.x));
        float fTime = 0;

        fTime += 0.1f;
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics2D.gravity.magnitude * fTime * fTime / 2.0f);

            Vector3 pos = new Vector3(_pos.x + dx, _pos.y + dy, 2);

            trajectoryPoints[i].transform.position = pos;
            trajectoryPoints[i].GetComponent<Renderer>().enabled = true;

            trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(vel.y - (Physics.gravity.magnitude) * fTime, vel.x) * Mathf.Rad2Deg);

            fTime += 0.1f;
        }
    }
    */
    private LineRenderer _renderer;
}
