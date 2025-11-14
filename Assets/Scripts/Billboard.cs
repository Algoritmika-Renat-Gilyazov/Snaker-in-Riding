using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera targetCamera; // Камера, на которую будет смотреть объект

    void Start()
    {
        // Если камеру не указали вручную, берём главную
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void LateUpdate()
    {
        // Вращаем объект, чтобы он всегда "смотрел" на камеру
        transform.LookAt(transform.position + targetCamera.transform.forward);
    }
}
