using UnityEngine;
using UnityEngine.UI;

public class TriggerTipText : MonoBehaviour
{
    [Header("全局共用提示UI面板（所有物体拖同一个）")]
    public GameObject globalTipPanel;
    [Header("当前物体触发显示的文字内容")]
    [TextArea] public string tipContent;

    private Text tipText;

    void Start()
    {
        // 获取UI文字组件
        if (globalTipPanel != null)
        {
            tipText = globalTipPanel.GetComponentInChildren<Text>();
            globalTipPanel.SetActive(false);
        }
    }

    // 玩家进入触发范围
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && globalTipPanel != null && tipText != null)
        {
            // 替换为本物体设置的文字
            tipText.text = tipContent;
            globalTipPanel.SetActive(true);
        }
    }

    // 玩家离开触发范围
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && globalTipPanel != null)
        {
            globalTipPanel.SetActive(false);
        }
    }
}