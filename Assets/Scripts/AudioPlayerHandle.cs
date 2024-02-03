using UnityEngine;

public class AudioPlayerHandle : MonoBehaviour
{
    [SerializeField] private AudioPlayer audioPlayerPrefab;
    
    private static AudioPlayerHandle _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (AudioPlayer.Instance == null)
        {
            AudioPlayer.Instance = Instantiate(audioPlayerPrefab, transform);
        }
    }
}
