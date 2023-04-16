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
        GameManager.instance.EnterState(State.MAP_GENERATE); //�� ������ �� ������Ʈ ����

        arPlaneManager.planesChanged += ArPlaneManager_planesChanged;
    }

    private void ArPlaneManager_planesChanged(ARPlanesChangedEventArgs obj)
    {
        if(GameManager.instance.state != State.MAP_GENERATE) //�� ���� �� �� �ƴϸ� ������
        {
            return;
        }

        List<ARPlane> addedPlanes = obj.added;
        if (addedPlanes.Count > 0)
        {
            foreach (ARPlane plane in addedPlanes)
            {
                GameManager.instance.arPlanes.Add(plane); //Plane�� �����Ǿ��� �� ����Ʈ�� �߰���
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
                        //plane�� �� �� �̻� ������ ����
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
                GameManager.instance.arPlanes.Remove(plane); //Plane�� ���ŵǸ� ����Ʈ���� ������
            }
        }
    }
}
