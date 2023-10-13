using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGameButton : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
