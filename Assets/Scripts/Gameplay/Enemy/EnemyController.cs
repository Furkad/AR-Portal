using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Collider))]
public class EnemyController : MonoBehaviour
{
    private GameObject portalGameObject;
    [SerializeField]
    private Vector3 randomPos = Vector3.zero;

    [SerializeField]
    private Vector3 posToMoveTo = Vector3.zero;

    [SerializeField]
    private float speed;

    private bool isGoingOutside = true;

    private void Update()
    {
        if (isGoingOutside)
        {
            MoveToPortal();
        }
        else
        {
            MoveToRandomPoint();
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ARPortal portal) && !portal.isBackSide)
        {
            posToMoveTo += portalGameObject.transform.forward;
        }
    }

    private void MoveToPortal()
    {
        if (portalGameObject == null)
        {
            portalGameObject = FindObjectOfType<ARPortal>().gameObject;
            return;
        }

        if (posToMoveTo == Vector3.zero)
            CalculatePortalPosition();

        transform.position = Vector3.MoveTowards(transform.position, posToMoveTo, Time.deltaTime * speed);

        transform.LookAt(posToMoveTo);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        if (transform.position == posToMoveTo)
            isGoingOutside = false;
    }

    private void MoveToRandomPoint()
    {
        if (randomPos == Vector3.zero || transform.position == randomPos)
        {
            CalculateRandomPosition();
        }
        transform.position = Vector3.MoveTowards(transform.position, randomPos, Time.deltaTime * speed);
        transform.LookAt(randomPos);
    }

    private void CalculatePortalPosition()
    {
        posToMoveTo = portalGameObject.transform.position;
        posToMoveTo.y /= 1.5f;
    }

    private void CalculateRandomPosition()
    {
        posToMoveTo.y = FindObjectOfType<ARPlane>(true).transform.position.y;
        randomPos = new Vector3(Random.Range(posToMoveTo.x + 1f, posToMoveTo.x + 3f), posToMoveTo.y, Random.Range(portalGameObject.transform.localPosition.z, portalGameObject.transform.localPosition.z + 2f));
    }
}
