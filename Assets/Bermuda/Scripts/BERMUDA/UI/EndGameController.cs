using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    public UnityEngine.UI.Button replayButton;

    private void Start()
    {
        replayButton.onClick.AddListener(() => { SceneManager.LoadScene("Take Fuel"); });
    }
}