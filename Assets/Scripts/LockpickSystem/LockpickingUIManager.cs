using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockpickingUIManager : MonoBehaviour
{
    private LockpickingSystem currentLockpickingSystem;
    public Image progressBarFill;
    public TextMeshProUGUI lockpickCountText;
    public TextMeshProUGUI moveCountText;
    public TextMeshProUGUI degreeAmountText;
    public float maxAngle = 90f;

    public void SetLockpickingSystem(LockpickingSystem lockpickingSystem)
    {
        currentLockpickingSystem = lockpickingSystem;
    }

    public void UpdateProgress(float currentAngle, float sweetSpotAngle)
    {
        if (currentLockpickingSystem == null)
            return;

        // Calculate progress based on proximity to sweet spot
        float proximity = 1 - (Mathf.Abs(currentAngle - sweetSpotAngle) / maxAngle);
        progressBarFill.fillAmount = Mathf.Clamp01(proximity);

        // Change color based on success proximity
        if (Mathf.Abs(currentAngle - sweetSpotAngle) < currentLockpickingSystem.tolerance)
        {
            progressBarFill.color = Color.green;
        }
        else
        {
            progressBarFill.color = Color.red;
        }
    }

    public void UpdateLockpickCountUI()
    {
        if (lockpickCountText != null && PlayerInventory.Instance != null)
        {
            lockpickCountText.text = $"Lockpicks: {PlayerInventory.Instance.GetLockpickCount()}";
        }
    }

    public void UpdateMoveCountUI(int remainingMoves)
    {
        if (moveCountText != null)
        {
            moveCountText.text = $"Moves Left: {remainingMoves}";
        }
    }
    public void UpdateDegreeAmountUI(float rotationStep)
    {
        if (degreeAmountText != null)
        {
            degreeAmountText.text = $"Degree Step: {rotationStep:F1}";
        }
    }
}