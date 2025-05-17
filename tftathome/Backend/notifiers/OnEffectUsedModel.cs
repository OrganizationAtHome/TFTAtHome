using TFTAtHome.Backend.models.Effect;

namespace TFTAtHome.Backend.notifiers;

public class OnEffectUsedModel
{
    public int CardId { get; set; }
    public MatchEffect Effect { get; set; }
}