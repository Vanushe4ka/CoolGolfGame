using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    List<Player> players = new List<Player>();
    public Player playerPrefab;
    int currentPlayerIndex;

    public Map[] allMaps;
    Map currentMap;
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
    public void SwitchPlayer()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count) { currentPlayerIndex = 0; }
        SelectPlayer(currentPlayerIndex);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
