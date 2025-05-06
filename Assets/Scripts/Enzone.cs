using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class Enzone : MonoBehaviour
{
    public Transform playerCamera;     
    public float triggerDistance = 1.5f;
    private bool triggered = false;
    public GameObject winCanvas;
    public Button restartButton;   
    void Start()
    {
        //Debug.Log("Enzone script started.");
    }
    void Update()  {
        if (triggered || playerCamera == null)
            return;

        float dist = Vector3.Distance(playerCamera.position, transform.position);

        if (dist < triggerDistance)
        {
            triggered = true;

            if (winCanvas != null)
            {
                winCanvas.SetActive(true);
            }

            if (restartButton != null)
            {
                restartButton.onClick.RemoveAllListeners(); // Prevent stacking listeners
                restartButton.onClick.AddListener(RestartScene);
            }
        }
    }
    public void RestartScene()
    {
        Debug.Log("Reloading scene...");
        
        GameObject voiceSystem = GameObject.Find("Voice2Action");
        if (voiceSystem != null)
        {
            DontDestroyOnLoad(voiceSystem);
        }

        Time.timeScale = 1f; // Just in case it was paused
        Scene current = SceneManager.GetActiveScene();
        Debug.Log("Current Scene: " + current.name);
        //SceneManager.LoadScene("StarterScene-Complete", LoadSceneMode.Single);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}