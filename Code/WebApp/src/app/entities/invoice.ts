import { Injectable } from '@angular/core';
import { Tax } from './tax';

@Injectable()
export class Invoice {
    id:number = 0;
    no:number = 0;
    date:Date = new Date();
    description:string = null;
    code:string = null;
    preferredVendorId: number = 0;
    vendorInvoiceNo: string = null;
    lineItems: InvoiceLineItem[] = new Array();
    subTotal: number = 0;
    tax: number = 0;
    discount: number = 0;
    total: number = 0;
}

@Injectable()
export class InvoiceLineItem {
    id: number = 0;
    itemId: number = 0;
    quantity: number = 0;
    unitPrice: number = 0;
    total: number = 0;
    taxAmount: number = 0;
    taxes: Tax[];
}