using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value = 5;
    Timer pickupEnabledTimer;
    private void Awake()
    {
        pickupEnabledTimer = new Timer(2);
    }
    private void Update()
    {
        pickupEnabledTimer.Update();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (pickupEnabledTimer.IsFinished() && other.transform.GetParentOrSelf().CompareTag("Player"))
        {
            GameManager.Instance.AddCoin(value);
            Destroy(gameObject);
        }
    }
}
