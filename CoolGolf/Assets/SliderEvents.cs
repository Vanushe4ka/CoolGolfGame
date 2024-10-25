using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System; // ��� ������������� ��������� � �������

public class SliderEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Slider slider;

    // ������� �������
    public event Action OnSliderPressed;

    // ������� ����������
    public event Action OnSliderReleased;

    // ������� ��������� �������� ��������
    public event Action<float> OnSliderValueChanged;

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
        slider.value = 0;
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
}
