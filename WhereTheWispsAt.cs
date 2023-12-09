﻿using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using SharpDX;
using System.Collections.Generic;
using System.Linq;

namespace WhereTheWispsAt
{
    public class WhereTheWispsAt : BaseSettingsPlugin<WhereTheWispsAtSettings>
    {
        public record WispData(List<Entity> Purple, List<Entity> Yellow, List<Entity> Blue, List<Entity> LightBomb, List<Entity> Wells, List<Entity> FuelRefill);
        public WispData Wisps = new([], [], [], [], [], []);

        public override bool Initialise() => true;

        public WhereTheWispsAt()
        {
            Name = "Where The Wisps At";
        }

        public override Job Tick()
        {
            if (Wisps.Wells.Count > 0)
            {
                var wellsToRemove = Wisps.Wells
                    .Where(well => well.TryGetComponent<StateMachine>(out var stateComp)
                                   && stateComp?.States.Any(x => x.Name == "activated" && x.Value == 1) == true)
                    .ToList();

                wellsToRemove.ForEach(well => RemoveEntityFromList(well, Wisps.Wells));
            }
            return null;
        }

        public override void EntityAdded(Entity entity)
        {
            switch (entity.Metadata)
            {
                case "Metadata/MiscellaneousObjects/Azmeri/AzmeriResourceBase":
                    var path = entity.TryGetComponent<Animated>(out var animatedComp) ? animatedComp?.BaseAnimatedObjectEntity?.Path : null;
                    if (path != null)
                    {
                        if (path.Contains("League_Azmeri/resources/wisp_doodads/wisp_primal")) Wisps.Blue.Add(entity);
                        else if (path.Contains("League_Azmeri/resources/wisp_doodads/wisp_warden")) Wisps.Yellow.Add(entity);
                        else if (path.Contains("League_Azmeri/resources/wisp_doodads/wisp_vodoo")) Wisps.Purple.Add(entity);
                    }
                    break;

                case "Metadata/MiscellaneousObjects/Azmeri/AzmeriLightBomb":
                    Wisps.LightBomb.Add(entity);
                    break;

                case "Metadata/MiscellaneousObjects/Azmeri/AzmeriFuelResupply":
                    Wisps.FuelRefill.Add(entity);
                    break;

                case "Metadata/MiscellaneousObjects/Azmeri/AzmeriFlaskRefill":
                    Wisps.Wells.Add(entity);
                    break;
            }
        }

        public override void EntityRemoved(Entity entity)
        {
            new[] { Wisps.Blue, Wisps.Purple, Wisps.Yellow, Wisps.LightBomb, Wisps.Wells, Wisps.FuelRefill }
                .ToList()
                .ForEach(list => RemoveEntityFromList(entity, list));
        }

        private static void RemoveEntityFromList(Entity entity, List<Entity> list)
        {
            var entityToRemove = list.FirstOrDefault(wisp => wisp.Id == entity.Id);
            if (entityToRemove != null) list.Remove(entityToRemove);
        }

        public override void AreaChange(AreaInstance area) => Wisps = new([], [], [], [], [], []);

        public override void Render()
        {
            if (!Settings.Enable.Value || !GameController.InGame) return;

            foreach (var (list, color, size) in new[]
            {
                (Wisps.Yellow, Settings.YellowWisp, Settings.YellowSize.Value),
                (Wisps.Purple, Settings.PurpleWisp, Settings.PurpleSize.Value),
                (Wisps.Blue, Settings.BlueWisp, Settings.BlueSize.Value),
                (Wisps.LightBomb, Settings.LightBomb, Settings.LightBombSize.Value),
                (Wisps.Wells, Settings.Wells, Settings.WellsSize.Value),
                (Wisps.FuelRefill, Settings.FuelRefill, Settings.FuelRefillSize.Value)
            })
            {
                DrawWisps(list, color, size);
            }

            void DrawWisps(List<Entity> entityList, Color color, int size)
            {
                if (Settings.DrawMap && GameController.IngameState.IngameUi.Map.LargeMap.IsVisibleLocal)
                {
                    foreach (var entity in entityList)
                    {
                        var mapPos = GameController.IngameState.Data.GetGridMapScreenPosition(entity.GridPosNum);
                        Graphics.DrawBox(new RectangleF(mapPos.X - size / 2, mapPos.Y - size / 2, size, size), color);
                    }
                }
            }
        }
    }
}