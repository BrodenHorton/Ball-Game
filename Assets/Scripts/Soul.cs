using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField] int value = 5;
    Timer pickupEnabledTimer;
    private void Awake()
    {
        pickupEnabledTimer = new Timer(1);
    }
    private void Update()
    {
        pickupEnabledTimer.Update();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (pickupEnabledTimer.IsFinished() && other.transform.GetParentOrSelf().CompareTag("Player"))
        {
            GameManager.Instance.GainSoul(value);
            Destroy(gameObject);
        }
    }
}
