using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int lives = 3;
    [SerializeField] private bool atArena;
    public GameObject death;
    public TMP_Text deathText;
    public static GameManager Instance {
        get {
            if (!instance) {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return instance;
        }
    }

    public static LoadnSave StateManager {
        get {
            if (!stateManager) {
                stateManager = instance.GetComponent<LoadnSave>();
            }
            return stateManager;
        }
    }

    private static GameManager instance;

    private static LoadnSave stateManager = null;

    private static bool shouldLoad = false;

    void Awake() {
        if (instance && (instance.GetInstanceID() != GetInstanceID())) {
            Destroy(gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (shouldLoad) {
            StateManager.Load(Application.persistentDataPath + "/SaveGame.xml");
            shouldLoad = false;
        }
    }

    public void SaveGame() {
        Debug.Log(Application.persistentDataPath);
        StateManager.Save(Application.persistentDataPath + "/SaveGame.xml");
    }
    public void LoadGame() {
        if (lives <= 0) {
            LoadTitle();
            return;
        }
        shouldLoad = true;
        StateManager.Load(Application.persistentDataPath + "/SaveGame.xml");
        //if (atArena) {
        //    LoadArena();
        //}
        //else {
        //    LoadDungeon();
        //}
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void LoadEntrance() {
        SceneManager.LoadScene("Entrance");
    }

    public void LoadDungeon() {
        SceneManager.LoadScene("Dungeon");
    }

    public void LoadArena() {
        SceneManager.LoadScene("Arena");
    }

    public void LoadTitle() {
        SceneManager.LoadScene("Title");
    }
    
    public int GetLife() {
        return lives;
    }

    public void TakeLife() {
        death.SetActive(true);
        deathText.text = $"Lives: {GetLife()}";
    }

    public void MinusLife() {
        lives -= 1;
    }

}
