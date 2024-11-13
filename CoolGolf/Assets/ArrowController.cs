using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform arrow;  // ��������� �������
    public Transform ball;
    public const int maxRotateSpeed = 180;
    public float rotationSpeed = 0f; // �������� �������� ������� (�������� � �������)
    public float maxRotationAngle = 45f; // ������������ ���� ���������� �� ������������ ����������� (� ��������)

    private bool rotatingRight = true; // ����������� �������� �������
    private float currentAngle = 0f; // ������� ����
    [SerializeField] Player player;

    void Update()
    {
        transform.position = ball.position;
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
        rotationSpeed = maxRotateSpeed * Mathf.Pow(force + 0.5f, 2);
        arrow.localScale = new Vector3(1, 1, force + 0.5f);
    }
    public void HideArrow()
    {
        gameObject.SetActive(false);
    }
    public void unhideArrow()
    {
        if (!player.isBallMoving)
        {

            gameObject.SetActive(true);
            ChangeArrowWithForce(0);
        }
    }
    // �����, ����� �������� ������� ������ �����������, �� ������� ��������� �������
    public Vector3 GetDirection()
    {
        // ���������� ����������� �� ��� Z ������� (������� ���������� ����� ������� ��� Z)
        return arrow.forward;
    }
    public void ResetArrowDirection()
    {
        arrow.localRotation = Quaternion.Euler(0,0,0);
    }
}
