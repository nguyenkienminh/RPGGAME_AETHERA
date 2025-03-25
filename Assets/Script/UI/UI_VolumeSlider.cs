using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parameter;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    public void SliderValue(float _value)
    {
        // Nếu giá trị quá thấp, chuyển sang mức dB tắt tiếng (-80 dB)
        if (_value <= 0.001f)
            audioMixer.SetFloat(parameter, -80f);
        else
            audioMixer.SetFloat(parameter, Mathf.Log10(_value) * multiplier);
    }

    public void LoadSlider(float _value)
    {
        slider.value = _value;
        SliderValue(_value); // Cập nhật AudioMixer ngay sau khi load
    }

}
