using UnityEngine;

public class ExplosionNave : MonoBehaviour
{
    MoveNave navePosition;

    private void Awake()
    {
        navePosition = FindObjectOfType<MoveNave>();
    }

    void Update()
    {
        transform.position = navePosition.transform.position;
    }
}
