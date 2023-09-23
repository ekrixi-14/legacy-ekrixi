using Content.Client.Computer;
using Content.Client.UserInterface.Controls;
using Content.Shared._FTL.ShipWeapons;
using Robust.Client.AutoGenerated;
using Robust.Client.GameObjects;
using Robust.Client.Input;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Map;
namespace Content.Client._FTL.ShipWeapons;

[GenerateTypedNameReferences]
public sealed partial class GunnerConsoleWindow : FancyWindow,
    IComputerWindow<GunnerConsoleBoundInterfaceState>
{
    [Dependency] private readonly IEntityManager _entityManager = default!;

    public GunnerConsoleWindow()
    {
        RobustXamlLoader.Load(this);
        IoCManager.InjectDependencies(this);

        WorldRangeChange(RadarScreen.WorldRange);
        RadarScreen.WorldRangeChanged += WorldRangeChange;
        RadarScreen.OnRadarClick += RadarClick;
    }

    private void WorldRangeChange(float value)
    {
        RadarRange.Text = $"{value:0}";
    }

    public void SetMatrix(EntityCoordinates? coordinates, Angle? angle)
    {
        RadarScreen.SetMatrix(coordinates, angle);
    }

    private void RadarClick(EntityCoordinates coordinates)
    {
        _entityManager.SpawnAtPosition("PlushieLizard", coordinates);
    }

    public void UpdateState(GunnerConsoleBoundInterfaceState scc)
    {
        RadarScreen.UpdateState(scc);
        AmmoCounter.Text = scc.CurrentAmmo <= 0 ? "NO AMMO" : $"{scc.CurrentAmmo}/{scc.MaxAmmo}";
        MaxRadarRange.Text = $"{scc.MaxRange:0}";
    }
}
