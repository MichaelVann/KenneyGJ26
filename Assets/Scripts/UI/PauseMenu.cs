using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button _restartButton;
    [SerializeField] Slider[] _volumeSliders;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < (int)AudioManager.eSoundChannel.Count; i++)
        {
            _volumeSliders[i].value = AudioManager.s_instance.GetChannelVolume((AudioManager.eSoundChannel)i);
        }
    }


    private void OnEnable()
    {
        // _restartButton.interactable = !BattleUIHandler.s_instance.GetTransitioningToRest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Quit()
    {
        GameHandler.s_instance.QuitGame();
    }

    public void Restart()
    {
        BattleHandler.s_instance.RestartLevel();
    }

    public void Close()
    {
        BattleUIHandler.s_instance.SetPauseMenuOpened(false);
    }

    public void OnVolumeSliderChanged(int a_channel)
    {
        AudioManager.s_instance.SetChannelVolume((AudioManager.eSoundChannel)a_channel, _volumeSliders[a_channel].value);
    }
}
