using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    [SerializeField] private ScriptableEventScreenWrap screenWrapEvent;

    [Header("Screen Bounds Thresholds")]
    [SerializeField] [Range(0f, 1f)] private float leftThreshold = .1f;
    [SerializeField] [Range(0f, 1f)] private float rightThreshold = .9f;
    [SerializeField] [Range(0f, 1f)] private float upThreshold = .9f;
    [SerializeField] [Range(0f, 1f)] private float downThreshold = .1f;

    [Header("Screen location offset")]
    [SerializeField] [Range(-1f, 1f)] private float leftOffset = 0f;
    [SerializeField] [Range(-1f, 1f)] private float rightOffset = 1f;
    [SerializeField] [Range(-1f, 1f)] private float upOffset = 1f;
    [SerializeField] [Range(-1f, 1f)] private float downOffset = -.1f;

    private Camera cam;

    private void OnBecameInvisible()
    {
        cam = Camera.main;
        screenWrapEvent.Raise(CalculateNewPos(transform.position));
    }

    private Vector3 CalculateNewPos(Vector3 pos)
    {
        Vector3 viewportPos = cam.WorldToViewportPoint(pos);

        //Adjust for Z offset on the camera position
        viewportPos.z = 0 - cam.transform.position.z;

        if (viewportPos.y > upThreshold)
            viewportPos.y = downOffset;
        else if (viewportPos.y < downThreshold)
            viewportPos.y = upOffset;
        else if (viewportPos.x > rightThreshold)
            viewportPos.x = leftOffset;
        else if (viewportPos.x < leftThreshold)
            viewportPos.x = rightOffset;

        return cam.ViewportToWorldPoint(viewportPos);
    }
}
