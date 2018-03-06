import { Injectable } from '@angular/core';

@Injectable()
export class Item {
    id: number = 0;
    code: string = null;
    cost: number = null;
    price: number = null;
    description: string = null;
    purchaseDescription: string = null;
    sellDescription: string = null;
    itemCategoryId: number = 0;
    modelId: number = 0;
    brandId: number = 0;
    sellMeasurementId: number = 0;
    smallestMeasurementId: number = 0;
    purchaseMeasurementId: number = 0;
    taxGroupId: number = 0;
    preferredVendorId: number = 0;
    costOfGoodsSoldAccountId: number = 0;
    inventoryAccountId: number = 0;
    salesAccountId: number = 0;
}
