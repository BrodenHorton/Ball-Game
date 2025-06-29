using UnityEngine;

public class SceneChangeButton : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManagement.Instance.LoadScene(sceneName);
    }
}
