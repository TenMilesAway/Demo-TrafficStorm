using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneratePool : MonoBehaviour
{
    [SerializeField] private List<string> VehicleConfigs = new List<string>();
    [SerializeField] private List<string> HumanConfigs   = new List<string>();

    [Header("��Ϸ����")]
    [SerializeField] [Tooltip("��ʼ���ɼ��")] private float initialSpawnInterval = 5f;
    [SerializeField] [Tooltip("��С���ɼ��")] private float minSpawnInterval = 0.2f;
    [SerializeField] [Tooltip("�Ѷ����Ӽ��")] private float difficultyIncreaseInterval = 5f;
    [SerializeField] [Tooltip("�������ϵ��")] [Range(0.1f, 0.99f)] private float spawnIntervalMultiplier = 0.9f;

    private float currentSpawnInterval;
    private float difficultyTimer;

    #region Unity ��������
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

    #region Main Methods
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // ���������߼�
            GenerateVehicle();
            GenerateHuman();
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    public void GenerateVehicle()
    {
        int index = Random.Range(0, VehicleConfigs.Count);

        if (VehicleConfigs.Count != 0) VehicleFactory.CreateProduct(VehicleConfigs[index]);
    }

    public void GenerateHuman()
    {
        int index = Random.Range(0, HumanConfigs.Count);

        if (VehicleConfigs.Count != 0) HumanFactory.CreateProduct(HumanConfigs[index]);
    }
    #endregion
}
