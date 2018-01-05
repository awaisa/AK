using BusinessCore.Domain.Financials;
using BusinessCore.Domain.Purchases;
using BusinessCore.Domain.Sales;
using BusinessCore.Domain.TaxSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Items
{
    public partial class Item : BaseEntity, ICompanyBaseEntity
    {
        public Item()
        {
            SalesInvoiceLines = new HashSet<SalesInvoiceLine>();
            PurchaseOrderLines = new HashSet<PurchaseOrderLine>();
            PurchaseReceiptLines = new HashSet<PurchaseReceiptLine>();
            PurchaseInvoiceLines = new HashSet<PurchaseInvoiceLine>();
            InventoryControlJournals = new HashSet<InventoryControlJournal>();
        }

        public int? ItemCategoryId { get; set; }
        public int? SmallestMeasurementId { get; set; }
        public int? SellMeasurementId { get; set; }
        public int? PurchaseMeasurementId { get; set; }
        public int? PreferredVendorId { get; set; }
        public int? ItemTaxGroupId { get; set; }
        public int? SalesAccountId { get; set; }
        public int? InventoryAccountId { get; set; }
        public int? CostOfGoodsSoldAccountId { get; set; }
        public int? InventoryAdjustmentAccountId { get; set; }
        public string No { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string PurchaseDescription { get; set; }
        public string SellDescription { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public int? ModelId { get; set; }
        public int? BrandId { get; set; }
        public virtual ItemCategory ItemCategory { get; set; }
        public virtual ItemTaxGroup ItemTaxGroup { get; set; }
        public virtual Vendor PreferredVendor { get; set; }
        public virtual Account InventoryAccount { get; set; }
        public virtual Account SalesAccount { get; set; }
        public virtual Account CostOfGoodsSoldAccount { get; set; }
        public virtual Account InventoryAdjustmentAccount { get; set; }
        public virtual Measurement SmallestMeasurement { get; set; }
        public virtual Measurement SellMeasurement { get; set; }
        public virtual Measurement PurchaseMeasurement { get; set; }

        public virtual ICollection<SalesInvoiceLine> SalesInvoiceLines { get; set; }
        public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        public virtual ICollection<PurchaseReceiptLine> PurchaseReceiptLines { get; set; }
        public virtual ICollection<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; }
        public virtual ICollection<InventoryControlJournal> InventoryControlJournals { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
       
        public virtual ItemBrand Brand { get; set; }

        public virtual ItemModel Model { get; set; }

        #region Not Mapped
        [NotMapped]
        public decimal ItemTaxAmountOutput { get { return ComputeItemTaxAmountOutput(); } }
        private decimal ComputeItemTaxAmountOutput()
        {
            decimal totalItemTaxAmount = 0;
            if (ItemTaxGroup != null)
            {
                foreach (var itemTaxGroup in ItemTaxGroup.ItemTaxGroupTax)
                {
                    decimal salesPrice = (Price.Value / (1 + (itemTaxGroup.Tax.Rate / 100)));
                    var taxAmount = salesPrice * (itemTaxGroup.Tax.Rate / 100);
                    totalItemTaxAmount += taxAmount;
                }
            }
            return totalItemTaxAmount;
        }

        [NotMapped]
        public decimal ItemTaxAmountInput { get { return ComputeItemTaxAmountInput(); } }
        private decimal ComputeItemTaxAmountInput()
        {
            decimal totalItemTaxAmount = 0;
            if (ItemTaxGroup != null)
            {
                foreach (var itemTaxGroup in ItemTaxGroup.ItemTaxGroupTax)
                {
                    var taxAmount = Cost.Value * (itemTaxGroup.Tax.Rate / 100);
                    totalItemTaxAmount += taxAmount;
                }
            }
            return totalItemTaxAmount;
        }
        #endregion

        private decimal _discountedPrice = 0;
        private decimal _discount = 0;
        public void AddDiscount(decimal discount)
        {
            _discount = discount;
        }

        public decimal ComputeDiscountedPrice()
        {
            _discountedPrice = Price.Value;
            if (_discount != 0)
            {
                _discountedPrice = Price.Value - ((_discount / 100) * Price.Value);
            }
            return _discountedPrice;
        }
        public decimal ComputeQuantityOnHand()
        {
            decimal inQty = 0;
            decimal outQty = 0;
            var invControlJOurnals = InventoryControlJournals.GetEnumerator();
            while (invControlJOurnals.MoveNext())
            {
                inQty += (invControlJOurnals.Current.INQty.HasValue && invControlJOurnals.Current.IsReverse == false) ? invControlJOurnals.Current.INQty.Value : 0;
                outQty += invControlJOurnals.Current.OUTQty.HasValue ? invControlJOurnals.Current.OUTQty.Value : 0;
            }
            return inQty - outQty;
        }

        public bool GLAccountsValidated()
        {
            bool validated = true;

            if (this.CostOfGoodsSoldAccountId == null
                || this.InventoryAccountId == null
                || this.InventoryAdjustmentAccountId == null
                || this.SalesAccountId == null)
            {
                validated = false;
            }

            return validated;
        }
    }
}
