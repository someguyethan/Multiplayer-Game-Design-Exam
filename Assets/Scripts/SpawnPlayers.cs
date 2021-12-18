using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject cameraPrefab;

    public float minX, maxX, minZ, maxZ;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 random = transform.position
            + Vector3.right * Random.Range(minX, maxX)
            + Vector3.forward * Random.Range(minZ, maxZ);
        random.y += 10f;
        GameObject test = PhotonNetwork.Instantiate(playerPrefab.name, random, Quaternion.identity);
        if (test.GetComponent<PhotonView>().IsMine)
        {
            test.GetComponent<PlayerMove>().SetData(Instantiate(cameraPrefab, random, Quaternion.identity));
        }
    }
}