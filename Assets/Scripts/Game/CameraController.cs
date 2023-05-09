using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField, Range(0f, 10f)] private float panSpeed = 1f;
        [SerializeField, Range(0f, 50f)] private float zoomSpeed = 1f;
        [SerializeField, Range(1f, 10f)] private float maxZoom = 1f;
        [SerializeField, Range(10f, 50f)] private float minZoom = 50f;
        [SerializeField] private bool useRawPanInput = true;
        private new Camera camera;  // cached for performance

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }
        private void Update()
        {
            float zoomAmount = Input.mouseScrollDelta.y;
            zoomAmount *= -zoomSpeed;
            zoomAmount *= camera.orthographicSize;
            zoomAmount *= Time.deltaTime;
            camera.orthographicSize = Mathf.Clamp
                (camera.orthographicSize+zoomAmount, maxZoom, minZoom);

            float panAmountX;
            float panAmountY;
            if (useRawPanInput)
            {
                panAmountX = Input.GetAxisRaw("Horizontal");
                panAmountY = Input.GetAxisRaw("Vertical");
            } else
            {
                panAmountX = Input.GetAxis("Horizontal");
                panAmountY = Input.GetAxis("Vertical");
            }
            Vector2 panAmount = new(panAmountX, panAmountY);
            panAmount *= panSpeed;
            panAmount *= camera.orthographicSize;
            panAmount *= Time.deltaTime;
            transform.position += (Vector3)panAmount;
        }
    }
}