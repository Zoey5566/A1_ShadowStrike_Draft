using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonFadeTeleport : MonoBehaviour
{
    [Header("点击按钮后显示的物体（开局自动隐藏）")]
    public GameObject showObj;

    [Header("=== 新增：指定物体传送设置 ===")]
    [Tooltip("需要传送的目标物体")]
    public Transform targetObjToMove;
    [Tooltip("该物体传送到达的坐标")]
    public Vector3 targetObjPos;
    [Tooltip("该物体传送到达的旋转")]
    public Vector3 targetObjRot;

    [Header("白屏传送设置（玩家）")]
    public Image whiteMask;
    public Transform player;
    public Vector3 teleportPos;
    public Vector3 teleportRot;
    public float fadeSpeed = 1.2f;
    public float whiteHoldTime = 0.3f;

    private Color maskColor;
    private bool isProcessing = false;

    void Start()
    {
        // 开局自动隐藏目标显示物体
        if (showObj != null)
        {
            showObj.SetActive(false);
        }

        // 空值检测防报错
        if (whiteMask == null)
        {
            Debug.LogError("请赋值全屏白色遮罩Image", gameObject);
            return;
        }
        if (player == null)
        {
            Debug.LogError("请拖拽玩家角色", gameObject);
            return;
        }

        maskColor = whiteMask.color;
        maskColor.a = 0;
        whiteMask.color = maskColor;
        whiteMask.gameObject.SetActive(false);
    }

    // 按钮绑定1：点击显示物体
    public void ShowTargetObject()
    {
        if (showObj != null)
        {
            showObj.SetActive(true);
        }
    }

    // 按钮绑定2：白屏渐变 + 传送玩家 + 传送指定物体
    public void StartFadeAndTeleport()
    {
        if (isProcessing || whiteMask == null || player == null)
            return;

        StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine()
    {
        isProcessing = true;
        whiteMask.gameObject.SetActive(true);

        // 画面渐白
        while (maskColor.a < 1f)
        {
            maskColor.a += fadeSpeed * Time.deltaTime;
            maskColor.a = Mathf.Clamp01(maskColor.a);
            whiteMask.color = maskColor;
            yield return null;
        }

        yield return new WaitForSeconds(whiteHoldTime);

        // 1. 传送玩家
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        player.position = teleportPos;
        player.rotation = Quaternion.Euler(teleportRot);
        yield return new WaitForEndOfFrame();
        if (cc != null) cc.enabled = true;

        // 2. 新增：传送指定物体（不为空才执行）
        if (targetObjToMove != null)
        {
            targetObjToMove.position = targetObjPos;
            targetObjToMove.rotation = Quaternion.Euler(targetObjRot);
        }

        // 画面淡出
        while (maskColor.a > 0f)
        {
            maskColor.a -= fadeSpeed * Time.deltaTime;
            maskColor.a = Mathf.Clamp01(maskColor.a);
            whiteMask.color = maskColor;
            yield return null;
        }

        whiteMask.gameObject.SetActive(false);
        isProcessing = false;
    }
}