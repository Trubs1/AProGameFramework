using UnityEngine;
using UnityEngine.UI;

public class FullBgBtnCtrl : MonoBehaviour
{
    public Button btn;
    private void Start()
    {
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
