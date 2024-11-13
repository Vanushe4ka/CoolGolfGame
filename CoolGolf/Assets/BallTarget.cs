using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isSelected;
    Vector3 mouseEnterPoint;
    Vector3 posWhereMouseEntered;
    [SerializeField] Player player;
    private void OnMouseDown()
    {
        if (player.aimingInProgress)
        {
            isSelected = true;
            mouseEnterPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
            posWhereMouseEntered = transform.position;
        }
    }

    private void OnMouseDrag()
    {
        if (player.aimingInProgress)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
            Vector3 mouseDelta = mousePos - mouseEnterPoint;
            transform.position = new Vector3(posWhereMouseEntered.x + mouseDelta.x, posWhereMouseEntered.y, posWhereMouseEntered.z + mouseDelta.z);
        }
    }
    public void SetPos(Vector3 point)
    {
        transform.position = new Vector3(point.x, GameController.CalcGroundHeight(point), point.z);
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
