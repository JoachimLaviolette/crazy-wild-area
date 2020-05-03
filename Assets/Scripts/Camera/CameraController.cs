using System;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          CameraController class
 * ------------------------------------------------
 */

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 2f;
    [SerializeField]
    private float _pitchRotationSpeed = 2f;
    [SerializeField]
    private float _yawRotationSpeed = 4f;
    [SerializeField]
    private float _zoomSpeed = 7f;

    private float _zoomAmount;
    private Func<Vector3> GetTargetPosition;

    private float _pitch, _lastPitch; // X axis
    private float _yaw, _lastYaw; // Y axis

    private const float _MIN_ZOOM_AMOUNT = 15f;
    private const float _MAX_ZOOM_AMOUNT = 135f;
    private const float _MIN_PITCH = 0f;
    private const float _MAX_PITCH = 75f;

    private void Start()
    {
        _zoomAmount = _MIN_ZOOM_AMOUNT;
        _pitch = _lastPitch = transform.rotation.eulerAngles.x;
        _yaw = _lastYaw = 0f;
    }

    private void Update()
    {
        HandleZoom();
        HandleRotation();
    }

    private void LateUpdate()
    {
        HandlePosition();
    }

    /**
     * Set the position of the target to follow
     */
    public void SetGetTargetPosition(Func<Vector3> GetTargetPosition)
    {
        this.GetTargetPosition = GetTargetPosition;
    }

    /**
     * Handle zoom
     */
    private void HandleZoom()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        AddZoomAmount(-Input.mouseScrollDelta.y);
    }

    /**
     * Handle camera position
     */
    private void HandlePosition()
    {
        if (GetTargetPosition == null) return;

        Vector3 targetPosition = GetTargetPosition() - transform.forward * _zoomAmount;
        transform.position = (_yaw == _lastYaw && _pitch == _lastPitch) ? // if no rotation was performed
            Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _moveSpeed)
            : targetPosition;
    }
    
    /**
     * Handle camera rotation
     */
    private void HandleRotation()
    {
        _lastPitch = _pitch;
        _lastYaw = _yaw;

        if (GetTargetPosition == null) return;
        if (!Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.D)) return;

        if (Input.GetKey(KeyCode.Z)) _pitch += Time.deltaTime * _pitchRotationSpeed;
        if (Input.GetKey(KeyCode.S)) _pitch -= Time.deltaTime * _pitchRotationSpeed;
        if (Input.GetKey(KeyCode.Q)) _yaw -= Time.deltaTime * _yawRotationSpeed;
        if (Input.GetKey(KeyCode.D)) _yaw += Time.deltaTime * _yawRotationSpeed;

        _pitch = Mathf.Clamp(_pitch, _MIN_PITCH, _MAX_PITCH);
        transform.eulerAngles = new Vector3(_pitch, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        
        transform.RotateAround(GetTargetPosition(), Vector3.up, _yaw);
        _yaw = 0f;
    }

    /**
     * Add some zoom amount to the current one
     */
    public void AddZoomAmount(float zoomAmount)
    {
        _zoomAmount += zoomAmount * _zoomSpeed;
        _zoomAmount = Mathf.Clamp(_zoomAmount, _MIN_ZOOM_AMOUNT, _MAX_ZOOM_AMOUNT);
    }
}
