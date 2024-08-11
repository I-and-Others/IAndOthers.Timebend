using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private CinemachineInputProvider inputProvider;
    private CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransform;


    [SerializeField] private int edgeScrolSize = 20;
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float dragSpeed = 5f;

    
    [SerializeField] private float zoomSpeed = 3f;
    [SerializeField] private float zoomInMax = 40f;
    [SerializeField] private float zoomOutMin = 80f;

    private Vector3 lastMousePosition = Vector3.zero;

    private bool dragPanMode = false;


    // Start is called before the first frame update
    void Start()
    {
        inputProvider = GetComponent<CinemachineInputProvider>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        PanMovement();
        Zoom();
        if(Input.GetMouseButtonDown(1)) { lastMousePosition = Input.mousePosition; dragPanMode = true; }
        if(Input.GetMouseButtonUp(1)) dragPanMode = false;
        if (dragPanMode) ClickDrag();

    }

    private Vector3 PanDirection(float x, float z)
    {
        Vector3 panDirection = Vector3.zero;

        if(z > Screen.height - edgeScrolSize) { panDirection.z += 1f; }
        else if (z < edgeScrolSize) { panDirection.z -= 1f; }

        if(x > Screen.width - edgeScrolSize) { panDirection.x += 1f; }
        else if (x < edgeScrolSize) { panDirection.x -= 1f; } 
        return panDirection;
    }

    private void PanMovement()
    {
        float x = inputProvider.GetAxisValue(0);
        float z = inputProvider.GetAxisValue(1);
        Vector3 panDirection = PanDirection(x, z);

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTransform.position + panDirection * panSpeed , Time.deltaTime);
    }

    private void Zoom()
    {
        float z = inputProvider.GetAxisValue(2);
        float fov = virtualCamera.m_Lens.FieldOfView;
        float targetFov = Mathf.Clamp(fov + z, zoomInMax, zoomOutMin);
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fov,targetFov,zoomSpeed * Time.deltaTime);
    }


    private void ClickDrag()
    {
        Vector3 mouseMovement = Camera.main.ScreenToViewportPoint(Input.mousePosition - lastMousePosition);
        Vector3 move = new Vector3(mouseMovement.x * dragSpeed, 0, mouseMovement.y * dragSpeed);

        cameraTransform.Translate(-move,Space.World);
        lastMousePosition = Input.mousePosition;
    }

}
