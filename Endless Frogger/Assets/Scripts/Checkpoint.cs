using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] private Renderer checkPointRender;
    LevelGenerator levelGenerator;
    bool hitCheckpoint = false;
    Player player;
    [SerializeField] private GameObject saveTile;

    void Awake()
    {
        player = FindObjectOfType(typeof(Player)) as Player;
        levelGenerator = FindObjectOfType<LevelGenerator>();
        hitCheckpoint = false;
        //levelGenerator.SetCheckpoint(player.transform.position, player.checkPointPrefab);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Checkpoint");
            // Save the current tile position as the checkpoint
            //levelGenerator.SetCheckpoint(other.transform.position, player.checkPointPrefab);
            hitCheckpoint = true;
        }
    }

    private void Update()
    {
        if(hitCheckpoint)
        {
            checkPointRender.material.color = Color.green;
        }
        else
        {
            checkPointRender.material.color = Color.red;
        }
    }

    public void SetCheckPoint(bool value)
    {
        hitCheckpoint = value;
    }
}
