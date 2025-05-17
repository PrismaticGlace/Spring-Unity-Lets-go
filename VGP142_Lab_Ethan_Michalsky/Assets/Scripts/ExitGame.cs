using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    public Button exit;

    void Update() {
        exit.onClick.AddListener(() => doExitGame());
    }

    void doExitGame() {
        Application.Quit();
    }
}
