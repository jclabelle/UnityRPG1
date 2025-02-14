using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    public float mapLimitRight;
    public float mapLimitLeft;
    public float mapLimitUp;
    public float mapLimitDown;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        gameObject.transform.position = player.transform.position + offset;
        respectMapLimits();

    }

    private void respectMapLimits()
    {
        if(gameObject.transform.position.x > mapLimitRight)
        {
            gameObject.transform.position = new Vector3(mapLimitRight, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        if (gameObject.transform.position.x < mapLimitLeft)
        {
            gameObject.transform.position = new Vector3(mapLimitLeft, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        if (gameObject.transform.position.y > mapLimitUp)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, mapLimitUp, gameObject.transform.position.z);
        }

        if (gameObject.transform.position.y < mapLimitDown)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, mapLimitDown, gameObject.transform.position.z);
        }

    }
}
