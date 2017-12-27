using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessCore.Domain
{
    public sealed class AccountCodes
    {
        private AccountCodes() { }

        public const string AccountsPayable_20110 = "20110";
        public const string CutomerAdvances_20120 = "20120";
        public const string SalesTax_20300 = "20300";

        public const string RegularCheckingAccount_10111 = "10111";
        public const string CashInHand_10113 = "10113";
        public const string AccountsReceivable_10120 = "10120";

        public const string Inventory_10800 = "10800";
        public const string GoodsReceiptNoteClearing_10810 = "10810";
        public const string AssemblyCost_10900 = "10900";

        public const string Sales_40100 = "40100";
        public const string HomeOwners_40200 = "40200";
        public const string SalesDiscount_40400 = "40400";
        public const string ShippingCharge_40500 = "40500";

        public const string Purchase_50200 = "50200";
        public const string CostOfGoodsSold_50300 = "50300";
        public const string PurchaseDiscount_50400 = "50400";
        public const string PurchasePriceVariance_50500 = "50500";
        public const string PurchaseTax_50700 = "50700";
    }
}
