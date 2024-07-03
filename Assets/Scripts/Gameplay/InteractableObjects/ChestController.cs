using UnityEngine;

public class ChestController : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private GameObject lockGO;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip openAudioClip;
    [SerializeField]
    private AudioClip closeAudioClip;

    public bool isUnlocked = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void UnlockChest()
    {
        isUnlocked = true;
        Destroy(lockGO);
    }

    public void SwitchState()
    {
        if (!isUnlocked)
            return;

        if (animator.GetBool("IsOpen"))
        {
            animator.SetBool("IsOpen", false);
            audioSource.clip = closeAudioClip;
            audioSource.Play();
        }
        else
        {
            animator.SetBool("IsOpen", true);
            audioSource.clip = openAudioClip;
            audioSource.Play();
        }
    }
}
