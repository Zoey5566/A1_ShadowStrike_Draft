using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerAudioFadeBlack : MonoBehaviour
{
    [Header("音频设置")]
    public AudioSource bgmAudioSource;
    public AudioClip enterBGM;
    [Header("延时设置")]
    public float waitTime = 3f;
    [Header("黑屏遮罩UI")]
    public Image blackMask;
    public float fadeSpeed = 1f;
    [Header("新增：完全黑屏后显示的UI面板")]
    public GameObject showUI;

    private Color maskColor;
    private bool triggered = false;

    void Start()
    {
        // 初始隐藏弹窗UI
        if (showUI != null)
        {
            showUI.SetActive(false);
        }

        if (blackMask != null)
        {
            maskColor = blackMask.color;
            maskColor.a = 0;
            blackMask.color = maskColor;
            blackMask.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 物体隐藏，直接退出，不执行任何音频、黑屏逻辑
        if (!gameObject.activeSelf)
            return;

        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            StartCoroutine(AudioAndBlackScreenCoroutine());
        }
    }

    IEnumerator AudioAndBlackScreenCoroutine()
    {
        // 仅物体显示触发时才停止旧音乐、切换新BGM
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Stop();
            if (enterBGM != null)
            {
                bgmAudioSource.clip = enterBGM;
                bgmAudioSource.Play();
            }
        }

        yield return new WaitForSeconds(waitTime);

        if (blackMask == null) yield break;
        blackMask.gameObject.SetActive(true);

        // 画面慢慢变黑
        while (maskColor.a < 1f)
        {
            maskColor.a += fadeSpeed * Time.deltaTime;
            maskColor.a = Mathf.Clamp01(maskColor.a);
            blackMask.color = maskColor;
            yield return null;
        }

        // 完全黑屏之后，弹出指定UI
        if (showUI != null)
        {
            showUI.SetActive(true);
        }
    }
}