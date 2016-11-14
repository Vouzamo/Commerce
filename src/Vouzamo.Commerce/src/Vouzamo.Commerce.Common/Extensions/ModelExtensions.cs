using System.Runtime.CompilerServices;
using Vouzamo.Commerce.Common.Models;

namespace Vouzamo.Commerce.Common.Extensions
{
    public static class ModelExtensions
    {
        public static MeasuredQuantity Measure(this UnitOfMeasure unitOfMeasure, int quantity)
        {
            return new MeasuredQuantity(quantity, unitOfMeasure.Id);
        }

        public static Inventory Receive(this Material material, MeasuredQuantity measuredQuantity)
        {
            return new Inventory(material.Id, measuredQuantity);
        }

        public static SourcedInventory SourceFrom(this Inventory inventory, Source source, decimal unitCost)
        {
            return new SourcedInventory(inventory.MaterialId,  inventory.Quantity, source.Id, unitCost);
        }
    }
}
