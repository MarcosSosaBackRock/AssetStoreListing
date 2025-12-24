using UnityEngine;

namespace BackRock
{
    public class OrbitCamera : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;

        [Header("Distance")]
        public float distance = 4f;
        public float minDistance = 2f;
        public float maxDistance = 7f;
        public float zoomSpeed = 2f;

        [Header("Rotation")]
        public float xSpeed = 120f;
        public float ySpeed = 80f;
        public float yMinLimit = -20f;
        public float yMaxLimit = 80f;


        [Header("Auto Rotation")]
        public bool autoRotate = false;
        public float autoRotateSpeed = 20f;

        private float x = 0f;
        private float y = 20f;

        private void Start()
        {
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;
        }

        private void LateUpdate()
        {
            if (!target) return;

            if (!autoRotate)
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                HandleMouse();
#else
                HandleTouch();
#endif
            }
            else
            {
                x += autoRotateSpeed * Time.deltaTime;
            }


            Quaternion rotation = Quaternion.Euler(y, x, 0);
            distance = Mathf.Clamp(distance, minDistance, maxDistance);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }

        void HandleMouse()
        {
            if (Input.GetMouseButton(0))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
                y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
                distance -= scroll * zoomSpeed;
        }

        void HandleTouch()
        {
            if (Input.touchCount == 1)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Moved)
                {
                    x += t.deltaPosition.x * xSpeed * 0.002f;
                    y -= t.deltaPosition.y * ySpeed * 0.002f;
                    y = ClampAngle(y, yMinLimit, yMaxLimit);
                }
            }
            else if (Input.touchCount == 2)
            {
                Touch t0 = Input.GetTouch(0);
                Touch t1 = Input.GetTouch(1);

                float prevMag = (t0.position - t0.deltaPosition - (t1.position - t1.deltaPosition)).magnitude;
                float currMag = (t0.position - t1.position).magnitude;

                float diff = currMag - prevMag;
                distance -= diff * 0.01f;
            }
        }

        float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360) angle += 360;
            if (angle > 360) angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }


        public void SetAutoRotate(bool value)
        {
            autoRotate = value;
        }

    }

}