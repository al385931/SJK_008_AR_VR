using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
/// <summary>
/// Para gestionar más de un marcador con un ARTrackedImageManager
/// </summary>
public class multimarca : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager arImManager;
    [SerializeField] private marcaObjeto[] marcasObjetos;



    private void OnEnable()
    {
        arImManager.trackedImagesChanged += ImageFound;
    }

    private void OnDisable()
    {
        arImManager.trackedImagesChanged -= ImageFound;
    }
    
    void Start()
    {
        for (int i = 0; i < marcasObjetos.Length; i++)
        {
            GameObject go = Instantiate(marcasObjetos[i].prefab,Vector3 .zero,Quaternion.identity);
            go.SetActive(false);
            marcasObjetos[i].model = go;
        }
        
    }

    private void ImageFound(ARTrackedImagesChangedEventArgs eventData)
    {
        foreach (ARTrackedImage imagen in eventData.updated)
        {
            if (imagen.trackingState == TrackingState.Tracking)
            {
                showModel(imagen);
            }
            else if (imagen.trackingState == TrackingState.Limited)
            {
                hideModel(imagen);
            }
        }
    }

    private void hideModel(ARTrackedImage imagen)
    {
        for (int i = 0; i < marcasObjetos.Length; i++)
        {
            
            if (marcasObjetos[i].nombreMarca == imagen.referenceImage.name)
            {
                if (marcasObjetos[i].model.activeSelf)
                {
                    marcasObjetos[i].model.SetActive(false);
                }
                return;
            }
        }
    }

    private void showModel(ARTrackedImage imagen)
    {
        for (int i = 0;i < marcasObjetos.Length;i++)
        {
            if (marcasObjetos[i].nombreMarca == imagen.referenceImage.name)
            {
                if (!marcasObjetos[i].model.activeSelf)
                {
                    marcasObjetos[i].model.SetActive(true);
                }
                marcasObjetos[i].model.transform.position = imagen.transform.position;
                marcasObjetos[i].model.transform.rotation = imagen.transform.rotation;
                return;
            }
        }
    }
}

[System.Serializable]
public class marcaObjeto
{
    public GameObject prefab;
    public string nombreMarca;
    [HideInInspector]
    public GameObject model;

}
