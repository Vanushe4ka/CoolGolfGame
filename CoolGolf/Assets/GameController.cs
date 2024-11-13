using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    List<Player> players = new List<Player>();
    public Player playerPrefab;
    int currentPlayerIndex;

    public Map[] allMaps;
    [SerializeField] Map currentMap;
    [SerializeField] UIController uIController;

    [SerializeField] LayerMask notGroundLayers;
    public float minDistanceToAiming;

    static GameController instance;
    public static GameController Instance()
    {
        return instance;
    }
    private void OnDrawGizmos()
    {
        if (currentMap != null && currentMap.lunkaTransform != null)
        {
            Gizmos.DrawWireSphere(currentMap.lunkaTransform.position, minDistanceToAiming);
            Gizmos.DrawSphere(currentMap.startPoint.position, 1);

            if (currentMap.polygonBounds.Count > 2)
            {
                Gizmos.color = Color.red; // Цвет линий
                for (int i = 0; i < currentMap.polygonBounds.Count; i++)
                {
                    Vector2 current = currentMap.polygonBounds[i];
                    Vector2 next = currentMap.polygonBounds[(i + 1) % currentMap.polygonBounds.Count]; // следущая точка, замкнувшая полигон
                    Gizmos.DrawLine(new Vector3(current.x, 0, current.y), new Vector3(next.x, 0, next.y));
                }
            }
        }
    }
    public bool IsPointInBounds(Vector3 tDpoint)
    {
        Vector2 point = new Vector2(tDpoint.x, tDpoint.z);
        List<Vector2> polygon = currentMap.polygonBounds;
        int n = polygon.Count;
        bool inside = false;
        
        // Проходим по всем рёбрам полигона
        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            Vector2 vertex1 = polygon[i];
            Vector2 vertex2 = polygon[j];

            // Проверка, пересекает ли луч горизонтальную линию
            if ((vertex1.y > point.y) != (vertex2.y > point.y) &&
                (point.x < (vertex2.x - vertex1.x) * (point.y - vertex1.y) / (vertex2.y - vertex1.y) + vertex1.x))
            {
                inside = !inside;
            }
        }

        return inside;
    }
    public Vector3 GetLunkaPos()
    {
        return currentMap.lunkaTransform.position;
    }
    public Vector3 GetStartPos()
    {
        return currentMap.startPoint.position;
    }
    public void SetMap(Map map)
    {
        for (int i = 0; i < allMaps.Length; i++)
        {
            allMaps[i].gameObject.SetActive(false);
        }
        currentMap = map;
        currentMap.gameObject.SetActive(true);
    }
    public void AddPlayer(string name)
    {
        GameObject playerObj = Instantiate(playerPrefab.gameObject, currentMap.startPoint.position, Quaternion.identity);
        Player newPlayer = playerObj.GetComponent<Player>();
        newPlayer.Name = name;
        newPlayer.gameController = this;
        newPlayer.SetSkin(players.Count);
        players.Add(newPlayer);
    }
    public void AddPlayers(List<string> names)
    {
        for (int i = 0; i < names.Count; i++)
        {
            AddPlayer(names[i]);
        }
        SelectPlayer(0);
    }
    
    public void SelectPlayer(int index)
    {
        currentPlayerIndex = index;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].gameObject.SetActive(false);
        }
        players[currentPlayerIndex].gameObject.SetActive(true);
    }
    bool isHaveAWinner()
    {
        List<Player> winners = new List<Player>();
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].isEnd) { winners.Add(players[i]); }
            Debug.Log($"{players[i].Name} = {players[i].isEnd}");
        }
        if (winners.Count > 0)
        {
            uIController.ShowWinPanel(winners, (winners.Count == players.Count && players.Count > 1));
            for (int i = 0; i < players.Count; i++)
            {
                Destroy(players[i].gameObject);
            }
            players.Clear();
            return true;
        }
        return false;
    }
    public void RemachAfterPause()
    {
        for (int i = 0; i < players.Count; i++)
        {
            Destroy(players[i].gameObject);
        }
        players.Clear();
    }
    public void SwitchPlayer()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count) {  currentPlayerIndex = 0; if (isHaveAWinner()) { return; } }
        SelectPlayer(currentPlayerIndex);
    }
    public static float CalcGroundHeight(Vector3 point)
    {
        Vector3 startPoint = point + Vector3.up * 100;
        RaycastHit hit;
        float height;
        int layerMask = ~(1 << 7);
        if (Physics.Raycast(startPoint, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            height = hit.point.y;
        }
        else
        {
            height = point.y;
            Debug.Log("Ray is not hit");
        }
        return height;
    }
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
