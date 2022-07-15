using UnityEngine;

public class ChainGenerator : MonoBehaviour
{
    public static Vector2 characterOffset = Vector2.up * 0.5f;

    public static void GenerateChain(GameObject chainlinkPrefab, Rigidbody2D startObject, Rigidbody2D endObject, Vector2 firstConnectorOffset, Vector2 lastConnectorOffset, float breakForce)
    {
        GameObject parentObject = new GameObject();
        parentObject.name = "Chain";

        // Calculate direction
        Rigidbody2D currentConnector = startObject;
        Vector2 displacement = (startObject.position + firstConnectorOffset) - (endObject.position + lastConnectorOffset);
        float angle = Mathf.Atan2(displacement.y, displacement.x);
        float linkLength = .5f;
        
        // Spawn links
        while (displacement.magnitude > linkLength)
        {
            GameObject newLink = Instantiate(chainlinkPrefab, currentConnector.position + firstConnectorOffset - displacement.normalized * linkLength, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg - 90), parentObject.transform);
            displacement -= displacement.normalized * linkLength;
            newLink.GetComponent<HingeJoint2D>().connectedBody = currentConnector;
            currentConnector = newLink.GetComponent<Rigidbody2D>();
            firstConnectorOffset = Vector2.zero;
            newLink.GetComponent<HingeJoint2D>().breakForce = breakForce;
        }

        // Add last link
        HingeJoint2D joint = endObject.gameObject.AddComponent<HingeJoint2D>();
        joint.connectedBody = currentConnector;
        joint.anchor = lastConnectorOffset;
        joint.breakForce = breakForce;
    }
}
