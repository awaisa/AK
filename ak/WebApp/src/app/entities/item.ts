import { Injectable } from '@angular/core';

@Injectable()
export class Item {
    Id:number = 0;
    ItemCategoryId:number = 0;
    Description:string = null;
    PurchaseDescription:string = null;
    SellDescription:string = null;
    Cost:number = 0;
    Price:number = 0;
    Code:string = null;
}
