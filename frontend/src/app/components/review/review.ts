import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { CardService } from '../../services/card';
import { ItemService } from '../../services/item';
import { ReviewService } from '../../services/review';
import { CardDto } from '../../dtos/card-dto';
import { ItemDto } from '../../dtos/item-dto';
import { ItemReadingDto } from '../../dtos/item-reading-dto';
import { CardType } from '../../enums/card-type';
import { CharacterType } from '../../enums/character-type';
import { romajiToKana, toHiragana } from '../../utils/romaji-to-kana';

const SESSION_SIZE = 20;

@Component({
    selector: 'app-review',
    imports: [
        RouterLink,
        MatButtonModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatProgressBarModule
    ],
    templateUrl: './review.html',
    styleUrl: './review.css'
})
export class Review implements OnInit {

    private cardService = inject(CardService);
    private itemService = inject(ItemService);
    private reviewService = inject(ReviewService);

    readonly CardType = CardType;

    isLoading = signal(true);
    errorMessage = signal('');

    cards = signal<CardDto[]>([]);
    index = signal(0);
    answer = signal('');
    result = signal<'correct' | 'wrong' | null>(null);
    correctCount = signal(0);

    private items = new Map<number, ItemDto>();
    private readings = new Map<number, ItemReadingDto[]>();

    card = computed(() => this.cards()[this.index()]);
    item = computed(() => this.items.get(this.card()?.itemId));
    itemReadings = computed(() => this.readings.get(this.card()?.itemId) ?? []);
    isFinished = computed(() => !this.isLoading() && this.index() >= this.cards().length);
    progress = computed(() => this.cards().length ? this.index() / this.cards().length * 100 : 0);

    question = computed(() => {
        const item = this.item();
        if (!item) return '';

        if (this.card().cardType !== CardType.Writing) return item.character;

        return item.characterType === CharacterType.Kanji
            ? item.meaning ?? ''
            : this.itemReadings()[0]?.readingRomaji ?? '';
    });

    hint = computed(() => {
        switch (this.card()?.cardType) {
            case CardType.Recognition:
                return 'Type the reading';
            case CardType.Meaning:
                return 'Type the meaning';
            default:
                return 'Pick the right character';
        }
    });

    solution = computed(() => {
        const item = this.item();
        if (!item) return '';

        switch (this.card().cardType) {
            case CardType.Recognition:
                return this.itemReadings()
                    .map(r => `${r.readingKana} (${r.readingRomaji})`)
                    .join(', ');
            case CardType.Meaning:
                return item.meaning ?? '';
            default:
                return item.character;
        }
    });

    options = computed(() => {
        const item = this.item();
        if (!item || this.card().cardType !== CardType.Writing) return [];

        const others = [...this.items.values()]
            .filter(i => i.characterType === item.characterType && i.character !== item.character)
            .sort(() => Math.random() - 0.5)
            .slice(0, 3)
            .map(i => i.character);

        return [...others, item.character].sort(() => Math.random() - 0.5);
    });

    ngOnInit(): void {
        forkJoin({
            cards: this.cardService.getCardsDueOnOrBefore(),
            items: this.itemService.getItems(),
            readings: this.itemService.getReadings()
        }).subscribe({
            next: ({ cards, items, readings }) => {
                items.forEach(i => this.items.set(i.id, i));
                readings.forEach(r => {
                    const list = this.readings.get(r.itemId) ?? [];
                    list.push(r);
                    this.readings.set(r.itemId, list);
                });

                this.cards.set(cards.filter(c => this.items.has(c.itemId)).slice(0, SESSION_SIZE));
                this.isLoading.set(false);
            },
            error: (err) => {
                console.error('Failed to load review session', err);
                this.errorMessage.set('Could not load your reviews.');
                this.isLoading.set(false);
            }
        });
    }

    onInput(event: Event): void {
        const input = event.target as HTMLInputElement;

        if (this.card().cardType === CardType.Recognition) {
            input.value = romajiToKana(
                input.value,
                false,
                this.item()?.characterType === CharacterType.Katakana
            );
        }

        this.answer.set(input.value);
    }

    submit(): void {
        if (this.result()) {
            this.grade(this.result() === 'correct' ? 4 : 1);
            return;
        }

        if (this.answer().trim()) this.check(this.answer());
    }

    check(answer: string): void {
        const item = this.item();
        if (!item) return;

        let isCorrect: boolean;

        switch (this.card().cardType) {
            case CardType.Recognition: {
                const typed = toHiragana(romajiToKana(answer.trim(), true));
                isCorrect = this.itemReadings().some(r => toHiragana(r.readingKana) === typed);
                answer = typed;
                break;
            }
            case CardType.Meaning:
                isCorrect = (item.meaning ?? '')
                    .toLowerCase()
                    .split(',')
                    .some(m => m.trim() === answer.trim().toLowerCase());
                break;
            default:
                isCorrect = answer === item.character;
        }

        this.answer.set(answer);
        this.result.set(isCorrect ? 'correct' : 'wrong');

        if (isCorrect) this.correctCount.update(c => c + 1);
    }

    grade(grade: number): void {
        this.reviewService.submit({
            cardId: this.card().id,
            answer: this.answer(),
            grade
        }).subscribe({
            error: (err) => console.error('Failed to save review', err)
        });

        this.answer.set('');
        this.result.set(null);
        this.index.update(i => i + 1);
    }
}
