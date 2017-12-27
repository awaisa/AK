import { Injectable } from '@angular/core';

@Injectable()
export class Item {
    id:number = 0;
    itemCategoryId:number = 0;
    description:string = null;
    purchaseDescription:string = null;
    sellDescription:string = null;
    cost:number = 0;
    price:number = 0;
    code:string = null;
}
