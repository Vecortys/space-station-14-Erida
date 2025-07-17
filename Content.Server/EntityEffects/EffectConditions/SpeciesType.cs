using Content.Server.Humanoid.Components;
using Content.Shared.EntityEffects;
using Content.Shared.Humanoid;
using Content.Shared.Mobs;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.EffectConditions;

public sealed partial class SpeciesType : EntityEffectCondition
{

    [DataField]
    public string Species;

    [DataField]
    public bool Invert = false;

    public override bool Condition(EntityEffectBaseArgs args)
    {
        if (args.EntityManager.TryGetComponent(args.TargetEntity, out HumanoidAppearanceComponent? humanoid))
        {
            if (humanoid.Species == Species)
                return true ^ Invert;
        }
        return Invert;
    }

    public override string GuidebookExplanation(IPrototypeManager prototype)
    {
        return Loc.GetString("reagent-effect-condition-guidebook-species-type",
            ("species", Species),
            ("invert", Invert));
    }
}
