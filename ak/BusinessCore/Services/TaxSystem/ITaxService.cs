﻿using BusinessCore.Domain.Purchases;
using BusinessCore.Domain.Sales;
using System.Collections.Generic;

namespace BusinessCore.Services.TaxSystem
{
    public interface ITaxService
    {
        IEnumerable<BusinessCore.Domain.TaxSystem.Tax> GetIntersectionTaxes(int itemId, int partyId, BusinessCore.Domain.PartyTypes partyType);
        List<KeyValuePair<int, decimal>> GetPurchaseTaxes(int vendorId, IEnumerable<PurchaseInvoiceLine> purchaseInvoiceLines);
        List<KeyValuePair<int, decimal>> GetPurchaseTaxes(int vendorId, int itemId, decimal quantity, decimal amount, decimal discount);
        List<KeyValuePair<int, decimal>> GetSalesTaxes(int customerId, IEnumerable<SalesInvoiceLine> salesInvoiceLines);
        List<KeyValuePair<int, decimal>> GetSalesTaxes(int customerId, int itemId, decimal quantity, decimal amount, decimal discount);
        decimal GetItemPrice(int itemId);
        decimal GetItemCost(int itemId);
        /// <summary>
        /// Deduct tax from a price which includes tax
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>Net price</returns>
        decimal PriceBeforeTax(int itemId);
        /// <summary>
        /// Add tax to a price.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>Gross price</returns>
        decimal PriceAfterTax(int itemId);
    }
}
