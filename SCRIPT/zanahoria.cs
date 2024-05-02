using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class zanahoria : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject reticlePrefab;
    [SerializeField] private GameObject movingPrefab;
    [SerializeField] private GameObject tocablesPrefab;
    private Vector2 pointerPos = Vector2.zero;
    private Vector2 pointerPosAnterior = Vector2.zero;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject reticleGo;
    private GameObject movingGo;


    
    void Update()
    {
        if (Pointer.current == null) return;
        pointerPos = Pointer.current.position.ReadValue();
        //pointerPos = new Vector2(Screen.width / 2, Screen.height / 2);
        if (pointerPos != pointerPosAnterior)
        //if (true)
        {
            //tiro rayo
            if (raycastManager.Raycast(pointerPos,hits,TrackableType.PlaneWithinPolygon))
            {
                if (reticleGo == null)
                {
                    reticleGo = Instantiate(reticlePrefab, hits[0].pose.position, hits[0].pose.rotation);
                    movingGo = Instantiate(movingPrefab, hits[0].pose.position, hits[0].pose.rotation);
                    posicionaObjsRecoger(hits[0].pose);
                }
                else 
                {
                    reticleGo.transform.position = hits[0].pose.position;
                    StopAllCoroutines();
                    StartCoroutine(LerpPosition(movingGo, reticleGo.transform.position, 3));
                    StartCoroutine(LerpRotation(movingGo, calculaRotationFinal(movingGo, reticleGo.transform.position), 0.5f));
                    //movingGo.transform.position = Vector3.Lerp(movingGo.transform.position, reticleGo.transform.position, 0.05f);
                    //movingGo.transform.rotation = Quaternion.Lerp(movingGo.transform.rotation, calculaRotationFinal(movingGo, reticleGo.transform.position), 0.1f);


                }
            }
        }
        pointerPosAnterior = pointerPos;
    }
    private Quaternion calculaRotationFinal(GameObject go, Vector3 targetPos)
    {
        Quaternion salida = Quaternion.identity;
        GameObject auxGo = new GameObject();
        auxGo.transform.position = go.transform.position;
        auxGo.transform.LookAt(targetPos);
        salida = auxGo.transform.rotation;
        Destroy(auxGo);
        return salida;
    }

    private void posicionaObjsRecoger(Pose primeraPose)
    {
        int numObjs = Random.Range(3,10);
        GameObject auxGo = new GameObject();
        auxGo.transform.position = primeraPose.position;
        auxGo.transform.rotation = primeraPose.rotation;
        for (int i = 0;i < numObjs;i++)
        {
            auxGo.transform.Rotate(auxGo.transform.up, Random.Range(30, 330));
            Vector3 destino = primeraPose.position + auxGo.transform.forward*Random.Range(0.5f,1.5f);
            GameObject go = Instantiate(tocablesPrefab, destino, primeraPose.rotation);
            go.transform.Rotate(go.transform.up, Random.Range(0,90));
        }
        Destroy(auxGo);

    }
    IEnumerator LerpPosition(GameObject go, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = movingGo.transform.position;
        while (time < duration)
        {
            movingGo.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        movingGo.transform.position = targetPosition;
    }

    IEnumerator LerpRotation(GameObject go, Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = go.transform.rotation;
        while (time < duration)
        {
            go.transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        go.transform.rotation = endValue;
    }
}
