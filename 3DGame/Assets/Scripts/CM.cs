using UnityEngine;

public class CM : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
