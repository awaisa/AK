export interface vendor {
    no: string;
    party: Party;
    paymentTermId: number;
    taxGroupId: number;
    accountsPayableAccountId: number;
    purchaseAccountId: number;
    purchaseDiscountAccountId: number;
  }
export  interface Party {
    name: string;
    address: string;
    email: string;
    phone: string;
    website: string;
    fax: string;
    contacts:Array<Contacts>;
  }
export  interface Contacts {
    firstName: string;
    lastName: string;
    middleName: string;
    isPrimary: string;
    id: string;
  }
