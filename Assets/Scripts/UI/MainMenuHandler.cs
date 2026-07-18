using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] GameObject _gameHandlerPrefab;
    void Awake()
    {
        if (!GameHandler.s_instance)
        {
            Instantiate(_gameHandlerPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }
}
