using Aftermath.Models.Characters.States;
using Aftermath.Models.Occupation;
using AftermathModels.Buildings;
using AftermathModels.Loot;
using AftermathModels.Map.Tiles;
using AftermathModels.Map.Tiles.Decorators;
using AftermathModels.Occupation;
using System.Windows;

namespace AftermathModels.Map.Factories
{
    public class NormalTilesFactory : ITileFactory
{
    public Tile CreateBaseTile(Point coordinates) =>
        new BaseTile(coordinates, new NoOccupation());

    public Tile CreateCampTile(Point coordinates)
    {
        CampingSiteTile tile = new(coordinates, new Exploration(
            "Camping Site",
            1,
            new TileLootTable([
                new LootDrop(new CannedMeat(), 30, 1, 2),
                new LootDrop(new FreshWater(), 30, 1, 2),
                new LootDrop(new Book(), 10, 0, 1),
                new LootDrop(new Flashlight(), 10, 0, 1),
                new LootDrop(new FishingRod(), 10, 0, 1),
                new LootDrop(new Bandage(), 30, 1, 2),
                new LootDrop(new Sunscreen(), 10, 0, 1),
            ]),
            OccupationType.CampingSite,
            Status.CampingSite,
            [new MutantAttack(20)]
        ));

        return tile;
    }

    public Tile CreateForestTile(Point coordinates)
    {
        ForestTile tile = new(coordinates, new Exploration(
            "Forest",
            1,
            new TileLootTable([
                new LootDrop(new Berries(), 20, 1, 2),
                new LootDrop(new Mushrooms(), 20, 1, 2),
                new LootDrop(new Axe(), 10, 0, 1)
            ]),
            OccupationType.Forest,
            Status.Forest,
            [
                new BearAttack(20),
                new MutantAttack(10),
                new SpiderAttack(20)
            ]
        ));

        return tile;
    }

    public Tile CreateHuntersHutTile(Point coordinates)
    {
        HuntersHutTile tile = new(coordinates, new Exploration(
            "Hunter's Hut",
            1,
            new TileLootTable([
                new LootDrop(new Axe(), 40, 0, 1),
                new LootDrop(new CannedMeat(), 30, 1, 2),
                new LootDrop(new FreshWater(), 30, 1, 2),
                new LootDrop(new Flashlight(), 10, 0, 1),
            ]),
            OccupationType.HuntersHut,
            Status.HuntersHut,
            [
                new BearAttack(30),
                new MutantAttack(30)
            ]
        ));

        return tile;
    }

    public Tile CreateFishingDockTile(Point coordinates)
    {
        FishingDockTile tile = new(coordinates, new Exploration(
            "Fishing Dock",
            1,
            new TileLootTable([
                new LootDrop(new UnprocessedWater(), 30, 1, 3),
                new LootDrop(new Fish(), 30, 1, 3),
                new LootDrop(new FishingRod(), 10, 0, 1)
            ]),
            OccupationType.FishingDock,
            Status.FishingDock,
            [
                new MutantAttack(10),
                new Overheating(10)
            ]
        ));

        return tile;
    }
}
}
