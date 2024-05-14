using UnityEngine;

public class RockCheck : MonoBehaviour, IRock
{
    public GameObject TreeObj;
    public bool EnableSprint = false;
    public bool EnableJump = false;
    public bool isFinalRock = false;
    public ParticleSystem[] particles;

    public int fireFlyCount = 0;

    private void Start()
    {
        fireFlyCount = TreeObj.transform.childCount;
    }

    public bool CheckRock(GameObject playerObj)
    {
        particles = playerObj.GetComponentsInChildren<ParticleSystem>();

        if (particles.Length == fireFlyCount )
        {
            foreach ( ParticleSystem particle in particles )
            {
                var shape = particle.shape;
                shape.radius = 10;

                particle.gameObject.transform.parent = null;
            }
            Destroy(gameObject, 3);

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetSprintState() { return EnableSprint; }
    public bool GetJumpState() { return EnableJump; }
    public bool GetFinalRock() { return isFinalRock; }
    public int GetMaxSoulCount() { return fireFlyCount; }
}
