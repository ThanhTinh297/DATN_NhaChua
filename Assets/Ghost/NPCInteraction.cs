using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCInteraction : MonoBehaviour
{
    public AudioClip soundClip;           // Âm thanh khi người chơi gần
    public float riseHeight = 5f;         // Độ cao bay lên
    public float riseSpeed = 2f;          // Tốc độ bay
    public float resetDelay = 30f;        // Delay trước khi reset
    private AudioSource audioSource;
    private bool triggered = false;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;

        // Cài đặt AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            audioSource.Play();             // Phát âm thanh
            StartCoroutine(RiseAndReset());
        }
    }

    IEnumerator RiseAndReset()
    {
        float targetY = initialPosition.y + riseHeight;
        // Bay lên dần dần
        while (transform.position.y < targetY)
        {
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            yield return null;
        }

        // Chờ audio kết thúc
        yield return new WaitForSeconds(audioSource.clip.length);

        // Chờ thêm delay 30 giây trước khi reset
        yield return new WaitForSeconds(resetDelay);

        // Reset cảnh về trạng thái ban đầu
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
