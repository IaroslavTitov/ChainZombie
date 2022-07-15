using UnityEngine;

public class CageScript : MonoBehaviour
{
    private ScoreSystem scoreSystem;
    private SoundManager soundManager;
    public GameObject pointNotification;
    void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Caged " + collision.name);
        collision.gameObject.layer = LayerMask.NameToLayer("CagedZombie");

        ContactPoint2D[] points = new ContactPoint2D[1];
        collision.GetContacts(points);
        PointNotificationScript.SpawnNotification(pointNotification, points[0].point, scoreSystem.scorePerCaged);

        scoreSystem.ZombieCaged();
        soundManager.playSoundEffect(soundManager.zombieCagedSound);
    }
}
