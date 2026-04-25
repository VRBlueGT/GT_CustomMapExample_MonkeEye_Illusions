using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomMapSupport;

public class BezierSpline : MonoBehaviour
{
    public Vector3[]? points;

    public BezierControlPointMode[]? modes;

    public bool loop;

    private float _totalArcLength;

    private float[]? _timesTable;

    private float[]? _lengthsTable;

    public bool Loop
    {
        get
        {
            return loop;
        }
        set
        {
            //IL_003c: Unknown result type (might be due to invalid IL or missing references)
            loop = value;
            if (value)
            {
                if (modes != null)
                {
                    modes[modes.Length - 1] = modes[0];
                }
                if (points != null)
                {
                    SetControlPoint(0, points[0]);
                }
            }
        }
    }

    public int ControlPointCount
    {
        get
        {
            Vector3[]? array = points;
            if (array == null)
            {
                return 0;
            }
            return array.Length;
        }
    }

    public int CurveCount
    {
        get
        {
            if (points != null)
            {
                return (points.Length - 1) / 3;
            }
            return 0;
        }
    }

    private void Awake()
    {
        //IL_0012: Unknown result type (might be due to invalid IL or missing references)
        //IL_0020: Unknown result type (might be due to invalid IL or missing references)
        //IL_0025: Unknown result type (might be due to invalid IL or missing references)
        //IL_002a: Unknown result type (might be due to invalid IL or missing references)
        float num = 0f;
        for (int i = 1; i < points?.Length; i++)
        {
            float num2 = num;
            Vector3 val = points[i] - points[i - 1];
            num = num2 + ((Vector3)(ref val)).magnitude;
        }
        int subdivisions = Mathf.RoundToInt(num / 0.1f);
        buildTimesLengthsTables(subdivisions);
    }

    private void buildTimesLengthsTables(int subdivisions)
    {
        //IL_0032: Unknown result type (might be due to invalid IL or missing references)
        //IL_0037: Unknown result type (might be due to invalid IL or missing references)
        //IL_0045: Unknown result type (might be due to invalid IL or missing references)
        //IL_004a: Unknown result type (might be due to invalid IL or missing references)
        //IL_0052: Unknown result type (might be due to invalid IL or missing references)
        //IL_0053: Unknown result type (might be due to invalid IL or missing references)
        //IL_005f: Unknown result type (might be due to invalid IL or missing references)
        //IL_0060: Unknown result type (might be due to invalid IL or missing references)
        _totalArcLength = 0f;
        float num = 1f / (float)subdivisions;
        _timesTable = new float[subdivisions];
        _lengthsTable = new float[subdivisions];
        Vector3 val = GetPoint(0f);
        for (int i = 1; i <= subdivisions; i++)
        {
            float num2 = num * (float)i;
            Vector3 point = GetPoint(num2);
            _totalArcLength += Vector3.Distance(point, val);
            val = point;
            _timesTable[i - 1] = num2;
            _lengthsTable[i - 1] = _totalArcLength;
        }
    }

    private float getPathFromTime(float t)
    {
        if (float.IsNaN(_totalArcLength) || _totalArcLength == 0f)
        {
            return t;
        }
        if (t > 0f && t < 1f)
        {
            float num = _totalArcLength * t;
            float num2 = 0f;
            float num3 = 0f;
            float num4 = 0f;
            float num5 = 0f;
            for (int i = 0; i < _lengthsTable?.Length; i++)
            {
                if (_lengthsTable[i] > num)
                {
                    num4 = ((_timesTable == null) ? 0f : _timesTable[i]);
                    num5 = _lengthsTable[i];
                    if (i > 0)
                    {
                        num3 = _lengthsTable[i - 1];
                    }
                    break;
                }
                num2 = ((_timesTable == null) ? 0f : _timesTable[i]);
            }
            t = num2 + (num - num3) / (num5 - num3) * (num4 - num2);
        }
        if (t > 1f)
        {
            t = 1f;
        }
        else if (t < 0f)
        {
            t = 0f;
        }
        return t;
    }

    public Vector3[]? GetControlPoints()
    {
        return points;
    }

    public BezierControlPointMode[]? GetControlPointModes()
    {
        return modes;
    }

    public Vector3 GetControlPoint(int index)
    {
        //IL_0015: Unknown result type (might be due to invalid IL or missing references)
        //IL_0008: Unknown result type (might be due to invalid IL or missing references)
        if (points == null)
        {
            return Vector3.zero;
        }
        return points[index];
    }

    public void SetControlPoint(int index, Vector3 point)
    {
        //IL_018c: Unknown result type (might be due to invalid IL or missing references)
        //IL_018d: Unknown result type (might be due to invalid IL or missing references)
        //IL_001c: Unknown result type (might be due to invalid IL or missing references)
        //IL_0024: Unknown result type (might be due to invalid IL or missing references)
        //IL_0029: Unknown result type (might be due to invalid IL or missing references)
        //IL_002e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0149: Unknown result type (might be due to invalid IL or missing references)
        //IL_014e: Unknown result type (might be due to invalid IL or missing references)
        //IL_014f: Unknown result type (might be due to invalid IL or missing references)
        //IL_0154: Unknown result type (might be due to invalid IL or missing references)
        //IL_004a: Unknown result type (might be due to invalid IL or missing references)
        //IL_004f: Unknown result type (might be due to invalid IL or missing references)
        //IL_0050: Unknown result type (might be due to invalid IL or missing references)
        //IL_0055: Unknown result type (might be due to invalid IL or missing references)
        //IL_0070: Unknown result type (might be due to invalid IL or missing references)
        //IL_0075: Unknown result type (might be due to invalid IL or missing references)
        //IL_0076: Unknown result type (might be due to invalid IL or missing references)
        //IL_007b: Unknown result type (might be due to invalid IL or missing references)
        //IL_0090: Unknown result type (might be due to invalid IL or missing references)
        //IL_0091: Unknown result type (might be due to invalid IL or missing references)
        //IL_0175: Unknown result type (might be due to invalid IL or missing references)
        //IL_017a: Unknown result type (might be due to invalid IL or missing references)
        //IL_017b: Unknown result type (might be due to invalid IL or missing references)
        //IL_0180: Unknown result type (might be due to invalid IL or missing references)
        //IL_0105: Unknown result type (might be due to invalid IL or missing references)
        //IL_010a: Unknown result type (might be due to invalid IL or missing references)
        //IL_010b: Unknown result type (might be due to invalid IL or missing references)
        //IL_0110: Unknown result type (might be due to invalid IL or missing references)
        //IL_0124: Unknown result type (might be due to invalid IL or missing references)
        //IL_0129: Unknown result type (might be due to invalid IL or missing references)
        //IL_012a: Unknown result type (might be due to invalid IL or missing references)
        //IL_012f: Unknown result type (might be due to invalid IL or missing references)
        //IL_00af: Unknown result type (might be due to invalid IL or missing references)
        //IL_00b0: Unknown result type (might be due to invalid IL or missing references)
        //IL_00c2: Unknown result type (might be due to invalid IL or missing references)
        //IL_00c7: Unknown result type (might be due to invalid IL or missing references)
        //IL_00c8: Unknown result type (might be due to invalid IL or missing references)
        //IL_00cd: Unknown result type (might be due to invalid IL or missing references)
        //IL_00e1: Unknown result type (might be due to invalid IL or missing references)
        //IL_00e6: Unknown result type (might be due to invalid IL or missing references)
        //IL_00e7: Unknown result type (might be due to invalid IL or missing references)
        //IL_00ec: Unknown result type (might be due to invalid IL or missing references)
        if (points == null || points.Length <= index)
        {
            return;
        }
        if (index % 3 == 0)
        {
            Vector3 val = point - points[index];
            if (loop)
            {
                if (index == 0)
                {
                    ref Vector3 reference = ref points[1];
                    reference += val;
                    ref Vector3 reference2 = ref points[points.Length - 2];
                    reference2 += val;
                    points[points.Length - 1] = point;
                }
                else if (index == points.Length - 1)
                {
                    points[0] = point;
                    ref Vector3 reference3 = ref points[1];
                    reference3 += val;
                    ref Vector3 reference4 = ref points[index - 1];
                    reference4 += val;
                }
                else
                {
                    ref Vector3 reference5 = ref points[index - 1];
                    reference5 += val;
                    ref Vector3 reference6 = ref points[index + 1];
                    reference6 += val;
                }
            }
            else
            {
                if (index > 0)
                {
                    ref Vector3 reference7 = ref points[index - 1];
                    reference7 += val;
                }
                if (index + 1 < points.Length)
                {
                    ref Vector3 reference8 = ref points[index + 1];
                    reference8 += val;
                }
            }
        }
        points[index] = point;
        EnforceMode(index);
    }

    public BezierControlPointMode GetControlPointMode(int index)
    {
        if (modes == null)
        {
            return BezierControlPointMode.Free;
        }
        return modes[(index + 1) / 3];
    }

    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        if (modes == null)
        {
            return;
        }
        int num = (index + 1) / 3;
        modes[num] = mode;
        if (loop)
        {
            if (num == 0)
            {
                modes[modes.Length - 1] = mode;
            }
            else if (num == modes.Length - 1)
            {
                modes[0] = mode;
            }
        }
        EnforceMode(index);
    }

    private void EnforceMode(int index)
    {
        //IL_009b: Unknown result type (might be due to invalid IL or missing references)
        //IL_00a0: Unknown result type (might be due to invalid IL or missing references)
        //IL_00a2: Unknown result type (might be due to invalid IL or missing references)
        //IL_00ab: Unknown result type (might be due to invalid IL or missing references)
        //IL_00b0: Unknown result type (might be due to invalid IL or missing references)
        //IL_00b5: Unknown result type (might be due to invalid IL or missing references)
        //IL_00e5: Unknown result type (might be due to invalid IL or missing references)
        //IL_00e7: Unknown result type (might be due to invalid IL or missing references)
        //IL_00e9: Unknown result type (might be due to invalid IL or missing references)
        //IL_00ee: Unknown result type (might be due to invalid IL or missing references)
        //IL_00bd: Unknown result type (might be due to invalid IL or missing references)
        //IL_00c2: Unknown result type (might be due to invalid IL or missing references)
        //IL_00cc: Unknown result type (might be due to invalid IL or missing references)
        //IL_00d6: Unknown result type (might be due to invalid IL or missing references)
        //IL_00db: Unknown result type (might be due to invalid IL or missing references)
        if (modes == null || points == null)
        {
            return;
        }
        int num = (index + 1) / 3;
        BezierControlPointMode bezierControlPointMode = modes[num];
        if (bezierControlPointMode == BezierControlPointMode.Free || (!loop && (num == 0 || num == modes.Length - 1)))
        {
            return;
        }
        int num2 = num * 3;
        int num3;
        int num4;
        if (index <= num2)
        {
            num3 = num2 - 1;
            if (num3 < 0)
            {
                num3 = points.Length - 2;
            }
            num4 = num2 + 1;
            if (num4 >= points.Length)
            {
                num4 = 1;
            }
        }
        else
        {
            num3 = num2 + 1;
            if (num3 >= points.Length)
            {
                num3 = 1;
            }
            num4 = num2 - 1;
            if (num4 < 0)
            {
                num4 = points.Length - 2;
            }
        }
        Vector3 val = points[num2];
        Vector3 val2 = val - points[num3];
        if (bezierControlPointMode == BezierControlPointMode.Aligned)
        {
            val2 = ((Vector3)(ref val2)).normalized * Vector3.Distance(val, points[num4]);
        }
        points[num4] = val + val2;
    }

    public Vector3 GetPoint(float t, bool ConstantVelocity)
    {
        //IL_0013: Unknown result type (might be due to invalid IL or missing references)
        //IL_000b: Unknown result type (might be due to invalid IL or missing references)
        if (ConstantVelocity)
        {
            return GetPoint(getPathFromTime(t));
        }
        return GetPoint(t);
    }

    public Vector3 GetPoint(float t)
    {
        //IL_0008: Unknown result type (might be due to invalid IL or missing references)
        //IL_0054: Unknown result type (might be due to invalid IL or missing references)
        //IL_0062: Unknown result type (might be due to invalid IL or missing references)
        //IL_0070: Unknown result type (might be due to invalid IL or missing references)
        //IL_007e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0084: Unknown result type (might be due to invalid IL or missing references)
        //IL_0089: Unknown result type (might be due to invalid IL or missing references)
        if (points == null)
        {
            return Vector3.zero;
        }
        int num;
        if (t >= 1f)
        {
            t = 1f;
            num = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * (float)CurveCount;
            num = (int)t;
            t -= (float)num;
            num *= 3;
        }
        return ((Component)this).transform.TransformPoint(Bezier.GetPoint(points[num], points[num + 1], points[num + 2], points[num + 3], t));
    }

    public Vector3 GetPointLocal(float t)
    {
        //IL_0008: Unknown result type (might be due to invalid IL or missing references)
        //IL_004e: Unknown result type (might be due to invalid IL or missing references)
        //IL_005c: Unknown result type (might be due to invalid IL or missing references)
        //IL_006a: Unknown result type (might be due to invalid IL or missing references)
        //IL_0078: Unknown result type (might be due to invalid IL or missing references)
        //IL_007e: Unknown result type (might be due to invalid IL or missing references)
        if (points == null)
        {
            return Vector3.zero;
        }
        int num;
        if (t >= 1f)
        {
            t = 1f;
            num = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * (float)CurveCount;
            num = (int)t;
            t -= (float)num;
            num *= 3;
        }
        return Bezier.GetPoint(points[num], points[num + 1], points[num + 2], points[num + 3], t);
    }

    private Vector3 GetVelocity(float t)
    {
        //IL_0008: Unknown result type (might be due to invalid IL or missing references)
        //IL_0054: Unknown result type (might be due to invalid IL or missing references)
        //IL_0062: Unknown result type (might be due to invalid IL or missing references)
        //IL_0070: Unknown result type (might be due to invalid IL or missing references)
        //IL_007e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0084: Unknown result type (might be due to invalid IL or missing references)
        //IL_0089: Unknown result type (might be due to invalid IL or missing references)
        //IL_0094: Unknown result type (might be due to invalid IL or missing references)
        //IL_0099: Unknown result type (might be due to invalid IL or missing references)
        if (points == null)
        {
            return Vector3.zero;
        }
        int num;
        if (t >= 1f)
        {
            t = 1f;
            num = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * (float)CurveCount;
            num = (int)t;
            t -= (float)num;
            num *= 3;
        }
        return ((Component)this).transform.TransformPoint(Bezier.GetFirstDerivative(points[num], points[num + 1], points[num + 2], points[num + 3], t)) - ((Component)this).transform.position;
    }

    public Vector3 GetDirection(float t, bool ConstantVelocity)
    {
        //IL_0013: Unknown result type (might be due to invalid IL or missing references)
        //IL_000b: Unknown result type (might be due to invalid IL or missing references)
        if (ConstantVelocity)
        {
            return GetDirection(getPathFromTime(t));
        }
        return GetDirection(t);
    }

    public Vector3 GetDirection(float t)
    {
        //IL_0002: Unknown result type (might be due to invalid IL or missing references)
        //IL_0007: Unknown result type (might be due to invalid IL or missing references)
        //IL_000a: Unknown result type (might be due to invalid IL or missing references)
        Vector3 velocity = GetVelocity(t);
        return ((Vector3)(ref velocity)).normalized;
    }

    public void AddCurve()
    {
        //IL_0021: Unknown result type (might be due to invalid IL or missing references)
        //IL_0026: Unknown result type (might be due to invalid IL or missing references)
        //IL_005c: Unknown result type (might be due to invalid IL or missing references)
        //IL_005d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0082: Unknown result type (might be due to invalid IL or missing references)
        //IL_0083: Unknown result type (might be due to invalid IL or missing references)
        //IL_00a8: Unknown result type (might be due to invalid IL or missing references)
        //IL_00a9: Unknown result type (might be due to invalid IL or missing references)
        //IL_0114: Unknown result type (might be due to invalid IL or missing references)
        //IL_0119: Unknown result type (might be due to invalid IL or missing references)
        if (modes != null && points != null)
        {
            Vector3 val = points[points.Length - 1];
            Array.Resize(ref points, points.Length + 3);
            val.x += 1f;
            points[points.Length - 3] = val;
            val.x += 1f;
            points[points.Length - 2] = val;
            val.x += 1f;
            points[points.Length - 1] = val;
            Array.Resize(ref modes, modes.Length + 1);
            modes[modes.Length - 1] = modes[modes.Length - 2];
            EnforceMode(points.Length - 4);
            if (loop)
            {
                points[points.Length - 1] = points[0];
                modes[modes.Length - 1] = modes[0];
                EnforceMode(0);
            }
        }
    }

    public void RemoveLastCurve()
    {
        if (modes != null && points != null && points.Length > 4)
        {
            Array.Resize(ref points, points.Length - 3);
            Array.Resize(ref modes, modes.Length - 1);
        }
    }

    public void RemoveCurve(int index)
    {
        if (modes != null && points != null && points.Length > 4)
        {
            List<Vector3> list = points.ToList();
            int i;
            for (i = 4; i < points.Length && index - 3 > i; i += 3)
            {
            }
            for (int j = 0; j < 3; j++)
            {
                list.RemoveAt(i);
            }
            points = list.ToArray();
            int index2 = (i - 4) / 3;
            List<BezierControlPointMode> list2 = modes.ToList();
            list2.RemoveAt(index2);
            modes = list2.ToArray();
        }
    }

    public void Reset()
    {
        //IL_0018: Unknown result type (might be due to invalid IL or missing references)
        //IL_001d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0033: Unknown result type (might be due to invalid IL or missing references)
        //IL_0038: Unknown result type (might be due to invalid IL or missing references)
        //IL_004e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0053: Unknown result type (might be due to invalid IL or missing references)
        //IL_0069: Unknown result type (might be due to invalid IL or missing references)
        //IL_006e: Unknown result type (might be due to invalid IL or missing references)
        points = (Vector3[]?)(object)new Vector3[4]
        {
            new Vector3(0f, -1f, 0f),
            new Vector3(0f, -1f, 2f),
            new Vector3(0f, -1f, 4f),
            new Vector3(0f, -1f, 6f)
        };
        modes = new BezierControlPointMode[2];
    }
}
