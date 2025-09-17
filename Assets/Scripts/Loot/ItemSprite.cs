using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Collider))]
public class ItemSprite : MonoBehaviour {
    [SerializeField] private float totalTravelTime;
    [SerializeField] private float minAmplitudeMultiplier;
    [SerializeField] private float maxAmplitudeMultiplier;
    [SerializeField] private List<string> collidableLayers;

    private QuadraticBezierCurve curve;
    private GameObject item;
    private float currentTravelTime;
    private bool isRunning;

    // Temp
    private Random rng = new Random();

    private void Awake() {
        currentTravelTime = 0f;
        isRunning = false;
    }

    private void Update() {
        if (curve == null || isRunning == false)
            return;

        if (currentTravelTime >= totalTravelTime) {
            isRunning = false;
            Instantiate(item, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        currentTravelTime += Time.deltaTime;
        float t = currentTravelTime / totalTravelTime;
        transform.position = curve.GetPosition(t);
    }

    public void Activate(Vector3 startingPosition, Vector3 endingPosition, GameObject item) {
        float amplitude = (float)rng.NextDouble() * (maxAmplitudeMultiplier - minAmplitudeMultiplier) + minAmplitudeMultiplier;
        curve = new QuadraticBezierCurve(startingPosition, new Vector3((startingPosition.x + endingPosition.x) / 2, startingPosition.y + amplitude, (startingPosition.z + endingPosition.z) / 2), endingPosition);
        this.item = item;
        currentTravelTime = 0f;
        transform.position = startingPosition;
        isRunning = true;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger entered for ItemSprite.");
        Debug.Log("Other tag: " + other.gameObject.tag);
        if(isRunning && collidableLayers.Contains(other.gameObject.tag)) {
            isRunning = false;
            Instantiate(item, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
