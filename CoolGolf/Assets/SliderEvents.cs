using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System; // для использования делегатов и событий

public class SliderEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Slider slider;

    // Событие нажатия
    public event Action OnSliderPressed;

    // Событие отпускания
    public event Action OnSliderReleased;

    // Событие изменения значения слайдера
    public event Action<float> OnSliderValueChanged;

    // Вызывается при нажатии на слайдер
    public void OnPointerDown(PointerEventData eventData)
    {
        // Запускаем событие OnSliderPressed, если на него кто-то подписан
        OnSliderPressed?.Invoke();
        OnValueChanged(slider.value);
    }

    // Вызывается при отпускании слайдера
    public void OnPointerUp(PointerEventData eventData)
    {
        // Запускаем событие OnSliderReleased, если на него кто-то подписан
        OnSliderReleased?.Invoke();
        slider.value = 0;
    }

    // Этот метод будет срабатывать при изменении значения слайдера
    public void OnValueChanged(float value)
    {
        // Запускаем событие OnSliderValueChanged, если на него кто-то подписан
        OnSliderValueChanged?.Invoke(value);
    }

    void Start()
    {
        if (slider != null)
        {
            // Подписка на стандартное изменение значения слайдера
            slider.onValueChanged.AddListener(OnValueChanged);
        }
    }
}
