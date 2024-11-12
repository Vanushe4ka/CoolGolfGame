using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System; // ��� ������������� ��������� � �������

public class SliderEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Slider slider;
    public Slider targetValue;
    public CanvasGroup group;
    // ������� �������
    public event Action OnSliderPressed;

    // ������� ����������
    public event Action OnSliderReleased;

    // ������� ��������� �������� ��������
    public event Action<float> OnSliderValueChanged;
    public void SetTargetSlider()
    {
        targetValue.gameObject.SetActive(true);
        targetValue.minValue = 0.1f;
        targetValue.maxValue = 1;
        targetValue.value = slider.value;
    }
    // ���������� ��� ������� �� �������
    public void OnPointerDown(PointerEventData eventData)
    {
        // ��������� ������� OnSliderPressed, ���� �� ���� ���-�� ��������
        OnSliderPressed?.Invoke();
        OnValueChanged(slider.value);
    }

    // ���������� ��� ���������� ��������
    public void OnPointerUp(PointerEventData eventData)
    {
        // ��������� ������� OnSliderReleased, ���� �� ���� ���-�� ��������
        OnSliderReleased?.Invoke();
    }

    // ���� ����� ����� ����������� ��� ��������� �������� ��������
    public void OnValueChanged(float value)
    {
        // ��������� ������� OnSliderValueChanged, ���� �� ���� ���-�� ��������
        OnSliderValueChanged?.Invoke(value);
    }

    void Start()
    {
        if (slider != null)
        {
            // �������� �� ����������� ��������� �������� ��������
            slider.onValueChanged.AddListener(OnValueChanged);
        }
    }
    public void HideUnhideSlider(bool isHide)
    {
        StartCoroutine(HideSlider(isHide));
    }
    
    public IEnumerator HideSlider(bool isHide)
    {
        slider.enabled = false;
        float time = 0.5f;
        float timer = 0;
        while (timer < time)
        {
            timer += Time.deltaTime;
            float t = timer / time; ;
            if (isHide)
            {
                group.alpha = 1 - t;
            }
            else
            {
                group.alpha = t;
            }
            yield return null;
        }
        if (isHide)
        {
            group.alpha = 0;
        }
        else
        {
            group.alpha = 1;
            slider.enabled = true;
        }
    }
}
