using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public string Name;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Text nameText;
    [SerializeField] SliderEvents sliderEvent;
    [SerializeField] ArrowController arrow;
    [SerializeField] Ball ball;
    [SerializeField] Ball ballPrefab;
    [SerializeField] LineController lineC;
    float throwingForce;
    [SerializeField] float forceCoef = 10;
    bool isNeedDrawLine = false;
    bool isBallMoving = false;

    [SerializeField] Transform throwingCameraTarget;
    [SerializeField] Transform upCameraTarget;
    Transform cameraTransform;
    bool aimingInProgress = true;
    public float camSpeed;

    public Vector2 prewMousePos;
    public float mouseSensyfity;

    [SerializeField]BallTarget ballTarget;
    [SerializeField] Transform playerPivot;
    float initCameraFieldOfView;
    float targetCameraFieldOfView;
    bool isSliderSelected;

    [SerializeField] GameObject goButton;
    public GameController gameController;
    bool isEnd;
    IEnumerator ActiveCanvas()
    {
        float time = 0;
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = time * 2;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }
    private void OnEnable()
    {

        arrow.HideArrow();
        aimingInProgress = true;
        goButton.SetActive(true);
        StartCoroutine(ActiveCanvas());
        AimingSliderSettings();
        LocateTarget();
    }
    public void GoButtonPressed()
    {
        goButton.SetActive(false);
        ThrowingSliderSettings();
        aimingInProgress = false;
    }
    void LocateTarget()
    {
        playerPivot.LookAt(ballTarget.transform.position, Vector3.up);
        lineC.PredictTrajectory(ball.transform.position, arrow.GetDirection() + Vector3.up * sliderEvent.slider.value, throwingForce, ballPrefab);
        ballTarget.SetPos(lineC.lastPoint);
    }
    private void Update()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetCameraFieldOfView, camSpeed * Time.deltaTime);
        if (aimingInProgress)
        {
            targetCameraFieldOfView -= Input.mouseScrollDelta.y;
            targetCameraFieldOfView = Mathf.Clamp(targetCameraFieldOfView, 10, 100);

            cameraTransform.position = Vector3.Lerp(cameraTransform.position, upCameraTarget.position, camSpeed * Time.deltaTime);
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, upCameraTarget.rotation, camSpeed * Time.deltaTime);
            if (Input.GetMouseButton(0) && !ballTarget.isSelected && !isSliderSelected)
            {
                Vector2 mousePos = Input.mousePosition;
                if (prewMousePos != Vector2.zero)
                {
                    upCameraTarget.position = new Vector3(upCameraTarget.position.x - (mousePos.x - prewMousePos.x)*mouseSensyfity, upCameraTarget.position.y, upCameraTarget.position.z - (mousePos.y - prewMousePos.y) * mouseSensyfity);
                    cameraTransform.position = Vector3.Lerp(cameraTransform.position, upCameraTarget.position, camSpeed * Time.deltaTime);
                }
                prewMousePos = mousePos;
            }
            if (Input.GetMouseButtonUp(0))
            {
                prewMousePos = Vector2.zero;
            }
            if (ballTarget.isSelected || isSliderSelected)
            {
                playerPivot.LookAt(ballTarget.transform.position, Vector3.up);
                lineC.PredictTrajectory(ball.transform.position, arrow.GetDirection() + Vector3.up * sliderEvent.slider.value, throwingForce, ballPrefab);
                ballTarget.SetPos(lineC.lastPoint);
            }
            
        }
        else
        {
            targetCameraFieldOfView = initCameraFieldOfView;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, throwingCameraTarget.position, camSpeed * Time.deltaTime);
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, throwingCameraTarget.rotation, camSpeed * Time.deltaTime);
        }
    }
    void Start()
    {
        cameraTransform = Camera.main.transform;
        initCameraFieldOfView = Camera.main.fieldOfView;
        targetCameraFieldOfView = initCameraFieldOfView;
        nameText.text = $"Player: {Name}";
        //sliderEvent.OnSliderValueChanged += ChangeThrowingForce;
        //sliderEvent.OnSliderReleased += Throw;
        //sliderEvent.OnSliderPressed += arrow.unhideArrow;
        
    }
    public void AimingSliderSettings()
    {
        sliderEvent.targetValue.gameObject.SetActive(false);

        sliderEvent.OnSliderValueChanged -= ChangeThrowingForce;
        sliderEvent.OnSliderReleased -= Throw;
        sliderEvent.OnSliderPressed -= arrow.unhideArrow;

        sliderEvent.slider.value = 0.75f;
        sliderEvent.slider.minValue = 0.25f;

        sliderEvent.OnSliderValueChanged += ChangeThrowingForceAim;
        sliderEvent.OnSliderPressed += SelectTarget;
        sliderEvent.OnSliderReleased += DeselectTarget;
        ChangeThrowingForceAim(0.75f);
    }
    public void ThrowingSliderSettings()
    {
        sliderEvent.OnSliderValueChanged -= ChangeThrowingForceAim;
        sliderEvent.OnSliderPressed -= SelectTarget;
        sliderEvent.OnSliderReleased -= DeselectTarget;

        sliderEvent.SetTargetSlider();
        sliderEvent.slider.minValue = 0.1f;
        sliderEvent.slider.value = 0.1f;

        sliderEvent.OnSliderValueChanged += ChangeThrowingForce;
        sliderEvent.OnSliderReleased += Throw;
        sliderEvent.OnSliderPressed += arrow.unhideArrow;
    }
    void SelectTarget()
    {
        isSliderSelected = true;
    }
    void DeselectTarget()
    {
        isSliderSelected = false;
    }
    void ChangeThrowingForceAim(float newForce)
    {
        throwingForce = newForce * forceCoef;
    }
    void ChangeThrowingForce(float newForce)
    {
        throwingForce = newForce * forceCoef;
        arrow.ChangeArrowWithForce(newForce);
    }
    void Throw()
    {
        if (!isBallMoving)
        {
            StartCoroutine(waitToBallStop());
            ball.Throw(arrow.GetDirection() + Vector3.up * sliderEvent.slider.value, throwingForce);
            arrow.HideArrow();
        }
        sliderEvent.slider.value = 0;
    }
    IEnumerator waitToBallStop()
    {
        isBallMoving = true;
        yield return new WaitForSeconds(lineC.poitsCount * lineC.timeStep);
        isEnd = ball.isEnd;
        ball.StopBall();
        isBallMoving = false;
        transform.position = ball.gameObject.transform.position;
        ball.transform.localPosition = Vector3.zero;
        gameController.SwitchPlayer();
    }
    // Update is called once per frame
    
}
