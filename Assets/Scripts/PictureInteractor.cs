
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(SnappingSystem))]
public class PictureInteractor : MonoBehaviour
{

    [SerializeField] private MonoBehaviour inputProviderSource;
    
    SnappingSystem snapSystem;
    IInputProvider inputProvider;

    bool isGrabbed = false;

    private void Awake()
    {
        inputProvider = inputProviderSource as IInputProvider;
        if (inputProvider == null)
            Debug.LogError("InputProvider Missing");

        snapSystem = GetComponent<SnappingSystem>();
    }

    private void Update()
    {
        if (inputProvider.IsGrabPressed())
            TryGrab();

        if (isGrabbed)
            snapSystem.UpdatePlacement(transform, inputProvider);

        if (inputProvider.IsGrabReleased())
            Release();

    }

    private void TryGrab()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
        {
            isGrabbed = true;
            snapSystem.BeginGrab(transform, hit.point);
        }
    }

    private void Release()
    {
        if (!isGrabbed) return;

        isGrabbed = false;
        snapSystem.EndGrab(transform);
    }
}
