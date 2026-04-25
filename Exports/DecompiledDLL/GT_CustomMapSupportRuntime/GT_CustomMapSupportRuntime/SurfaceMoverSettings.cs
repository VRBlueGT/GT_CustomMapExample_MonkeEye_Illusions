using UnityEngine;

namespace GT_CustomMapSupportRuntime;

public class SurfaceMoverSettings : MonoBehaviour
{
    public enum MoveType
    {
        Translation,
        Rotation
    }

    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    [SerializeField]
    public MoveType moveType;

    [Range(0.001f, float.MaxValue)]
    [Tooltip("Meters per second for Translation | Revolutions per second for Rotation")]
    [SerializeField]
    public float velocity = 0.001f;

    [Range(0f, float.MaxValue)]
    [Tooltip("How long in seconds should the cycle be delayed?")]
    [SerializeField]
    public float cycleDelay;

    [Tooltip("If TRUE, Translation mode will move from End to Start; Rotation mode will rotate in the negative direction.")]
    [SerializeField]
    public bool reverseDir;

    [Tooltip("If TRUE, Translation mode movement direction will be reversed when it reaches Start or End; Rotation mode rotation direction will be reversed once it's rotated the full Rotation Amount")]
    [SerializeField]
    public bool reverseDirOnCycle = true;

    [SerializeField]
    public Transform? start;

    [SerializeField]
    public Transform? end;

    [Tooltip("Which local axis should the object rotate around?")]
    [SerializeField]
    public RotationAxis rotationAxis = RotationAxis.Y;

    [Range(0.001f, 360f)]
    [Tooltip("How far should the object rotate per-cycle (in degrees)")]
    [SerializeField]
    public float rotationAmount = 360f;

    [Tooltip("If TRUE the rotation starting point will be the initial Y-axis rotation value of the object when the map is loaded, otherwise it will start at 0")]
    [SerializeField]
    public bool rotationRelativeToStarting = true;

    public bool hasBeenExported;

    private AnimationCurve? lerpAlpha;

    private Vector3 startingRotation;

    private float cycleDuration;

    private float distance;

    private float currT;

    private float percent;

    private bool currForward;

    public void OnEnable()
    {
        //IL_007d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0082: Unknown result type (might be due to invalid IL or missing references)
        //IL_0086: Unknown result type (might be due to invalid IL or missing references)
        //IL_008b: Unknown result type (might be due to invalid IL or missing references)
        //IL_00f6: Unknown result type (might be due to invalid IL or missing references)
        //IL_00fc: Unknown result type (might be due to invalid IL or missing references)
        //IL_0103: Unknown result type (might be due to invalid IL or missing references)
        //IL_0109: Unknown result type (might be due to invalid IL or missing references)
        //IL_012d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0132: Unknown result type (might be due to invalid IL or missing references)
        //IL_0151: Unknown result type (might be due to invalid IL or missing references)
        //IL_0156: Unknown result type (might be due to invalid IL or missing references)
        //IL_015b: Unknown result type (might be due to invalid IL or missing references)
        //IL_0165: Expected O, but got Unknown
        //IL_0034: Unknown result type (might be due to invalid IL or missing references)
        //IL_003f: Unknown result type (might be due to invalid IL or missing references)
        if (hasBeenExported)
        {
            return;
        }
        if (moveType == MoveType.Translation && (Object)(object)start != (Object)null && (Object)(object)end != (Object)null)
        {
            distance = Vector3.Distance(end.position, start.position);
            float num = distance / velocity;
            cycleDuration = num + cycleDelay;
        }
        else
        {
            if (rotationRelativeToStarting)
            {
                Quaternion localRotation = ((Component)this).transform.localRotation;
                startingRotation = ((Quaternion)(ref localRotation)).eulerAngles;
            }
            cycleDuration = rotationAmount / 360f / velocity;
            cycleDuration += cycleDelay;
        }
        float num2 = cycleDelay / cycleDuration;
        Vector2 val = default(Vector2);
        ((Vector2)(ref val))._002Ector(num2 / 2f, 0f);
        Vector2 val2 = default(Vector2);
        ((Vector2)(ref val2))._002Ector(1f - num2 / 2f, 1f);
        float num3 = (val2.y - val.y) / (val2.x - val.x);
        lerpAlpha = new AnimationCurve((Keyframe[])(object)new Keyframe[2]
        {
            new Keyframe(num2 / 2f, 0f, 0f, num3),
            new Keyframe(1f - num2 / 2f, 1f, num3, 0f)
        });
    }

    private void FixedUpdate()
    {
        if (!hasBeenExported)
        {
            Move();
        }
    }

    private long NetworkTimeMs()
    {
        return (long)(Time.time * 1000f);
    }

    private long CycleLengthMs()
    {
        return (long)(cycleDuration * 1000f);
    }

    private double PlatformTime()
    {
        long num = NetworkTimeMs();
        long num2 = CycleLengthMs();
        return (double)(num - num / num2 * num2) / 1000.0;
    }

    private int CycleCount()
    {
        return (int)(NetworkTimeMs() / CycleLengthMs());
    }

    private float CycleCompletionPercent()
    {
        return Mathf.Clamp((float)(PlatformTime() / (double)cycleDuration), 0f, 1f);
    }

    private bool IsEvenCycle()
    {
        return CycleCount() % 2 == 0;
    }

    public void Move()
    {
        //IL_0022: Unknown result type (might be due to invalid IL or missing references)
        Progress();
        switch (moveType)
        {
            case MoveType.Translation:
                ((Component)this).transform.localPosition = UpdatePointToPoint(percent);
                break;
            case MoveType.Rotation:
                UpdateRotation(percent);
                break;
        }
    }

    private Vector3 UpdatePointToPoint(float percentage)
    {
        //IL_002a: Unknown result type (might be due to invalid IL or missing references)
        //IL_0043: Unknown result type (might be due to invalid IL or missing references)
        //IL_004e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0054: Unknown result type (might be due to invalid IL or missing references)
        if (lerpAlpha == null || (Object)(object)start == (Object)null || (Object)(object)end == (Object)null)
        {
            return ((Component)this).transform.localPosition;
        }
        float num = lerpAlpha.Evaluate(percentage);
        return Vector3.Lerp(start.localPosition, end.localPosition, num);
    }

    private void UpdateRotation(float percentage)
    {
        //IL_0026: Unknown result type (might be due to invalid IL or missing references)
        //IL_002b: Unknown result type (might be due to invalid IL or missing references)
        //IL_00a2: Unknown result type (might be due to invalid IL or missing references)
        //IL_00a7: Unknown result type (might be due to invalid IL or missing references)
        //IL_00b9: Unknown result type (might be due to invalid IL or missing references)
        //IL_00be: Unknown result type (might be due to invalid IL or missing references)
        //IL_00d0: Unknown result type (might be due to invalid IL or missing references)
        //IL_00d5: Unknown result type (might be due to invalid IL or missing references)
        //IL_0075: Unknown result type (might be due to invalid IL or missing references)
        //IL_0076: Unknown result type (might be due to invalid IL or missing references)
        if (lerpAlpha == null)
        {
            return;
        }
        float num = lerpAlpha.Evaluate(percentage) * rotationAmount;
        if (rotationRelativeToStarting)
        {
            Vector3 val = startingRotation;
            switch (rotationAxis)
            {
                case RotationAxis.X:
                    val.x += num;
                    break;
                case RotationAxis.Y:
                    val.y += num;
                    break;
                case RotationAxis.Z:
                    val.z += num;
                    break;
            }
            ((Component)this).transform.localRotation = Quaternion.Euler(val);
        }
        else
        {
            switch (rotationAxis)
            {
                case RotationAxis.X:
                    ((Component)this).transform.localRotation = Quaternion.AngleAxis(num, Vector3.right);
                    break;
                case RotationAxis.Y:
                    ((Component)this).transform.localRotation = Quaternion.AngleAxis(num, Vector3.up);
                    break;
                case RotationAxis.Z:
                    ((Component)this).transform.localRotation = Quaternion.AngleAxis(num, Vector3.forward);
                    break;
            }
        }
    }

    private void Progress()
    {
        currT = CycleCompletionPercent();
        currForward = IsEvenCycle();
        percent = currT;
        if (reverseDirOnCycle)
        {
            percent = (currForward ? currT : (1f - currT));
        }
        if (reverseDir)
        {
            percent = 1f - percent;
        }
    }
}
