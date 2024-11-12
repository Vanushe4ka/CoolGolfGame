using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isSelected;
    Vector3 mouseEnterPoint;
    Vector3 posWhereMouseEntered;
    private void OnMouseDown()
    {
        isSelected = true;

        // �������� ������� ������� ������� � ������� �����������
        mouseEnterPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));

        // ��������� ������� �������, ��� ���� ��������
        posWhereMouseEntered = transform.position;
    }

    private void OnMouseDrag()
    {
        // �������� ������� ������� ���� � ������� �����������
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));

        // ��������� �������� ������ �� X � Z
        Vector3 mouseDelta = mousePos - mouseEnterPoint;

        // ��������� �������� � X � Z, Y ������� ����������
        transform.position = new Vector3(posWhereMouseEntered.x + mouseDelta.x, posWhereMouseEntered.y, posWhereMouseEntered.z + mouseDelta.z);
    }
    public void SetPos(Vector3 point)
    {
        transform.position = new Vector3(point.x, transform.position.y, point.z);
    }
    private void OnMouseUp()
    {
        isSelected = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
