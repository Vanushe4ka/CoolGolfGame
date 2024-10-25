using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform arrow;  // Трансформ стрелки
    public const int maxRotateSpeed = 360;
    public float rotationSpeed = 0f; // Скорость вращения стрелки (градусов в секунду)
    public float maxRotationAngle = 45f; // Максимальный угол отклонения от центрального направления (в градусах)

    private bool rotatingRight = true; // Направление вращения стрелки
    private float currentAngle = 0f; // Текущий угол

    void Update()
    {
        // Рассчитаем направление вращения
        float rotationStep = rotationSpeed * Time.deltaTime; // Шаг вращения за кадр

        if (rotatingRight)
        {
            currentAngle += rotationStep; // Вращаем вправо
            if (currentAngle >= maxRotationAngle)
            {
                rotatingRight = false; // Меняем направление
                currentAngle = maxRotationAngle; // Ограничим угол
            }
        }
        else
        {
            currentAngle -= rotationStep; // Вращаем влево
            if (currentAngle <= -maxRotationAngle)
            {
                rotatingRight = true; // Меняем направление
                currentAngle = -maxRotationAngle; // Ограничим угол
            }
        }

        // Применяем вращение к стрелке вокруг оси Y
        arrow.localRotation = Quaternion.Euler(0, currentAngle, 0);
    }
    public void ChangeArrowWithForce(float force)
    {
        rotationSpeed = maxRotateSpeed * Mathf.Pow(force, 2);
        arrow.localScale = new Vector3(1, 1, force);
    }
    public void HideArrow()
    {
        gameObject.SetActive(false);
    }
    public void unhideArrow()
    {
        gameObject.SetActive(true);
        ChangeArrowWithForce(0);
    }
    // Метод, чтобы получить текущий вектор направления, на который указывает стрелка
    public Vector3 GetDirection()
    {
        // Возвращаем направление по оси Z стрелки (стрелка направлена вдоль местной оси Z)
        return arrow.forward;
    }
}
