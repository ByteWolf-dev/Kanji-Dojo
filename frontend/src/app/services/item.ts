import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ItemDto } from '../dtos/item-dto';
import { ItemReadingDto } from '../dtos/item-reading-dto';

@Injectable({
    providedIn: 'root'
})
export class ItemService {

    private http = inject(HttpClient);

    getItems(): Observable<ItemDto[]> {
        return this.http.get<ItemDto[]>('/api/Item');
    }

    getReadings(): Observable<ItemReadingDto[]> {
        return this.http.get<ItemReadingDto[]>('/api/ItemReading');
    }
}
