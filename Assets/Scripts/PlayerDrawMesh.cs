using UnityEngine;
using System.Collections;

public class PlayerDrawMesh : MonoBehaviour {

    [SerializeField] [Range(0, 360)] float aimAngle = 0;
    [SerializeField] float meshResolution = 1.0f;
    [SerializeField] public Transform Arm;

    public MeshFilter _rangeMFilter;
    public MeshFilter _powerMFilter;
    public Mesh _rangeMesh;
    public Mesh _powerMesh;

    private PlayerController _Controller;

    void Awake () {
        // -------------------------- Mesh Setting -------------------------- //
        _rangeMesh = new Mesh();
        _powerMesh = new Mesh();
        _rangeMFilter.mesh = _rangeMesh;
        _powerMFilter.mesh = _powerMesh;
        // ------------------------------------------------------------------ //

        _Controller = GetComponent<PlayerController>();
	}
	

    public void AimRangeDraw(float degree, float aimCharged){

        int stepCount = Mathf.RoundToInt(aimAngle * meshResolution);
        float stepAngleSize = aimAngle / stepCount;

        int aimVertexCount = stepCount + 1;
        Vector3[] vertices = new Vector3[aimVertexCount];
        int[] triangles = new int[(aimVertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < aimVertexCount - 1; i++){
            float angle = degree - (aimAngle / 2) + (stepAngleSize * i);

            vertices[i + 1] = Arm.transform.InverseTransformPoint(Arm.transform.position + DirToAngle(angle) * aimCharged);

            if (i < aimVertexCount - 2){
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
            }
        }

        _rangeMesh.Clear();
        _rangeMesh.vertices = vertices;
        _rangeMesh.triangles = triangles;
        _rangeMesh.RecalculateNormals();
    }

    public void AimPowerDraw(float degree, float powerCharged){

        int stepCount = Mathf.RoundToInt(360 * meshResolution);
        float stepAngleSize = stepCount / 360;

        int aimVertexCount = stepCount + 1;
        Vector3[] vertices = new Vector3[aimVertexCount];
        int[] triangles = new int[(aimVertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < aimVertexCount - 1; i++){
            float angle = degree - (180) + (stepAngleSize * i);

            vertices[i + 1] = Arm.transform.InverseTransformPoint(Arm.transform.position + DirToAngle(angle) * powerCharged);

            if (i < aimVertexCount - 2){
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
            }
        }

        _powerMesh.Clear();
        _powerMesh.vertices = vertices;
        _powerMesh.triangles = triangles;
        _powerMesh.RecalculateNormals();
    }


    public Vector3 DirToAngle(float degree){
        return new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad), 0);
    }

}
