using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject player;
    Vector3 difference;
    // Start is called before the first frame update
    void Start()
    {
        difference = transform.position - (player.transform.position - new Vector3(0, 100, 0));
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, (gameManager.currentPlatform.transform.position + gameManager.nextPlatform.transform.position) / 2 + difference, 0.03f);
    }
}
