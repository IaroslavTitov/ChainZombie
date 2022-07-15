using UnityEngine;

public class CageScript : MonoBehaviour
{
    public GameObject pointNotification;

    private ScoreSystem scoreSystem;
    private SoundManager soundManager;
    void Start()
    {
        scoreSystem = FindObjectOfType<ScoreSystem>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.layer = LayerMask.NameToLayer("CagedZombie");

        ContactPoint2D[] points = new ContactPoint2D[1];
        collision.GetContacts(points);
        PointNotificationScript.SpawnNotification(pointNotification, points[0].point, scoreSystem.scorePerCaged);

        scoreSystem.ZombieCaged();
        soundManager.playSoundEffect(soundManager.zombieCagedSound);
    }
}
