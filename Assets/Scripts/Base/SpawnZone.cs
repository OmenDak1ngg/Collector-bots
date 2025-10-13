using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] private float _minDistanceBetweenRobots = 1f;

    [SerializeField] private int _maxCountOfAttempts = 50;
    [SerializeField] private ErrorViewer _errorViewer;

    private bool IsSpawnpointTaked(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, _minDistanceBetweenRobots);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Robot>(out _) == true)
                return true;
        }

        return false;
    }

    public Vector3 GetRandomAvalaibleSpawnpoint()
    {
        int attempts = 0;
        Vector3 parentScale = transform.parent.localScale;
        float sizeX = transform.localScale.x * parentScale.x;
        float sizeZ = transform.localScale.z * parentScale.z;

        Vector3 randomSpawnpoint;

        do
        {
            attempts++;

            float randomX = Random.Range(-sizeX / 2, sizeX / 2);
            float randomZ = Random.Range(-sizeZ / 2, sizeZ / 2);

            randomSpawnpoint = transform.position + new Vector3(randomX, 0, randomZ);

            if (attempts >= _maxCountOfAttempts)
            {
                return Vector3.zero;
            }


        } while (IsSpawnpointTaked(randomSpawnpoint));

        return randomSpawnpoint;
    }
}