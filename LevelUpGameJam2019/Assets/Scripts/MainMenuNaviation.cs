using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuNaviation : MonoBehaviour
{
    
    public List<DeliveryLocation> locations = new List<DeliveryLocation>();
    [Space]

    [Header("Public References")]
    public GameObject deliveryLocationPrefab;
    public GameObject locationButtonPrefab;
    public Transform modelTransform;
    public Transform locationsButtonsContainer;

    [Space]

    [Header("Tween Settings")]
    public float lookDuration;
    public Ease lookEase;

    [Space]

    public Vector2 visualOffset;

    // Start is called before the first frame update
    void Start()
    {
        foreach (DeliveryLocation l in locations)
        {
            SpawnDeliveryLocation(l);
        }

        if(locations.Count > 0)
        {
            LookAtLocation(locations[0]);
            //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(locationsButtonsContainer).GetChild(0).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LookAtLocation(DeliveryLocation l)
    {
        Transform cameraParent = Camera.main.transform.parent;
        Transform cameraPivot = cameraParent.parent;

        cameraParent.DOLocalRotate(new Vector3(l.y,0,0), lookDuration, RotateMode.Fast).SetEase(lookEase);
        cameraPivot.DOLocalRotate(new Vector3(0,-l.x,0), lookDuration, RotateMode.Fast).SetEase(lookEase);

        FindObjectOfType<FollowTarget>().target = l.visualPoint;
    }

    private void SpawnDeliveryLocation(DeliveryLocation l)
    {
        GameObject dl = Instantiate(deliveryLocationPrefab);
        dl.transform.localEulerAngles = new Vector3(l.y + visualOffset.y, -l.x - visualOffset.x, 0);
        l.visualPoint = dl.transform.GetChild(0);
        SpawnLocationButton(l);
    }

    private void SpawnLocationButton(DeliveryLocation l)
    {
        DeliveryLocation dl = l;
        Button locationButton = Instantiate(locationButtonPrefab, locationsButtonsContainer).GetComponent<Button>();
        locationButton.onClick.AddListener(() => LookAtLocation(dl));
        locationButton.transform.GetChild(0).GetComponentInChildren<Text>().text = l.name;
    }


[System.Serializable]
public class DeliveryLocation
    {
        public string name;

        [Range(-180, 180)]
        public float x;

        [Range(-89, 89)]
        public float y;

        [HideInInspector]
        public Transform visualPoint;
    }
}
