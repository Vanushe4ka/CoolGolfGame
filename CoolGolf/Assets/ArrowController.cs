using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform arrow;  // ��������� �������
    public const int maxRotateSpeed = 360;
    public float rotationSpeed = 0f; // �������� �������� ������� (�������� � �������)
    public float maxRotationAngle = 45f; // ������������ ���� ���������� �� ������������ ����������� (� ��������)

    private bool rotatingRight = true; // ����������� �������� �������
    private float currentAngle = 0f; // ������� ����

    void Update()
    {
        // ���������� ����������� ��������
        float rotationStep = rotationSpeed * Time.deltaTime; // ��� �������� �� ����

        if (rotatingRight)
        {
            currentAngle += rotationStep; // ������� ������
            if (currentAngle >= maxRotationAngle)
            {
                rotatingRight = false; // ������ �����������
                currentAngle = maxRotationAngle; // ��������� ����
            }
        }
        else
        {
            currentAngle -= rotationStep; // ������� �����
            if (currentAngle <= -maxRotationAngle)
            {
                rotatingRight = true; // ������ �����������
                currentAngle = -maxRotationAngle; // ��������� ����
            }
        }

        // ��������� �������� � ������� ������ ��� Y
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
    // �����, ����� �������� ������� ������ �����������, �� ������� ��������� �������
    public Vector3 GetDirection()
    {
        // ���������� ����������� �� ��� Z ������� (������� ���������� ����� ������� ��� Z)
        return arrow.forward;
    }
}
