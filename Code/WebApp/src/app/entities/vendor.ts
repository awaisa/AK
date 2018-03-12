export class Vendor {
  no: string;
  party: Party;
  paymentTermId: number;
  taxGroupId: number;
  accountsPayableAccountId: number;
  purchaseAccountId: number;
  purchaseDiscountAccountId: number;
}
export class Party {
  name: string;
  address: string;
  email: string;
  phone: string;
  website: string;
  fax: string;
  contacts:Array<Contact>;
}
export class Contact {
  firstName: string = '';
  lastName: string = '';
  middleName: string = '';
  isPrimary: boolean = false;
  id: number = 0;
}
