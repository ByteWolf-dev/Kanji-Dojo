import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProgressDto } from '../dtos/progress-dto';
import { BossAnswerDto, BossQuestionDto, BossResultDto } from '../dtos/boss-dto';

@Injectable({
    providedIn: 'root'
})
export class DojoService {

    private http = inject(HttpClient);
    private apiUrl = '/api/Dojo';

    getProgress(): Observable<ProgressDto> {
        return this.http.get<ProgressDto>(`${this.apiUrl}/progress`);
    }

    getBossFight(): Observable<BossQuestionDto[]> {
        return this.http.get<BossQuestionDto[]>(`${this.apiUrl}/boss`);
    }

    submitBossFight(answers: BossAnswerDto[]): Observable<BossResultDto> {
        return this.http.post<BossResultDto>(`${this.apiUrl}/boss`, answers);
    }
}
