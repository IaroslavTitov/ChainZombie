using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointNotificationScript : MonoBehaviour
{
    public int points;
    public float disappearSpeedCoef;

    private float timer = 0;
    private TMP_Text label;
    void Start()
    {
        label = GetComponent<TMP_Text>();
        label.text = "+" + points;
    }

    void Update()
    {
        timer += disappearSpeedCoef * Time.deltaTime;
        label.color = new Color(label.color.r, label.color.g, label.color.b, Mathf.Sin(timer));

        if (Mathf.Sin(timer) < 0)
        {
            Destroy(gameObject);
        }
    }

    public static void SpawnNotification(GameObject notificationPrefab, Vector3 position, int points)
    {
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        Vector2 viewportPosition = GameObject.FindObjectOfType<Camera>().WorldToScreenPoint(position);
        GameObject spawned = Instantiate(notificationPrefab, viewportPosition, Quaternion.identity, canvas.transform);
        spawned.GetComponent<PointNotificationScript>().points = points;
    }
}