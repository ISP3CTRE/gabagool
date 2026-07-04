using Content.Shared._Mriya.MouseSense;
using Content.Shared.Actions;

namespace Content.Server._Mriya.MouseSense;

public sealed class FelinidMouseSenseSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<FelinidMouseSenseComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(Entity<FelinidMouseSenseComponent> ent, ref MapInitEvent args)
    {
        _actions.AddAction(ent, ref ent.Comp.ActionEntity, ent.Comp.Action, ent);
    }
}
