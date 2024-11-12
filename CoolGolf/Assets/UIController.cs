using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameController gameController;
    public List<InputField> inputFields;
    public GameObject inputFieldPrefab;
    public RectTransform plusMinusPlayersPanel;
    public Button playerPlusButton;
    public Button playerMinusButton;
    public RectTransform playersContent;

    public CanvasGroup selectMapPanel;
    public CanvasGroup addPlayersPanel;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwitchPanels(null, selectMapPanel));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddPlayerInputField()
    {
        GameObject newInputField = Instantiate(inputFieldPrefab, inputFields[inputFields.Count - 1].transform);
        inputFields.Add(newInputField.GetComponent<InputField>());
        plusMinusPlayersPanel.SetParent(newInputField.transform);
        plusMinusPlayersPanel.localPosition = new Vector3(0, -70, 0);

        Vector2 offsetMin = playersContent.offsetMin;
        offsetMin.y -= 70; // Уменьшаем значение offsetMin.y, чтобы увеличить отступ снизу
        playersContent.offsetMin = offsetMin;

        playerMinusButton.interactable = true;
    }
    public void DelPlayerInputField()
    {
        if (inputFields.Count == 1)
        {
            playerMinusButton.interactable = false;
            return;
        }
        plusMinusPlayersPanel.SetParent(inputFields[inputFields.Count - 2].transform);
        plusMinusPlayersPanel.localPosition = new Vector3(0, -70, 0);
        InputField lastField = inputFields[inputFields.Count - 1];
        inputFields.Remove(lastField);
        Destroy(lastField.gameObject);
        if (inputFields.Count == 1)
        {
            playerMinusButton.interactable = false;
        }

        Vector2 offsetMin = playersContent.offsetMin;
        offsetMin.y += 70; // Уменьшаем значение offsetMin.y, чтобы увеличить отступ снизу
        playersContent.offsetMin = offsetMin;
    }
    public void TryConfirmAddPlayers()
    {
        List<string> names = new List<string>();
        for (int i = 0; i < inputFields.Count; i++)
        {
            if (string.IsNullOrEmpty(inputFields[i].text.Replace(" ", "")))
            {
                StartCoroutine(Shake(inputFields[i].GetComponent<RectTransform>(),2));
            }
            else
            {
                names.Add(inputFields[i].text);
            }
        }
        if (names.Count == inputFields.Count)
        {
            StartCoroutine(SwitchPanels(addPlayersPanel, null));
            gameController.AddPlayers(names);
        }
    }
    private IEnumerator Shake(RectTransform rectTransform, int firstIndexUnmovedChilds)
    {
        Vector3 originalPosition = rectTransform.position;

        List<Vector3> chilrOriginalPositions = new List<Vector3>();
        for (int i = firstIndexUnmovedChilds; i < rectTransform.childCount; i++)
        {
            RectTransform child = rectTransform.GetChild(i) as RectTransform;
            if (child != null)
            {
                chilrOriginalPositions.Add(child.position); // Компенсируем смещение
            }
        }

        float elapsedTime = 0f;

        while (elapsedTime < 1)
        {
            float offsetX = Mathf.Sin(elapsedTime * Mathf.PI * 10f) * 10; // Вычисляем смещение для тряски
            Vector3 shakeOffset = new Vector3(offsetX, 0, 0);

            rectTransform.position = originalPosition + shakeOffset;

            // Корректируем позицию дочерних объектов обратно
            for (int i = firstIndexUnmovedChilds; i < rectTransform.childCount; i++)
            {
                RectTransform child = rectTransform.GetChild(i) as RectTransform;
                if (child != null)
                {
                    child.position = chilrOriginalPositions[i - firstIndexUnmovedChilds]; // Компенсируем смещение
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

       
        rectTransform.position = originalPosition; // Возвращаем родитель в исходную позицию
        for (int i = firstIndexUnmovedChilds; i < rectTransform.childCount; i++)
        {
            RectTransform child = rectTransform.GetChild(i) as RectTransform;
            if (child != null)
            {
                child.position = chilrOriginalPositions[i - firstIndexUnmovedChilds]; // Компенсируем смещение
            }
        }
    }
    IEnumerator SwitchPanels(CanvasGroup first, CanvasGroup second)
    {
        if (first != null)
        {
            float time = 0.5f;
            while (time > 0)
            {
                time -= Time.deltaTime;
                first.alpha = time*2;
                yield return null;
            }
            first.alpha = 0;
            first.gameObject.SetActive(false);
        }
        if (second != null)
        {
            second.gameObject.SetActive(true);
            float time = 0;
            while (time < 0.5f)
            {
                time += Time.deltaTime;
                second.alpha = time*2;
                yield return null;
            }
            second.alpha = 1;
        }
    }
    public void SwitchPanelsAfterSelectMap()
    {
        StartCoroutine(SwitchPanels(selectMapPanel, addPlayersPanel));
    }
}
