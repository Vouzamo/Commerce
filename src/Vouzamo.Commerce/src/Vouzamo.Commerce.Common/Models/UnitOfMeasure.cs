using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Vouzamo.Commerce.Common.Models
{
    public class UnitOfMeasure : BoundedContext
    {
        public string Name { get; protected set; }
        public string Abbreviation { get; protected set; }

        public UnitOfMeasure(string name, string abbreviation)
        {
            Name = name;
            Abbreviation = abbreviation;
        }
    }

    public class Material : BoundedContext
    {
        public string Name { get; protected set; }
        public IDictionary<Guid, int> BillOfMaterials { get; protected set; }

        public Material(string name)
        {
            Name = name;
            BillOfMaterials = new Dictionary<Guid, int>();
        }

        public Material(string name, IDictionary<Guid, int> billOfMaterials) : this(name)
        {
            BillOfMaterials = billOfMaterials;
        }
    }

    public class Source : BoundedContext
    {
        public string Name { get; protected set; }

        public Source(string name)
        {
            Name = name;
        }
    }

    public interface IInventory
    {
        Guid Id { get; }
        Guid MaterialId { get; }
        Guid SourceId { get; }
        int Quantity { get; }
        decimal UnitCost { get; }
    }

    public class Inventory : BoundedContext, IInventory
    {
        public Guid MaterialId { get; protected set; }
        public Guid SourceId { get; protected set; }
        public int Quantity { get; protected set; }
        public decimal UnitCost { get; protected set; }

        public Inventory(Guid materialId, Guid sourceId, int quantity, decimal unitCost)
        {
            MaterialId = materialId;
            SourceId = sourceId;
            Quantity = quantity;
            UnitCost = unitCost;
        }
    }

    public class AssembledInventory : BoundedContext, IInventory
    {
        public Guid MaterialId { get; protected set; }
        public Guid SourceId { get; protected set; }
        public int Quantity { get; protected set; }
        public decimal UnitCost => Inventory.Sum(x => x.Quantity * UnitCost);
        public IEnumerable<IInventory> Inventory { get; protected set; }

        public AssembledInventory(Guid materialId, Guid sourceId, int quantity, IEnumerable<IInventory> inventory)
        {
            MaterialId = materialId;
            SourceId = sourceId;
            Quantity = quantity;
            Inventory = inventory;
        }
    }

    public interface IInventoryManager
    {
        Inventory SplitInventory(Inventory inventory, int quantity);
        Inventory ReceiveInventory(Material material, Source source, int quantity, decimal unitCost);
        Inventory AssembleInventory(Material material, IEnumerable<Inventory> inventory, int quantity);
    }

    public class InventoryManager : IInventoryManager
    {
        public Inventory SplitInventory(Inventory inventory, int quantity)
        {
            return inventory;
        }

        public Inventory ReceiveInventory(Material material, Source source, int quantity, decimal unitCost)
        {
            return new Inventory(material.Id, source.Id, quantity, unitCost);
        }

        public Inventory AssembleInventory(Material material, Source source, IEnumerable<Inventory> inventory, int quantity)
        {
            var source = new Source("");

            var remainingInventory = inventory.ToList();

            while (quantity > 0)
            {
                var assemblyInventories = new List<Inventory>();

                foreach (var assemblyMaterial in material.BillOfMaterials)
                {
                    var assemblyInventory = remainingInventory.FirstOrDefault(x => x.Id == assemblyMaterial.Key);

                    if (assemblyInventory.Quantity >= assemblyMaterial.Value)
                    {
                        assemblyInventories.Add(SplitInventory(assemblyInventory, assemblyMaterial.Value));
                    }
                    else
                    {
                        break;
                    }
                }

                quantity--;
            }
        }
    }
}