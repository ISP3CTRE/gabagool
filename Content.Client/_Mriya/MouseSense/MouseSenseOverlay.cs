using Content.Shared._Mriya.MouseSense;
using Content.Shared.Tag;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Client.ResourceManagement;
using Robust.Shared.Enums;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Maths;

namespace Content.Client._Mriya.MouseSense;

public sealed class MouseSenseOverlay : Overlay
{
    private readonly IEntityManager _entMan;
    private readonly IPlayerManager _player;
    private readonly EntityLookupSystem _lookup;
    private readonly TagSystem _tag;
    private readonly SharedTransformSystem _xform;
    private readonly Texture _markerTexture;

    private const float MarkerSize = 1.0f;

    public override OverlaySpace Space => OverlaySpace.WorldSpace;

    public MouseSenseOverlay(
        IEntityManager entMan,
        IPlayerManager player,
        EntityLookupSystem lookup,
        TagSystem tag,
        IResourceCache resCache)
    {
        _entMan = entMan;
        _player = player;
        _lookup = lookup;
        _tag = tag;
        _xform = _entMan.System<SharedTransformSystem>();
        ZIndex = 100;

        _markerTexture = resCache.GetResource<TextureResource>("/Textures/_Mriya/Interface/mouse_marker.png").Texture;
    }

    protected override bool BeforeDraw(in OverlayDrawArgs args)
    {
        var player = _player.LocalEntity;
        return player != null && _entMan.HasComponent<MouseSenseComponent>(player.Value);
    }

   	protected override void Draw(in OverlayDrawArgs args)
	{
    	var player = _player.LocalEntity!.Value;
    	var comp = _entMan.GetComponent<MouseSenseComponent>(player);
    	var playerXform = _entMan.GetComponent<TransformComponent>(player);
    	var mapPos = _xform.GetMapCoordinates(playerXform);

    	var handle = args.WorldHandle;
    	var entities = _lookup.GetEntitiesInRange(mapPos, comp.Range);

    	var eyeRotation = args.Viewport.Eye?.Rotation ?? default;

    	foreach (var ent in entities)
    	{
        	if (!_tag.HasTag(ent, "MRMouseDetectable"))
            	continue;

        	var mouseXform = _entMan.GetComponent<TransformComponent>(ent);
        	var worldPos = _xform.GetWorldPosition(mouseXform);

        	var half = MarkerSize / 2f;
        	var box = new Box2(
            	worldPos.X - half, worldPos.Y - half,
            	worldPos.X + half, worldPos.Y + half);

        	var rotatedBox = new Box2Rotated(box, -eyeRotation, worldPos);

        	handle.DrawTextureRect(_markerTexture, rotatedBox);
    	}
	}
}
