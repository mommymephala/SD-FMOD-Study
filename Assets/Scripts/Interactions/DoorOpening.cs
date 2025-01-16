using System.Collections;
using UnityEngine;

public class DoorOpening : MonoBehaviour, IInteractable
{
    public bool isOpen;
    [SerializeField] private float speed = 1f;

    [Header("Door Type Configs")]
    [SerializeField] private bool isRotatingDoor = true;

    [Header("Sliding Configs")]
    [SerializeField] private Vector3 slideDirection = Vector3.right;
    [SerializeField] private float slideAmount = 3f;

    [Header("Rotation Configs")]
    [SerializeField] private float rotationAmount = -90f;

    [Header("Key Requirements")]
    public Key requiredKey;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _startRotation;
    private Coroutine _animationCoroutine;
    private bool isInteractable = true;

    private void Awake()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition + slideAmount * slideDirection;
        _startRotation = transform.rotation.eulerAngles;
    }

    public void Interact()
    {
        if (!isInteractable) return;

        if (isOpen)
        {
            Close();
        }
        else
        {
            if (requiredKey != null && !PlayerInventory.Instance.HasKey(requiredKey))
            {
                Debug.Log($"You need the {requiredKey.keyName} to open this door.");
                AudioManager.instance.PlaySound(AudioManager.instance.errorEvent);
                return;
            }

            if (requiredKey != null && PlayerInventory.Instance.HasKey(requiredKey))
            {
                AudioManager.instance.PlaySoundAtPosition(AudioManager.instance.doorUnlockEvent, transform.position);
            }
            else
            {
                AudioManager.instance.PlaySoundAtPosition(AudioManager.instance.doorOpenEvent, transform.position);
            }

            Open();
        }
    }

    public string GetHintText()
    {
        if (isOpen)
            return "Press E to close the door";
        else if (requiredKey != null && !PlayerInventory.Instance.HasKey(requiredKey))
            return $"You need the {requiredKey.keyName} to open this door";
        else
            return "Press E to open the door";
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;
        isInteractable = false; // Prevent interaction during animation

        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        if (isRotatingDoor)
        {
            _animationCoroutine = StartCoroutine(RotateDoor(true));
        }
        else
        {
            _animationCoroutine = StartCoroutine(SlideDoor(_endPosition));
        }
    }

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;
        isInteractable = false; // Prevent interaction during animation

        AudioManager.instance.PlaySoundAtPosition(AudioManager.instance.doorOpenEvent, transform.position);

        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        if (isRotatingDoor)
        {
            _animationCoroutine = StartCoroutine(RotateDoor(false));
        }
        else
        {
            _animationCoroutine = StartCoroutine(SlideDoor(_startPosition));
        }
    }

    private IEnumerator SlideDoor(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float duration = Vector3.Distance(startPosition, targetPosition) / speed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isInteractable = true; // Allow interaction again
    }

    private IEnumerator RotateDoor(bool opening)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = opening
            ? Quaternion.Euler(new Vector3(0, _startRotation.y + rotationAmount, 0))
            : Quaternion.Euler(_startRotation);

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        transform.rotation = endRotation;
        isInteractable = true; // Allow interaction again
    }
}