using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    float _xMaxOffset;
    [SerializeField]
    float _xMinOffset;

    [SerializeField]
    float _yMaxOffset;
    [SerializeField]
    float _yMinOffset;

    [SerializeField]
    float _deadzoneRatio = 0.33f;

    Vector2 _normalizedMousePos = Vector2.zero;

    Quaternion _initialRotation;
    Vector3 _initialPosition;

    void Start()
    {
        _initialRotation = transform.rotation;
        _initialPosition = transform.position;
    }

    
    void Update()
    {
        _normalizedMousePos.x = Mathf.Clamp01(Input.mousePosition.x / Screen.width);
        _normalizedMousePos.y = Mathf.Clamp01(Input.mousePosition.y / Screen.height);

        // No movement in deadzone
        float deadzoneHighBound = 0.5f + _deadzoneRatio / 2;
        float deadzoneLowBound = 0.5f - _deadzoneRatio / 2;
        bool xInZone = _normalizedMousePos.x < deadzoneHighBound && _normalizedMousePos.x > deadzoneLowBound;
        bool yInZone = _normalizedMousePos.y < deadzoneHighBound && _normalizedMousePos.y > deadzoneLowBound;

        float xLerpedOffset;

        if (yInZone)
        {
            xLerpedOffset = 0;
        }
        else
        {
            if (_normalizedMousePos.y > deadzoneHighBound)
            {
                xLerpedOffset = Mathf.Lerp(0, _xMaxOffset, RemapValueToRange(_normalizedMousePos.y, deadzoneHighBound, 1, 0, 1));
            }
            else if (_normalizedMousePos.y < deadzoneLowBound)
            {
                xLerpedOffset = Mathf.Lerp(_xMinOffset, 0, RemapValueToRange(_normalizedMousePos.y, 0, deadzoneLowBound, 0, 1));
            }
            else
            {
                xLerpedOffset = 0;
            }
        }

        float zLerpedOffset;

        if (xInZone)
        {
            zLerpedOffset = 0;
        }
        else
        {
            if (_normalizedMousePos.x > deadzoneHighBound)
            {
                zLerpedOffset = Mathf.Lerp(0, _yMaxOffset, RemapValueToRange(_normalizedMousePos.x, deadzoneHighBound, 1, 0, 1));
            }
            else if (_normalizedMousePos.x < deadzoneLowBound)
            {
                zLerpedOffset = Mathf.Lerp(_yMinOffset, 0, RemapValueToRange(_normalizedMousePos.x, 0, deadzoneLowBound, 0, 1));
            }
            else
            {
                zLerpedOffset = 0;
            }
        }

        // Rotation
        Quaternion xRotation = Quaternion.AngleAxis(-xLerpedOffset, Vector3.right);
        //Quaternion zRotation = Quaternion.AngleAxis(-zLerpedOffset, Vector3.forward);

        //Quaternion rotationAngle = xRotation * zRotation;

        Quaternion finalRotation = _initialRotation * xRotation;


        // Movement
        Vector3 finalPosition = new Vector3(_initialPosition.x + zLerpedOffset, _initialPosition.y, _initialPosition.z);


        transform.position = finalPosition;
        transform.rotation = finalRotation;
    }

    float RemapValueToRange(float value, float low1, float high1, float low2, float high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }
}
