using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour {

    public bool isAutoMove = true;

    PlayerController controller;

    [Header("Path Moving")]
    public PathManager pathContains;

    [Header("Camera Rotation")]
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    bool clampVerticalRotation = true;
    public float MinimumX = -60F;
    public float MaximumX = 60F;

    Quaternion m_CharacterTargetRot;
    Quaternion m_CameraTargetRot;

    void Start()
    {
        controller = GetComponent<PlayerController>();


        if (!isAutoMove)
        {
            m_CharacterTargetRot = transform.localRotation;
            m_CameraTargetRot = Camera.main.transform.localRotation;
            StartCoroutine("unAtMoveInput");
        }
        else
        {
            if (pathContains != null)
            {
                controller.AutoMoving(pathContains);
                StopCoroutine("unAtMoveInput");
            }
            
        }
    }



    IEnumerator unAtMoveInput()
    {
        
        while (true)
        {
            MoveInput();
            RotateInput();
            yield return new WaitForSeconds(0);
        }
    }

    void MoveInput()
    {
        
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {
                controller.MoveNavMesh(hit.point);

            }
        }
    }

    void RotateInput()
    {

        if (Input.GetMouseButton(1))
        {

            float yRot = Input.GetAxisRaw("Mouse X") * XSensitivity;
            float xRot = Input.GetAxisRaw("Mouse Y") * YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            controller.RotateView(m_CharacterTargetRot, m_CameraTargetRot);


        }
    }
    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
