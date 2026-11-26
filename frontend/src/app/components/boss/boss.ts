import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { DojoService } from '../../services/dojo';
import { BossAnswerDto, BossQuestionDto, BossResultDto } from '../../dtos/boss-dto';
import { CharacterType } from '../../enums/character-type';
import { romajiToKana } from '../../utils/romaji-to-kana';

@Component({
    selector: 'app-boss',
    imports: [
        RouterLink,
        MatButtonModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatProgressBarModule
    ],
    templateUrl: './boss.html',
    styleUrl: './boss.css'
})
export class Boss implements OnInit {

    private dojoService = inject(DojoService);

    isLoading = signal(true);
    errorMessage = signal('');

    questions = signal<BossQuestionDto[]>([]);
    answers = signal<BossAnswerDto[]>([]);
    answer = signal('');
    result = signal<BossResultDto | null>(null);

    index = computed(() => this.answers().length);
    question = computed(() => this.questions()[this.index()]);
    progress = computed(() => this.questions().length ? this.index() / this.questions().length * 100 : 0);

    ngOnInit(): void {
        this.dojoService.getBossFight().subscribe({
            next: (questions) => {
                this.questions.set(questions);
                this.isLoading.set(false);
            },
            error: (err) => {
                this.errorMessage.set(typeof err.error === 'string' ? err.error : 'Could not start the boss fight.');
                this.isLoading.set(false);
            }
        });
    }

    onInput(event: Event): void {
        const input = event.target as HTMLInputElement;

        input.value = romajiToKana(
            input.value,
            false,
            this.question()?.characterType === CharacterType.Katakana
        );

        this.answer.set(input.value);
    }

    next(): void {
        const answer = romajiToKana(this.answer().trim(), true);

        if (!answer) return;

        this.answers.update(a => [...a, { itemId: this.question().itemId, answer }]);
        this.answer.set('');

        if (this.index() === this.questions().length) this.submit();
    }

    private submit(): void {
        this.isLoading.set(true);

        this.dojoService.submitBossFight(this.answers()).subscribe({
            next: (result) => {
                this.result.set(result);
                this.isLoading.set(false);
            },
            error: (err) => {
                this.errorMessage.set(typeof err.error === 'string' ? err.error : 'Could not submit the boss fight.');
                this.isLoading.set(false);
            }
        });
    }
}
