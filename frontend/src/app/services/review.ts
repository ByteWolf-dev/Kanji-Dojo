import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReviewDto } from '../dtos/review-dto';

@Injectable({
    providedIn: 'root'
})
export class ReviewService {

    private http = inject(HttpClient);
    private apiUrl = '/api/Review';

    submit(review: ReviewDto): Observable<ReviewDto> {
        return this.http.post<ReviewDto>(this.apiUrl, review);
    }
}
