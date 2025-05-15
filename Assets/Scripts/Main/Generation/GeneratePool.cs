using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneratePool : MonoBehaviour
{
    [SerializeField] private string VehicleRootPath;
    [SerializeField] private string HumanRootPath;

    [SerializeField] private List<string> VehicleConfigs = new List<string>();
    [SerializeField] private List<string> HumanConfigs   = new List<string>();

    [Header("游戏变量")]
    [SerializeField] [Tooltip("初始生成间隔")] private float initialSpawnInterval = 5f;
    [SerializeField] [Tooltip("最小生成间隔")] private float minSpawnInterval = 0.2f;
    [SerializeField] [Tooltip("难度增加间隔")] private float difficultyIncreaseInterval = 5f;
    [SerializeField] [Tooltip("间隔缩减系数")] [Range(0.1f, 0.99f)] private float spawnIntervalMultiplier = 0.9f;

    private float currentSpawnInterval;
    private float difficultyTimer;
    private bool canGenerate = false;

    private static GeneratePool instance;
    public static GeneratePool GetInstance()
    {
        return instance;
    }

    #region Unity 生命周期
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnRoutine());
    }

    private void Update()
    {
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= difficultyIncreaseInterval)
        {
            difficultyTimer = 0f;
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval * spawnIntervalMultiplier);
        }
    }
    #endregion

    #region Init Methods
    public void InitData()
    {
        canGenerate = true;
    }
    #endregion

    #region Main Methods
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 调用生成逻辑
            if (!canGenerate)
            {
                yield return null;
                continue;
            }
            GenerateVehicle();
            GenerateHuman();
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    public void GenerateVehicle()
    {
        int index = Random.Range(0, VehicleConfigs.Count);

        if (VehicleConfigs.Count != 0) VehicleFactory.CreateProduct(VehicleRootPath + VehicleConfigs[index]);
    }

    public void GenerateHuman()
    {
        int index = Random.Range(0, HumanConfigs.Count);

        if (VehicleConfigs.Count != 0) HumanFactory.CreateProduct(HumanRootPath + HumanConfigs[index]);
    }

    public void StopGenerateForSeconds(float seconds)
    {
        StopGenerateForSecondsCo(seconds);
    }

    public IEnumerator StopGenerateForSecondsCo(float seconds)
    {
        StopGenerate();

        yield return new WaitForSeconds(seconds);

        StartGenerate();
    }

    public void StartGenerate() => canGenerate = true;
    public void StopGenerate() => canGenerate = false;

    #endregion
}
