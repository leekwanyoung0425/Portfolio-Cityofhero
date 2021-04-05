using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public LayerMask CrashMask;
    public float RotSpeed = 180.0f;
    public float ZoomSpeed = 10.0f;

    public Vector2 LookUpArea = new Vector2(-60f, 80f);
    public Vector2 ZoomArea = new Vector2(1f, 10f);
    float Dist = 5f;
    float TargetDist = 0.0f;
    public Transform trCamera;
    public float CameraCollisionOffset = 1.0f;

    public float ZoomSmoothSpeed = 10.0f;


    Vector3 LookUp;
    Vector3 TurnRight;

    // Start is called before the first frame update
    void Start()
    {
        TargetDist = Dist = Mathf.Abs(trCamera.localPosition.z);
        LookUp = this.transform.localRotation.eulerAngles;
        TurnRight = this.transform.parent.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            //LookUp
            float delta = -Input.GetAxis("Mouse Y") * RotSpeed * Time.deltaTime;
            LookUp.x += delta;
            if (LookUp.x > 180f) LookUp.x -= 360.0f;
            LookUp.x = Mathf.Clamp(LookUp.x, LookUpArea.x, LookUpArea.y);

            //TurnRight
            delta = Input.GetAxis("Mouse X") * RotSpeed * Time.deltaTime;
            TurnRight.y += delta;


            this.transform.parent.localRotation = Quaternion.Euler(TurnRight);
            this.transform.localRotation = Quaternion.Euler(LookUp);
  
        }

        if (Input.GetAxis("Mouse ScrollWheel") > Mathf.Epsilon ||
                Input.GetAxis("Mouse ScrollWheel") < -Mathf.Epsilon)
        {
            float delta = -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime;
            TargetDist = Mathf.Clamp(TargetDist + delta, ZoomArea.x, ZoomArea.y);
        }

        //TargetDist값이 Dist값으로 보간되면서 바뀐다.
        Dist = Mathf.Lerp(Dist, TargetDist, Time.smoothDeltaTime * ZoomSmoothSpeed);

        //화면 이동시 충돌체가 있으면 그 앞으로 카메라를 이동
        Ray ray = new Ray();
        ray.origin = this.transform.position;
        ray.direction = -this.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Dist + CameraCollisionOffset, CrashMask))
        {
            Dist = Vector3.Distance(hit.point - ray.direction * CameraCollisionOffset, this.transform.position);
        }

        trCamera.localPosition = new Vector3(0, 0, -Dist);
    }
}
