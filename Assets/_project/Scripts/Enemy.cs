using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PlayerStateManager player;

    private void Start()
    {
        player = FindObjectOfType<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 3f * Time.deltaTime);
    }
}
