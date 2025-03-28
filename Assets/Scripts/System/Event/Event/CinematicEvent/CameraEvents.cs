using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using Visual.Camera;

public abstract class CameraEvent : Event
{
    private static CameraController _cameraController;
    protected static CameraController CameraController
    {
        get
        {
            if (_cameraController == null)
            {
                _cameraController = EffectManager.Instance.GetCameraController();
            }
            return _cameraController;
        }
    }
}

[Serializable]
public class ShakeCameraEvent : CameraEvent
{
    public float time;
    public override IEnumerator Execute(EventObject eventObject = null)
    {
        return EffectManager.Instance.ShakeCamera(time);
    }
}

public class StopFollowCameraEvent : CameraEvent
{
    public override IEnumerator Execute(EventObject eventObject = null)
    {
        CameraController.IsFollowing = false;
        return null;
    }
}

public class StartFollowCameraEvent : CameraEvent
{
    public override IEnumerator Execute(EventObject eventObject = null)
    {
        CameraController.GetComponent<Camera>().orthographicSize = 5;
        CameraController.IsFollowing = true;
        return null;
    }  
}

public class CameraMoveEvent : CameraEvent
{
    [SerializeField] private Vector3 target;
    [SerializeField] private float size = 5;
    [SerializeField] private float duration;
    public override IEnumerator Execute(EventObject eventObject = null)
    {
        return CameraController.MoveTo(target, duration, size);
    }
}

public class CameraSet : CameraEvent
{
    public override IEnumerator Execute(EventObject eventObject = null)
    {
        CameraManager.Instance.ChangeCamera(eventObject.GetComponentInChildren<CinemachineVirtualCamera>());
        return null;
    }
}

public class CameraMove : CameraEvent
{
    [SerializeField] private Vector3 destination;
    [SerializeField] private float speed;
    public override IEnumerator Execute(EventObject eventObject = null)
    {
        return CameraManager.Instance.CameraMove(destination, speed);
    }
}

public class CameraFollow : CameraEvent
{
    [SerializeField] private GameObject followObject;
    public override IEnumerator Execute(EventObject eventObject = null)
    {
        CameraManager.Instance.CameraFollow(followObject.transform);
        return null;
    }
}

public class CameraZoomInOut : CameraEvent
{
    [SerializeField] private float size;
    [SerializeField] private float duration;
    public override IEnumerator Execute(EventObject eventObject = null)
    {
        return CameraManager.Instance.CameraZoomInOut(size, duration);
    }
}

public class CameraZoomReset : CameraEvent
{
    public override IEnumerator Execute(EventObject eventObject = null)
    {
        CameraManager.Instance.CameraZoomReset();
        return null;
    }
}

public class CameraShake : CameraEvent
{
    [SerializeField] private float force;
    public override IEnumerator Execute(EventObject eventObject = null)
    {
        CameraManager.Instance.CameraShake(force);
        return null;
    }
}