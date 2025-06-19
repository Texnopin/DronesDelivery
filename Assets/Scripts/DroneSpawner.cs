using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneSpawner : MonoBehaviour
{
    [Header("��������� ������")]
    public GameObject dronePrefab;

    [Header("���� �������")]
    public Transform redBase;
    public Transform blueBase;

    [Header("����� ������")]
    public Transform redSpawnPoint;
    public Transform blueSpawnPoint;

    [Header("���������")]
    public Material redFactionMaterial;
    public Material blueFactionMaterial;

    [Header("UI ��������")]
    public Slider factionSlider;
    public Slider speedSlider;
    public Toggle pathToggle;

    public Text redFactionResourceText;
    public Text blueFactionResourceText;

    [Header("������� ������� � ��������")]
    public List<RenderTexture> renderTextures; // 10 ������-�������
    public List<GameObject> monitorObjects;    // 10 ������� (�����)

    private List<GameObject> redDrones = new List<GameObject>();
    private List<GameObject> blueDrones = new List<GameObject>();

    private int redFactionResources = 0;
    private int blueFactionResources = 0;

    private float globalSpeed;

    private void Start()
    {
        factionSlider.onValueChanged.AddListener(OnFactionSliderChanged);
        speedSlider.onValueChanged.AddListener(OnSpeedSliderChanged);
        pathToggle.onValueChanged.AddListener(OnPathToggleChanged);

        globalSpeed = speedSlider.value;
        AdjustDrones((int)factionSlider.value);

        UpdateResourceUI();
    }

    private void OnFactionSliderChanged(float value)
    {
        AdjustDrones((int)value);
    }

    private void AdjustDrones(int targetCount)
    {
        AdjustFactionDrones(redDrones, targetCount, redSpawnPoint, redBase, redFactionMaterial);
        AdjustFactionDrones(blueDrones, targetCount, blueSpawnPoint, blueBase, blueFactionMaterial);
        UpdateMonitorDisplays();
    }

    private void AdjustFactionDrones(List<GameObject> droneList, int targetCount, Transform spawnPoint, Transform baseStation, Material factionMaterial)
    {
        while (droneList.Count > targetCount)
        {
            GameObject drone = droneList[droneList.Count - 1];
            droneList.RemoveAt(droneList.Count - 1);
            Destroy(drone);
        }

        while (droneList.Count < targetCount)
        {
            GameObject newDrone = Instantiate(dronePrefab, spawnPoint.position + Random.insideUnitSphere * 2f, Quaternion.identity);
            Drone droneScript = newDrone.GetComponent<Drone>();
            droneScript.Initialize(baseStation, factionMaterial, globalSpeed);

            if (baseStation == redBase)
            {
                droneScript.OnResourceCollected += () => AddResourceToFaction(true);
            }
            else if (baseStation == blueBase)
            {
                droneScript.OnResourceCollected += () => AddResourceToFaction(false);
            }

            droneList.Add(newDrone);

            LineRenderer lineRenderer = newDrone.GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.enabled = pathToggle.isOn;
            }
        }
    }

    private void AddResourceToFaction(bool isRedFaction)
    {
        if (isRedFaction)
        {
            redFactionResources++;
        }
        else
        {
            blueFactionResources++;
        }
        UpdateResourceUI();
    }

    private void UpdateResourceUI()
    {
        redFactionResourceText.text = $"Red: {redFactionResources}";
        blueFactionResourceText.text = $"Blue: {blueFactionResources}";
    }

    private void OnSpeedSliderChanged(float newSpeed)
    {
        globalSpeed = newSpeed;
        UpdateDroneSpeed(redDrones, newSpeed);
        UpdateDroneSpeed(blueDrones, newSpeed);
    }

    private void UpdateDroneSpeed(List<GameObject> droneList, float newSpeed)
    {
        foreach (var drone in droneList)
        {
            drone.GetComponent<Drone>().SetSpeed(newSpeed);
        }
    }

    private void OnPathToggleChanged(bool isPathVisible)
    {
        UpdateDronePathVisibility(redDrones, isPathVisible);
        UpdateDronePathVisibility(blueDrones, isPathVisible);
    }

    private void UpdateDronePathVisibility(List<GameObject> droneList, bool isPathVisible)
    {
        foreach (var drone in droneList)
        {
            LineRenderer lineRenderer = drone.GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.enabled = isPathVisible;
            }
        }
    }

    private void UpdateMonitorDisplays()
    {
        // �������� ����� ������ ������
        List<GameObject> allDrones = new List<GameObject>(redDrones);
        allDrones.AddRange(blueDrones);

        // ��������� ������-�������� ��� ����� ������
        for (int i = 0; i < monitorObjects.Count; i++)
        {
            if (i < allDrones.Count && i < renderTextures.Count)
            {
                Camera droneCamera = allDrones[i].GetComponentInChildren<Camera>();
                if (droneCamera != null)
                {
                    droneCamera.targetTexture = renderTextures[i];
                    MeshRenderer monitorRenderer = monitorObjects[i].GetComponent<MeshRenderer>();
                    if (monitorRenderer != null)
                    {
                        monitorRenderer.material.mainTexture = renderTextures[i];
                    }
                }
            }
            else
            {
                // ���������� ������, ���� ������ ������, ��� ���������
                MeshRenderer monitorRenderer = monitorObjects[i].GetComponent<MeshRenderer>();
                if (monitorRenderer != null)
                {
                    monitorRenderer.material.mainTexture = null;
                }
            }
        }
    }
}
