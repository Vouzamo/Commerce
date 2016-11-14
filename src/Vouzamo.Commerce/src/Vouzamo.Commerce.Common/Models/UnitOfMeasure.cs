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
        IInventory SplitInventory(IInventory inventory, int quantity);
        IInventory ReceiveInventory(Material material, Source source, int quantity, decimal unitCost);
        IInventory AssembleInventory(Material material, Source source, int quantity, IEnumerable<IInventory> inventory);
    }

    public class InventoryManager : IInventoryManager
    {
        public IInventory SplitInventory(IInventory inventory, int quantity)
        {
            return inventory;
        }

        public IInventory ReceiveInventory(Material material, Source source, int quantity, decimal unitCost)
        {
            return new Inventory(material.Id, source.Id, quantity, unitCost);
        }

        public IInventory AssembleInventory(Material material, Source source, int quantity, IEnumerable<IInventory> inventory)
        {
            // Split the inventory to accomodate the quantity and bill of materials

            return new AssembledInventory(material.Id, source.Id, quantity, inventory);
        }
    }
}