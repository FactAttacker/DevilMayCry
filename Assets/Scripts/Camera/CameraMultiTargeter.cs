using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class CameraMultiTargeter : MonoBehaviour
{
    #region -------- Rotation ---------
    [System.Serializable]
    class CameraRotation
    {
        [Header("x Rotation")]
        public float roll;
        [Header("y Rotation")]
        public float pitch;
        [Header("z Rotation")]
        public float yaw;
    }
    [SerializeField]
    [Header("Camera Rotation")]
    CameraRotation cmRotation;
    #endregion

    #region -------- Padding ---------
    [System.Serializable]
    class Padding
    {
        public float left;
        public float right;
        public float up;
        public float down;
    }
    [SerializeField]
    [Header("Camera Padding")]
    Padding padding;
    #endregion

    #region -------- Shake ---------
    [System.Serializable]
    class ShakeInfo
    {
        [HideInInspector]
        public Vector3 vector;
        public float time = 1.5f;
        public float amount = 0.05f;
    }
    [SerializeField]
    ShakeInfo shakeInfo;
    #endregion

    #region -------- PositionAndRotation ---------
    class PositionAndRotation
    {
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        public PositionAndRotation(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
    #endregion

    #region -------- ProjectionHits ---------
    class ProjectionHits
    {
        public float Max { get; private set; }
        public float Min { get; private set; }

        public ProjectionHits(float max, float min)
        {
            Min = min;
            Max = max;
        }

        public ProjectionHits AddPadding(float paddingToMax, float paddingToMin)
        {
            return new ProjectionHits(Max + paddingToMax, Min - paddingToMin);
        }
    }
    #endregion

    #region -------- Inner ---------
    Transform bossPos;
    Transform playerPos;
    bool isShake = true;
    public Camera _camera;
    public float MoveSmoothTime = 0.19f;
    [ContextMenuItem("Shaking",nameof(OnShake))]
    public Vector3 cmPosition;

    GameObject[] targets = new GameObject[0];
    
    enum ProjectionEdgeHits { TOP_BOTTOM, LEFT_RIGHT }
    #endregion

    public static CameraMultiTargeter instance;
    void Awake()
    {
        if (instance == null) instance = this;

        //gameObject.TryGetComponent(out _camera);
        //OnShake();
    }

    void Start()
    {
        shakeInfo.vector = new Vector3(0f, 0f, -5f);
    }

    /// <summary>
    /// 타겟 지정
    /// </summary>
    /// <param name="targets">타겟 배열</param>
    public void SetTargets(GameObject[] targets)
    {
        this.targets = targets;
    }

    /// <summary>
    /// Camera Shake
    /// </summary>
    public void OnShake(float time = 1.5f)
    {
        shakeInfo.time = time;
        if (isShake)
        {
            isShake = false;
            StartCoroutine(ShakeCoroutine());
        }
    }
    IEnumerator ShakeCoroutine()
    {
        while (shakeInfo.time > 0)
        {
            shakeInfo.time -= Time.deltaTime;
            cmPosition = (Random.insideUnitSphere * shakeInfo.amount) + shakeInfo.vector;
            yield return null;
        }
        isShake = true;
        shakeInfo.time = 0.0f;
        cmPosition = shakeInfo.vector;
    }
    
    void LateUpdate()
    {
        //// Shake
        //if (isShake)
        //{
        //    if(shakeInfo.time > 0)
        //    {
        //        //shakeInfo.time -= Time.deltaTime;
        //        cmPosition = (Random.insideUnitSphere * shakeInfo.amount) + shakeInfo.vector;
        //    }
        //    else
        //    {
        //        isShake = false;
        //        shakeInfo.time = 0.0f;
        //        cmPosition = shakeInfo.vector;
        //    }
        //}

        //if (targets.Length != 0)
        //{
        //    var targetPositionAndRotation = TargetPositionAndRotation(targets);

        //    Vector3 velocity = Vector3.zero;
        //    transform.position = Vector3.SmoothDamp(transform.position, targetPositionAndRotation.Position, ref velocity, MoveSmoothTime);
        //    transform.rotation = targetPositionAndRotation.Rotation;

        //    transform.position = cmPosition;
        //}
    }

    PositionAndRotation TargetPositionAndRotation(GameObject[] targets)
    {
        float halfVerticalFovRad = (_camera.fieldOfView * Mathf.Deg2Rad) / 2f;
        float halfHorizontalFovRad = Mathf.Atan(Mathf.Tan(halfVerticalFovRad) * _camera.aspect);

        var rotation = Quaternion.Euler(cmRotation.pitch, cmRotation.yaw, cmRotation.roll);
        var inverseRotation = Quaternion.Inverse(rotation);

        var targetsRotatedToCameraIdentity = targets.Select(target => inverseRotation * target.transform.position).ToArray();

        float furthestPointDistanceFromCamera = targetsRotatedToCameraIdentity.Max(target => target.z);
        float projectionPlaneZ = furthestPointDistanceFromCamera + 3f;

        ProjectionHits viewProjectionLeftAndRightEdgeHits =
            ViewProjectionEdgeHits(targetsRotatedToCameraIdentity, ProjectionEdgeHits.LEFT_RIGHT, projectionPlaneZ, halfHorizontalFovRad).AddPadding(padding.right, padding.left);
        ProjectionHits viewProjectionTopAndBottomEdgeHits =
            ViewProjectionEdgeHits(targetsRotatedToCameraIdentity, ProjectionEdgeHits.TOP_BOTTOM, projectionPlaneZ, halfVerticalFovRad).AddPadding(padding.up, padding.down);

        var requiredCameraPerpedicularDistanceFromProjectionPlane =
            Mathf.Max(
                RequiredCameraPerpedicularDistanceFromProjectionPlane(viewProjectionTopAndBottomEdgeHits, halfVerticalFovRad),
                RequiredCameraPerpedicularDistanceFromProjectionPlane(viewProjectionLeftAndRightEdgeHits, halfHorizontalFovRad)
        );

        Vector3 cameraPositionIdentity = new Vector3(
            (viewProjectionLeftAndRightEdgeHits.Max + viewProjectionLeftAndRightEdgeHits.Min) / 2f,
            (viewProjectionTopAndBottomEdgeHits.Max + viewProjectionTopAndBottomEdgeHits.Min) / 2f,
            projectionPlaneZ - requiredCameraPerpedicularDistanceFromProjectionPlane);

        return new PositionAndRotation(rotation * cameraPositionIdentity, rotation);
    }

    private ProjectionHits ViewProjectionEdgeHits(IEnumerable<Vector3> targetsRotatedToCameraIdentity, ProjectionEdgeHits alongAxis, float projectionPlaneZ, float halfFovRad)
    {
        float[] projectionHits = targetsRotatedToCameraIdentity
            .SelectMany(target => TargetProjectionHits(target, alongAxis, projectionPlaneZ, halfFovRad))
            .ToArray();
        return new ProjectionHits(projectionHits.Max(), projectionHits.Min());
    }

    private static float RequiredCameraPerpedicularDistanceFromProjectionPlane(ProjectionHits viewProjectionEdgeHits, float halfFovRad)
    {
        float distanceBetweenEdgeProjectionHits = viewProjectionEdgeHits.Max - viewProjectionEdgeHits.Min;
        return (distanceBetweenEdgeProjectionHits / 2f) / Mathf.Tan(halfFovRad);
    }

    private float[] TargetProjectionHits(Vector3 target, ProjectionEdgeHits alongAxis, float projectionPlaneDistance, float halfFovRad)
    {
        float distanceFromProjectionPlane = projectionPlaneDistance - target.z;
        float projectionHalfSpan = Mathf.Tan(halfFovRad) * distanceFromProjectionPlane;

        if (alongAxis == ProjectionEdgeHits.LEFT_RIGHT)
        {
            return new[] { target.x + projectionHalfSpan, target.x - projectionHalfSpan };
        }
        else
        {
            return new[] { target.y + projectionHalfSpan, target.y - projectionHalfSpan };
        }

    }
}
 