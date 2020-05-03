using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * ------------------------------------------------
 *          Author: Joachim Laviolette
 *          MinimapCameraController class
 * ------------------------------------------------
 */

public class MinimapCameraController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 2f;
    [SerializeField]
    private float _zoomSpeed = 7f;
    private float _zoomAmount;
    private Func<Vector3> GetTargetPosition;
    [SerializeField]
    private LayerMask _mapUILayerMask;
    private Camera _camera;

    private const float _MIN_ZOOM_AMOUNT = 50f;
    private const float _MAX_ZOOM_AMOUNT = 128f;

    private void Start()
    {
        _camera = AssetManager.GetMainCamera();
        _zoomAmount = _MAX_ZOOM_AMOUNT;
    }

    private void Update()
    {
        HandleZoom();
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
        _zoomAmount = _MIN_ZOOM_AMOUNT;
        GetComponent<Camera>().orthographicSize = _zoomAmount;
    }

    /**
     * Handle zoom
     */
    private void HandleZoom()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) return;
        if (!IsMouseOverMapUI()) return;

        AddZoomAmount(-Input.mouseScrollDelta.y);
    }

    /**
     * Check if the mouse is over the minimap
     */
    private bool IsMouseOverMapUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        foreach (RaycastResult rs in raycastResults)
            if (rs.gameObject.GetComponent<MapUI>() != null) return true;

        return false;
    }

    /**
     * Handle camera position
     */
    private void HandlePosition()
    {
        if (GetTargetPosition == null) return;

        Vector3 targetPosition = GetTargetPosition() - transform.forward * _zoomAmount;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _moveSpeed);
        transform.eulerAngles = new Vector3(90f, 0f, 180f);
    }

    /**
     * Add some zoom amount to the current one
     */
    public void AddZoomAmount(float zoomAmount)
    {
        _zoomAmount += zoomAmount * _zoomSpeed;
        _zoomAmount = Mathf.Clamp(_zoomAmount, _MIN_ZOOM_AMOUNT, _MAX_ZOOM_AMOUNT);
        GetComponent<Camera>().orthographicSize = _zoomAmount;
    }
}