using UnityEngine;
using Random = System.Random;

public class ItemSprite : MonoBehaviour {
    [SerializeField] private float totalTravelTime;
    [SerializeField] private float minAmplitude;
    [SerializeField] private float maxAmplitude;

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
        float amplitude = (float)rng.NextDouble() * (maxAmplitude - minAmplitude) + minAmplitude;
        curve = new QuadraticBezierCurve(startingPosition, new Vector3(startingPosition.x, startingPosition.y + amplitude, startingPosition.z), endingPosition);
        this.item = item;
        currentTravelTime = 0f;
        transform.position = startingPosition;
        isRunning = true;
    }
}
