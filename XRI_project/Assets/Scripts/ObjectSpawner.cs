using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform spawnPoint;
    public XRNode controllerNode = XRNode.RightHand;
    public float spawnCooldown = 1.0f;
    private bool _canSpawn = true;


    // Update is called once per frame
    void Update()
    {
        if (_canSpawn && IsAButtonPressed()) {
            StartCoroutine(SpawnObjectWithCooldown());
        }       
    }

    bool IsAButtonPressed() {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        bool buttonPressed= false;
        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressed) && buttonPressed) {
            return true;
        }
        return false;
    }

    IEnumerator SpawnObjectWithCooldown() {
        _canSpawn = false;
        SpawnObject();
        yield return new WaitForSeconds(spawnCooldown);
        _canSpawn = true;
    }

    void SpawnObject() {
        if (objectPrefab != null && spawnPoint != null) {
            GameObject spawnedObject = Instantiate(objectPrefab);

        }
    }
}

