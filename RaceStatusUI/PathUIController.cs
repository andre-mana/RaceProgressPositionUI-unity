using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

class Car
{
    public Transform Transform { get; set; }
    public float DistanceTravelled { get; set; }

    public Car(Transform transform)
    {
        Transform = transform;
    }
}

public class PathUIController : MonoBehaviour
{
    Vector3 positionInLine = new Vector3(0, 0, 0);
    Transform playerTransform;
    List<Vector3> wayPointsList;
    float[] waypointDistanceToOrigin;
    float pathLength = 0;
    float distanceInSegement = 0;
    float distanceTravelled = 0;
    int numberOfwayPoints;
    int indexClosestWayPoint, currentWayPoint = 0;
    Vector3 segmentNormalized, endPointSegment, begininPointSegment, originToCarSegment;
    int place;
    int progress;
    int newValue;
    [SerializeField] TextMeshProUGUI textProgress;
    [SerializeField] TextMeshProUGUI textPosition;
    Car player;
    List<Car> AIDrivers;

    private void OnDrawGizmos()
    {
        for (int index = 0; index < transform.childCount - 1; index++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.GetChild(index).position, 0.1f);
            Gizmos.DrawLine(transform.GetChild(index).position, transform.GetChild(index + 1).position);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(positionInLine, 1.5f);
    }

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().enabled = false;
        }
    }

    void Start()
    {

        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj == null)
        {
            Debug.Log("Player tag not found");
            return;
        }

        AIDrivers = new List<Car>();
        GameObject[] AIDriversObj = GameObject.FindGameObjectsWithTag("AIDriver");

        foreach (GameObject carObj in AIDriversObj)
        {
            Car aiDriver = new Car(carObj.transform);
            AIDrivers.Add(aiDriver);
        }

        place = 1;
        playerTransform = playerObj.transform;
        player = new Car(playerTransform);

        if (currentWayPoint <= 1)
        {
            currentWayPoint = 1;
        }

        wayPointsList = new List<Vector3>();

        foreach (Transform child in transform)
        {
            numberOfwayPoints++;
            wayPointsList.Add(new Vector3(child.transform.position.x, child.transform.position.y, child.transform.position.z));
        }

        waypointDistanceToOrigin = new float[numberOfwayPoints];

        for (int i = 0; i < numberOfwayPoints - 1; i++)
        {
            pathLength += Vector3.Distance(wayPointsList[i + 1], wayPointsList[i]);
            waypointDistanceToOrigin[i] = pathLength;
        }
    }

    void Update()
    {
        if (playerTransform == null)
        {
            Debug.Log("Player tag not found");
            return;
        }

        SetDistanceTravelled(player);

        for (int i = 0; i < AIDrivers.Count; i++)
        {

            SetDistanceTravelled(AIDrivers[i]);
        }

        SetProgressText();
        SetPositionText();
    }

    void SetDistanceTravelled(Car car)
    {
        endPointSegment = wayPointsList[currentWayPoint + 1];
        begininPointSegment = wayPointsList[currentWayPoint];

        originToCarSegment = car.Transform.position - begininPointSegment;
        segmentNormalized = (endPointSegment - begininPointSegment).normalized;

        distanceInSegement = Vector3.Dot(originToCarSegment, segmentNormalized);

        if (currentWayPoint < 1)
        {
            distanceTravelled = (((distanceInSegement) / pathLength));
        }
        else
        {
            distanceTravelled = (((waypointDistanceToOrigin[currentWayPoint - 1] + distanceInSegement) / pathLength));
        }

        if (currentWayPoint < numberOfwayPoints - 2)
        {
            if (distanceInSegement > Vector3.Distance(wayPointsList[currentWayPoint + 1], wayPointsList[currentWayPoint]))
            {
                currentWayPoint++;
            }
        }

        if (distanceInSegement < 0 && currentWayPoint > 0)
        {
            currentWayPoint--;
        }

        car.DistanceTravelled = distanceTravelled;
    }

    public void SetProgressText()
    {
        if (progress < 0)
        {
            progress = 0;
        }

        newValue = (int)(player.DistanceTravelled * 100);

        if (newValue > progress)
        {
            progress = newValue;
        }

        if (progress > 100)
        {
            progress = 100;
        }

        if (progress < 10)
        {
            textProgress.text = "0" + progress + "%";
        }
        else
        {
            textProgress.text = progress + "%";
        }
    }

    void SetPositionText()
    {
        place = 1;
        for (int i = 0; i < AIDrivers.Count; i++)
        {

            if (player.DistanceTravelled < AIDrivers[i].DistanceTravelled)
                place++;
        }

        switch (place)
        {
            case 1:
                textPosition.text = "1st";
                break;
            case 2:
                textPosition.text = "2nd";
                break;
            case 3:
                textPosition.text = "3rd";
                break;
            default:
                textPosition.text = place + "th";
                break;
        }
    }
}
