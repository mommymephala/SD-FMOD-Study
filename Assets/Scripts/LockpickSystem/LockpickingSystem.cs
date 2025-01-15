using ECM.Components;
using ECM.Controllers;
using UnityEngine;

public class LockpickingSystem : MonoBehaviour, IInteractable
{
    public Transform lockpick;
    public LockpickingUIManager uiManager;
    public GameObject lockpickingUI;
    public float rotationStep = 15f;
    public float minRotationStep = 1f;
    public float maxRotationStep = 45f;
    public float tolerance = 5f;
    public int maxMoves = 10;

    private int remainingMoves;
    private float sweetSpotAngle;
    private bool isLockpicking = false;
    private bool isChestUnlocked = false;
    private float currentAngle = 0f;

    public GameObject rewardObject;
    public Transform chestLid;
    public float rotationSpeed = 2f;
    private bool isChestOpening = false;

    private PlayerInventory playerInventory;
    private MonoBehaviour playerLook;
    private MonoBehaviour playerMovement;
    private PlayerInteraction playerInteraction;
    private FireballShooter fireballShooter;

    void Start()
    {
        playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
        playerLook = playerInventory.GetComponent<MouseLook>();
        playerMovement = playerInventory.GetComponent<BaseFirstPersonController>();
        playerInteraction = playerInventory.GetComponent<PlayerInteraction>();
        fireballShooter = playerInventory.GetComponentInChildren<FireballShooter>();

        GenerateSweetSpot();
    }

    void Update()
    {
        if (isLockpicking)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopLockpicking();
                return;
            }

            if (remainingMoves > 0)
            {
                bool moved = false;

                // Move lockpick using Q and E
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    currentAngle -= rotationStep;
                    moved = true;

                    // Play a random lockpicking sound
                    AudioManager.instance.PlayRandomSound(AudioManager.instance.lockpickSounds);
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    currentAngle += rotationStep;
                    moved = true;

                    // Play a random lockpicking sound
                    AudioManager.instance.PlayRandomSound(AudioManager.instance.lockpickSounds);
                }

                // Clamp the lockpick rotation between 0 and 360 degrees
                currentAngle = Mathf.Repeat(currentAngle, 360f);
                lockpick.localEulerAngles = new Vector3(0, 0, currentAngle);

                // Update progress bar UI
                uiManager.UpdateProgress(currentAngle, sweetSpotAngle);

                // Check for successful lockpicking
                if (Mathf.Abs(currentAngle - sweetSpotAngle) < tolerance && Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("Lock successfully picked!");
                    UnlockChest();
                }

                // Reduce remaining moves if moved
                if (moved)
                {
                    remainingMoves--;
                    uiManager.UpdateMoveCountUI(remainingMoves);

                    if (remainingMoves <= 0)
                    {
                        FailLockpick();
                    }
                }
            }

            // Adjust rotation step dynamically
            if (Input.GetKeyDown(KeyCode.Z))
            {
                AdjustRotationStep(-5f); // Decrease rotation step
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                AdjustRotationStep(5f); // Increase rotation step
            }
        }

        // Handle chest opening
        if (isChestOpening && chestLid != null)
        {
            RotateChestLid();
        }
    }

    void RotateLockpick(float angle)
    {
        currentAngle += angle;
        currentAngle = Mathf.Repeat(currentAngle, 360f); // Clamp angle between 0 and 360
        lockpick.localEulerAngles = new Vector3(0, 0, currentAngle);
    }

    void AdjustRotationStep(float adjustment)
    {
        rotationStep = Mathf.Clamp(rotationStep + adjustment, minRotationStep, maxRotationStep);

        // Update degree amount UI
        if (uiManager != null)
        {
            uiManager.UpdateDegreeAmountUI(rotationStep);
        }

        Debug.Log($"Rotation Step adjusted to: {rotationStep}");
    }

    void GenerateSweetSpot()
    {
        sweetSpotAngle = Random.Range(0f, 360f);

        // Update progress bar UI for the new sweet spot
        if (uiManager != null)
        {
            uiManager.UpdateProgress(currentAngle, sweetSpotAngle);
        }
    }

    void UnlockChest()
    {
        Debug.Log("Unlocking chest...");
        isLockpicking = false;
        isChestUnlocked = true; // Mark the chest as unlocked
        StopLockpicking();

        if (rewardObject != null)
        {
            rewardObject.SetActive(true); // Activate or spawn the unique reward
        }

        isChestOpening = true;
    }

    void FailLockpick()
    {
        Debug.Log("Lockpick failed!");
        playerInventory.UseLockpick(); // Deduct a lockpick on failure

        if (playerInventory.GetLockpickCount() > 0)
        {
            remainingMoves = maxMoves; // Reset moves for retry
            if (uiManager != null)
            {
                uiManager.UpdateMoveCountUI(remainingMoves);
            }
        }
        else
        {
            Debug.Log("Out of lockpicks!");
            StopLockpicking();
        }
    }

    void RotateChestLid()
    {
        Quaternion targetRotation = Quaternion.Euler(-45, 0, 0);
        chestLid.localRotation = Quaternion.Slerp(chestLid.localRotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (Quaternion.Angle(chestLid.localRotation, targetRotation) < 0.1f)
        {
            chestLid.localRotation = targetRotation;
            isChestOpening = false;
        }
    }

    public void StartLockpicking()
    {
        if (isChestUnlocked)
        {
            Debug.Log("Chest already unlocked!");
            return;
        }

        if (playerInventory.GetLockpickCount() > 0)
        {
            isLockpicking = true;
            remainingMoves = maxMoves;

            if (lockpickingUI != null)
            {
                lockpickingUI.SetActive(true);
                uiManager.SetLockpickingSystem(this);
            }

            if (uiManager != null)
            {
                uiManager.UpdateMoveCountUI(remainingMoves);
                uiManager.UpdateDegreeAmountUI(rotationStep);
                uiManager.UpdateProgress(currentAngle, sweetSpotAngle);
            }

            playerLook.enabled = false;
            playerMovement.enabled = false;
            fireballShooter.enabled = false;
            playerInteraction.enabled = false;
            playerInteraction.hintText.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("No lockpicks available!");
        }
    }

    public void StopLockpicking()
    {
        if (lockpickingUI != null)
        {
            lockpickingUI.SetActive(false);
        }

        // Unfreeze player controls
        playerLook.enabled = true;
        playerMovement.enabled = true;
        fireballShooter.enabled = true;
        playerInteraction.enabled = true;
        playerInteraction.hintText.gameObject.SetActive(true);

        isLockpicking = false;
    }

    public void Interact()
    {
        StartLockpicking();
    }

    public string GetHintText()
    {
        if (isChestUnlocked)
        {
            return "Chest already unlocked";
        }

        return playerInventory.GetLockpickCount() > 0 ? "Press E to lockpick" : "No lockpicks available";
    }
}