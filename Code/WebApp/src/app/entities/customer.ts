import { Party } from '.';

export class Customer {
  id: number;
  no: string;
  party: Party;
  paymentTermId: number;
  taxGroupId: number;

  accountsReceivableAccountId: number;
  salesAccountId: number;
  salesDiscountAccountId: number;
  promptPaymentDiscountAccountId: number;
  customerAdvancesAccountId: number;
}