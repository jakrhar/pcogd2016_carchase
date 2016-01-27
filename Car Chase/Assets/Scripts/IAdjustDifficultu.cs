using UnityEngine.EventSystems;

/// <summary>
/// This interface describes methods that gameobjects that can affect into the difficulty levels may implement.
/// 
/// The idea is that e.g. the gameController may call these methods to chance the difficulty level of the game objects 
/// in the scene.
/// </summary>
public interface IAdjustDifficulty : IEventSystemHandler
{
    /// <summary>
    /// Adjust difficulty level
    /// </summary>
    /// <param name="value">amount of difficulty level increase in percentage, negative value will decrease it.</param>
    void AdjustDifficulty(int percentage);

    /// <summary>
    /// Sets the difficulty level in percetage.
    /// </summary>
    /// <param name="percentage">difficulty level in percentage</param>
    void SetDifficultyLevel(int percentage);
}

