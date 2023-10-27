using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneName
{
    MainMenuScene,
    HowToPlayScene,
    GameScene
}

[RequireComponent(typeof(Button))]
public class ChangeLevelButton : MonoBehaviour
{
    [SerializeField] private SceneName sceneToLoad;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(sceneToLoad.ToString());
        });
    }
}
