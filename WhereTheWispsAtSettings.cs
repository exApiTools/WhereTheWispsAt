﻿using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using SharpDX;

namespace WhereTheWispsAt;

public class WhereTheWispsAtSettings : ISettings
{
    public ToggleNode Enable { get; set; } = new ToggleNode(false);
    public ToggleNode DrawMap { get; set; } = new ToggleNode(true);
    public ColorNode YellowWisp { get; set; } = new ColorNode(Color.Yellow);
    public RangeNode<int> YellowSize { get; set; } = new RangeNode<int>(5, 1, 100);
    public ColorNode BlueWisp { get; set; } = new ColorNode(Color.SkyBlue);
    public RangeNode<int> BlueSize { get; set; } = new RangeNode<int>(5, 1, 100);
    public ColorNode PurpleWisp { get; set; } = new ColorNode(Color.Purple);
    public RangeNode<int> PurpleSize { get; set; } = new RangeNode<int>(5, 1, 100);
    public ColorNode ChestColor { get; set; } = new ColorNode(Color.White);
    public RangeNode<int> ChestSize { get; set; } = new RangeNode<int>(5, 1, 100);
    public RangeNode<int> ChestScreenDisplayMaxDistance { get; set; } = new RangeNode<int>(100, 1, 200);
    public ColorNode EncounterColor { get; set; } = new ColorNode(Color.White);
    public ColorNode LightBomb { get; set; } = new ColorNode(Color.White);
    public ColorNode Wells { get; set; } = new ColorNode(Color.Orange);
    public ColorNode FuelRefill { get; set; } = new ColorNode(Color.Green);
    public ColorNode Altars { get; set; } = new ColorNode(Color.Red);
    public ColorNode DustConverters { get; set; } = new ColorNode(Color.HotPink);
    public ColorNode Dealer { get; set; } = new ColorNode(Color.HotPink);
    public ToggleNode IgnoreFullscreenPanels { get; set; } = new ToggleNode(false);
    public ToggleNode IgnoreLargePanels { get; set; } = new ToggleNode(false);
}