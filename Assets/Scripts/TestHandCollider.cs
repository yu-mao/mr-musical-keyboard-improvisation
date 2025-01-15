using UnityEngine;

public class TestHandCollider : MonoBehaviour
{
    public bool hasCollided = false;
    public Vector3 collisionPosition;
    
    private void OnCollisionEnter(Collision other)
    {
        hasCollided = true;
        collisionPosition = other.contacts[0].point;
    }

    private void OnCollisionExit(Collision other)
    {
        hasCollided = false;
    }
}
