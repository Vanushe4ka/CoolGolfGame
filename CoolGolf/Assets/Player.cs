using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SliderEvents slider;
    [SerializeField] ArrowController arrow;
    [SerializeField] Ball ball;
    float throwingForce;
    void Start()
    {
        arrow.HideArrow();
        slider.OnSliderValueChanged += ChangeThrowingForce;
        slider.OnSliderReleased += Throw;
        slider.OnSliderPressed += arrow.unhideArrow;
    }
    void ChangeThrowingForce(float newForce)
    {
        throwingForce = newForce;
        arrow.ChangeArrowWithForce(newForce);
    }
    void Throw()
    {
        ball.Throw(throwingForce, arrow.GetDirection());
        arrow.HideArrow();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
