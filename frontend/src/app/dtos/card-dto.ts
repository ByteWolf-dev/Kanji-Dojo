import { CardType } from '../enums/card-type';

export interface CardDto {
    id: number;
    userId: number;
    itemId: number;
    cardType: CardType;
    easeFactor: number;
    intervalDays: number;
    repetitions: number;
    dueAt: string;
    lastReviewAt: string;
}