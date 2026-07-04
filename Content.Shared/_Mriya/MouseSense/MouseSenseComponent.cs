using Robust.Shared.GameStates;

namespace Content.Shared._Mriya.MouseSense;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class MouseSenseComponent : Component
{
    [DataField, AutoNetworkedField]
    public TimeSpan EndTime;

    [DataField, AutoNetworkedField]
    public float Range = 5f;

    [DataField]
    public TimeSpan Duration = TimeSpan.FromSeconds(6);
}
