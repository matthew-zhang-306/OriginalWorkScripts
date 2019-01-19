using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    public GameObject menuScreen;
    public GameObject mainMenu;
    public GameObject gameMenu;
    int homeScene;

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }

    void OnEnable() {
        SceneManager.activeSceneChanged += NewSceneLoaded;
    }
    void OnDisable() {
        SceneManager.activeSceneChanged -= NewSceneLoaded;
    }

    void Start() {
        SetMenuActive(1);
        homeScene = SceneManager.GetActiveScene().buildIndex;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            LoadScene(homeScene);
    }

    public static IEnumerator EndGameCoroutine(string finishText, float delay) {
        if (MainMenu.Instance == null) {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else {
            MainMenu.Instance.EndGame(finishText);
            yield return 0;
        }
    }

    void EndGame(string finishText) {
        gameMenu.GetComponentInChildren<Text>().text = finishText;
        SetMenuActive(2);
    }

    public void ReloadScene() {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int scene) {
        SceneManager.LoadScene(scene);
    }

    void NewSceneLoaded(Scene oldScene, Scene newScene) {
        if (newScene.buildIndex == homeScene)
            SetMenuActive(1);
        else
            SetMenuActive(0);
    }

    void SetMenuActive(int menuIndex) {
        menuScreen.SetActive(menuIndex != 0);
        mainMenu.SetActive(menuIndex == 1);
        gameMenu.SetActive(menuIndex == 2);
    }
}
