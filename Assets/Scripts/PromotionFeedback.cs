using MoreMountains.Feedbacks;
using UnityEngine;

public class PromotionFeedback : MonoBehaviour
{
    public static PromotionFeedback Instance { get; private set; }

    [SerializeField] private MMFeedbacks promotionFeedback;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayPromotionFeedback()
    {
        if (promotionFeedback != null)
        {
            promotionFeedback.PlayFeedbacks();
        }
    }
}
