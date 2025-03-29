using UnityEngine;

public class GrabDisplayPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel; 
    [SerializeField] private Vector3 offset = new Vector3(0, 0.2f, 0); 
    private CustomGrabbable customGrabbable; 
    private Camera mainCamera; 

    private void Awake()
    {
        customGrabbable = GetComponent<CustomGrabbable>();
        mainCamera = Camera.main; 
    }

    private void OnEnable()
    {
        customGrabbable.OnGrabBegin += ShowPanel;
        customGrabbable.OnGrabEnd += HidePanel;
    }

    private void OnDisable()
    {
        customGrabbable.OnGrabBegin -= ShowPanel;
        customGrabbable.OnGrabEnd -= HidePanel;
    }

    private void ShowPanel()
    {
        PositionPanelAboveObject();
        panel.SetActive(true); // パネルを表示
    }

    private void HidePanel()
    {
        panel.SetActive(false); // パネルを非表示
    }

    private void PositionPanelAboveObject()
    {
        panel.transform.position = transform.position + offset;

        panel.transform.LookAt(mainCamera.transform);

        panel.transform.Rotate(0, 180, 0);
    }
}
