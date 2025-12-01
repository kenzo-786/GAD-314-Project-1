using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldIcon : MonoBehaviour
{
    public Image iconImage;
    public float floatDuration = 3f;
    public float floatSpeed = 1f;

    private Transform _target;
    private Camera _mainCam;
    private float _timer;

    public void Setup(Transform target, Sprite icon, Color color)
    {
        _target = target;
        _mainCam = Camera.main;

        if (iconImage)
        {
            iconImage.sprite = icon;
            iconImage.color = color;
        }

        transform.forward = _mainCam.transform.forward;
    }

    private void Update()
    {
        if (_mainCam)
        {
            transform.rotation = _mainCam.transform.rotation;
        }

        _timer += Time.deltaTime;

        if (_target != null)
        {
            transform.position = _target.position + Vector3.up * 3.0f + (Vector3.up * (_timer * floatSpeed));

            if (_mainCam)
            {
                transform.rotation = _mainCam.transform.rotation;
            }
        }
        else
        {
            Destroy(gameObject);
        }

        if (_timer >= floatDuration)
        {
            Destroy(gameObject);
        }
        else
        {
            if (iconImage)
            {
                Color c = iconImage.color;
                c.a = 1f - (_timer / floatDuration);
                iconImage.color = c;
            }
        }
    }
}
