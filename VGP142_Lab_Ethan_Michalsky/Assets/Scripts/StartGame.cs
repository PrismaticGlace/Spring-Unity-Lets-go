using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button star;

    void Update() {
        star.onClick.AddListener(() => SceneManager.LoadScene("Entrance"));
    }
}
