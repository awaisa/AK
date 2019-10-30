
import {Injectable} from '@angular/core'
import { InvoiceLineItem } from './invoice';

@Injectable()
export class SaleInvoice{
    id: number = 0;
    no: number = 0;
    date: Date = new Date();
    customerId?: number = 0;
    description: string = null;
    subTotal: number = 0;
    tax: number = 0;
    discount: number = 0;
    total: number = 0;
    shippingHandlingCharge: number = 0;
    status: number = 0;
    invoiceItems: InvoiceLineItem[] = new Array();
}