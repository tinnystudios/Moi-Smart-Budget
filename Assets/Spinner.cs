using UnityEngine;

public class Spinner : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        Target.Rotate(-Vector3.forward * 10);
    }

    public void Begin()
    {
        gameObject.SetActive(true);
    }

    public void End()
    {
        gameObject.SetActive(false);
    }
}
