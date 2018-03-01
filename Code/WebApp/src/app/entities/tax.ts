import { Injectable } from '@angular/core';

@Injectable()
export class Tax {
    id:number = 0;
    taxName:string = null;
    rate:number = null;
}