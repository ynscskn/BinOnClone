using System.Collections;
using UnityEngine;

public class Money : MonoBehaviour
{
    [HideInInspector] public Rigidbody Rg;
    public bool Jump = false;

    private void Awake()
    {
        Rg = GetComponent<Rigidbody>();
    }

    float h = 1f;
    float gravity = -20;
    Transform target;


    private void Update()
    {
        if (Jump == true)
        {
            Launch();
        }
    }

    public IEnumerator zýpla()
    {
        Jump = true;
        yield return new WaitForSeconds(0.1f);
        Jump = false;
    }

    public void Launch()
    {
        //target = M_Money.I.Target;
        Physics.gravity = Vector3.up * gravity;
        Rg.useGravity = true;

        Rg.velocity = CalculateLaunchData().initialVelocity;
    }

    LaunchData CalculateLaunchData()
    {
        float displacementY = target.position.y - Rg.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - Rg.position.x, 0, target.position.z - Rg.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }
}
