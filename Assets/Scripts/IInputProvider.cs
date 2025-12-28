using UnityEngine;

public interface IInputProvider
{
    // Grab (left click)
    bool IsGrabPressed();
    bool IsGrabReleased();

    Vector3 GetPointerWorldPosition();
}
