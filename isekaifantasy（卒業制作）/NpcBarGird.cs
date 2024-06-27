using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// RawImageを継承し、hpスライダーのfillに追加
/// SliderのValueによってBarの刻み数を変える。
/// TextureのFilterModeはBilinearに設定してください。
/// </summary>
public class NpcBarGird : RawImage
{
    private Slider slider;
    private float value;

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        if (slider == null)
        {
            slider = transform.parent.parent.GetComponent<Slider>();
        }
        if (slider != null)
        {
            value = slider.value / 200;
            uvRect = new Rect(0, 0, value, 1);
        }

    }
}
