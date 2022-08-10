using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ControllerView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI total;

    [SerializeField]
    private TextMeshProUGUI keyPerSec;

    [SerializeField]
    private KeyView[] keyViews = new KeyView[GameDefine.KEY_NUM];

    [SerializeField]
    private ScratchView scratchL;
    [SerializeField]
    private ScratchView scratchR;

    [Header("PlaySide")]
    [SerializeField]
    private RectTransform keyRectTransform;
    [SerializeField]
    private RectTransform scratchRectTransform;

    [SerializeField]
    private RectTransform[] keyPositions;
    [SerializeField]
    private RectTransform[] scratchPositions;
    
    private ReactiveProperty<Color> color = new ReactiveProperty<Color>();

    public void SetTotal(int count)
    {
        total.text = count.ToString();
    }

    public void SetKeyPerSec(int count)
    {
        keyPerSec.text = count + " key/s";
    }

    public void SetButtonState(int index, bool active)
    {
        keyViews[index].Active.SetActive(active);
    }

    public void SetStateScratchL(bool active)
    {
        scratchL.Active.SetActive(active);
    }
    
    public void SetStateScratchR(bool active)
    {
        scratchR.Active.SetActive(active);
    }
    
    public void SetColor(Color color)
    {
        foreach (var key in keyViews)
        {
            var image = key.Active.GetComponent<Image>();
            image.color = color;
        }
        scratchL.Active.GetComponent<Image>().color = color;
        scratchR.Active.GetComponent<Image>().color = color;
    }

    public void ApplyPlaySide(PlaySide side)
    {
        keyRectTransform.position = keyPositions[(int) side].position;
        scratchRectTransform.position = scratchPositions[(int) side].position;
    }
}
