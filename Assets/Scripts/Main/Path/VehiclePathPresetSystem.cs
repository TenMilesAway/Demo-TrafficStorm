using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ʻ�뷽��
/// </summary>
public enum DirectionType { North, East, South, West }
/// <summary>
/// �ƶ�����
/// </summary>
public enum MovementType { Straight, LeftTurn, RightTurn }

[System.Serializable]
public class PathPreset
{
    public DirectionType startDirection;
    public MovementType movementType;

    public Transform startPoint;
    public Transform endPoint;
    /// <summary>
    /// ����ת��뾶
    /// </summary>
    public float radius;
    /// <summary>
    /// ����ת��ο���
    /// </summary>
    public Transform turnCenter;
}

public class VehiclePathPresetSystem : MonoBehaviour
{
    [SerializeField] private List<PathPreset> VehiclePaths = new List<PathPreset>();
    [SerializeField] [field: Range(1f, 40f)] [Tooltip("��ֵ��")] private int curveResolution;

    private static readonly Dictionary<string, List<Vector3>> pathCache = new Dictionary<string, List<Vector3>>();

    #region Unity ��������
    private void Awake()
    {
        PrecalculateAllPaths();
    }
    #endregion

    #region Main Methods
    private void PrecalculateAllPaths()
    {
        foreach (PathPreset preset in VehiclePaths)
        {
            List<Vector3> pathPoints = new List<Vector3>();

            // ֱ��·��
            if (preset.movementType == MovementType.Straight)
            {
                pathPoints.Add(preset.startPoint.position);
                pathPoints.Add(preset.endPoint.position);
            }
            // ת��·��
            else
            {
                pathPoints = CalculateTurnPath(
                    preset.startPoint.position,
                    preset.turnCenter.position,
                    preset.endPoint.position,
                    preset.radius,
                    curveResolution
                );
            }

            string key = GetPathKey(preset.startDirection, preset.movementType);
            pathCache[key] = pathPoints;
        }
    }

    private List<Vector3> CalculateTurnPath(Vector3 start, Vector3 mid, Vector3 end, float radius, int resolution)
    {
        List<Vector3> pathPoints = new List<Vector3>();

        // 1. ���㷽������
        Vector3 v1 = mid - start;
        Vector3 v2 = end - mid;
        v1 = Normal(v1);
        v2 = Normal(v2);

        // 2. �����е�
        Vector3 turnStart = mid - v1 * radius;
        Vector3 turnEnd   = mid + v2 * radius;

        // 3. ����Բ��
        // �ȼ�����ת����
        float crossY = v1.x * v2.z - v1.z * v2.x;
        // <0 ˳ʱ��; >0 ��ʱ��;
        // ��ֱ����
        Vector3 perp;
        if (crossY < 0f)
            perp = new Vector3(v1.z, 0, -v1.x); // ˳ʱ��
        else
            perp = new Vector3(-v1.z, 0, v1.x); // ��ʱ��
        Vector3 center = turnStart + perp * radius;

        // 4. ������
        pathPoints.Add(start);
        pathPoints.Add(turnStart);

        // 5. ����Բ��
        float startAngle = Mathf.Atan2(turnStart.z - center.z, turnStart.x - center.x);
        float endAngle   = Mathf.Atan2(turnEnd.z   - center.z, turnEnd.x   - center.x);
        if (crossY < 0f)
        {
            if (endAngle > startAngle)
                endAngle -= 2f * Mathf.PI;
        }
        else
        {
            if (endAngle < startAngle)
                endAngle += 2f * Mathf.PI;
        }

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            float ang = Mathf.Lerp(startAngle, endAngle, t);
            float x = Mathf.Cos(ang) * radius + center.x;
            float z = Mathf.Sin(ang) * radius + center.z;
            pathPoints.Add(new Vector3(x, 0f, z));
        }

        // 6. ����յ�
        pathPoints.Add(turnEnd);
        pathPoints.Add(end);

        return pathPoints;
    }

    private Vector3 Normal(Vector3 v)
    {
        v.y = 0;
        v.Normalize();
        return v;
    }

    private static string GetPathKey(DirectionType dir, MovementType moveType)
    {
        return $"{dir}_{moveType}";
    }

    public static List<Vector3> GetPath(DirectionType startDir, MovementType moveType)
    {
        string key = GetPathKey(startDir, moveType);
        return pathCache.ContainsKey(key) ? pathCache[key] : null;
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (VehiclePaths == null || VehiclePaths.Count == 0)
            return;

        foreach (PathPreset preset in VehiclePaths)
        {
            if (preset.movementType == MovementType.Straight)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(preset.startPoint.position, preset.endPoint.position);
            }
            else
            {
                Gizmos.color = Color.green;
                List<Vector3> pathPoints = new List<Vector3>();
                pathPoints = CalculateTurnPath(
                    preset.startPoint.position,
                    preset.turnCenter.position,
                    preset.endPoint.position,
                    preset.radius,
                    curveResolution
                );

                for (int i = 0; i < pathPoints.Count - 1; i++)
                {
                    Gizmos.DrawLine(pathPoints[i], pathPoints[i + 1]);
                }
            }
        }
    }
#endif
}
