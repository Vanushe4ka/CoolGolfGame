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
    public bool isBallMoving = false;

    [SerializeField] Transform throwingCameraTarget;
    [SerializeField] Transform upCameraTarget;
    [SerializeField] Transform ballCameraTarget;
    [SerializeField] Transform ballCameraRotationTarget;
    [SerializeField] float ballCameraTargetDistance;
    [SerializeField] float ballCameraTargetHeight;
    Transform cameraTransform;
    public bool aimingInProgress = true;
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
    public bool isEnd { get; private set; }

    [SerializeField] Text commentText;
    [SerializeField] CanvasGroup commentPanel;
    bool isOnGrass = false;

    [SerializeField] Animator animator;
    [SerializeField] string throwAnimStateName;
    [SerializeField] float throwAnimShift;
    [SerializeField] string putAnimStateName;
    [SerializeField] float putAnimShift;

    bool isNeedThrowAnimate = false;

    [SerializeField] SkinnedMeshRenderer skinMeshRenderer;
    [SerializeField] Material[] materials;
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
    public void SetSkin(int i)
    {
        skinMeshRenderer.material = materials[i % materials.Length];
    }
    private void OnEnable()
    {

        arrow.HideArrow();
        aimingInProgress = true;
        goButton.SetActive(true);
        StartCoroutine(ActiveCanvas());
        AimingSliderSettings();
        LocateTarget();
        isOnGrass = false;
        ball.SetSleepThreshold(1);
        upCameraTarget.position = new Vector3(transform.position.x, upCameraTarget.position.y, transform.position.z);
        if (gameController != null && Vector3.Distance(transform.position,gameController.GetLunkaPos()) < gameController.minDistanceToAiming)
        {
            isOnGrass = true;
            throwingForce = 0;
            lineC.HideLine();
            ballTarget.SetPos(gameController.GetLunkaPos());
            GoButtonPressed();
            ball.SetSleepThreshold(0.1f);
            sliderEvent.targetValue.gameObject.SetActive(false);
        }
    }
    public void GoButtonPressed()
    {
        LookAtTarget();
        goButton.SetActive(false);
        ThrowingSliderSettings();
        aimingInProgress = false;
    }
    void LocateTarget()
    {
        lineC.DrawLine(ball.transform.position, arrow.GetDirection() + CalcUpDirection(), throwingForce, ballPrefab);
        ballTarget.SetPos(lineC.lastPoint);
        LookAtTarget();
    }
    void LookAtTarget()
    {
        Vector3 targetPosition = ballTarget.transform.position;
        targetPosition.y = playerPivot.position.y;
        playerPivot.LookAt(targetPosition, Vector3.up);
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
                    Vector3 newTarget = new Vector3(upCameraTarget.position.x - (mousePos.x - prewMousePos.x) * mouseSensyfity, upCameraTarget.position.y, upCameraTarget.position.z - (mousePos.y - prewMousePos.y) * mouseSensyfity);
                    if (gameController.IsPointInBounds(newTarget))
                    {
                        upCameraTarget.position = newTarget;
                    }
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
                LookAtTarget();
                lineC.DrawLine(ball.transform.position, arrow.GetDirection() + CalcUpDirection(), throwingForce, ballPrefab);
                ballTarget.SetPos(lineC.lastPoint);
                
            }
            
        }
        else if (!isBallMoving)
        {
            targetCameraFieldOfView = initCameraFieldOfView;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, throwingCameraTarget.position, camSpeed * Time.deltaTime);
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, throwingCameraTarget.rotation, camSpeed * Time.deltaTime);
            if (isOnGrass)
            {
                lineC.DrawLine(ball.transform.position, arrow.GetDirection() + CalcUpDirection(), throwingForce, ballPrefab);
            }
            if (isNeedThrowAnimate)
            {
                if (!isOnGrass)
                {
                    animator.Play(throwAnimStateName , 0, Mathf.Lerp(0,throwAnimShift, throwingForce / forceCoef));
                }
                else
                {
                    animator.Play(putAnimStateName , 0, Mathf.Lerp(0, putAnimShift, throwingForce / forceCoef));
                }
            }

        }
        else
        {
            targetCameraFieldOfView = initCameraFieldOfView;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, ballCameraTarget.position, camSpeed* Time.deltaTime);
            Vector3 camPos = cameraTransform.position;
            camPos.y = Mathf.Max(camPos.y, GameController.CalcGroundHeight(camPos) + ballCameraTargetHeight);
            cameraTransform.position = camPos;

            ballCameraRotationTarget.position = cameraTransform.position;
            ballCameraRotationTarget.LookAt(ball.transform);
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, ballCameraRotationTarget.rotation, camSpeed *2 * Time.deltaTime);
            
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
        sliderEvent.OnSliderPressed -= SetNeedAnimateThrow;
        
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
        sliderEvent.OnSliderPressed += SetNeedAnimateThrow;
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
    void SetNeedAnimateThrow()
    {
        isNeedThrowAnimate = true;
    }
    void ChangeThrowingForce(float newForce)
    {
        throwingForce = newForce * forceCoef;
        arrow.ChangeArrowWithForce(newForce);
        
    }
    Vector3 CalcUpDirection()
    {
        if (!isOnGrass)
        {
            return Vector3.up * sliderEvent.slider.value;
        }
        return Vector3.zero;
    }
    void Throw()
    {
        isNeedThrowAnimate = false;
        if (!isBallMoving)
        {
            isBallMoving = true;
            if (isOnGrass)
            {
                lineC.DrawLine(ball.transform.position, arrow.GetDirection() + CalcUpDirection(), throwingForce, ballPrefab);
            }
            List<Vector3> points  = lineC.PredictTrajectory(ball.transform.position, arrow.GetDirection() + CalcUpDirection(), throwingForce, ballPrefab);
            ball.Throw(arrow.GetDirection() + CalcUpDirection(), throwingForce);
            ballCameraTarget.position = GenerateRandomPointOnCircle(points[points.Count - 1], ballCameraTargetDistance);
            StartCoroutine(waitToBallStop(points.Count));
            arrow.HideArrow();
        }
        sliderEvent.slider.value = 0;
    }
    Vector3 GenerateRandomPointOnCircle(Vector3 center, float radius, int counter = 0)
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float x = center.x + radius * Mathf.Cos(angle);
        float z = center.z + radius * Mathf.Sin(angle);
        float height = GameController.CalcGroundHeight(center) + ballCameraTargetHeight;
        Vector3 generatedPoint = new Vector3(x, height, z);
        
        Vector3 direction = (generatedPoint - center).normalized;
        if (Physics.Raycast(center, direction, out RaycastHit hit, Vector3.Distance(center,direction)) && counter < 10)
        {
            return GenerateRandomPointOnCircle(center, radius, ++counter);
        }
        return generatedPoint;
    }
    IEnumerator waitToBallStop(int pointsCount)
    {
        isBallMoving = true;
        while (!ball.rb.IsSleeping())
        {
            yield return null;
        }
        if (ball.isOutOfBounds)
        {
            yield return StartCoroutine(showCommentText(1.5f, "Ball out of bounds"));
            ball.transform.localPosition = Vector3.zero;
            transform.position = gameController.GetStartPos();
            ball.ResetAfterOutOfBounds();
        }
        if (Vector3.Distance(ball.transform.position, gameController.GetLunkaPos()) < gameController.minDistanceToAiming)
        {
            yield return StartCoroutine(showCommentText(1.5f, "Ball on Green"));
        }

        //yield return new WaitForSeconds(pointsCount * lineC.timeStep);
        isEnd = ball.isEnd;
        ball.StopBall();
        isBallMoving = false;
        transform.position = ball.gameObject.transform.position;
        ball.transform.localPosition = Vector3.zero;
        arrow.ResetArrowDirection();
        gameController.SwitchPlayer();
    }
    IEnumerator showCommentText(float time,string text)
    {
        commentText.text = text;
        commentPanel.gameObject.SetActive(true);
        float timer = 0;
        while (timer < time)
        {
            timer += Time.deltaTime;
            float t = timer / time;
            commentPanel.alpha = t;
            yield return null;
        }
        while (timer > 0)
        {
            timer -= Time.deltaTime * 2;
            float t = timer / time;
            commentPanel.alpha = t;
            yield return null;
        }
        commentPanel.alpha = 0;
        commentPanel.gameObject.SetActive(false);
    }
    // Update is called once per frame
    
}
