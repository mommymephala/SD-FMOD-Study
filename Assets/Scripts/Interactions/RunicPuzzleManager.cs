using System.Collections.Generic;
using UnityEngine;

public class RunicPuzzleManager : MonoBehaviour
{
    public static RunicPuzzleManager Instance; // Singleton for global access
    public List<RuneStone> runeStones = new List<RuneStone>();
    public GameObject puzzleReward; // The door, staircase, or object unlocked by the puzzle

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Deactivate the puzzle reward initially
        if (puzzleReward != null)
        {
            puzzleReward.SetActive(false);
        }
    }

    public void RuneActivated()
    {
        // Check if all rune stones are activated
        foreach (RuneStone rune in runeStones)
        {
            if (!rune.isActivated)
            {
                return; // If any rune is not activated, do nothing
            }
        }

        // All runes are activated - solve the puzzle
        Debug.Log("Runic puzzle solved!");
        SolvePuzzle();
    }

    private void SolvePuzzle()
    {
        // Unlock the next section or reward
        if (puzzleReward != null)
        {
            puzzleReward.SetActive(true); // Activate the reward (e.g., open door or reveal staircase)
        }

        // Optional: Play a sound or visual effect
        AudioManager.instance?.PlaySound(AudioManager.instance.puzzleSolvedSound);
    }
}