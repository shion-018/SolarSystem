using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
public class PopUpHowToPlay : MonoBehaviour
{
    [SerializeField] private Grabbable _grabbable; 
    [SerializeField] private GameObject _panel; 
    [SerializeField] private Vector3 _offset = new Vector3(0, 0.2f, 0); 

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _grabbable.WhenPointerEventRaised += OnPointerEvent;
    }

    private void OnDisable()
    {
        _grabbable.WhenPointerEventRaised -= OnPointerEvent;
    }

    private void OnPointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select)
        {
            PositionPanelAboveObject();
            _panel.SetActive(true); 
        }
        else if (evt.Type == PointerEventType.Unselect)
        {
            _panel.SetActive(false); 
        }
    }

    private void PositionPanelAboveObject()
    {
        Transform grabbedObjectTransform = _grabbable.transform;

        _panel.transform.position = grabbedObjectTransform.position + _offset;

        _panel.transform.LookAt(_mainCamera.transform);

        _panel.transform.Rotate(0, 180, 0);
    }
}
