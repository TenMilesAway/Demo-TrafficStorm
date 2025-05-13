using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionHumanType { NorthL, NorthR, EastL, EastR, SouthL, SouthR, WestL, WestR }

[System.Serializable]
public class HumanPathPreset
{
    public DirectionHumanType startDirection;
    public MovementType movementType;

    public Transform startPoint;
    public Transform endPoint;
    public Transform turnCenter;
}

public class HumanPathPresetSystem : MonoBehaviour
{
    [SerializeField] private List<HumanPathPreset> HumanPaths = new List<HumanPathPreset>();

    private static readonly Dictionary<string, List<Vector3>> pathCache = new Dictionary<string, List<Vector3>>();

    #region Unity ÉúÃüÖÜÆÚ
    private void Awake()
    {
        PrecalculateAllPaths();
    }
    #endregion

    #region Main Methods
    private void PrecalculateAllPaths()
    {
        foreach (HumanPathPreset preset in HumanPaths)
        {
            List<Vector3> pathPoints = new List<Vector3>()
            {
                preset.startPoint.position,
                preset.turnCenter.position,
                preset.endPoint.position
            };
            string key = GetPathKey(preset.startDirection, preset.movementType);
            pathCache[key] = pathPoints;
        }
    }

    private static string GetPathKey(DirectionHumanType dir, MovementType moveType)
    {
        return $"{dir}_{moveType}";
    }

    public static List<Vector3> GetPath(DirectionHumanType startDir, MovementType moveType)
    {
        string key = GetPathKey(startDir, moveType);
        return pathCache.ContainsKey(key) ? pathCache[key] : null;
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (HumanPaths == null || HumanPaths.Count == 0)
            return;

        foreach (HumanPathPreset preset in HumanPaths)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(preset.startPoint.position, preset.turnCenter.position);
            Gizmos.DrawLine(preset.turnCenter.position, preset.endPoint.position);
        }
    }
#endif
}
