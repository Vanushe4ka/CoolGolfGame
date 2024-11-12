using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System; // для использования делегатов и событий

public class SliderEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Slider slider;
    public Slider targetValue;
    public CanvasGroup group;
    // Событие нажатия
    public event Action OnSliderPressed;

    // Событие отпускания
    public event Action OnSliderReleased;

    // Событие изменения значения слайдера
    public event Action<float> OnSliderValueChanged;
    public void SetTargetSlider()
    {
        targetValue.gameObject.SetActive(true);
        targetValue.minValue = 0.1f;
        targetValue.maxValue = 1;
        targetValue.value = slider.value;
    }
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
