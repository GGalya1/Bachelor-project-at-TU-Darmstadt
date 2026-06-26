using UnityEngine;

/// <summary>
/// Listens to <see cref="LevelEvents"/> and decides which achievements to unlock.
/// </summary>
public class AchievementObserver : MonoBehaviour
{
    private const int FIRST_LEVEL_SCENE_INDEX = 0;

    private void OnEnable()
    {
        LevelEvents.LevelCompleted += OnLevelCompleted;
        LevelEvents.SolutionFailed += OnSolutionFailed;
    }

    private void OnDisable()
    {
        LevelEvents.LevelCompleted -= OnLevelCompleted;
        LevelEvents.SolutionFailed -= OnSolutionFailed;
    }

    private void OnLevelCompleted(LevelCompletedData data)
    {
        // First level ever completed
        if (data.SceneIndex == FIRST_LEVEL_SCENE_INDEX)
            AchievementManager.Unlock(AchievementIds.FirstSteps);

        // Three-star solution
        if (data.EarnedStars == 3)
            AchievementManager.Unlock(AchievementIds.PerfectTry);

        // Player beat the last level in the game
        if (data.IsLastLevel)
            AchievementManager.Unlock(AchievementIds.AllLevelsComplete);
    }

    private void OnSolutionFailed(int totalFailedTries)
    {
        // Increment the global "correct solutions" counter (incremental achievement)
        AchievementManager.Increment(AchievementIds.SolutionCounter);
    }
}
