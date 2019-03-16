import { Injectable } from '@angular/core';

@Injectable()
export class DataTablesResponse {
    data: any[];
    draw: number;
    recordsFiltered: number;
    recordsTotal: number;
}