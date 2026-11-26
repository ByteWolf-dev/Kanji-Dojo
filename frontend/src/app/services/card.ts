import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CardDto } from '../dtos/card-dto';

@Injectable({
    providedIn: 'root'
})
export class CardService {

    private http = inject(HttpClient);
    private apiUrl = '/api/Card';

    getByUserId(userId: number): Observable<CardDto[]> {

        const params = new HttpParams()
            .set('userId', userId);

        return this.http.get<CardDto[]>(
            this.apiUrl,
            { params }
        );
    }

    getCardsDueOnOrBefore(
        date: Date = new Date()
    ): Observable<CardDto[]> {

        const params = new HttpParams()
            .set('date', date.toISOString());

        return this.http.get<CardDto[]>(
            `${this.apiUrl}/due`,
            { params }
        );
    }
}