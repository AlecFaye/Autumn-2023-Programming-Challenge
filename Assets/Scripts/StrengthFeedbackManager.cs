using MoreMountains.Feedbacks;
using UnityEngine;

public class StrengthFeedbackManager : MonoBehaviour
{
    public static StrengthFeedbackManager Instance { get; private set; }

    [SerializeField] private MMFeedbacks neutralFeedback;
    [SerializeField] private MMFeedbacks strongFeedback;
    [SerializeField] private MMFeedbacks weakFeedback;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayNeutralFeedback()
    {
        if (neutralFeedback != null)
        {
            neutralFeedback.PlayFeedbacks();
        }
    }

    public void PlayStrongFeedback()
    {
        if (strongFeedback != null)
        {
            strongFeedback.PlayFeedbacks();
        }
    }

    public void PlayWeakFeedback()
    {
        if (weakFeedback != null)
        {
            weakFeedback.PlayFeedbacks();
        }
    }
}
