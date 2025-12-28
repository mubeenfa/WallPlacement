using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputProvider : MonoBehaviour, IInputProvider
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float depth = 2f;

    // Left click (grab)
    public bool IsGrabPressed() => Input.GetMouseButtonDown(0);
    public bool IsGrabReleased() => Input.GetMouseButtonUp(0);


    public Vector3 GetPointerWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = depth;
        return _camera.ScreenToWorldPoint(mousePos);
    }

}
