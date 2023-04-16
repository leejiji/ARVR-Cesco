using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private ARPlaneManager arPlaneManager;

    void Start()
    {
        GameManager.instance.EnterState(State.MAP_GENERATE); //맵 생성될 때 스테이트 변경

        arPlaneManager.planesChanged += ArPlaneManager_planesChanged;
    }

    private void ArPlaneManager_planesChanged(ARPlanesChangedEventArgs obj)
    {
        if(GameManager.instance.state != State.MAP_GENERATE) //맵 생성 할 때 아니면 리턴함
        {
            return;
        }

        List<ARPlane> addedPlanes = obj.added;
        if (addedPlanes.Count > 0)
        {
            foreach (ARPlane plane in addedPlanes)
            {
                GameManager.instance.arPlanes.Add(plane); //Plane이 생성되었을 때 리스트에 추가함
            }
        }

        /*List<ARPlane> updatedPlanes = obj.updated;
        ARPlane overlapPlane = null;
        if (updatedPlanes.Count > 0)
        {
            foreach (ARPlane plane in updatedPlanes)
            {
                foreach (ARPlane listPlane in GameManager.instance.arPlanes)
                {
                    if (listPlane == plane && overlapPlane == listPlane)
                    {
                        //plane이 두 개 이상 있으면 삭제
                        GameManager.instance.arPlanes.Remove(listPlane);
                    }
                    overlapPlane = listPlane;
                }
            }
        }*/


        List<ARPlane> removedPlanes = obj.removed;

        if (removedPlanes.Count > 0)
        {
            foreach (ARPlane plane in removedPlanes)
            {
                GameManager.instance.arPlanes.Remove(plane); //Plane이 제거되면 리스트에서 제거함
            }
        }
    }
}
