using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappingSystem : MonoBehaviour
{

    [Header("Snapping Settings")]
    [SerializeField] private float wallOffset = 0.005f; // 0.5 cm
    [SerializeField] private float snapAngle = 10f;
    [SerializeField] private float maxSnapDistance = 0.1f;

    private Transform currentWall;
    private Vector3 wallNormal;
    private Vector3 wallRight, wallUp;
    private Vector3 slideGrabOffset; // offset from mouse hit to picture center

    public bool IsSnapped { get; private set; }

    public void UpdatePlacement(Transform picture, IInputProvider input)
    {
        Ray pointerRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!IsSnapped)
        {
            FreeMove(picture, input);
            TrySnap(picture);
        }
        else
        {
            SlideAlongWall(picture, pointerRay);
        }
    }

    public void BeginGrab(Transform picture, Vector3 hitPoint)
    {
        slideGrabOffset = picture.position - hitPoint;
    }

    public void EndGrab(Transform picture)
    {
        if (!IsSnapped) return;
        picture.position = ProjectOntoWall(picture.position);
    }

    void FreeMove(Transform picture, IInputProvider input)
    {
        picture.position = input.GetPointerWorldPosition();
    }

    void TrySnap(Transform picture)
    {
        if (!Physics.Raycast(picture.position, picture.forward, out RaycastHit hit, maxSnapDistance))
            return;

        if (!hit.collider.CompareTag("Wall"))
            return;

        float angle = Vector3.Angle(picture.forward, -hit.normal);
        if (angle > snapAngle)
            return;

        currentWall = hit.transform;
        wallNormal = hit.normal;

        wallRight = Vector3.Cross(Vector3.up, wallNormal).normalized;
        if (wallRight == Vector3.zero)
            wallRight = Vector3.right;

        wallUp = Vector3.Cross(wallNormal, wallRight).normalized;

        AlignToWall(picture, hit.point);
        IsSnapped = true;
    }


    void SlideAlongWall(Transform picture, Ray pointerRay)
    {
        Plane wallPlane = new Plane(-wallNormal, picture.position);
        if (!wallPlane.Raycast(pointerRay, out float enter))
            return;

        Vector3 hitPoint = pointerRay.GetPoint(enter) + slideGrabOffset;
        Vector3 delta = hitPoint - picture.position;

        Vector3 slide =
            Vector3.Project(delta, wallRight) +
            Vector3.Project(delta, wallUp);

        Vector3 newPos = picture.position + slide;
        newPos = ProjectOntoWall(newPos);

        picture.position = newPos;
        picture.rotation = Quaternion.LookRotation(-wallNormal, Vector3.up);
    }

    void AlignToWall(Transform picture, Vector3 hitPoint)
    {
        picture.rotation = Quaternion.LookRotation(-wallNormal, Vector3.up);
        picture.position = hitPoint + wallNormal * wallOffset;
    }

    Vector3 ProjectOntoWall(Vector3 point)
    {
        return point - Vector3.Dot(point - currentWall.position, wallNormal) * wallNormal 
            + wallNormal * wallOffset;
    }
}
