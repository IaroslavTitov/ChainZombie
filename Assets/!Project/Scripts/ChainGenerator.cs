using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainGenerator : MonoBehaviour
{
    public static void GenerateChain(GameObject chainlinkPrefab, Rigidbody2D startObject, Rigidbody2D endObject)
    {
        GameObject parentObject = new GameObject();
        parentObject.name = "Chain";

        Rigidbody2D currentConnector = startObject;
        Vector2 displacement = startObject.position - endObject.position;
        float angle = Mathf.Atan2(displacement.y, displacement.x);
        float linkLength = .3f;
        
        while (displacement.magnitude > linkLength)
        {
            GameObject newLink = Instantiate(chainlinkPrefab, currentConnector.position - displacement.normalized * linkLength, Quaternion.Euler(0,0,angle), parentObject.transform);
            displacement += displacement.normalized * linkLength;
            newLink.GetComponent<HingeJoint2D>().connectedBody = currentConnector;
            currentConnector = newLink.GetComponent<Rigidbody2D>();
        }

        HingeJoint2D joint = endObject.gameObject.AddComponent<HingeJoint2D>();
        joint.connectedBody = currentConnector;
    }
}
